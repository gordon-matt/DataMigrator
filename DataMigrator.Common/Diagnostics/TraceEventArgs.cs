namespace DataMigrator.Common.Diagnostics;

public sealed class TraceEventArgs : EventArgs
{
    public TraceEventArgs(string message)
        : base()
    {
        Message = message;
    }

    public string Message { get; private set; }
}