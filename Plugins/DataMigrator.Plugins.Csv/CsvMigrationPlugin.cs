using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMigrator.Common;
using System.ComponentModel.Composition;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Csv
{
    [Export(typeof(IMigrationPlugin))]
    public class CsvMigrationPlugin : IMigrationPlugin
    {
        #region IMigrationPlugin Members

        public string ProviderName
        {
            get { return Constants.PROVIDER_NAME; }
        }

        public IConnectionControl ConnectionControl
        {
            get { return new CsvConnectionControl(); }
        }

        public BaseProvider GetDataProvider(ConnectionDetails connectionDetails)
        {
            return new CsvProvider(connectionDetails);
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