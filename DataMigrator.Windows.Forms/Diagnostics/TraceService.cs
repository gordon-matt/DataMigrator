namespace DataMigrator.Windows.Forms.Diagnostics;

public enum TraceEvent
{
    Error,
    Information,
    Debug,
    Warning
}

public sealed class TraceService
{
    public delegate void TraceEventHandler(TraceEventArgs e);

    public event TraceEventHandler Trace;

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

    public void WriteMessage(TraceEvent traceEvent, string message) => WriteMessage(string.Concat(traceEvent.ToString().ToUpperInvariant(), ": ", message));
}

public sealed class TraceEventArgs : EventArgs
{
    public TraceEventArgs(string message)
        : base()
    {
        Message = message;
    }

    public string Message { get; private set; }
}