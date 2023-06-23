using DataMigrator.Common.Models;

namespace DataMigrator.Common.Data
{
    public interface IProvider
    {
        string DbProviderName { get; }

        IEnumerable<string> TableNames { get; }

        bool CreateTable(string tableName, string schemaName, IEnumerable<Field> fields);

        FieldCollection GetFields(string tableName, string schemaName);

        int GetRecordCount(string tableName, string schemaName);

        IEnumerator<Record> GetRecordsEnumerator(string tableName, string schemaName, IEnumerable<Field> fields);

        void InsertRecords(string tableName, string schemaName, IEnumerable<Record> records);
    }
}