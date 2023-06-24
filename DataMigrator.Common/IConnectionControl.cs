namespace DataMigrator.Common;

public interface IConnectionControl : IDisposable
{
    ConnectionDetails ConnectionDetails { get; set; }

    UserControl ControlContent { get; }

    bool ValidateConnection();
}