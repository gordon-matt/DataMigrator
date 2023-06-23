using System.Data;
using System.Data.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso;
using Extenso.Collections;
using Microsoft.Data.SqlClient;
using AppContext = DataMigrator.Common.AppContext;

namespace DataMigrator.Sql;

public class SqlProvider : BaseProvider
{
    public override string DbProviderName => "Microsoft.Data.SqlClient";

    public SqlProvider(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    static SqlProvider()
    {
        DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
    }

    protected override FieldType GetDataMigratorFieldType(string providerFieldType) =>
        AppContext.SqlDbTypeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<SqlDbType>(providerFieldType, true));

    protected override string GetDataProviderFieldType(FieldType fieldType) =>
        AppContext.SqlDbTypeConverter.GetDataProviderFieldType(fieldType).ToString();

    public override async Task InsertRecordsAsync(string tableName, string schemaName, IEnumerable<Record> records)
    {
        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
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
}