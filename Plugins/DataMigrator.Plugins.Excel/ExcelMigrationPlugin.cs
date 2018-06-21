using System.Collections.Generic;
using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Excel
{
    [Export(typeof(IMigrationPlugin))]
    public class ExcelMigrationPlugin : IMigrationPlugin
    {
        #region IMigrationPlugin Members

        public string ProviderName
        {
            get { return Constants.PROVIDER_NAME; }
        }

        public IConnectionControl ConnectionControl
        {
            get { return new ExcelConnectionControl(); }
        }

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails)
        {
            return new ExcelProvider(connectionDetails);
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