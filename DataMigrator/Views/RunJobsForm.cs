using System.Data;
using DataMigrator.Windows.Forms.Diagnostics;
using Krypton.Toolkit;

namespace DataMigrator.Views;

public partial class RunJobsForm : KryptonForm
{
    private const string COLUMN_RUN = "Run";
    private const string COLUMN_NAME = "Name";
    private const string COLUMN_STATUS = "Status";

    private CancellationTokenSource cancellationTokenSource;

    public RunJobsForm()
    {
        InitializeComponent();

        var table = new DataTable("Jobs");
        table.Columns.Add(COLUMN_RUN, typeof(bool));
        table.Columns.Add(COLUMN_NAME);
        table.Columns.Add(COLUMN_STATUS);
        foreach (var job in Program.Configuration.Jobs)
        {
            var row = table.NewRow();
            row[COLUMN_RUN] = false;
            row[COLUMN_NAME] = job.Name;
            row[COLUMN_STATUS] = "Pending";
            table.Rows.Add(row);
        }
        dataGridView.DataSource = table;
    }

    private async void btnRun_Click(object sender, EventArgs e)
    {
        btnRun.Enabled = false;
        btnCancel.Enabled = true;

        progressBar.Value = 0;
        await DoWorkAsync();

        TraceService.Instance.WriteMessage(TraceEvent.Information, "Completed");
        progressBar.Value = 0;
        btnRun.Enabled = true;
        btnCancel.Enabled = false;

        cancellationTokenSource?.Dispose();
    }

    private async Task DoWorkAsync()
    {
        cancellationTokenSource = new CancellationTokenSource();

        var progressHandler = new Progress<int>(value =>
        {
            progressBar.BeginInvoke(
                (MethodInvoker)delegate ()
                {
                    progressBar.Value = value;
                    progressBar.Refresh();
                });
        });

        foreach (DataRow row in ((DataTable)dataGridView.DataSource).Rows)
        {
            if (bool.Parse(row[COLUMN_RUN].ToString()))
            {
                string jobName = row[COLUMN_NAME].ToString();
                var job = Program.Configuration.Jobs[jobName];

                if (job == null)
                {
                    TraceService.Instance.WriteConcat(TraceEvent.Warning, "Could not find job, '", jobName, "'");
                    continue;
                }

                try
                {
                    row[COLUMN_STATUS] = "Running";

                    await Controller.RunJobAsync(job, progressHandler, cancellationTokenSource.Token);

                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        row[COLUMN_STATUS] = "Cancelled";
                        TraceService.Instance.WriteConcat(TraceEvent.Information, "User cancelled job");
                        return;
                    }
                    else
                    {
                        row[COLUMN_STATUS] = "Completed";
                    }
                }
                catch (Exception x)
                {
                    row[COLUMN_STATUS] = "Error";
                    TraceService.Instance.WriteException(x, string.Concat("Job Name: ", jobName));
                }
            }
        }
    }

    private void btnCancel_Click(object sender, EventArgs e) => cancellationTokenSource.Cancel();
}