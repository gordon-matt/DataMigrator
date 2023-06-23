using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Common;

public interface IMigrationPlugin
{
    string ProviderName { get; }

    IConnectionControl ConnectionControl { get; }

    IProvider GetDataProvider(ConnectionDetails connectionDetails);

    ISettingsControl SettingsControl { get; }

    IEnumerable<IMigrationTool> Tools { get; }
}