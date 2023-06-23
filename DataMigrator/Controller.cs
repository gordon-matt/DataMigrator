using System.ComponentModel;
using DataMigrator.Common;
using DataMigrator.Common.Configuration;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso;

namespace DataMigrator;

public static class Controller
{
    public static IConnectionControl GetConnectionControl(string providerName)
    {
        return Program.Plugins.SingleOrDefault(p => p.ProviderName == providerName).ConnectionControl;
    }

    public static IProvider GetProvider(ConnectionDetails connection)
    {
        return Program.Plugins.SingleOrDefault(p => p.ProviderName == connection.ProviderName).GetDataProvider(connection);
    }

    public static void RunJob(ref BackgroundWorker backgroundWorker, Job job)
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
        var recordsEnumerator = sourceProvider.GetRecordsEnumerator(sourceTable, sourceSchema, sourceFields);

        while (recordsEnumerator.MoveNext())
        {
            var record = recordsEnumerator.Current.Clone();

            buffer.Add(record);
            processedRecordCount++;

            if (processedRecordCount.IsMultipleOf(Program.Configuration.BatchSize) || processedRecordCount == recordCount)
            {
                if (backgroundWorker.CancellationPending)
                { return; }

                buffer.ReMapFields(job.FieldMappings);

                // TODO: Apply Transform Functions
                //  Create an ITransformerPlugin and let users assign them to columns
                //  Then run them here on each record in `buffer`.

                string destinationSchema = job.DestinationTable.Contains('.') ? job.DestinationTable.LeftOf('.') : string.Empty;
                string destinationTable = job.DestinationTable.Contains('.') ? job.DestinationTable.RightOf('.') : job.DestinationTable;

                destinationProvider.InsertRecords(destinationTable, destinationSchema, buffer);
                buffer = new RecordCollection();

                double percent = processedRecordCount / (double)recordCount;
                percent *= 100;
                backgroundWorker.ReportProgress((int)percent);
                TraceService.Instance.WriteFormat(TraceEvent.Information, "{0}/{1} Records Processed", processedRecordCount, recordCount);

                if (backgroundWorker.CancellationPending)
                { return; }
            }
        }
    }

    public static bool CreateDestinationTable(string tableName)
    {
        var sourceProvider = GetProvider(Program.Configuration.SourceConnection);
        var destinationProvider = GetProvider(Program.Configuration.DestinationConnection);

        string schemaName = tableName.Contains('.') ? tableName.LeftOf('.') : string.Empty;
        string tableNameWithoutSchema = tableName.Contains('.') ? tableName.RightOf('.') : tableName;

        return destinationProvider.CreateTable(tableNameWithoutSchema, schemaName, sourceProvider.GetFields(tableNameWithoutSchema, schemaName));
    }
}