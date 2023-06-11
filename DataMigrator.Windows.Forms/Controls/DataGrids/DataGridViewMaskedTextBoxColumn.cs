//---------------------------------------------------------------------
//  This file is part of the Microsoft .NET Framework SDK Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//This source code is intended only as a supplement to Microsoft
//Development Tools and/or on-line documentation.  See these other
//materials for detailed information regarding Microsoft code samples.
//
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------

using System.ComponentModel;

namespace DataMigrator.Windows.Forms.Controls.DataGrids
{
    public class DataGridViewMaskedTextBoxOptions
    {
        static public Color foreClr;
    }

    //  The base object for the custom column type.  Programmers manipulate
    //  the column types most often when working with the DataGridView, and
    //  this one sets the basics and Cell Template values controlling the
    //  default behaviour for cells of this column type.
    public class DataGridViewMaskedTextBoxColumn : DataGridViewColumn
    {
        private string mask;
        private char promptChar;
        private bool includePrompt;
        private bool includeLiterals;
        private Type validatingType;

        //CUSTOM CODE
        private Color foreColor;

        public Color ForeColor
        {
            get
            {
                return foreColor;
            }
            set
            {
                foreColor = value;
                DataGridViewMaskedTextBoxOptions.foreClr = foreColor;
            }
        }

        //CUSTOM CODE
        public override object Clone()
        {
            DataGridViewMaskedTextBoxColumn col = (DataGridViewMaskedTextBoxColumn)base.Clone();
            col.foreColor = this.foreColor;
            return col;
        }

        //  Initializes a new instance of this class, making sure to pass
        //  to its base constructor an instance of a DataGridViewMaskedTextBoxCell
        //  class to use as the basic template.
        public DataGridViewMaskedTextBoxColumn() : base(new DataGridViewMaskedTextBoxCell())
        {
        }

        //  Routine to convert from boolean to DataGridViewTriState.
        private static DataGridViewTriState TriBool(bool value)
        {
            return value ? DataGridViewTriState.True : DataGridViewTriState.False;
        }

        //  The template cell that will be used for this column by default,
        //  unless a specific cell is set for a particular row.
        //
        //  A DataGridViewMaskedTextBoxCell cell which will serve as the template cell
        //  for this column.
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                //  Only cell types that derive from DataGridViewMaskedTextBoxCell are supported as the cell template.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewMaskedTextBoxCell)))
                {
                    string s = "Cell type is not based upon the DataGridViewMaskedTextBoxCell.";//CustomColumnMain.GetResourceManager().GetString("excNotDataGridViewMaskedTextBox");
                    throw new InvalidCastException(s);
                }

                base.CellTemplate = value;
            }
        }

        //  Indicates the Mask property that is used on the DataGridViewMaskedTextBox
        //  for entering new data into cells of this type.
        //
        //  See the DataGridViewMaskedTextBox control documentation for more details.
        public virtual string Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                DataGridViewMaskedTextBoxCell mtbc;
                DataGridViewCell dgvc;
                int rowCount;

                if (this.mask != value)
                {
                    this.mask = value;
                    //
                    // first, update the value on the template cell.
                    //
                    mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    mtbc.Mask = value;

                    //
                    // now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvc is DataGridViewMaskedTextBoxCell)
                            {
                                mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                                mtbc.Mask = value;
                            }
                        }
                    }
                }
            }
        }

        //  By default, the DataGridViewMaskedTextBox uses the underscore (_) character
        //  to prompt for required characters.  This propertly lets you
        //  choose a different one.
        //
        //  See the DataGridViewMaskedTextBox control documentation for more details.
        public virtual char PromptChar
        {
            get
            {
                return this.promptChar;
            }
            set
            {
                DataGridViewMaskedTextBoxCell mtbc;
                DataGridViewCell dgvc;
                int rowCount;

                if (this.promptChar != value)
                {
                    this.promptChar = value;
                    //
                    // first, update the value on the template cell.
                    //
                    mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    mtbc.PromptChar = value;
                    //
                    // now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvc is DataGridViewMaskedTextBoxCell)
                            {
                                mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                                mtbc.PromptChar = value;
                            }
                        }
                    }
                }
            }
        }

        //   Indicates whether any unfilled characters in the mask should be
        //   be included as prompt characters when somebody asks for the text
        //   of the DataGridViewMaskedTextBox for a particular cell programmatically.
        //
        //   See the DataGridViewMaskedTextBox control documentation for more details.
        public virtual bool IncludePrompt
        {
            get
            {
                return this.includePrompt;
            }
            set
            {
                DataGridViewMaskedTextBoxCell mtbc;
                DataGridViewCell dgvc;
                int rowCount;

                if (this.includePrompt != value)
                {
                    this.includePrompt = value;

                    //
                    // first, update the value on the template cell.
                    //
                    mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    mtbc.IncludePrompt = TriBool(value);

                    //
                    // now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvc is DataGridViewMaskedTextBoxCell)
                            {
                                mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                                mtbc.IncludePrompt = TriBool(value);
                            }
                        }
                    }
                }
            }
        }

        //  Controls whether or not literal (non-prompt) characters should
        //  be included in the output of the Text property for newly entered
        //  data in a cell of this type.
        //
        //  See the DataGridViewMaskedTextBox control documentation for more details.
        public virtual bool IncludeLiterals
        {
            get
            {
                return this.includeLiterals;
            }
            set
            {
                DataGridViewMaskedTextBoxCell mtbc;
                DataGridViewCell dgvc;
                int rowCount;

                if (this.includeLiterals != value)
                {
                    this.includeLiterals = value;
                    //
                    // first, update the value on the template cell.
                    //
                    mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    mtbc.IncludeLiterals = TriBool(value);
                    //
                    // now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvc is DataGridViewMaskedTextBoxCell)
                            {
                                mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                                mtbc.IncludeLiterals = TriBool(value);
                            }
                        }
                    }
                }
            }
        }

        //  Indicates the type against any data entered in the DataGridViewMaskedTextBox
        //  should be validated.  The DataGridViewMaskedTextBox control will attempt to
        //  instantiate this type and assign the value from the contents of
        //  the text box.  An error will occur if it fails to assign to this
        //  type.
        //
        //  See the DataGridViewMaskedTextBox control documentation for more details.
        public virtual Type ValidatingType
        {
            get
            {
                return this.validatingType;
            }
            set
            {
                DataGridViewMaskedTextBoxCell mtbc;
                DataGridViewCell dgvc;
                int rowCount;

                if (this.validatingType != value)
                {
                    this.validatingType = value;
                    //
                    // first, update the value on the template cell.
                    //
                    mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    mtbc.ValidatingType = value;
                    //
                    // now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvc is DataGridViewMaskedTextBoxCell)
                            {
                                mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                                mtbc.ValidatingType = value;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class DataGridViewMaskedTextBoxCell : DataGridViewTextBoxCell
    {
        private string mask;
        private char promptChar;
        private DataGridViewTriState includePrompt;
        private DataGridViewTriState includeLiterals;
        private Type validatingType;

        //=------------------------------------------------------------------=
        // DataGridViewMaskedTextBoxCell
        //=------------------------------------------------------------------=
        /// <summary>
        ///   Initializes a new instance of this class.  Fortunately, there's
        ///   not much to do here except make sure that our base class is
        ///   also initialized properly.
        /// </summary>
        ///
        public DataGridViewMaskedTextBoxCell() : base()
        {
            this.mask = "";
            this.promptChar = '_';
            this.includePrompt = DataGridViewTriState.NotSet;
            this.includeLiterals = DataGridViewTriState.NotSet;
            this.validatingType = typeof(string);
            //CUSTOM CODE
            //this.Style.ForeColor = DataGridViewMaskedTextBoxOptions.foreClr;
        }

        ///   Whenever the user is to begin editing a cell of this type, the editing
        ///   control must be created, which in this column type's
        ///   case is a subclass of the DataGridViewMaskedTextBox control.
        ///
        ///   This routine sets up all the properties and values
        ///   on this control before the editing begins.
        public override void InitializeEditingControl(int rowIndex,
            object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            DataGridViewMaskedTextBoxEditingControl mtbec;
            DataGridViewMaskedTextBoxColumn mtbcol;
            DataGridViewColumn dgvc;

            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);

            mtbec = DataGridView.EditingControl as DataGridViewMaskedTextBoxEditingControl;
            //
            // set up props that are specific to the DataGridViewMaskedTextBox
            //
            dgvc = this.OwningColumn;   // this.DataGridView.Columns[this.ColumnIndex];
            if (dgvc is DataGridViewMaskedTextBoxColumn)
            {
                mtbcol = dgvc as DataGridViewMaskedTextBoxColumn;
                //
                // get the mask from this instance or the parent column.
                //
                if (string.IsNullOrEmpty(this.mask))
                {
                    mtbec.Mask = mtbcol.Mask;
                }
                else
                {
                    mtbec.Mask = this.mask;
                }
                //
                // prompt char.
                //
                mtbec.PromptChar = this.PromptChar;
                //
                // IncludePrompt
                //
                if (this.includePrompt == DataGridViewTriState.NotSet)
                {
                    //mtbec.IncludePrompt = mtbcol.IncludePrompt;
                }
                else
                {
                    //mtbec.IncludePrompt = BoolFromTri(this.includePrompt);
                }
                //
                // IncludeLiterals
                //
                if (this.includeLiterals == DataGridViewTriState.NotSet)
                {
                    //mtbec.IncludeLiterals = mtbcol.IncludeLiterals;
                }
                else
                {
                    //mtbec.IncludeLiterals = BoolFromTri(this.includeLiterals);
                }
                //
                // Finally, the validating type ...
                //
                if (this.ValidatingType == null)
                {
                    mtbec.ValidatingType = mtbcol.ValidatingType;
                }
                else
                {
                    mtbec.ValidatingType = this.ValidatingType;
                }

                mtbec.Text = (string)this.Value;
            }
        }

        //  Returns the type of the control that will be used for editing
        //  cells of this type.  This control must be a valid Windows Forms
        //  control and must implement IDataGridViewEditingControl.
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewMaskedTextBoxEditingControl);
            }
        }

        //   A string value containing the Mask against input for cells of
        //   this type will be verified.
        public virtual string Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                this.mask = value;
            }
        }

        //  The character to use for prompting for new input.
        //
        public virtual char PromptChar
        {
            get
            {
                return this.promptChar;
            }
            set
            {
                this.promptChar = value;
            }
        }

        //  A boolean indicating whether to include prompt characters in
        //  the Text property's value.
        public virtual DataGridViewTriState IncludePrompt
        {
            get
            {
                return this.includePrompt;
            }
            set
            {
                this.includePrompt = value;
            }
        }

        //  A boolean value indicating whether to include literal characters
        //  in the Text property's output value.
        public virtual DataGridViewTriState IncludeLiterals
        {
            get
            {
                return this.includeLiterals;
            }
            set
            {
                this.includeLiterals = value;
            }
        }

        //  A Type object for the validating type.
        public virtual Type ValidatingType
        {
            get
            {
                return this.validatingType;
            }
            set
            {
                this.validatingType = value;
            }
        }

        //   Quick routine to convert from DataGridViewTriState to boolean.
        //   True goes to true while False and NotSet go to false.
        protected static bool BoolFromTri(DataGridViewTriState tri)
        {
            return (tri == DataGridViewTriState.True) ? true : false;
        }
    }

    //  Identifies the editing control for the DataGridViewMaskedTextBox column type.  It
    //  isn't too much different from a regular DataGridViewMaskedTextBox control,
    //  except that it implements the IDataGridViewEditingControl interface.
    [ToolboxItem(false)]
    public class DataGridViewMaskedTextBoxEditingControl : MaskedTextBox, IDataGridViewEditingControl
    {
        protected int rowIndex;
        protected DataGridView dataGridView;
        protected bool valueChanged = false;

        public DataGridViewMaskedTextBoxEditingControl()
        {
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            // Let the DataGridView know about the value change
            NotifyDataGridViewOfValueChange();
        }

        //  Notify DataGridView that the value has changed.
        protected virtual void NotifyDataGridViewOfValueChange()
        {
            this.valueChanged = true;
            if (this.dataGridView != null)
            {
                this.dataGridView.NotifyCurrentCellDirty(true);
            }
        }

        #region IDataGridViewEditingControl Members

        //  Indicates the cursor that should be shown when the user hovers their
        //  mouse over this cell when the editing control is shown.
        public Cursor EditingPanelCursor
        {
            get
            {
                return Cursors.IBeam;
            }
        }

        //  Returns or sets the parent DataGridView.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }
            set
            {
                this.dataGridView = value;
            }
        }

        //  Sets/Gets the formatted value contents of this cell.
        public object EditingControlFormattedValue
        {
            set
            {
                this.Text = value.ToString();
                NotifyDataGridViewOfValueChange();
            }
            get
            {
                return this.Text;
            }
        }

        //   Get the value of the editing control for formatting.
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        //  Process input key and determine if the key should be used for the editing control
        //  or allowed to be processed by the grid. Handle cursor movement keys for the DataGridViewMaskedTextBox
        //  control; otherwise if the DataGridView doesn't want the input key then let the editing control handle it.
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                    //
                    // If the end of the selection is at the end of the string
                    // let the DataGridView treat the key message
                    //
                    if (!(this.SelectionLength == 0
                          && this.SelectionStart == this.ToString().Length))
                    {
                        return true;
                    }
                    break;

                case Keys.Left:
                    //
                    // If the end of the selection is at the begining of the
                    // string or if the entire text is selected send this character
                    // to the dataGridView; else process the key event.
                    //
                    if (!(this.SelectionLength == 0
                          && this.SelectionStart == 0))
                    {
                        return true;
                    }
                    break;

                case Keys.Home:
                case Keys.End:
                    if (this.SelectionLength != this.ToString().Length)
                    {
                        return true;
                    }
                    break;

                case Keys.Prior:
                case Keys.Next:
                    if (this.valueChanged)
                    {
                        return true;
                    }
                    break;

                case Keys.Delete:
                    if (this.SelectionLength > 0 || this.SelectionStart < this.ToString().Length)
                    {
                        return true;
                    }
                    break;
            }
            //
            // defer to the DataGridView and see if it wants it.
            //
            return !dataGridViewWantsInputKey;
        }

        //  Prepare the editing control for edit.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                SelectAll();
            }
            else
            {
                //
                // Do not select all the text, but position the caret at the
                // end of the text.
                //
                this.SelectionStart = this.ToString().Length;
            }
        }

        //  Indicates whether or not the parent DataGridView control should
        //  reposition the editing control every time value change is indicated.
        //  There is no need to do this for the DataGridViewMaskedTextBox.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        //  Indicates the row index of this cell.  This is often -1 for the
        //  template cell, but for other cells, might actually have a value
        //  greater than or equal to zero.
        public int EditingControlRowIndex
        {
            get
            {
                return this.rowIndex;
            }
            set
            {
                this.rowIndex = value;
            }
        }

        //  Make the DataGridViewMaskedTextBox control match the style and colors of
        //  the host DataGridView control and other editing controls
        //  before showing the editing control.
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            //this.ForeColor = dataGridViewCellStyle.ForeColor;
            //CUSTOM CODE
            this.ForeColor = DataGridViewMaskedTextBoxOptions.foreClr;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.TextAlign = translateAlignment(dataGridViewCellStyle.Alignment);
        }

        //  Gets or sets our flag indicating whether the value has changed.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                this.valueChanged = value;
            }
        }

        #endregion IDataGridViewEditingControl Members

        ///   Routine to translate between DataGridView
        ///   content alignments and text box horizontal alignments.
        private static HorizontalAlignment translateAlignment(DataGridViewContentAlignment align)
        {
            switch (align)
            {
                case DataGridViewContentAlignment.TopLeft:
                case DataGridViewContentAlignment.MiddleLeft:
                case DataGridViewContentAlignment.BottomLeft:
                    return HorizontalAlignment.Left;

                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    return HorizontalAlignment.Center;

                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    return HorizontalAlignment.Right;
            }
            throw new ArgumentException("Error: Invalid Content Alignment!");
        }
    }
}