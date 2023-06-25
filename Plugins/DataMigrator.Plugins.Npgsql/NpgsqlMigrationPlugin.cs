using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Plugins.Npgsql;

[Export(typeof(IMigrationPlugin))]
public class NpgsqlMigrationPlugin : IMigrationPlugin
{
    #region IMigrationPlugin Members

    public string ProviderName => Constants.PROVIDER_NAME;

    public IConnectionControl ConnectionControl => new NpgsqlConnectionControl();

    public IMigrationService GetDataProvider(ConnectionDetails connectionDetails) => new NpgsqlMigrationService(connectionDetails);

    public ISettingsControl SettingsControl => null;

    public IEnumerable<IMigrationTool> Tools => null;

    #endregion IMigrationPlugin Members
}