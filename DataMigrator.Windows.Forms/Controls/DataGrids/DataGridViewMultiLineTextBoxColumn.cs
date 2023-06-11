using System.ComponentModel;

namespace DataMigrator.Windows.Forms.Controls.DataGrids;

public class DataGridViewMultiLineTextBoxOptions
{
    static public bool multiline = false;
    static public bool wordwrap = false;
}

public class DataGridViewMultiLineTextBoxColumn : DataGridViewColumn
{
    public DataGridViewMultiLineTextBoxColumn()
        : base(new DataGridViewMultiLineTextBoxCell())
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
            // Ensure that the cell used for the template is a MultiLineTextBoxCell.
            if (value != null &&
                !value.GetType().IsAssignableFrom(typeof(DataGridViewMultiLineTextBoxCell)))
            {
                throw new InvalidCastException("Must be a MultiLineTextBoxCell");
            }
            base.CellTemplate = value;
        }
    }

    #region MultiLine Options

    private bool multiLine;

    public bool MultiLine
    {
        get
        {
            return multiLine;
        }
        set
        {
            multiLine = value;
        }
    }

    private bool wordWrap;

    public bool WordWrap
    {
        get
        {
            return wordWrap;
        }
        set
        {
            wordWrap = value;
        }
    }

    public override object Clone()
    {
        DataGridViewMultiLineTextBoxColumn col =
            (DataGridViewMultiLineTextBoxColumn)base.Clone();
        col.multiLine = this.multiLine;
        col.wordWrap = this.wordWrap;
        return col;
    }

    #endregion MultiLine Options
}

public class DataGridViewMultiLineTextBoxCell : DataGridViewTextBoxCell
{
    public DataGridViewMultiLineTextBoxCell()
        : base()
    {
    }

    public override void InitializeEditingControl(int rowIndex, object
        initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        // Set the value of the editing control to the current cell value.
        base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);
        DataGridViewMultiLineTextBoxEditingControl ctl =
            DataGridView.EditingControl as DataGridViewMultiLineTextBoxEditingControl;

        DataGridViewMultiLineTextBoxOptions.multiline =
            (this.DataGridView.Columns[this.ColumnIndex] as DataGridViewMultiLineTextBoxColumn).MultiLine;
        DataGridViewMultiLineTextBoxOptions.wordwrap =
            (this.DataGridView.Columns[this.ColumnIndex] as DataGridViewMultiLineTextBoxColumn).WordWrap;

        ctl.Multiline = DataGridViewMultiLineTextBoxOptions.multiline;
        ctl.WordWrap = DataGridViewMultiLineTextBoxOptions.wordwrap;

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
            // Return the type of the editing contol that DataGridViewMultiLineTextBoxCell uses.
            return typeof(DataGridViewMultiLineTextBoxEditingControl);
        }
    }

    public override Type ValueType
    {
        get
        {
            // Return the type of the value that DataGridViewMultiLineTextBoxCell contains.
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
internal class DataGridViewMultiLineTextBoxEditingControl : TextBox, IDataGridViewEditingControl
{
    private DataGridView dataGridView;
    private bool valueChanged = false;
    private int rowIndex;

    public DataGridViewMultiLineTextBoxEditingControl()
    {
        this.Multiline = DataGridViewMultiLineTextBoxOptions.multiline;
        this.WordWrap = DataGridViewMultiLineTextBoxOptions.wordwrap;
        this.ScrollBars = ScrollBars.Vertical;
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