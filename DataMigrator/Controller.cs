using System.Linq.Expressions;
using Newtonsoft.Json;

namespace DataMigrator;

public static class Controller
{
    public static IMigrationPlugin GetPlugin(string providerName) =>
        Program.Plugins.SingleOrDefault(p => p.ProviderName == providerName);

    public static IProvider GetProvider(ConnectionDetails connection) => GetPlugin(connection.ProviderName).GetDataProvider(connection);

    public static async Task RunJobAsync(Job job, IProgress<int> progress, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(job.SourceTable))
        {
            TraceService.Instance.WriteConcat(TraceEvent.Error, job.Name, " does not have a source table specified!");
            throw new ArgumentException("You must specify a source table!");
        }
        if (string.IsNullOrEmpty(job.DestinationTable))
        {
            TraceService.Instance.WriteConcat(TraceEvent.Error, job.Name, " does not have a destination table specified!");
            throw new ArgumentException("You must specify a destination table!");
        }
        if (job.FieldMappings.Count == 0)
        {
            TraceService.Instance.WriteConcat(TraceEvent.Error, job.Name, " does not have any mapped fields!");
            throw new ArgumentException("You must have at least one Field Mapped!");
        }

        var sourceProvider = GetProvider(Program.Configuration.SourceConnection);
        var destinationProvider = GetProvider(Program.Configuration.DestinationConnection);

        var sourceFields = job.FieldMappings.Select(f => f.SourceField);
        var destinationFields = job.FieldMappings.Select(f => f.DestinationField);

        string sourceSchema = job.SourceTable.Contains('.') ? job.SourceTable.LeftOf('.') : string.Empty;
        string sourceTable = job.SourceTable.Contains('.') ? job.SourceTable.RightOf('.') : job.SourceTable;

        int recordCount = sourceProvider.GetRecordCount(sourceTable, sourceSchema);

        var buffer = new RecordCollection();
        int processedRecordCount = 0;
        var recordsEnumerator = sourceProvider.GetRecordsEnumeratorAsync(sourceTable, sourceSchema, sourceFields);

        using var connection = destinationProvider.CreateDbConnection();
        while (await recordsEnumerator.MoveNextAsync())
        {
            var record = recordsEnumerator.Current.Clone();

            try
            {
                buffer.Add(record);
                processedRecordCount++;

                if (processedRecordCount.IsMultipleOf(Program.Configuration.BatchSize) || processedRecordCount == recordCount)
                {
                    if (cancellationToken.IsCancellationRequested)
                    { return; }

                    buffer.ReMapFields(job.FieldMappings);

                    // TODO: Apply Transform Functions
                    //  Create an ITransformerPlugin and let users assign them to columns
                    //  Then run them here on each record in `buffer`.

                    string destinationSchema = job.DestinationTable.Contains('.') ? job.DestinationTable.LeftOf('.') : string.Empty;
                    string destinationTable = job.DestinationTable.Contains('.') ? job.DestinationTable.RightOf('.') : job.DestinationTable;

                    await destinationProvider.InsertRecordsAsync(connection, destinationTable, destinationSchema, buffer);
                    buffer = new RecordCollection();

                    double percent = processedRecordCount / (double)recordCount;
                    percent *= 100;
                    progress.Report((int)percent);
                    TraceService.Instance.WriteFormat(TraceEvent.Information, "{0}/{1} Records Processed", processedRecordCount, recordCount);

                    if (cancellationToken.IsCancellationRequested)
                    { return; }
                }
            }
            catch
            {
                string recordJson = record.JsonSerialize(new JsonSerializerSettings { Formatting = Formatting.Indented });
                TraceService.Instance.WriteMessage(TraceEvent.Error, $"Error when attempting to process record #{processedRecordCount} with the following data:{Environment.NewLine}{recordJson}");
                throw;
            }
        }
    }

    public static async Task<bool> CreateDestinationTableAsync(string tableName)
    {
        var sourceProvider = GetProvider(Program.Configuration.SourceConnection);
        var destinationProvider = GetProvider(Program.Configuration.DestinationConnection);

        string schemaName = tableName.Contains('.') ? tableName.LeftOf('.') : string.Empty;
        string tableNameWithoutSchema = tableName.Contains('.') ? tableName.RightOf('.') : tableName;

        return await destinationProvider.CreateTableAsync(
            tableNameWithoutSchema,
            schemaName,
            await sourceProvider.GetFieldsAsync(tableNameWithoutSchema, schemaName));
    }
}