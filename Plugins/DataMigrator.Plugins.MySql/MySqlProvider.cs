using System.Data;
using System.Data.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso;
using MySql.Data.MySqlClient;

namespace DataMigrator.MySql;

//TODO: Test! This class not yet tested.
public class MySqlProvider : BaseProvider
{
    private readonly MySqlDbTypeConverter typeConverter = new();

    public override string DbProviderName => "MySql.Data.MySqlClient";

    public MySqlProvider(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
        EscapeIdentifierStart = "`";
        EscapeIdentifierEnd = "`";
    }

    public override bool CreateTable(string tableName)
    {
        using (var connection = new MySqlConnection(ConnectionDetails.ConnectionString))
        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format(
                "CREATE TABLE {0}(Id INT NOT NULL AUTO_INCREMENT, PRIMARY KEY(Id)) ENGINE=InnoDB CHARSET=utf8 COLLATE=utf8_unicode_ci",
                tableName);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }

    protected override void CreateTable(string tableName, string pkColumnName, string pkDataType, bool pkIsIdentity)
    {
        throw new NotSupportedException();
    }

    public override bool CreateField(string tableName, Field field)
    {
        var existingFieldNames = GetFieldNames(tableName);
        if (existingFieldNames.Contains(field.Name))
        {
            TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the table, {1}", field.Name, tableName);
            //throw new ArgumentException("etc");
            return false;
        }

        using var connection = new MySqlConnection(ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();
        string fieldType = GetDataProviderFieldType(field.Type);
        string maxLength = string.Empty;
        string characterSet = string.Empty;
        if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char))
        {
            if (field.MaxLength > 0)
            {
                maxLength = string.Concat("(", field.MaxLength, ")");
            }
            //MySql does not have MAX keyword
            characterSet = " CHARACTER SET utf8";
        }
        string isRequired = string.Empty;
        if (field.IsRequired)
        { isRequired = " NOT NULL"; }

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
            "ALTER TABLE {0} ADD {1}",
            EncloseIdentifier(tableName),
            string.Concat(EncloseIdentifier(field.Name), " ", fieldType, maxLength, characterSet, isRequired));
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
        return true;
    }

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

    public override FieldType GetDataMigratorFieldType(string providerFieldType)
    {
        var mySqlType = MySqlDbTypeConverter.GetMySqlDataType(providerFieldType);
        return typeConverter.GetDataMigratorFieldType(mySqlType);
    }

    public override string GetDataProviderFieldType(FieldType fieldType)
    {
        var mySqlType = typeConverter.GetDataProviderFieldType(fieldType);
        return MySqlDbTypeConverter.GetMySqlDataTypeStringValue(mySqlType);
    }

    protected override DbConnection CreateDbConnection(string providerName, string connectionString)
    {
        return new MySqlConnection(connectionString);
    }
}