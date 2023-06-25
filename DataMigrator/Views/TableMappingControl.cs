using System.Data;
using Extenso.Data;

namespace DataMigrator.Views;

public partial class TableMappingControl : UserControl, IConfigControl
{
    #region Public Properties

    public string SourceTable
    {
        get => cmbSourceTable.SelectedIndex != -1 ? cmbSourceTable.SelectedItem.ToString() : string.Empty;
        set => cmbSourceTable.SelectedItem = value;
    }

    //public string SourceSchema
    //{
    //    get
    //    {
    //        string sourceTable = SourceTable;
    //        if (sourceTable.Contains('.'))
    //        {
    //            return sourceTable.LeftOf('.');
    //        }
    //        return string.Empty;
    //    }
    //}

    public string DestinationTable
    {
        get => cmbDestinationTable.SelectedIndex != -1 ? cmbDestinationTable.SelectedItem.ToString() : string.Empty;
        set => cmbDestinationTable.SelectedItem = value;
    }

    //public string DestinationSchema
    //{
    //    get
    //    {
    //        string destinationTable = DestinationTable;
    //        if (destinationTable.Contains('.'))
    //        {
    //            return destinationTable.LeftOf('.');
    //        }
    //        return string.Empty;
    //    }
    //}

    public IEnumerable<FieldMapping> FieldMappings
    {
        get
        {
            var mappings = new List<FieldMapping>();

            if (SourceFields == null || DestinationFields == null)
            {
                return mappings;
            }

            MappingsTable?.Rows.Cast<DataRow>().ForEach(row =>
            {
                mappings.Add(new FieldMapping
                {
                    SourceField = SourceFields[row["Source"].ToString()],
                    DestinationField = DestinationFields[row["Destination"].ToString()]
                });
            });
            return mappings;
        }
    }

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

        if (Program.Configuration.SourceConnection == null)
        {
            MessageBox.Show("Please set the source connection before trying to map fields",
                "Source Connection Not Set",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }
        if (Program.Configuration.DestinationConnection == null)
        {
            MessageBox.Show("Please set the destination connection before trying to map fields",
                "Destination Connection Not Set",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return;
        }

        SourceController = Controller.GetProvider(Program.Configuration.SourceConnection);
        DestinationController = Controller.GetProvider(Program.Configuration.DestinationConnection);

        AsyncHelper.RunSync(SourceController.GetTableNamesAsync).ForEach(x => cmbSourceTable.Items.Add(x));
        AsyncHelper.RunSync(DestinationController.GetTableNamesAsync).ForEach(x => cmbDestinationTable.Items.Add(x));

        SourceTable = Program.CurrentJob.SourceTable;
        DestinationTable = Program.CurrentJob.DestinationTable;

        MappingsTable = new DataTable();
        MappingsTable.Columns.AddRange("Source", "Destination");

        foreach (var mapping in Program.CurrentJob.FieldMappings)
        {
            var row = MappingsTable.NewRow();
            row["Source"] = mapping.SourceField.Name;
            row["Destination"] = mapping.DestinationField.Name;
            MappingsTable.Rows.Add(row);

            using var sourceRow = dgvSource.Rows.Cast<DataGridViewRow>()
                .Where(x => x.Index != dgvSource.NewRowIndex)
                .SingleOrDefault(x => x.Cells[0].Value.ToString() == mapping.SourceField.Name);

            if (sourceRow != null)
            {
                dgvSource.Rows.Remove(sourceRow);
            }

            using var destinationRow = dgvDestination.Rows.Cast<DataGridViewRow>()
                .Where(x => x.Index != dgvDestination.NewRowIndex)
                .SingleOrDefault(x => x.Cells[0].Value.ToString() == mapping.DestinationField.Name);

            if (destinationRow != null)
            {
                dgvDestination.Rows.Remove(destinationRow);
            }
        }

        dgvMappings.DataSource = MappingsTable;
    }

    #endregion Constructor

    private DataTable GetFieldsDataTable(IEnumerable<Field> fields)
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

                if (destinationRow.Cells[0].Value.ToString() == sourceRowValue)
                {
                    var mappedRow = MappingsTable.NewRow();
                    mappedRow["Source"] = sourceRowValue;
                    mappedRow["Destination"] = sourceRowValue;
                    MappingsTable.Rows.Add(mappedRow);

                    sourceRowsToRemove.Add(sourceRow);
                    destinationRowsToRemove.Add(destinationRow);
                    break;
                }
            }
        }

        sourceRowsToRemove.ForEach(row => { dgvSource.Rows.Remove(row); });
        destinationRowsToRemove.ForEach(row => { dgvDestination.Rows.Remove(row); });
    }

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
        MappingsTable.Rows.Add(row);

        dgvSource.Rows.Remove(sourceRow);
        dgvDestination.Rows.Remove(destinationRow);
    }

    private async void btnCreateTable_Click(object sender, EventArgs e)
    {
        await Controller.CreateDestinationTableAsync(SourceTable);
        cmbDestinationTable.Items.Clear();
        (await DestinationController.GetTableNamesAsync()).ForEach(x => cmbDestinationTable.Items.Add(x));
        cmbDestinationTable.SelectedItem = SourceTable;
        Program.CurrentJob.DestinationTable = SourceTable;
    }

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

        var sourceMappedFields = dgvMappings.Rows.Cast<DataGridViewRow>()
            .Where(x => x.Index != dgvMappings.NewRowIndex)
            .Select(r => r.Cells["Source"].Value.ToString());

        var destinationMappedFields = dgvMappings.Rows.Cast<DataGridViewRow>()
            .Where(x => x.Index != dgvMappings.NewRowIndex)
            .Select(r => r.Cells["Destination"].Value.ToString());

        var sourceFields = SourceFields.Where(x => !x.Name.In(sourceMappedFields));
        var destinationFields = DestinationFields.Where(x => !x.Name.In(destinationMappedFields));

        dgvSource.DataSource = GetFieldsDataTable(sourceFields);
        dgvDestination.DataSource = GetFieldsDataTable(destinationFields);
    }

    #endregion Buttons

    #region Combo Boxes

    private async void cmbSourceTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbSourceTable.SelectedIndex != -1)
        {
            dgvSource.DataSource = null;
            string sourceSchema = SourceTable.Contains('.') ? SourceTable.LeftOf('.') : string.Empty;
            string sourceTable = SourceTable.Contains('.') ? SourceTable.RightOf('.') : SourceTable;
            SourceFields = await SourceController.GetFieldsAsync(sourceTable, sourceSchema);
            dgvSource.DataSource = GetFieldsDataTable(SourceFields);
        }
    }

    private async void cmbDestinationTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbDestinationTable.SelectedIndex != -1)
        {
            dgvDestination.DataSource = null;
            string destinationSchema = DestinationTable.Contains('.') ? DestinationTable.LeftOf('.') : string.Empty;
            string destinationTable = DestinationTable.Contains('.') ? DestinationTable.RightOf('.') : DestinationTable;
            DestinationFields = await DestinationController.GetFieldsAsync(destinationTable, destinationSchema);
            dgvDestination.DataSource = GetFieldsDataTable(DestinationFields);
        }
    }

    #endregion Combo Boxes

    private void dgvSource_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
    }

    private void dgvDestination_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
    }

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
        Program.CurrentJob.SourceTable = SourceTable;
        Program.CurrentJob.DestinationTable = DestinationTable;

        Program.CurrentJob.FieldMappings.Clear();
        Program.CurrentJob.FieldMappings.AddRange(FieldMappings);

        var existingJob = Program.Configuration.Jobs[Program.CurrentJob.Name];
        if (existingJob == null)
        {
            Program.Configuration.Jobs.Add(Program.CurrentJob);
        }
    }
}