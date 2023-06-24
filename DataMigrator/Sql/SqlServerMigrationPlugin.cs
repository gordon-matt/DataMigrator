using System.ComponentModel.Composition;

namespace DataMigrator.Sql
{
    [Export(typeof(IMigrationPlugin))]
    public class SqlServerMigrationPlugin : IMigrationPlugin
    {
        #region IMigrationPlugin Members

        public string ProviderName => Constants.SQL_PROVIDER_NAME;

        public IConnectionControl ConnectionControl => new SqlConnectionControl();

        public IProvider GetDataProvider(ConnectionDetails connectionDetails)
        {
            return new SqlProvider(connectionDetails);
        }

        public ISettingsControl SettingsControl => null;

        public IEnumerable<IMigrationTool> Tools => null;

        #endregion IMigrationPlugin Members
    }
}