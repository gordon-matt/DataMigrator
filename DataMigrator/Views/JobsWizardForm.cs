namespace DataMigrator.Views;

public partial class JobsWizardForm : KryptonForm
{
    public JobsWizardForm()
    {
        InitializeComponent();

        var sourceController = Controller.GetProvider(AppState.ConfigFile.SourceConnection);
        var destinationController = Controller.GetProvider(AppState.ConfigFile.DestinationConnection);

        var sourceTables = AsyncHelper.RunSync(sourceController.GetTableNamesAsync);
        var destinationTables = AsyncHelper.RunSync(destinationController.GetTableNamesAsync);

        DestinationColumn.DataSource = destinationTables.OrderBy(x => x).ToList();

        foreach (string table in sourceTables.OrderBy(x => x))
        {
            var match = destinationTables.FirstOrDefault(x => x.Equals(table, StringComparison.InvariantCultureIgnoreCase));
            dataGridView.Rows.Add(false, table, match);
        }
    }

    public IDictionary<string, string> MappedSelections { get; set; } = new Dictionary<string, string>();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            var cell = (DataGridViewCheckBoxCell)row.Cells[SelectColumn.Index];
            cell.Value = cbSelectAll.Checked;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnOK_Click(object sender, EventArgs e)
    {
        MappedSelections = dataGridView.Rows.OfType<DataGridViewRow>()
            .Where(x => bool.Parse(x.Cells[SelectColumn.Index].Value.ToString()))
            .ToDictionary(
                k => k.Cells[SourceColumn.Index].Value.ToString(),
                v => v.Cells[DestinationColumn.Index].Value.ToString());

        Close();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnCancel_Click(object sender, EventArgs e) => Close();
}