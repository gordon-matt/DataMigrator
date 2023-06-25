namespace DataMigrator.Common;

public interface IMigrationPlugin
{
    string ProviderName { get; }

    IConnectionControl ConnectionControl { get; }

    IMigrationService GetDataProvider(ConnectionDetails connectionDetails);

    ISettingsControl SettingsControl { get; }

    IEnumerable<IMigrationTool> Tools { get; }
}