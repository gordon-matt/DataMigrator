﻿using Autofac;
using Newtonsoft.Json;

namespace DataMigrator;

public static class Controller
{
    public static IMigrationPlugin GetPlugin(string providerName) =>
        Program.Plugins.SingleOrDefault(p => p.ProviderName == providerName);

    public static IMigrationService GetProvider(ConnectionDetails connection) => GetPlugin(connection.ProviderName).GetDataProvider(connection);

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

        var sourceProvider = GetProvider(AppState.ConfigFile.SourceConnection);
        var destinationProvider = GetProvider(AppState.ConfigFile.DestinationConnection);

        var sourceFields = job.FieldMappings.Select(f => f.SourceField);
        var destinationFields = job.FieldMappings.Select(f => f.DestinationField);

        string sourceSchema = job.SourceTable.Contains('.') ? job.SourceTable.LeftOf('.') : string.Empty;
        string sourceTable = job.SourceTable.Contains('.') ? job.SourceTable.RightOf('.') : job.SourceTable;

        int recordCount = sourceProvider.CountRecords(sourceTable, sourceSchema);

        var buffer = new RecordCollection();
        int processedRecordCount = 0;
        var records = sourceProvider.GetRecordsAsync(sourceTable, sourceSchema, sourceFields);

        using var connection = destinationProvider.CreateDbConnection();
        await foreach (var record in records)
        {
            var clone = record.Clone();

            try
            {
                buffer.Add(clone);
                processedRecordCount++;

                if (processedRecordCount.IsMultipleOf(AppState.ConfigFile.BatchSize) || processedRecordCount == recordCount)
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
                string recordJson = clone.JsonSerialize(new JsonSerializerSettings { Formatting = Formatting.Indented });
                TraceService.Instance.WriteMessage(TraceEvent.Error, $"Error when attempting to process record #{processedRecordCount} with the following data:{Environment.NewLine}{recordJson}");
                throw;
            }
        }
    }

    public static async Task<string> CreateDestinationTableAsync(string tableName)
    {
        var sourceProvider = GetProvider(AppState.ConfigFile.SourceConnection);
        var destinationProvider = GetProvider(AppState.ConfigFile.DestinationConnection);

        string schemaName = tableName.Contains('.') ? tableName.LeftOf('.') : string.Empty;
        string tableNameWithoutSchema = tableName.Contains('.') ? tableName.RightOf('.') : tableName;

        return await destinationProvider.CreateTableAsync(
            tableNameWithoutSchema,
            schemaName,
            await sourceProvider.GetFieldsAsync(tableNameWithoutSchema, schemaName));
    }
}