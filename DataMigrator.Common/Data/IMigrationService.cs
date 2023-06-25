using System.Data.Common;

namespace DataMigrator.Common.Data
{
    public interface IMigrationService
    {
        string DbProviderName { get; }

        DbConnection CreateDbConnection();

        Task<IEnumerable<string>> GetTableNamesAsync();

        Task<bool> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields);

        Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName);

        int CountRecords(string tableName, string schemaName);

        IAsyncEnumerable<Record> GetRecordsAsync(string tableName, string schemaName, IEnumerable<Field> fields);

        Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records);
    }
}