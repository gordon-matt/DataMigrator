namespace DataMigrator.Common;

public interface IMigrationTool
{
    string Name { get; }

    string Description { get; }

    Icon Icon { get; }

    UserControl ControlContent { get; }
}