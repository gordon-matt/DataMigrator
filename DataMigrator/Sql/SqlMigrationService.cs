using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using AppContext = DataMigrator.Common.AppContext;

namespace DataMigrator.Sql;

public class SqlMigrationService : BaseMigrationService
{
    public SqlMigrationService(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    static SqlMigrationService()
    {
        DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
    }

    #region IMigrationService Members

    public override string DbProviderName => "Microsoft.Data.SqlClient";

    public override async Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records)
    {
        await connection.OpenAsync();
        var table = records.ToDataTable();

        using var bulkCopy = new SqlBulkCopy(connection as SqlConnection);
        bulkCopy.DestinationTableName = GetFullTableName(tableName, schemaName);

        foreach (DataColumn column in table.Columns)
        {
            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
        }

        await bulkCopy.WriteToServerAsync(table);
        await connection.CloseAsync();
    }

    #endregion IMigrationService Members

    #region Field Conversion

    protected override FieldType GetDataMigratorFieldType(string providerFieldType) =>
        AppContext.SqlDbTypeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<SqlDbType>(providerFieldType, true));

    protected override string GetDataProviderFieldType(FieldType fieldType) =>
        AppContext.SqlDbTypeConverter.GetDataProviderFieldType(fieldType).ToString();

    #endregion Field Conversion
}