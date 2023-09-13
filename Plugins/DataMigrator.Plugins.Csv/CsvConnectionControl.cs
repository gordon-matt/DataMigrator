using DataMigrator.Common;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using DataMigrator.Plugins.Csv;
using Extenso;

namespace DataMigrator.Csv;

public partial class CsvConnectionControl : UserControl, IConnectionControl
{
    public string FilePath
    {
        get => txtFile.Text.Trim();
        set => txtFile.Text = value;
    }

    public bool HasHeaderRow
    {
        get => cbHasHeaderRow.Checked;
        set => cbHasHeaderRow.Checked = value;
    }

    public FileDelimiter Delimiter
    {
        get
        {
            if (cmbDelimiter.SelectedIndex != -1)
            {
                return (FileDelimiter)cmbDelimiter.SelectedItem;
            }
            return FileDelimiter.Comma;
        }
        set => cmbDelimiter.SelectedItem = value;
    }

    public string ConnectionString
    {
        get
        {
            #region Checks

            if (!File.Exists(FilePath))
            {
                TraceService.Instance.WriteMessage(TraceEvent.Error, "FilePath is invalid. Please try again.");
                return string.Empty;
            }

            #endregion Checks

            return FilePath;
        }
    }

    public CsvConnectionControl()
    {
        InitializeComponent();
        cmbDelimiter.DataSource = EnumExtensions.GetValues<FileDelimiter>().ToList();
    }

    #region IConnectionControl Members

    public UserControl ControlContent => this;

    public ConnectionDetails ConnectionDetails
    {
        get
        {
            var connectionDetails = new ConnectionDetails
            {
                Database = this.FilePath,
                ProviderName = Constants.PROVIDER_NAME,
                ConnectionString = this.ConnectionString
            };

            connectionDetails.ExtendedProperties.Add("HasHeaderRow", HasHeaderRow);
            connectionDetails.ExtendedProperties.Add("Delimiter", Delimiter);
            return connectionDetails;
        }
        set
        {
            FilePath = value.Database;
            HasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            Delimiter = ConnectionDetails.ExtendedProperties["Delimiter"].GetValue<FileDelimiter>();
        }
    }

    public bool ValidateConnection() => File.Exists(FilePath);

    #endregion IConnectionControl Members

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnBrowse_Click(object sender, EventArgs e)
    {
        if (dlgOpenFile.ShowDialog() == DialogResult.OK)
        {
            FilePath = dlgOpenFile.FileName;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void cmbDelimiter_Format(object sender, ListControlConvertEventArgs e)
    {
        e.Value = EnumExtensions.GetDisplayName((FileDelimiter)e.Value);
    }
}