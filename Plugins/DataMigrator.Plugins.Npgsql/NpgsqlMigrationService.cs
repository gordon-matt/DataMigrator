using System.Data;
using System.Data.Common;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso;
using Extenso.Collections;
using Extenso.Data.Npgsql;
using Npgsql;

namespace DataMigrator.Plugins.Npgsql;

public class NpgsqlMigrationService : BaseMigrationService
{
    private readonly NpgsqlDbTypeConverter typeConverter = new();

    public NpgsqlMigrationService(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
        EscapeIdentifierStart = "\"";
        EscapeIdentifierEnd = "\"";
    }

    #region IMigrationService Members

    public override string DbProviderName => "Npgsql";

    public override DbConnection CreateDbConnection() => new NpgsqlConnection(ConnectionDetails.ConnectionString);

    public override int CountRecords(string tableName, string schemaName)
    {
        using var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString);
        return connection.GetRowCount(schemaName, tableName);
    }

    #endregion IMigrationService Members

    protected override async Task<bool> CreateTableAsync(string tableName, string schemaName)
    {
        using var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format($@"CREATE TABLE {GetFullTableName(tableName, schemaName)}()");

        await connection.OpenAsync();
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

        using var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString);
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
            string.Concat(EncloseIdentifier(field.Name), " ", fieldType, maxLength, isRequired));

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return true;
    }

    public override async Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records)
    {
        const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";

        var parameterNames = CreateParameterNames(records.ElementAt(0).Fields.Select(f => f.Name));
        string fieldNames = parameterNames.Keys.Join(",");

        fieldNames = fieldNames
            .Replace(",", string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart)) // "],["
            .Prepend(EscapeIdentifierStart) // "["
            .Append(EscapeIdentifierEnd); // "]"

        //using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        connection.Open();
        using (var transaction = await connection.BeginTransactionAsync())
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = string.Format(INSERT_INTO_FORMAT, GetFullTableName(tableName, schemaName), fieldNames, parameterNames.Values.Join(","));

                records.ElementAt(0).Fields.ForEach(field =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = parameterNames[field.Name];
                    parameter.DbType = TypeConvert.DbTypeConverter.GetDataProviderFieldType(field.Type);
                    command.Parameters.Add(parameter);
                });

                records.ForEach(record =>
                {
                    record.Fields.ForEach(field =>
                    {
                        command.Parameters[parameterNames[field.Name]].Value = field.Value ?? DBNull.Value;
                    });

                    command.ExecuteNonQuery();
                });
            }
            await transaction.CommitAsync();
        }
        await connection.CloseAsync();
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

    #endregion Field Conversion
}