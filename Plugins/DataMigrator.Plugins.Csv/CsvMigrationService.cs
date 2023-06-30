using System.Data;
using System.Data.Common;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Extensions;
using DataMigrator.Common.Models;
using DataMigrator.Plugins.Csv;
using Extenso.Collections;
using Extenso.Data;
using Extenso.IO;

namespace DataMigrator.Csv;

public class CsvMigrationService : IMigrationService
{
    public CsvMigrationService(ConnectionDetails connectionDetails)
    {
        ConnectionDetails = connectionDetails;
    }

    private ConnectionDetails ConnectionDetails { get; set; }

    #region IMigrationService Members

    public DbConnection CreateDbConnection() => null;

    public Task<IEnumerable<string>> GetTableNamesAsync() =>
        Task.FromResult(new string[] { Path.GetFileNameWithoutExtension(ConnectionDetails.Database) }.AsEnumerable());

    public Task<bool> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        var table = new DataTable();
        fields.ForEach(field => table.Columns.Add(field.Name));
        table.ToCsv(ConnectionDetails.Database, true);
        return Task.FromResult(true);
    }

    public async Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        if (IsLargeFile)
        {
            // Fast, but don't get the max column lengths..
            var fieldNames = await GetFieldNamesAsync();
            return new FieldCollection(fieldNames.Select((x, i) => new Field
            {
                Name = x,
                Ordinal = i,
                Type = FieldType.String
            }));
        }

        // Preferred, for smaller files
        return new FieldCollection(ReadCsv().Columns.OfType<DataColumn>().Select(x => new Field
        {
            Name = x.ColumnName,
            DisplayName = x.ColumnName,
            IsRequired = !x.AllowDBNull,
            MaxLength = x.ColumnLength(),
            Ordinal = x.Ordinal,
            Type = FieldType.String
        }));
    }

    public int CountRecords(string tableName, string schemaName)
    {
        //int rowCount = new FileInfo(ConnectionDetails.ConnectionString).ReadAllText().ToLines().Count(); //Very inefficient!

        using var fileStream = new FileStream(ConnectionDetails.ConnectionString, FileMode.Open, FileAccess.Read);
        int rowCount = fileStream.CountLines();
        return HasHeaderRow ? rowCount - 1 : rowCount;
    }

    public async IAsyncEnumerable<Record> GetRecordsAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        using var fileSteam = File.OpenRead(ConnectionDetails.Database);
        using var streamReader = new StreamReader(fileSteam);
        using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = AppState.ConfigFile.TrimStrings ? TrimOptions.Trim : TrimOptions.None,
            HasHeaderRecord = HasHeaderRow,
            Delimiter = Delimiter
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

                string value = HasHeaderRow
                    ? csvReader.GetField(field.Name)
                    : csvReader.GetField(i);

                record[field.Name].Value = string.IsNullOrEmpty(value) ? null : value;
            }

            yield return await Task.FromResult(record);
        }
    }

    public Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records)
    {
        if (IsLargeFile)
        {
            using var fileStream = new FileStream(ConnectionDetails.Database, FileMode.Append, FileAccess.Write);
            using var streamWriter = new StreamWriter(fileStream);
            foreach (var record in records)
            {
                string line = string.Join($"\"{Delimiter}\"", record.Fields.OrderBy(x => x.Ordinal).Select(x => x.Value));
                streamWriter.WriteLine($"\"{line}\"");
            }
        }

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

    #endregion IMigrationService Members

    private async Task<IEnumerable<string>> GetFieldNamesAsync()
    {
        using var fileStream = new FileStream(ConnectionDetails.ConnectionString, FileMode.Open, FileAccess.Read);
        using var streamReader = new StreamReader(fileStream);
        string line1 = await streamReader.ReadLineAsync();
        streamReader.Close();
        fileStream.Close();

        var fields = new Queue<string>(); // Use queue to make 100% sure the order stays the same.
        string[] items = line1.Split(',');

        for (int i = 0; i < items.Length; i++)
        {
            fields.Enqueue(HasHeaderRow ? items[i] : $"Column_{i + 1}");
        }

        return fields;
    }

    private DataTable ReadCsv()
    {
        using var fileStream = File.OpenRead(ConnectionDetails.Database);
        using var streamReader = new StreamReader(fileStream);
        using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = AppState.ConfigFile.TrimStrings ? TrimOptions.Trim : TrimOptions.None,
            HasHeaderRecord = HasHeaderRow,
            Delimiter = Delimiter
        });
        csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add(string.Empty);
        using var csvDataReader = new CsvDataReader(csvReader);

        var table = new DataTable();
        table.Load(csvDataReader);
        return table;
    }

    private string Delimiter => (FileDelimiter)ConnectionDetails.ExtendedProperties["Delimiter"].GetValue<byte>() switch
    {
        FileDelimiter.Tab => "\t",
        FileDelimiter.VerticalBar => "|",
        FileDelimiter.Semicolon => ";",
        _ => ",",
    };

    private bool HasHeaderRow => ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();

    private bool IsLargeFile => new FileInfo(ConnectionDetails.Database).FileSizeInMegaBytes() >= 100;
}