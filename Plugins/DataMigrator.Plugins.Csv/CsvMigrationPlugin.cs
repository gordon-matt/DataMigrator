using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Csv;

[Export(typeof(IMigrationPlugin))]
public class CsvMigrationPlugin : IMigrationPlugin
{
    #region IMigrationPlugin Members

    public string ProviderName => Constants.PROVIDER_NAME;

    public IConnectionControl ConnectionControl => new CsvConnectionControl();

    public IMigrationService GetDataProvider(ConnectionDetails connectionDetails) => new CsvMigrationService(connectionDetails);

    public ISettingsControl SettingsControl => null;

    public IEnumerable<IMigrationTool> Tools => null;

    #endregion IMigrationPlugin Members
}