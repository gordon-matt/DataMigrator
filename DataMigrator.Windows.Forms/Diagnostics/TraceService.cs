namespace DataMigrator.Windows.Forms.Diagnostics;

public sealed class TraceService
{
    public delegate void TraceEventHandler(TraceEventArgs e);

    public static event TraceEventHandler Trace;

    public static TraceService Instance => new();

    public void WriteConcat(TraceEvent traceEvent, params object[] args) => WriteMessage(traceEvent, string.Concat(args));

    public void WriteException(Exception x) => WriteException(x, string.Empty);

    public void WriteException(Exception x, string additionalInfo)
    {
        string errorMessage = string.Concat(
            "ERROR:",
            Environment.NewLine,
            "MACHINE NAME: ", Environment.MachineName,
            Environment.NewLine,
            "TARGET SITE: ", x.TargetSite,
            Environment.NewLine,
            "EXCEPTION: ", x.Message,
            Environment.NewLine,
            "INNER EXCEPTION: ", x.InnerException == null ? string.Empty : x.InnerException.ToString(),
            Environment.NewLine,
            "STACK TRACE: ", x.StackTrace,
            Environment.NewLine,
            Environment.NewLine,
            "ADDITIONAL INFO: ",
            Environment.NewLine,
            additionalInfo);

        WriteMessage(errorMessage);
    }

    public void WriteFormat(TraceEvent traceEvent, string format, params object[] args) => WriteMessage(traceEvent, string.Format(format, args));

    public void WriteMessage(string message) => Trace?.Invoke(new TraceEventArgs(message));

    public void WriteMessage(TraceEvent traceEvent, string message) => WriteMessage($"{traceEvent.ToString().ToUpperInvariant()}: {message}");
}