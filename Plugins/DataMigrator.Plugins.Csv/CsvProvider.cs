using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso;
using Extenso.Collections;
using Extenso.Data;
using Extenso.IO;

namespace DataMigrator.Csv;

public class CsvProvider : BaseProvider
{
    public override string DbProviderName => throw new NotSupportedException();

    public CsvProvider(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    protected override Task<bool> CreateFieldAsync(string tableName, string schemaName, Field field)
    {
        var table = ReadCsv();
        table.Columns.Add(field.Name);
        table.ToCsv(ConnectionDetails.Database, true);
        return Task.FromResult(true);
    }

    protected override Task<bool> CreateTableAsync(string tableName, string schemaName) => throw new NotSupportedException();

    public override Task<bool> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        var table = ReadCsv();
        fields.ForEach(field => table.Columns.Add(field.Name));
        table.ToCsv(ConnectionDetails.Database, true);
        return Task.FromResult(true);
    }

    protected override Task CreateTableAsync(string tableName, string schemaName, string pkColumnName, string pkDataType, bool pkIsIdentity) =>
        throw new NotSupportedException();

    protected override Task<IEnumerable<string>> GetFieldNamesAsync(string tableName, string schemaName) =>
        Task.FromResult(ReadCsv().Columns.Cast<DataColumn>().Select(c => c.ColumnName));

    public override Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        var table = ReadCsv();
        var fields = new FieldCollection();

        table.Columns.Cast<DataColumn>().ForEach(c => fields.Add(new Field
        {
            Name = c.ColumnName,
            DisplayName = c.ColumnName,
            IsRequired = !c.AllowDBNull,
            MaxLength = c.ColumnLength(),
            Ordinal = c.Ordinal,
            Type = FieldType.String
        }));

        return Task.FromResult(fields);
    }

    public override int GetRecordCount(string tableName, string schemaName)
    {
        int rowCount = new FileInfo(ConnectionDetails.ConnectionString).ReadAllText().ToLines().Count();
        bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
        return hasHeaderRow ? rowCount - 1 : rowCount;
    }

    public override async IAsyncEnumerator<Record> GetRecordsEnumeratorAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        var table = ReadCsv();
        foreach (DataRow row in table.Rows)
        {
            var record = new Record();
            record.Fields.AddRange(fields);
            fields.ForEach(f =>
            {
                record[f.Name].Value = row.Field<string>(f.Name);
            });
            yield return await Task.FromResult(record);
        }
    }

    public override Task InsertRecordsAsync(string tableName, string schemaName, IEnumerable<Record> records)
    {
        var table = ReadCsv();

        records.ForEach(record =>
        {
            var row = table.NewRow();
            record.Fields.ForEach(field =>
            {
                row[field.Name] = field.Value.ToString();
            });
            table.Rows.Add(row);
        });

        table.ToCsv(ConnectionDetails.Database, true);
        return Task.CompletedTask;
    }

    public override Task<IEnumerable<string>> GetTableNamesAsync() =>
        Task.FromResult(new string[] { Path.GetFileNameWithoutExtension(ConnectionDetails.Database) }.AsEnumerable());

    protected override FieldType GetDataMigratorFieldType(string providerFieldType) => FieldType.String;

    protected override string GetDataProviderFieldType(FieldType fieldType) => typeof(string).ToString();

    private DataTable ReadCsv()
    {
        bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
        using var fileStream = File.OpenRead(ConnectionDetails.Database);
        using var streamReader = new StreamReader(fileStream);
        using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
            HasHeaderRecord = hasHeaderRow
        });
        using var csvDataReader = new CsvDataReader(csvReader);

        var table = new DataTable();
        table.Load(csvDataReader);
        return table;
    }
}