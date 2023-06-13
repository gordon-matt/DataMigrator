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

    public static BaseProvider GetProvider(ConnectionDetails connection)
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

        int recordCount = sourceProvider.GetRecordCount(job.SourceTable);

        var buffer = new RecordCollection();
        int processedRecordCount = 0;
        var recordsEnumerator = sourceProvider.GetRecordsEnumerator(job.SourceTable, sourceFields);

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

                destinationProvider.InsertRecords(job.DestinationTable, buffer);
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

        return destinationProvider.CreateTable(tableName, sourceProvider.GetFields(tableName));
    }
}