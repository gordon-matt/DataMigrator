﻿using System.Data;
using System.Data.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso;
using Extenso.Data.Npgsql;
using Npgsql;

namespace DataMigrator.Plugins.Npgsql;

public class NpgsqlProvider : BaseProvider
{
    private readonly NpgsqlDbTypeConverter typeConverter = new();

    public override string DbProviderName => "Npgsql";

    public NpgsqlProvider(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
        EscapeIdentifierStart = "\"";
        EscapeIdentifierEnd = "\"";
    }

    //public override IEnumerable<string> TableNames
    //{
    //    get
    //    {
    //        using var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString);
    //        return connection.GetTableNames(schema: Schema);
    //    }
    //}

    protected override DbConnection CreateDbConnection(string providerName, string connectionString) => new NpgsqlConnection(connectionString);

    protected override async Task<bool> CreateTableAsync(string tableName, string schemaName)
    {
        using var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
            @"CREATE TABLE {0}.""{1}""()",
            schemaName,
            tableName);

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
            @"ALTER TABLE {0}.""{1}"" ADD {2}",
            schemaName,
            tableName,
            string.Concat(EncloseIdentifier(field.Name), " ", fieldType, maxLength, isRequired));

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return true;
    }

    public override int GetRecordCount(string tableName, string schemaName)
    {
        using var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString);
        return connection.GetRowCount(schemaName, tableName);
    }

    //public override IEnumerator<Record> GetRecordsEnumerator(string tableName, string schemaName, IEnumerable<Field> fields)
    //{
    //    var sb = new StringBuilder();
    //    sb.Append("SELECT ");
    //    sb.Append(fields.Select(f => f.Name).Join(
    //        string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart))
    //        .Prepend(EscapeIdentifierStart)
    //        .Append(EscapeIdentifierEnd));
    //    sb.Append(" FROM ");
    //    sb.Append(GetFullTableName(tableName, schemaName));

    //    using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
    //    using var command = connection.CreateCommand();
    //    command.CommandType = CommandType.Text;
    //    command.CommandText = sb.ToString();

    //    connection.Open();
    //    using (var reader = command.ExecuteReader())
    //    {
    //        while (reader.Read())
    //        {
    //            var record = new Record();
    //            record.Fields.AddRange(fields);
    //            fields.ForEach(f =>
    //            {
    //                record[f.Name].Value = reader[f.Name];
    //            });
    //            yield return record;
    //        }
    //    }
    //    connection.Close();
    //}

    //public override void InsertRecords(string tableName, string schemaName, IEnumerable<Record> records)
    //{
    //    const string INSERT_INTO_FORMAT = @"INSERT INTO {0}.""{1}""({2}) VALUES({3})";

    //    var parameterNames = CreateParameterNames(records.ElementAt(0).Fields.Select(f => f.Name));
    //    string fieldNames = parameterNames.Keys.Join(",");

    //    fieldNames = fieldNames
    //        .Replace(",", string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart)) // "],["
    //        .Prepend(EscapeIdentifierStart) // "["
    //        .Append(EscapeIdentifierEnd); // "]"

    //    using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
    //    connection.Open();
    //    using (var transaction = connection.BeginTransaction())
    //    {
    //        using (var command = connection.CreateCommand())
    //        {
    //            command.Transaction = transaction;
    //            command.CommandText = string.Format(INSERT_INTO_FORMAT, schemaName, tableName, fieldNames, parameterNames.Values.Join(","));

    //            records.ElementAt(0).Fields.ForEach(field =>
    //            {
    //                var parameter = command.CreateParameter();
    //                parameter.ParameterName = parameterNames[field.Name];
    //                parameter.DbType = Common.AppContext.DbTypeConverter.GetDataProviderFieldType(field.Type);
    //                command.Parameters.Add(parameter);
    //            });

    //            records.ForEach(record =>
    //            {
    //                record.Fields.ForEach(field =>
    //                {
    //                    command.Parameters[parameterNames[field.Name]].Value = field.Value;
    //                });

    //                command.ExecuteNonQuery();
    //            });
    //        }
    //        transaction.Commit();
    //    }
    //    connection.Close();
    //}

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
}