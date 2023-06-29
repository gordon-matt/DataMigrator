using System.Data;
using System.Data.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso;
using MySql.Data.MySqlClient;
using CommonConstants = DataMigrator.Common.Constants;

namespace DataMigrator.MySql;

//TODO: Test! This class not yet tested.
public class MySqlMigrationService : BaseMigrationService
{
    private readonly MySqlDbTypeConverter typeConverter = new();

    public MySqlMigrationService(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
        EscapeIdentifierStart = "`";
        EscapeIdentifierEnd = "`";
    }

    #region IMigrationService Members

    public override string DbProviderName => "MySql.Data.MySqlClient";

    public override DbConnection CreateDbConnection() => new MySqlConnection(ConnectionDetails.ConnectionString);

    //public override void InsertRecords(string tableName, IEnumerable<Record> records)
    //{
    //    const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
    //    string fieldNames = records.ElementAt(0).Fields
    //        .Select(f => f.Name)
    //        .OrderBy(f => f)
    //        .Join("`,`").Prepend("`").Append("`");

    //    using (MySqlConnection connection = new MySqlConnection(ConnectionDetails.ConnectionString))
    //    {
    //        connection.Open();
    //        using (MySqlTransaction transaction = connection.BeginTransaction())
    //        {
    //            using (MySqlCommand command = connection.CreateCommand())
    //            {
    //                command.Transaction = transaction;
    //                //command.CommandText = "SET NAMES UTF8; SET CHARACTER SET UTF8;";
    //                //command.ExecuteNonQuery();

    //                StringBuilder sbValues = null;
    //                records.ForEach(record =>
    //                {
    //                    sbValues = new StringBuilder(50);
    //                    record.Fields.OrderBy(f => f.Name).ForEach(field =>
    //                    {
    //                        if (field.IsNumeric)
    //                        {
    //                            sbValues.Append(field.Value);
    //                        }
    //                        else if (field.Type == FieldType.DateTime)
    //                        {
    //                            sbValues.Append(field.GetValue<DateTime>().ToISO8601DateString().AddSingleQuotes());
    //                        }
    //                        else
    //                        {
    //                            sbValues.Append("'", field.Value.ToString().Replace("'", "''"), "'");
    //                        }
    //                        sbValues.Append(",");
    //                    });
    //                    sbValues.Length -= 1; // Remove last comma ","

    //                    command.CommandText = string.Format(INSERT_INTO_FORMAT, tableName, fieldNames, sbValues.ToString());
    //                    command.ExecuteNonQuery();
    //                });
    //            }
    //            transaction.Commit();
    //        }
    //        connection.Close();
    //    }
    //}

    #endregion IMigrationService Members

    #region Field Conversion

    protected override FieldType GetDataMigratorFieldType(string providerFieldType)
    {
        var mySqlType = MySqlDbTypeConverter.GetMySqlDataType(providerFieldType);
        return typeConverter.GetDataMigratorFieldType(mySqlType);
    }

    protected override string GetDataProviderFieldType(FieldType fieldType)
    {
        var mySqlType = typeConverter.GetDataProviderFieldType(fieldType);
        return MySqlDbTypeConverter.GetMySqlDataTypeStringValue(mySqlType);
    }

    #endregion Field Conversion

    protected override async Task<bool> CreateTableAsync(string tableName, string schemaName)
    {
        using (var connection = new MySqlConnection(ConnectionDetails.ConnectionString))
        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format(
                "CREATE TABLE {0}(Id INT NOT NULL AUTO_INCREMENT, PRIMARY KEY(Id)) ENGINE=InnoDB CHARSET=utf8 COLLATE=utf8_unicode_ci",
                GetFullTableName(tableName, schemaName));

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
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

        using var connection = new MySqlConnection(ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();
        string fieldType = GetDataProviderFieldType(field.Type);
        string maxLength = string.Empty;
        string characterSet = string.Empty;
        string isRequired = string.Empty;

        if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char))
        {
            if (field.MaxLength > 0)
            {
                maxLength = $"({field.MaxLength})";
            }
            //MySql does not have MAX keyword
            characterSet = " CHARACTER SET utf8";
        }

        if (field.IsRequired)
        { isRequired = " NOT NULL"; }

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
            "ALTER TABLE {0} ADD {1}",
            GetFullTableName(tableName, schemaName),
            string.Concat(EncloseIdentifier(field.Name), " ", fieldType, maxLength, characterSet, isRequired));
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return true;
    }

    public override async Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
@"SELECT COLUMN_NAME, ORDINAL_POSITION, COLUMN_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}'", tableName);

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
            command.CommandText = string.Format(CommonConstants.Data.CMD_IS_PRIMARY_KEY_FORMAT, tableName);

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
}