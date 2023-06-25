using System.ComponentModel.Composition;

namespace DataMigrator.Sql
{
    [Export(typeof(IMigrationPlugin))]
    public class SqlServerMigrationPlugin : IMigrationPlugin
    {
        #region IMigrationPlugin Members

        public string ProviderName => Constants.SQL_PROVIDER_NAME;

        public IConnectionControl ConnectionControl => new SqlConnectionControl();

        public IMigrationService GetDataProvider(ConnectionDetails connectionDetails)
        {
            return new SqlMigrationService(connectionDetails);
        }

        public ISettingsControl SettingsControl => null;

        public IEnumerable<IMigrationTool> Tools => null;

        #endregion IMigrationPlugin Members
    }
}