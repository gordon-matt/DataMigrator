using System.Collections.Generic;
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

        public string ProviderName
        {
            get { return Constants.SQL_PROVIDER_NAME; }
        }

        public IConnectionControl ConnectionControl
        {
            get { return new SqlConnectionControl(); }
        }

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails)
        {
            return new SqlProvider(connectionDetails);
        }

        public ISettingsControl SettingsControl
        {
            get { return null; }
        }

        public IEnumerable<IMigrationTool> Tools
        {
            get { return null; }
        }

        #endregion
    }
}