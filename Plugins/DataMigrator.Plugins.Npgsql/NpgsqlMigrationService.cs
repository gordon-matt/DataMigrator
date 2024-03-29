﻿using System.Data;
using System.Data.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso;
using Extenso.Data;
using Extenso.Data.Npgsql;
using Npgsql;
using NpgsqlTypes;

namespace DataMigrator.Plugins.Npgsql;

public class NpgsqlMigrationService : BaseAdoNetMigrationService
{
    private readonly NpgsqlDbTypeConverter typeConverter = new();

    protected override string QuotePrefix => "\"";

    protected override string QuoteSuffix => "\"";

    public NpgsqlMigrationService(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    #region IMigrationService Members

    public override string DbProviderName => "Npgsql";

    public override DbConnection CreateDbConnection() => new NpgsqlConnection(ConnectionDetails.ConnectionString);

    public override int CountRecords(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection() as NpgsqlConnection;
        return connection.GetRowCount(schemaName, tableName);
    }

    #endregion IMigrationService Members

    protected override async Task<bool> CreateTableAsync(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection() as NpgsqlConnection;
        using var command = connection.CreateCommand();

        await connection.OpenAsync();

        command.CommandType = CommandType.Text;

        command.CommandText = $"CREATE SCHEMA IF NOT EXISTS {QuoteIdentifier(schemaName)}";
        await command.ExecuteNonQueryAsync();

        command.CommandText = string.Format($@"CREATE TABLE {GetFullTableName(tableName, schemaName)}()");
        await command.ExecuteNonQueryAsync();

        await connection.CloseAsync();

        return true;
    }

    protected override Task CreateTableAsync(string tableName, string schemaName, string pkColumnName, string pkDataType, bool pkIsIdentity) =>
        throw new NotSupportedException();

    protected override async Task<bool> CreateFieldAsync(string tableName, string schemaName, Field field)
    {
        var existingFieldNames = await GetFieldNamesAsync(tableName, schemaName);
        if (existingFieldNames.Contains(field.Name))
        {
            TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the table, {1}", field.Name, GetFullTableName(tableName, schemaName));
            //throw new ArgumentException("etc");
            return false;
        }

        using var connection = CreateDbConnection() as NpgsqlConnection;
        using var command = connection.CreateCommand();
        string fieldType = GetDataProviderFieldType(field.Type);
        string maxLength = string.Empty;
        if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char))
        {
            if (field.MaxLength > 0)
            {
                maxLength = $"({field.MaxLength})";
            }
            else
            {
                fieldType = "text";
            }
        }
        string isRequired = string.Empty;
        if (field.IsRequired)
        { isRequired = " NOT NULL"; }

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
            @"ALTER TABLE {0} ADD {1}",
            GetFullTableName(tableName, schemaName),
            string.Concat(QuoteIdentifier(field.Name), " ", fieldType, maxLength, isRequired));

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return true;
    }

    public override Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection() as NpgsqlConnection;
        var columnInfo = connection.GetColumnData(tableName, schemaName);

        var fields = columnInfo.Select(x => new Field
        {
            Name = x.ColumnName,
            Ordinal = x.OrdinalPosition,
            Type = GetDataMigratorFieldType(x.DataTypeNative),
            IsRequired = !x.IsNullable,
            MaxLength = (int)x.MaximumLength,
            IsPrimaryKey = x.KeyType == KeyType.PrimaryKey
        });

        return Task.FromResult(new FieldCollection(fields));
    }

    #region Field Conversion

    protected override FieldType GetDataMigratorFieldType(string providerFieldType)
    {
        var npgsqlType = NpgsqlDbTypeConverter.GetNpgsqlDataType(providerFieldType);
        return typeConverter.GetDataMigratorFieldType(npgsqlType);
    }

    protected override string GetDataProviderFieldType(FieldType fieldType)
    {
        var npgsqlType = typeConverter.GetDataProviderFieldType(fieldType);
        return NpgsqlDbTypeConverter.GetNpgsqlDataTypeStringValue(npgsqlType);
    }

    protected override DbParameter EnsureCorrectParameterType(FieldType fieldType, DbParameter parameter)
    {
        var npgsqlParameter = parameter as NpgsqlParameter;
        if (npgsqlParameter.NpgsqlDbType == NpgsqlDbType.TimestampTz)
        {
            // Prevent errors regarding DateTimeKind.Unspecified not being supported.
            npgsqlParameter.NpgsqlDbType= NpgsqlDbType.Timestamp;
        }
        return npgsqlParameter;
    }

    #endregion Field Conversion
}