using System.Data;
using System.Data.Common;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataMigrator.Common.Data;
using DataMigrator.Common.Extensions;
using DataMigrator.Common.Models;
using Extenso.Collections;
using Extenso.Data;

namespace DataMigrator.Csv;

public class CsvProvider : BaseProvider
{
    public CsvProvider(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    public override string DbProviderName => throw new NotSupportedException();

    public override DbConnection CreateDbConnection() => null;

    public override Task<bool> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        var table = ReadCsv();
        fields.ForEach(field => table.Columns.Add(field.Name));
        table.ToCsv(ConnectionDetails.Database, true);
        return Task.FromResult(true);
    }

    public override async Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        var fieldNames = await GetFieldNamesAsync(tableName, schemaName);
        return new FieldCollection(fieldNames.Select((x, i) => new Field
        {
            Name = x,
            Ordinal = i,
            Type = FieldType.String
        }));

        //var table = ReadCsv();

        //table.Columns.Cast<DataColumn>().ForEach(c => fields.Add(new Field
        //{
        //    Name = c.ColumnName,
        //    DisplayName = c.ColumnName,
        //    IsRequired = !c.AllowDBNull,
        //    MaxLength = c.ColumnLength(),
        //    Ordinal = c.Ordinal,
        //    Type = FieldType.String
        //}));
    }

    public override int GetRecordCount(string tableName, string schemaName)
    {
        //int rowCount = new FileInfo(ConnectionDetails.ConnectionString).ReadAllText().ToLines().Count(); //Very inefficient!

        using var fileStream = new FileStream(ConnectionDetails.ConnectionString, FileMode.Open, FileAccess.Read);
        int rowCount = fileStream.CountLines();
        bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
        return hasHeaderRow ? rowCount - 1 : rowCount;
    }

    public override async IAsyncEnumerator<Record> GetRecordsEnumeratorAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
        using var fileSteam = File.OpenRead(ConnectionDetails.Database);
        using var streamReader = new StreamReader(fileSteam);
        using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
            HasHeaderRecord = hasHeaderRow
        });

        csvReader.Read();
        csvReader.ReadHeader();
        while (csvReader.Read())
        {
            var record = new Record();
            record.Fields.AddRange(fields);
            for (int i = 0; i < fields.Count(); i++)
            {
                var field = fields.ElementAt(i);

                string value = hasHeaderRow
                    ? csvReader.GetField(field.Name)
                    : csvReader.GetField(i);

                record[field.Name].Value = string.IsNullOrEmpty(value) ? null : value;
            }

            yield return await Task.FromResult(record);
        }
    }

    public override Task<IEnumerable<string>> GetTableNamesAsync() =>
        Task.FromResult(new string[] { Path.GetFileNameWithoutExtension(ConnectionDetails.Database) }.AsEnumerable());

    public override Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records)
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

    protected override Task<bool> CreateFieldAsync(string tableName, string schemaName, Field field)
    {
        var table = ReadCsv();
        table.Columns.Add(field.Name);
        table.ToCsv(ConnectionDetails.Database, true);
        return Task.FromResult(true);
    }

    protected override Task<bool> CreateTableAsync(string tableName, string schemaName) => throw new NotSupportedException();

    protected override Task CreateTableAsync(string tableName, string schemaName, string pkColumnName, string pkDataType, bool pkIsIdentity) =>
        throw new NotSupportedException();

    protected override FieldType GetDataMigratorFieldType(string providerFieldType) => FieldType.String;

    protected override string GetDataProviderFieldType(FieldType fieldType) => typeof(string).ToString();

    protected override async Task<IEnumerable<string>> GetFieldNamesAsync(string tableName, string schemaName)
    {
        using var fileStream = new FileStream(ConnectionDetails.ConnectionString, FileMode.Open, FileAccess.Read);
        using var streamReader = new StreamReader(fileStream);
        string line1 = await streamReader.ReadLineAsync();
        streamReader.Close();
        fileStream.Close();

        var fields = new Queue<string>(); // Use queue to make 100% sure the order stays the same.
        string[] items = line1.Split(',');

        bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
        for (int i = 0; i < items.Length; i++)
        {
            fields.Enqueue(hasHeaderRow ? items[i] : $"Column_{i + 1}");
        }

        return fields;
    }

    protected override string GetFullTableName(string tableName, string schemaName) => ConnectionDetails.Database;

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
        csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add(string.Empty);
        using var csvDataReader = new CsvDataReader(csvReader);

        var table = new DataTable();
        table.Load(csvDataReader);
        return table;
    }
}