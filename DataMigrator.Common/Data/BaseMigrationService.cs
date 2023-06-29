﻿using System.Data.Common;
using System.Text;
using DataMigrator.Common.Diagnostics;
using Extenso.Data.Common;

namespace DataMigrator.Common.Data;

public abstract class BaseMigrationService : IMigrationService
{
    public BaseMigrationService(ConnectionDetails connectionDetails)
    {
        ConnectionDetails = connectionDetails;
    }

    protected ConnectionDetails ConnectionDetails { get; set; }

    protected virtual string EscapeIdentifierEnd { get; set; } = "]";

    protected virtual string EscapeIdentifierStart { get; set; } = "[";

    #region IMigrationService Members

    public abstract string DbProviderName { get; }

    public virtual DbConnection CreateDbConnection()
    {
        // Assume failure.
        DbConnection connection = null;

        // Create the DbProviderFactory and DbConnection.
        if (ConnectionDetails.ConnectionString != null)
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(DbProviderName);

                connection = factory.CreateConnection();
                connection.ConnectionString = ConnectionDetails.ConnectionString;
            }
            catch (Exception ex)
            {
                // Set the connection to null if it was created.
                if (connection != null)
                {
                    connection = null;
                }
                Console.WriteLine(ex.Message);
            }
        }
        // Return the connection.
        return connection;
    }

    public virtual async Task<IEnumerable<string>> GetTableNamesAsync()
    {
        using var connection = CreateDbConnection();
        string[] restrictions = new string[4];
        restrictions[3] = "BASE TABLE";

        await connection.OpenAsync();
        var schema = await connection.GetSchemaAsync("Tables", restrictions);
        await connection.CloseAsync();

        var tableNames = new List<string>();
        foreach (DataRow row in schema.Rows)
        {
            string schemaName = row.Field<string>("TABLE_SCHEMA");
            string tableName = row.Field<string>("TABLE_NAME");

            tableNames.Add($"{schemaName}.{tableName}");
        }
        return tableNames;
    }

    public virtual async Task<bool> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        bool ok = await CreateTableAsync(tableName, schemaName);

        if (!ok)
        { return false; }

        foreach (var field in fields)
        {
            await CreateFieldAsync(tableName, schemaName, field);
        }
        return true;
    }

    public virtual async Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(Constants.Data.CMD_SELECT_INFO_SCHEMA_COLUMNS, tableName);

        if (!string.IsNullOrEmpty(schemaName))
        {
            command.CommandText = $"{command.CommandText} AND TABLE_SCHEMA = '{schemaName}'";
        }

        var fields = new FieldCollection();

        await connection.OpenAsync();
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                var field = new Field
                {
                    Name = reader.GetString(0)
                };
                if (!reader.IsDBNull(1)) { field.Ordinal = reader.GetInt32(1); }
                if (!reader.IsDBNull(2)) { field.Type = GetDataMigratorFieldType(reader.GetString(2)); }
                if (!reader.IsDBNull(3)) { field.IsRequired = reader.GetString(3) == "NO"; }
                if (!reader.IsDBNull(4)) { field.MaxLength = reader.GetInt32(4); }
                fields.Add(field);
            }
        }
        await connection.CloseAsync();

        try
        {
            command.CommandText = string.Format(Constants.Data.CMD_IS_PRIMARY_KEY_FORMAT, tableName);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string pkColumn = reader.GetString(0);
                    var match = fields.SingleOrDefault(f => f.Name == pkColumn);
                    if (match != null)
                    {
                        match.IsPrimaryKey = true;
                    }
                }
            }

            await connection.CloseAsync();
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteConcat(TraceEvent.Error, "Error: Could not get primary key info - ", x.Message);
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }

        return fields;
    }

    public virtual int CountRecords(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection();
        return connection.ExecuteScalar($"SELECT COUNT(*) FROM {GetFullTableName(tableName, schemaName)}");
    }

    public virtual async IAsyncEnumerable<Record> GetRecordsAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        //Query query = new Query();
        //fields.ForEach(f => { query.Select(f.Name); });
        //query.From(tableName);

        var sb = new StringBuilder();
        sb.Append("SELECT ");
        sb.Append(fields.Select(f => f.Name).Join(
            string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart))
            .Prepend(EscapeIdentifierStart)
            .Append(EscapeIdentifierEnd));
        sb.Append(" FROM ");
        sb.Append(GetFullTableName(tableName, schemaName));

        using var connection = CreateDbConnection();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = sb.ToString();

        await connection.OpenAsync();
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                var record = new Record();
                record.Fields.AddRange(fields);
                fields.ForEach(f =>
                {
                    if (f.Type == FieldType.String)
                    {
                        string value = reader.GetString(f.Name);
                        if (AppState.ConfigFile.TrimStrings)
                        {
                            value = value.Trim();
                        }

                        if (string.IsNullOrEmpty(value))
                        {
                            value = null;
                        }

                        record[f.Name].Value = value;
                    }
                    else
                    {
                        record[f.Name].Value = reader[f.Name];
                    }
                });
                yield return record;
            }
        }
        await connection.CloseAsync();
    }

    // TODO: See if can improve performance.
    public virtual async Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records)
    {
        const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";

        var parameterNames = CreateParameterNames(records.ElementAt(0).Fields.Select(f => f.Name));
        string fieldNames = parameterNames.Keys.Join(",");

        fieldNames = fieldNames
            .Replace(",", string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart)) // "],["
            .Prepend(EscapeIdentifierStart) // "["
            .Append(EscapeIdentifierEnd); // "]"

        //using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        await connection.OpenAsync();
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

                    command.ExecuteNonQuery(); // Not using Async version as it gives an error: "A command is already in progress".
                });
            }
            await transaction.CommitAsync();
        }
        await connection.CloseAsync();
    }

    #endregion IMigrationService Members

    #region Field Conversion

    protected abstract FieldType GetDataMigratorFieldType(string providerFieldType);

    protected abstract string GetDataProviderFieldType(FieldType fieldType);

    #endregion Field Conversion

    protected static IDictionary<string, string> CreateParameterNames(IEnumerable<string> fieldNames)
    {
        var parameterNames = new Dictionary<string, string>();
        fieldNames.ForEach(f =>
        {
            string parameterName = f;
            "¬`!\"£$%^&*()-=+{}[]:;@'~#|<>,.?/ ".ToCharArray().ForEach(c => { parameterName = parameterName.Replace(c, '_'); });
            parameterNames.Add(f, parameterName.ToPascalCase().Prepend("@"));
        });
        return parameterNames;
    }

    protected virtual async Task<bool> CreateTableAsync(string tableName, string schemaName)
    {
        try
        {
            await CreateTableAsync(tableName, schemaName, "Id", GetDataProviderFieldType(FieldType.Int32), true);
        }
        catch (DbException x)
        {
            TraceService.Instance.WriteException(x);
            return false;
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteException(x);
            return false;
        }

        return true;
    }

    protected virtual async Task CreateTableAsync(string tableName, string schemaName, string pkColumnName, string pkDataType, bool pkIsIdentity)
    {
        using var connection = CreateDbConnection();

        string commandText =
$@"CREATE TABLE {GetFullTableName(tableName, schemaName)}
(
    {EncloseIdentifier(pkColumnName)} {pkDataType} {(pkIsIdentity ? "IDENTITY(1,1)" : string.Empty)} NOT NULL
        CONSTRAINT {EncloseIdentifier("PK_" + tableName)} PRIMARY KEY
)";

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = commandText;
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }

    protected virtual async Task<bool> CreateFieldAsync(string tableName, string schemaName, Field field)
    {
        var existingFieldNames = await GetFieldNamesAsync(tableName, schemaName);
        if (existingFieldNames.Contains(field.Name))
        {
            TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the table, {1}", field.Name, GetFullTableName(tableName, schemaName));
            //throw new ArgumentException("etc");
            return false;
        }

        using var connection = CreateDbConnection();
        using var command = connection.CreateCommand();
        string fieldType = GetDataProviderFieldType(field.Type);
        string maxLength = string.Empty;
        if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char))
        {
            if (field.MaxLength is > 0 and <= 8000)
            {
                maxLength = $"({field.MaxLength})";
            }
            else
            {
                if (field.Type.In(FieldType.String, FieldType.RichText)) //Not supported for CHAR
                {
                    maxLength = "(MAX)";
                }
            }
        }
        string isRequired = string.Empty;
        if (field.IsRequired)
        { isRequired = " NOT NULL"; }

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
            Constants.Data.CMD_ADD_COLUMN,
            GetFullTableName(tableName, schemaName),
            string.Concat(
                EncloseIdentifier(field.Name), " ",
                fieldType,
                maxLength,
                isRequired));
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return true;
    }

    protected string EncloseIdentifier(string value) => $"{EscapeIdentifierStart}{value}{EscapeIdentifierEnd}";

    protected virtual string GetFullTableName(string tableName, string schemaName) =>
        !string.IsNullOrEmpty(schemaName)
            ? $"{EncloseIdentifier(schemaName)}.{EncloseIdentifier(tableName)}"
            : EncloseIdentifier(tableName);

    protected virtual async Task<IEnumerable<string>> GetFieldNamesAsync(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(Constants.Data.CMD_SELECT_INFO_SCHEMA_COLUMN_NAMES, tableName);

        if (!string.IsNullOrEmpty(schemaName))
        {
            command.CommandText = $"{command.CommandText} AND TABLE_SCHEMA = '{schemaName}'";
        }

        var columns = new List<string>();

        await connection.OpenAsync();
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                columns.Add(reader.GetString(0));
            }
        }
        await connection.CloseAsync();
        return columns;
    }
}