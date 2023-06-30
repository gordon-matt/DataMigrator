using System.Data;
using DataMigrator.Common.Data;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso;

namespace DataMigrator.Access;

//TODO: Test this class
public class AccessMigrationService : BaseAdoNetMigrationService
{
    private readonly AccessDbTypeConverter typeConverter = new();

    public AccessMigrationService(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    public override string DbProviderName => "System.Data.Odbc";

    protected override string QuotePrefix => "'";

    protected override string QuoteSuffix => "'";

    //public override async Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    //{
    //    using var connection = CreateDbConnection();
    //    var columnInfo = connection.GetColumnData(tableName);
    //    var fields = new FieldCollection();

    //    columnInfo.ForEach(c =>
    //    {
    //        var field = new Field
    //        {
    //            DisplayName = c.ColumnName,
    //            IsPrimaryKey = c.KeyType == KeyType.PrimaryKey,
    //            IsRequired = !c.IsNullable,
    //            MaxLength = (int)c.MaximumLength,
    //            Name = c.ColumnName,
    //            Ordinal = (int)c.OrdinalPosition,
    //            Type = AppContext.SystemTypeConverter.GetDataMigratorFieldType(c.DataType)
    //        };
    //        fields.Add(field);
    //    });

    //    return fields;
    //}

    protected override async Task<bool> CreateFieldAsync(string tableName, string schemaName, Field field)
    {
        var existingFieldNames = await GetFieldNamesAsync(tableName, schemaName);
        if (existingFieldNames.Contains(field.Name))
        {
            TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the table, {1}", field.Name, GetFullTableName(tableName, schemaName));
            return false;
        }

        using var connection = CreateDbConnection();
        using var command = connection.CreateCommand();
        string fieldType = GetDataProviderFieldType(field.Type);
        string maxLength = string.Empty;

        if (field.Type == FieldType.String)
        {
            if (field.MaxLength > 0 && field.MaxLength <= 255)
            {
                maxLength = $"({field.MaxLength})";
            }
            else { maxLength = "(255)"; }
        }

        string isRequired = string.Empty;
        if (field.IsRequired) { isRequired = " NOT NULL"; }

        command.CommandType = CommandType.Text;

        command.CommandText = string.Format(
            "ALTER TABLE {0} ADD {1}",
            GetFullTableName(tableName, schemaName),
            string.Concat(QuoteIdentifier(field.Name), " ", fieldType, maxLength, isRequired));

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return true;
    }

    protected override FieldType GetDataMigratorFieldType(string providerFieldType) =>
        typeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<AccessDbType>(providerFieldType, true));

    protected override string GetDataProviderFieldType(FieldType fieldType) => typeConverter.GetDataProviderFieldType(fieldType).ToString();
}