namespace DataMigrator.Common;

public interface ISettingsControl
{
    UserControl ControlContent { get; }

    void Save();
}