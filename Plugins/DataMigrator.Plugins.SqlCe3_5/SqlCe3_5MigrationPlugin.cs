using System.Collections.Generic;
using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.SqlCe3_5
{
    [Export(typeof(IMigrationPlugin))]
    public class SqlCe3_5MigrationPlugin : IMigrationPlugin
    {
        #region IMigrationPlugin Members

        public string ProviderName => Constants.PROVIDER_NAME;

        public IConnectionControl ConnectionControl => new SqlCe3_5ConnectionControl();

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails) => new SqlCe3_5Provider(connectionDetails);

        public ISettingsControl SettingsControl => null;

        public IEnumerable<IMigrationTool> Tools => null;

        #endregion IMigrationPlugin Members
    }
}