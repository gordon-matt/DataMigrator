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

        public string ProviderName => Constants.PROVIDER_NAME;

        public IConnectionControl ConnectionControl => new AccessConnectionControl();

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails) => new AccessProvider(connectionDetails);

        public ISettingsControl SettingsControl => null;

        public IEnumerable<IMigrationTool> Tools => null;

        #endregion IMigrationPlugin Members
    }
}