using System;
using System.Data;
using DataMigrator.Controls.Dialogs;
using Extenso.Data;
using Microsoft.VisualStudio.Threading;

namespace DataMigrator.Views;

public partial class TableMappingControl : UserControl, IConfigControl, ITransientControl
{
    #region Public Properties

    public string SourceTable
    {
        get => cmbSourceTable.SelectedIndex != -1 ? cmbSourceTable.SelectedItem.ToString() : string.Empty;
        set => cmbSourceTable.SelectedItem = value;
    }

    public string DestinationTable
    {
        get => cmbDestinationTable.SelectedIndex != -1 ? cmbDestinationTable.SelectedItem.ToString() : string.Empty;
        set => cmbDestinationTable.SelectedItem = value;
    }

    public IEnumerable<FieldMapping> FieldMappings
    {
        get
        {
            var mappings = new List<FieldMapping>();

            if (SourceFields == null || DestinationFields == null)
            {
                return mappings;
            }

            MappingsTable?.Rows.OfType<DataRow>().ForEach(row =>
            {
                string sourceValue = row["Source"].ToString();
                string destinationValue = row["Destination"].ToString();

                mappings.Add(new FieldMapping
                {
                    SourceField = SourceFields[sourceValue],
                    DestinationField = DestinationFields[destinationValue],
                    TransformScript = scripts.TryGetValue($"{sourceValue}|{destinationValue}", out string value) ? value : null,
                });
            });
            return mappings;
        }
    }

    private readonly Dictionary<string, string> scripts = new();

    #endregion Public Properties

    #region Private Properties

    private IMigrationService SourceController { get; set; }

    private IMigrationService DestinationController { get; set; }

    private DataTable MappingsTable { get; set; }

    private FieldCollection SourceFields { get; set; }

    private FieldCollection DestinationFields { get; set; }

    #endregion Private Properties

    #region Constructor

    public TableMappingControl()
    {
        InitializeComponent();
    }

    private void TableMappingControl_Load(object sender, EventArgs e)
    {
        if (AppState.ConfigFile.SourceConnection == null)
        {
            MessageBox.Show("Please set the source connection before trying to map fields",
                "Source Connection Not Set",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }
        if (AppState.ConfigFile.DestinationConnection == null)
        {
            MessageBox.Show("Please set the destination connection before trying to map fields",
                "Destination Connection Not Set",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }

        dgvMappings_Script.DefaultCellStyle.NullValue = null;

        SourceController = Controller.GetProvider(AppState.ConfigFile.SourceConnection);
        DestinationController = Controller.GetProvider(AppState.ConfigFile.DestinationConnection);

        AsyncHelper.RunSync(SourceController.GetTableNamesAsync).ForEach(x => cmbSourceTable.Items.Add(x));
        AsyncHelper.RunSync(DestinationController.GetTableNamesAsync).ForEach(x => cmbDestinationTable.Items.Add(x));

        SourceTable = AppState.CurrentJob.SourceTable;
        DestinationTable = AppState.CurrentJob.DestinationTable;

        MappingsTable = new DataTable();
        MappingsTable.Columns.AddRange("Source", "Destination");
        MappingsTable.Columns.Add("Script", typeof(byte[]));

        foreach (var mapping in AppState.CurrentJob.FieldMappings)
        {
            var row = MappingsTable.NewRow();
            row["Source"] = mapping.SourceField.Name;
            row["Destination"] = mapping.DestinationField.Name;
            row["Script"] = !string.IsNullOrEmpty(mapping.TransformScript) ? Constants.ImageBytes.Script_24x24 : null;

            scripts.Add($"{mapping.SourceField.Name}|{mapping.DestinationField.Name}", mapping.TransformScript);

            //if (mapping.DestinationField.Type != mapping.SourceField.Type)
            //{
            //    //TODO: Show icon for data type change
            //    //row["Transforms"] =
            //}

            MappingsTable.Rows.Add(row);

            using var sourceRow = dgvSource.Rows.OfType<DataGridViewRow>()
                .Where(x => x.Index != dgvSource.NewRowIndex)
                .SingleOrDefault(x => x.Cells[0].Value.ToString() == mapping.SourceField.Name);

            if (sourceRow != null)
            {
                dgvSource.Rows.Remove(sourceRow);
            }

            using var destinationRow = dgvDestination.Rows.OfType<DataGridViewRow>()
                .Where(x => x.Index != dgvDestination.NewRowIndex)
                .SingleOrDefault(x => x.Cells[0].Value.ToString() == mapping.DestinationField.Name);

            if (destinationRow != null)
            {
                dgvDestination.Rows.Remove(destinationRow);
            }
        }

        dgvMappings.AutoGenerateColumns = false;
        dgvMappings.DataSource = MappingsTable;

        if (dgvMappings.Rows.Count > 0)
        {
            cmbSourceTable.Enabled = false;
            cmbDestinationTable.Enabled = false;
        }
    }

    #endregion Constructor

    private static DataTable GetFieldsDataTable(IEnumerable<Field> fields)
    {
        var table = new DataTable();
        table.Columns.AddRange("Field Name", "Type");
        fields.ForEach(field =>
        {
            var row = table.NewRow();
            row["Field Name"] = field.Name;
            row["Type"] = field.Type.ToString();
            table.Rows.Add(row);
        });
        return table;
    }

    #region Buttons

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnAutoMap_Click(object sender, EventArgs e)
    {
        var sourceRowsToRemove = new List<DataGridViewRow>();
        var destinationRowsToRemove = new List<DataGridViewRow>();

        foreach (DataGridViewRow sourceRow in dgvSource.Rows)
        {
            if (sourceRow.Index == dgvSource.NewRowIndex)
            { continue; }

            string sourceRowValue = sourceRow.Cells[0].Value.ToString();
            string sourceFieldType = sourceRow.Cells[1].Value.ToString();

            foreach (DataGridViewRow destinationRow in dgvDestination.Rows)
            {
                if (destinationRow.Index == dgvDestination.NewRowIndex)
                { continue; }

                string destinationFieldType = destinationRow.Cells[1].Value.ToString();
                //If Field types do not match
                if (!sourceFieldType.Equals(destinationFieldType, StringComparison.InvariantCultureIgnoreCase))
                {
                    //Then do not map
                    continue;
                }

                string destinationRowValue = destinationRow.Cells[0].Value.ToString();
                if (destinationRowValue.Equals(sourceRowValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var mappedRow = MappingsTable.NewRow();
                    mappedRow["Source"] = sourceRowValue;
                    mappedRow["Destination"] = destinationRowValue;
                    mappedRow["Script"] = null;
                    MappingsTable.Rows.Add(mappedRow);

                    sourceRowsToRemove.Add(sourceRow);
                    destinationRowsToRemove.Add(destinationRow);
                    break;
                }
            }
        }

        sourceRowsToRemove.ForEach(dgvSource.Rows.Remove);
        destinationRowsToRemove.ForEach(dgvDestination.Rows.Remove);

        if (dgvMappings.Rows.Count > 0)
        {
            cmbSourceTable.Enabled = false;
            cmbDestinationTable.Enabled = false;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (dgvSource.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a Source field",
                "Details Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }
        if (dgvDestination.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a Destination field",
                "Details Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }

        var sourceRow = dgvSource.SelectedRows[0];
        var destinationRow = dgvDestination.SelectedRows[0];

        string sourceFieldType = sourceRow.Cells[1].Value.ToString();
        string destinationFieldType = destinationRow.Cells[1].Value.ToString();
        if (!sourceFieldType.Equals(destinationFieldType, StringComparison.InvariantCultureIgnoreCase))
        {
            if (MessageBox.Show(
                "The source field type is different from the destination field type. Are you sure you want to map them?",
                "Are you sure?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
        }

        var row = MappingsTable.NewRow();
        row["Source"] = sourceRow.Cells[0].Value.ToString();
        row["Destination"] = destinationRow.Cells[0].Value.ToString();
        row["Script"] = null;
        MappingsTable.Rows.Add(row);

        dgvSource.Rows.Remove(sourceRow);
        dgvDestination.Rows.Remove(destinationRow);

        cmbSourceTable.Enabled = false;
        cmbDestinationTable.Enabled = false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnAddEditScript_Click(object sender, EventArgs e)
    {
        if (dgvMappings.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a mapped field",
                "Details Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }

        var mappedRow = dgvMappings.SelectedRows[0];

        using var dialog = new ScriptDialog();

        string source = mappedRow.Cells[dgvMappings_Source.Index].Value.ToString();
        string destination = mappedRow.Cells[dgvMappings_Destination.Index].Value.ToString();
        string key = $"{source}|{destination}";

        if (scripts.ContainsKey(key))
        {
            dialog.Script = scripts[key];
        }

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            if (scripts.ContainsKey(key))
            {
                scripts[key] = dialog.Script;
            }
            else
            {
                scripts.Add(key, dialog.Script);
            }

            var row = MappingsTable.Rows.OfType<DataRow>().FirstOrDefault(x =>
                x["Source"].ToString() == source &&
                x["Destination"].ToString() == destination);

            row["Script"] = Constants.ImageBytes.Script_24x24;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private async void btnCreateTable_Click(object sender, EventArgs e)
    {
        string destinationTable = await Controller.CreateDestinationTableAsync(SourceTable);
        cmbDestinationTable.Items.Clear();
        (await DestinationController.GetTableNamesAsync()).ForEach(x => cmbDestinationTable.Items.Add(x));
        cmbDestinationTable.SelectedItem = destinationTable;
        AppState.CurrentJob.DestinationTable = destinationTable;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnRemove_Click(object sender, EventArgs e)
    {
        if (dgvMappings.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a mapped field",
                "Details Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }

        var mappedRow = dgvMappings.SelectedRows[0];
        dgvMappings.Rows.Remove(mappedRow);

        var sourceMappedFields = dgvMappings.Rows.OfType<DataGridViewRow>()
            .Where(x => x.Index != dgvMappings.NewRowIndex)
            .Select(r => r.Cells[dgvMappings_Source.Index].Value.ToString());

        var destinationMappedFields = dgvMappings.Rows.OfType<DataGridViewRow>()
            .Where(x => x.Index != dgvMappings.NewRowIndex)
            .Select(r => r.Cells[dgvMappings_Destination.Index].Value.ToString());

        var sourceFields = SourceFields.Where(x => !x.Name.In(sourceMappedFields));
        var destinationFields = DestinationFields.Where(x => !x.Name.In(destinationMappedFields));

        dgvSource.DataSource = GetFieldsDataTable(sourceFields);
        dgvDestination.DataSource = GetFieldsDataTable(destinationFields);

        if (dgvMappings.Rows.Count == 0)
        {
            cmbSourceTable.Enabled = true;
            cmbDestinationTable.Enabled = true;
        }
    }

    #endregion Buttons

    #region Combo Boxes

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private async void cmbSourceTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbSourceTable.SelectedIndex != -1)
        {
            dgvSource.DataSource = null;
            string sourceSchema = SourceTable.Contains('.') ? SourceTable.LeftOf('.') : string.Empty;
            string sourceTable = SourceTable.Contains('.') ? SourceTable.RightOf('.') : SourceTable;
            SourceFields = await SourceController.GetFieldsAsync(sourceTable, sourceSchema);

            var sourceMappedFields = dgvMappings.Rows.OfType<DataGridViewRow>()
                .Where(x => x.Index != dgvMappings.NewRowIndex)
                .Select(r => r.Cells[dgvMappings_Source.Index].Value.ToString());

            dgvSource.DataSource = GetFieldsDataTable(SourceFields.Where(x => !x.Name.In(sourceMappedFields)));

            //if (!isLoading)
            //{
            //    MappingsTable.Clear();
            //}
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private async void cmbDestinationTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbDestinationTable.SelectedIndex != -1)
        {
            dgvDestination.DataSource = null;
            string destinationSchema = DestinationTable.Contains('.') ? DestinationTable.LeftOf('.') : string.Empty;
            string destinationTable = DestinationTable.Contains('.') ? DestinationTable.RightOf('.') : DestinationTable;
            DestinationFields = await DestinationController.GetFieldsAsync(destinationTable, destinationSchema);

            var destinationMappedFields = dgvMappings.Rows.OfType<DataGridViewRow>()
                .Where(x => x.Index != dgvMappings.NewRowIndex)
                .Select(r => r.Cells[dgvMappings_Destination.Index].Value.ToString());

            dgvDestination.DataSource = GetFieldsDataTable(DestinationFields.Where(x => !x.Name.In(destinationMappedFields)));

            //if (!isLoading)
            //{
            //    MappingsTable.Clear();
            //}
        }
    }

    #endregion Combo Boxes

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dgvSource_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dgvDestination_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dgvSource_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvSource.SelectedRows.Count == 0)
        {
            return;
        }

        var row = dgvSource.SelectedRows[0];
        if (row.Index.In(-1, dgvSource.NewRowIndex))
        {
            return;
        }

        var field = SourceFields[row.Cells["Field Name"].Value.ToString()];
        pGridSource.SelectedObject = field;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dgvDestination_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvDestination.SelectedRows.Count == 0)
        {
            return;
        }

        var row = dgvDestination.SelectedRows[0];
        if (row.Index.In(-1, dgvDestination.NewRowIndex))
        {
            return;
        }

        var field = DestinationFields[row.Cells["Field Name"].Value.ToString()];
        pGridDestination.SelectedObject = field;
    }

    public void Save()
    {
        AppState.CurrentJob.SourceTable = SourceTable;
        AppState.CurrentJob.DestinationTable = DestinationTable;

        AppState.CurrentJob.FieldMappings.Clear();
        AppState.CurrentJob.FieldMappings.AddRange(FieldMappings);

        var existingJob = AppState.ConfigFile.Jobs[AppState.CurrentJob.Name];
        if (existingJob == null)
        {
            AppState.ConfigFile.Jobs.Add(AppState.CurrentJob);
        }
    }
}