using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Sql
{
    [Export(typeof(IMigrationPlugin))]
    public class SqlServerMigrationPlugin : IMigrationPlugin
    {
        #region IMigrationPlugin Members

        public string ProviderName => Constants.SQL_PROVIDER_NAME;

        public IConnectionControl ConnectionControl => new SqlConnectionControl();

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails) => new SqlProvider(connectionDetails);

        public ISettingsControl SettingsControl => null;

        public IEnumerable<IMigrationTool> Tools => null;

        #endregion IMigrationPlugin Members
    }
}