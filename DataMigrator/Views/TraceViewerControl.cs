namespace DataMigrator.Views;

public partial class TraceViewerControl : UserControl
{
    public TraceViewerControl()
    {
        InitializeComponent();

        TraceService.Trace += Instance_Trace;
    }

    private void Instance_Trace(TraceEventArgs e)
    {
        if (txtTrace.InvokeRequired)
        {
            Invoke(new MethodInvoker(() =>
            {
                txtTrace.AppendText(e.Message);
                txtTrace.AppendText(Environment.NewLine);
            }));
        }
        else
        {
            txtTrace.AppendText(e.Message);
            txtTrace.AppendText(Environment.NewLine);
        }
        Application.DoEvents();
    }
}