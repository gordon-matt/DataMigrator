using System.Data.Common;

namespace DataMigrator.Common.Data
{
    public interface IProvider
    {
        string DbProviderName { get; }

        Task<IEnumerable<string>> GetTableNamesAsync();

        Task<bool> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields);

        Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName);

        int GetRecordCount(string tableName, string schemaName);

        IAsyncEnumerator<Record> GetRecordsEnumeratorAsync(string tableName, string schemaName, IEnumerable<Field> fields);

        Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records);

        DbConnection CreateDbConnection();
    }
}