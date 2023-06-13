using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.MySql;

[Export(typeof(IMigrationPlugin))]
public class MySqlMigrationPlugin : IMigrationPlugin
{
    #region IMigrationPlugin Members

    public string ProviderName => Constants.PROVIDER_NAME;

    public IConnectionControl ConnectionControl => new MySqlConnectionControl();

    public BaseProvider GetDataProvider(ConnectionDetails connectionDetails)
    {
        return new MySqlProvider(connectionDetails);
    }

    public ISettingsControl SettingsControl => null;

    public IEnumerable<IMigrationTool> Tools => null;

    #endregion IMigrationPlugin Members
}