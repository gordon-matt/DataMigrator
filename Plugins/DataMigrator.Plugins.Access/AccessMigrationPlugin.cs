using System.Collections.Generic;
using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Access
{
    [Export(typeof(IMigrationPlugin))]
    public class AccessMigrationPlugin : IMigrationPlugin
    {
        #region IMigrationPlugin Members

        public string ProviderName
        {
            get { return Constants.PROVIDER_NAME; }
        }

        public IConnectionControl ConnectionControl
        {
            get { return new AccessConnectionControl(); }
        }

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails)
        {
            return new AccessProvider(connectionDetails);
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
