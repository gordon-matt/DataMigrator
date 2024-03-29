﻿using System.ComponentModel.Composition;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.SharePoint;

[Export(typeof(IMigrationPlugin))]
public class SharePointMigrationPlugin : IMigrationPlugin
{
    #region IMigrationPlugin Members

    public string ProviderName => Constants.PROVIDER_NAME;

    public IConnectionControl ConnectionControl => new SharePointConnectionControl();

    public IMigrationService GetDataProvider(ConnectionDetails connectionDetails) => new SharePointMigrationService(connectionDetails);

    public ISettingsControl SettingsControl => null;

    public IEnumerable<IMigrationTool> Tools => null;

    #endregion IMigrationPlugin Members
}