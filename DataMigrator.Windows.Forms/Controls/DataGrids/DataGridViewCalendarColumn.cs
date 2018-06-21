using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DataMigrator.Windows.Forms.Controls.DataGrids
{
    public static class DateFormat
    {
        private static DateTimePickerFormat dateTimePickerFormat;
        private static String customFormat;

        public static DateTimePickerFormat _DateTimePickerFormat
        {
            get { return dateTimePickerFormat; }
            set { dateTimePickerFormat = value; }
        }
        public static String CustomFormat
        {
            get { return customFormat; }
            set { customFormat = value; }
        }
    }
    public class DataGridViewCalendarColumn : DataGridViewColumn
    {
        public DataGridViewCalendarColumn() : base(new DataGridViewCalendarCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            { return base.CellTemplate; }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewCalendarCell)))
                { throw new InvalidCastException("Must be a CalendarCell"); }
                base.CellTemplate = value;
            }
        }

        private Boolean showCheckBox;
        public Boolean ShowCheckBox
        {
            get { return showCheckBox; }
            set { showCheckBox = value; }
        }
        private DateTimePickerFormat dateTimePickerFormat;
        public DateTimePickerFormat _DateTimePickerFormat
        {
            get { return dateTimePickerFormat; }
            set
            {
                dateTimePickerFormat = value;
                DateFormat._DateTimePickerFormat = dateTimePickerFormat;
            }
        }
        private String customFormat;
        public String CustomFormat
        {
            get { return customFormat; }
            set
            {
                customFormat = value;
                DateFormat.CustomFormat = customFormat;
            }
        }

        public override Object Clone()
        {
            DataGridViewCalendarColumn dataGridViewCalendarColumn = (DataGridViewCalendarColumn)base.Clone();
            dataGridViewCalendarColumn.showCheckBox = this.showCheckBox;
            dataGridViewCalendarColumn.dateTimePickerFormat = this.dateTimePickerFormat;
            dataGridViewCalendarColumn.customFormat = this.customFormat;
            return dataGridViewCalendarColumn;
        }
    }
    public class DataGridViewCalendarCell : DataGridViewTextBoxCell
    {
        public DataGridViewCalendarCell() : base()
        {
            switch (DateFormat._DateTimePickerFormat)
            {
                case DateTimePickerFormat.Long:
                    //this.Style.Format = "MMMM dd, yyyy";
                    this.Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern;
                    break;
                case DateTimePickerFormat.Short:
                    //this.Style.Format = "d/M/yyyy";
                    this.Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    break;
                case DateTimePickerFormat.Time:
                    //this.Style.Format = "hh:mm";
                    this.Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                    break;
                case DateTimePickerFormat.Custom:
                    this.Style.Format = DateFormat.CustomFormat;
                    break;
                default:
                    break;
            }
        }

        public override void InitializeEditingControl(Int32 rowIndex, Object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            DataGridViewCalendarEditingControl dataGridViewCalendarEditingControl =
                DataGridView.EditingControl as DataGridViewCalendarEditingControl;

            dataGridViewCalendarEditingControl.ShowCheckBox =
                (this.DataGridView.Columns[this.ColumnIndex] as DataGridViewCalendarColumn).ShowCheckBox;

            try
            {
                dataGridViewCalendarEditingControl.Value = (DateTime)this.Value;

                switch (DateFormat._DateTimePickerFormat)
                {
                    case DateTimePickerFormat.Long:
                        dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Long;
                        break;
                    case DateTimePickerFormat.Short:
                        dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Short;
                        break;
                    case DateTimePickerFormat.Time:
                        dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Time;
                        break;
                    case DateTimePickerFormat.Custom:
                        dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Custom;
                        dataGridViewCalendarEditingControl.CustomFormat = DateFormat.CustomFormat;
                        break;
                    default:
                        break;
                }
            }
            catch   //ArgumentOutOfRangeException
            //InvalidCastException
            {
            }
        }
        public override Type EditType
        {
            // Return the type of the editing contol that CalendarCell uses.
            get { return typeof(DataGridViewCalendarEditingControl); }
        }
        public override Type ValueType
        {
            // Return the type of the value that CalendarCell contains.
            get { return typeof(DateTime); }
        }
        public override Object DefaultNewRowValue
        {
            // Use the current date and time as the default value.
            get { return null; } //return DateTime.Now; }
        }
    }
    [ToolboxItem(false)]
    class DataGridViewCalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        #region Properties
        private DataGridView editingControlDataGridView;
        private Int32 editingControlRowIndex;
        private Boolean editingControlValueChanged = false;

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get { return editingControlDataGridView; }
            set { editingControlDataGridView = value; }
        }
        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public Int32 EditingControlRowIndex
        {
            get { return editingControlRowIndex; }
            set { editingControlRowIndex = value; }
        }
        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public Boolean EditingControlValueChanged
        {
            get { return editingControlValueChanged; }
            set { editingControlValueChanged = value; }
        }
        #endregion

        public DataGridViewCalendarEditingControl()
        {
            this.Format = DateFormat._DateTimePickerFormat;
            this.CustomFormat = DateFormat.CustomFormat;
        }
        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public Object EditingControlFormattedValue
        {
            get
            { return this.Value.ToShortDateString(); }
            set
            {
                if (value is String)
                { this.Value = DateTime.Parse((String)value); }
            }
        }
        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public Object GetEditingControlFormattedValue(DataGridViewDataErrorContexts dataGridViewDataErrorContexts)
        { return EditingControlFormattedValue; }
        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }
        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
        public Boolean EditingControlWantsInputKey(Keys key, Boolean dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
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
                    return true;

                case Keys.Delete:
                case Keys.Back:
                    DataGridViewCell CurCell = this.editingControlDataGridView.CurrentCell;
                    if ((CurCell != null))
                    { CurCell.Value = DBNull.Value; }
                    if (DateFormat._DateTimePickerFormat != DateTimePickerFormat.Custom)
                    { this.Format = DateTimePickerFormat.Custom; }
                    this.CustomFormat = " ";
                    return false;

                default: return false;
            }
        }
        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(Boolean selectAll)
        {
            // No preparation needs to be done.
        }
        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public Boolean RepositionEditingControlOnValueChange
        {
            get { return false; }
        }
        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }
        protected override void OnValueChanged(EventArgs eventargs)
        {
            if (this.CustomFormat == " ")
            {
                switch (DateFormat._DateTimePickerFormat)
                {
                    case DateTimePickerFormat.Long:
                        this.Format = DateTimePickerFormat.Long;
                        break;
                    case DateTimePickerFormat.Short:
                        this.Format = DateTimePickerFormat.Short;
                        break;
                    case DateTimePickerFormat.Time:
                        this.Format = DateTimePickerFormat.Time;
                        break;
                    case DateTimePickerFormat.Custom:
                        this.Format = DateTimePickerFormat.Custom;
                        this.CustomFormat = DateFormat.CustomFormat;
                        break;
                    default:
                        break;
                }
            }

            // Notify the DataGridView that the contents of the cell
            // have changed.
            editingControlValueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}