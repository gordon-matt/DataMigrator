using System.Data;
using System.Data.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso;
using Extenso.Data;
using Extenso.Data.MySql;
using MySql.Data.MySqlClient;

namespace DataMigrator.MySql;

public class MySqlMigrationService : BaseAdoNetMigrationService
{
    private readonly MySqlDbTypeConverter typeConverter = new();

    protected override string QuotePrefix => "`";

    protected override string QuoteSuffix => "`";

    public MySqlMigrationService(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    #region IMigrationService Members

    public override string DbProviderName => "MySql.Data.MySqlClient";

    public override DbConnection CreateDbConnection() => new MySqlConnection(ConnectionDetails.ConnectionString);

    public override async Task<string> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        string result = await base.CreateTableAsync(tableName, schemaName, fields);
        return $"{ConnectionDetails.Database}.{result}".ToLowerInvariant(); // For MySQL, the database name takes the place of schema name
    }

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

        if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char, FieldType.Binary))
        {
            if (field.MaxLength is > 0 and <= 8000)
            {
                if (field.Type == FieldType.Binary && field.MaxLength > 255)
                {
                    fieldType = "VARBINARY";
                }

                maxLength = $"({field.MaxLength})";
            }
            else
            {
                switch (field.Type)
                {
                    case FieldType.String:
                    case FieldType.RichText: maxLength = "(MAX)"; break;
                    case FieldType.Binary:
                        {
                            fieldType = "BLOB";
                        }
                        break;
                    case FieldType.Char: maxLength = $"(128)"; break;
                    default: break;
                }
            }

            if (field.Type != FieldType.Binary)
            {
                //MySql does not have MAX keyword
                characterSet = " CHARACTER SET utf8";
            }
        }

        if (field.IsRequired)
        { isRequired = " NOT NULL"; }

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
            "ALTER TABLE {0} ADD {1}",
            GetFullTableName(tableName, schemaName),
            string.Concat(QuoteIdentifier(field.Name), " ", fieldType, maxLength, characterSet, isRequired));

        await connection.OpenAsync();
        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteException(x, $"Error when trying to add field. Command Text: {command.CommandText}");
        }
        finally
        {
            await connection.CloseAsync();
        }

        return true;
    }

    public override Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection() as MySqlConnection;
        var columnInfo = connection.GetColumnData(tableName);

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

    // Schema in MySQL is just a different database altogether.. so just ignore it and return the table name directly..
    protected override string GetFullTableName(string tableName, string schemaName) => QuoteIdentifier(tableName);
}