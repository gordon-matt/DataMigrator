using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DataMigrator.Windows.Forms.Controls.DataGrids
{
    public class DataGridViewNumericTextBoxColumn : DataGridViewColumn
    {
        public DataGridViewNumericTextBoxColumn()
            : base(new DataGridViewNumericTextBoxCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a NumericTextBoxCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewNumericTextBoxCell)))
                {
                    throw new InvalidCastException("Must be a NumericTextBoxCell");
                }
                base.CellTemplate = value;
            }
        }

        public override object Clone()
        {
            DataGridViewNumericTextBoxColumn col = (DataGridViewNumericTextBoxColumn)base.Clone();
            return col;
        }
    }

    public class DataGridViewNumericTextBoxCell : DataGridViewTextBoxCell
    {
        public DataGridViewNumericTextBoxCell()
            : base()
        {
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            DataGridViewNumericTextBoxEditingControl ctl =
               DataGridView.EditingControl as DataGridViewNumericTextBoxEditingControl;

            try
            {
                ctl.Text = this.Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that  DataGridViewNumericTextBoxCell uses.
                return typeof(DataGridViewNumericTextBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that  DataGridViewNumericTextBoxCell contains.
                return typeof(System.String);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return null;
            }
        }
    }

    [ToolboxItem(false)]
    internal class DataGridViewNumericTextBoxEditingControl : DataMigrator.Windows.Forms.Controls.NumericTextBox,
        IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged = false;
        private int rowIndex;

        public DataGridViewNumericTextBoxEditingControl()
        {
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value is String)
                {
                    this.Text = value.ToString();
                }
            }
        }

        // Implements the
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex
        // property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey
        // method.
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Delete:
                case Keys.Back:
                    return true;

                default:
                    return false;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);
        }
    }
}