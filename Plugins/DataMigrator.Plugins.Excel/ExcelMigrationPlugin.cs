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

        public string ProviderName => Constants.PROVIDER_NAME;

        public IConnectionControl ConnectionControl => new ExcelConnectionControl();

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails) => new ExcelProvider(connectionDetails);

        public ISettingsControl SettingsControl => null;

        public IEnumerable<IMigrationTool> Tools => null;

        #endregion IMigrationPlugin Members
    }
}