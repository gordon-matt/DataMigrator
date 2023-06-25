using System.Data;
using System.Data.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso;
using MySql.Data.MySqlClient;

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
}