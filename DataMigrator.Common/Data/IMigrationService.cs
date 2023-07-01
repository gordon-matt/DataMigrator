using System.Data.Common;

namespace DataMigrator.Common.Data
{
    // TODO: Think about this a little more..
    // On one hand, we want to pass the DbConnection in to reuse it for performance reasons..
    // On the other hand, it's a code smell to expect non-ADO.NET implementations to implement CreateDbConnection()
    //  and have a DbConnection in InsertRecordsAsync()..
    // Even schemaName is not really appropriate for cases like delimited files or SharePoint..

    public interface IMigrationService
    {
        DbConnection CreateDbConnection();

        Task<IEnumerable<string>> GetTableNamesAsync();

        Task<string> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields);

        Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName);

        int CountRecords(string tableName, string schemaName);

        IAsyncEnumerable<Record> GetRecordsAsync(string tableName, string schemaName, IEnumerable<Field> fields);

        Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records);
    }
}