using System.ComponentModel;

namespace DataMigrator.Views;

public partial class DataMigratorSettingsControl : UserControl, ISettingsControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BatchSize
    {
        get => (int)nudBatchSize.Value;
        set => nudBatchSize.Value = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool TrimStrings
    {
        get => cbTrimStrings.Checked;
        set => cbTrimStrings.Checked = value;
    }

    public DataMigratorSettingsControl()
    {
        InitializeComponent();
        BatchSize = AppState.ConfigFile.BatchSize;
        TrimStrings = AppState.ConfigFile.TrimStrings;
    }

    #region ISettingsControl Members

    public UserControl ControlContent => this;

    public void Save()
    {
        AppState.ConfigFile.BatchSize = BatchSize;
        AppState.ConfigFile.TrimStrings = TrimStrings;
    }

    #endregion ISettingsControl Members
}