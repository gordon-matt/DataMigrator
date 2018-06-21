using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Kore;
using Kore.Collections;
using Kore.Data;

namespace DataMigrator.Views
{
    public partial class TableMappingControl : UserControl
    {
        #region Public Properties

        public string SourceTable
        {
            get
            {
                if (cmbSourceTable.SelectedIndex != -1)
                {
                    return cmbSourceTable.SelectedItem.ToString();
                }
                return string.Empty;
            }
            set { cmbSourceTable.SelectedItem = value; }
        }

        public string DestinationTable
        {
            get
            {
                if (cmbDestinationTable.SelectedIndex != -1)
                {
                    return cmbDestinationTable.SelectedItem.ToString();
                }
                return string.Empty;
            }
            set { cmbDestinationTable.SelectedItem = value; }
        }

        public IEnumerable<FieldMapping> FieldMappings
        {
            get
            {
                List<FieldMapping> mappings = new List<FieldMapping>();

                if (SourceFields == null || DestinationFields == null)
                {
                    return mappings;
                }

                if (MappingsTable != null)
                {
                    MappingsTable.Rows.Cast<DataRow>().ForEach(row =>
                    {
                        mappings.Add(new FieldMapping
                        {
                            SourceField = SourceFields[row["Source"].ToString()],
                            DestinationField = DestinationFields[row["Destination"].ToString()]
                        });
                    });
                }
                return mappings;
            }
        }

        #endregion

        #region Private Properties

        private BaseProvider SourceController { get; set; }

        private BaseProvider DestinationController { get; set; }

        private DataTable MappingsTable { get; set; }

        private FieldCollection SourceFields { get; set; }

        private FieldCollection DestinationFields { get; set; }

        #endregion

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

            SourceController.TableNames.ForEach(x => cmbSourceTable.Items.Add(x));
            DestinationController.TableNames.ForEach(x => cmbDestinationTable.Items.Add(x));

            SourceTable = Program.CurrentJob.SourceTable;
            DestinationTable = Program.CurrentJob.DestinationTable;

            MappingsTable = new DataTable();
            MappingsTable.Columns.AddRange("Source", "Destination");

            foreach (FieldMapping mapping in Program.CurrentJob.FieldMappings)
            {
                DataRow row = MappingsTable.NewRow();
                row["Source"] = mapping.SourceField.Name;
                row["Destination"] = mapping.DestinationField.Name;
                MappingsTable.Rows.Add(row);

                DataGridViewRow sourceRow = dgvSource.Rows.Cast<DataGridViewRow>()
                    .Where(x => x.Index != dgvSource.NewRowIndex)
                    .SingleOrDefault(x => x.Cells[0].Value.ToString() == mapping.SourceField.Name);
                DataGridViewRow destinationRow = dgvDestination.Rows.Cast<DataGridViewRow>()
                    .Where(x => x.Index != dgvDestination.NewRowIndex)
                    .SingleOrDefault(x => x.Cells[0].Value.ToString() == mapping.DestinationField.Name);

                if (sourceRow != null)
                { dgvSource.Rows.Remove(sourceRow); }
                if (destinationRow != null)
                { dgvDestination.Rows.Remove(destinationRow); }
            }

            dgvMappings.DataSource = MappingsTable;
        }

        #endregion

        private DataTable GetFieldsDataTable(IEnumerable<Field> fields)
        {
            DataTable table = new DataTable();
            table.Columns.AddRange("Field Name", "Type");
            fields.ForEach(field =>
            {
                DataRow row = table.NewRow();
                row["Field Name"] = field.Name;
                row["Type"] = field.Type.ToString();
                table.Rows.Add(row);
            });
            return table;
        }

        #region Buttons

        private void btnAutoMap_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> sourceRowsToRemove = new List<DataGridViewRow>();
            List<DataGridViewRow> destinationRowsToRemove = new List<DataGridViewRow>();

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
                        DataRow mappedRow = MappingsTable.NewRow();
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

            DataGridViewRow sourceRow = dgvSource.SelectedRows[0];
            DataGridViewRow destinationRow = dgvDestination.SelectedRows[0];

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

            DataRow row = MappingsTable.NewRow();
            row["Source"] = sourceRow.Cells[0].Value.ToString();
            row["Destination"] = destinationRow.Cells[0].Value.ToString();
            MappingsTable.Rows.Add(row);

            dgvSource.Rows.Remove(sourceRow);
            dgvDestination.Rows.Remove(destinationRow);
        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            Controller.CreateDestinationTable(SourceTable);
            cmbDestinationTable.Items.Clear();
            DestinationController.TableNames.ForEach(x => cmbDestinationTable.Items.Add(x));
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

            DataGridViewRow mappedRow = dgvMappings.SelectedRows[0];
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

        #endregion

        #region Combo Boxes

        private void cmbSourceTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSourceTable.SelectedIndex != -1)
            {
                dgvSource.DataSource = null;
                SourceFields = SourceController.GetFields(cmbSourceTable.SelectedItem.ToString());
                dgvSource.DataSource = GetFieldsDataTable(SourceFields);
            }
        }

        private void cmbDestinationTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDestinationTable.SelectedIndex != -1)
            {
                dgvDestination.DataSource = null;
                DestinationFields = DestinationController.GetFields(cmbDestinationTable.SelectedItem.ToString());
                dgvDestination.DataSource = GetFieldsDataTable(DestinationFields);
            }
        }

        #endregion

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

            Field field = SourceFields[row.Cells["Field Name"].Value.ToString()];
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

            Field field = DestinationFields[row.Cells["Field Name"].Value.ToString()];
            pGridDestination.SelectedObject = field;
        }

    }
}