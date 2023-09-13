//https://stackoverflow.com/questions/2479238/visual-marker-when-moving-rows-on-datagridview

using System.ComponentModel;

namespace DataMigrator.Controls;

internal class KryptonDataGridViewWithDraggableRows : KryptonDataGridView
{
    private int? predictedInsertIndex; //Index to draw divider at.  Null means no divider
    private System.Windows.Forms.Timer autoScrollTimer;
    private int scrollDirection;
    private static DataGridViewRow selectedRow;
    private bool ignoreSelectionChanged;

    private static event EventHandler<EventArgs> OverallSelectionChanged;

    private SolidBrush dividerBrush;
    private Pen selectionPen;

    #region Designer properties

    /// <summary>
    /// The color of the divider displayed between rows while dragging
    /// </summary>
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Appearance")]
    [Description("The color of the divider displayed between rows while dragging")]
    public Color DividerColor
    {
        get => dividerBrush.Color;
        set => dividerBrush = new SolidBrush(value);
    }

    /// <summary>
    /// The color of the border drawn around the selected row
    /// </summary>
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Appearance")]
    [Description("The color of the border drawn around the selected row")]
    public Color SelectionColor
    {
        get => selectionPen.Color;
        set => selectionPen = new Pen(value);
    }

    /// <summary>
    /// Height (in pixels) of the divider to display
    /// </summary>
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Appearance")]
    [Description("Height (in pixels) of the divider to display")]
    [DefaultValue(4)]
    public int DividerHeight { get; set; }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Appearance")]
    [Description("Draw a border around the selected row")]
    [DefaultValue(false)]
    public bool HighlightSelectedRow { get; set; }

    /// <summary>
    /// Width (in pixels) of the border around the selected row
    /// </summary>
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Appearance")]
    [Description("Width (in pixels) of the border around the selected row")]
    [DefaultValue(3)]
    public int SelectionWidth { get; set; }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Behavior")]
    //[Description("")]
    [DefaultValue(DragMode.FullRow)]
    public DragMode DragMode { get; set; }

    #endregion Designer properties

    #region Form setup

    public KryptonDataGridViewWithDraggableRows()
    {
        InitializeProperties();
        SetupTimer();
    }

    private void InitializeProperties()
    {
        #region Designer Code

        this.CellMouseDown += dataGridView_CellMouseDown;
        this.DragOver += dataGridView_DragOver;
        this.DragLeave += dataGridView_DragLeave;
        this.DragEnter += dataGridView_DragEnter;
        this.Paint += dataGridView_Paint_Selection;
        this.Paint += dataGridView_Paint_RowDivider;
        this.DefaultCellStyleChanged += dataGridView_DefaultcellStyleChanged;
        this.Scroll += dataGridView_Scroll;

        #endregion Designer Code

        ignoreSelectionChanged = false;
        OverallSelectionChanged += OnOverallSelectionChanged;
        dividerBrush = new SolidBrush(Color.Red);
        selectionPen = new Pen(Color.Blue);
        DividerHeight = 4;
        SelectionWidth = 3;
    }

    #endregion Form setup

    #region Selection

    /// <summary>
    /// All instances of this class share an event, so that only one row
    /// can be selected throughout all instances.
    /// This method is called when a row is selected on any DataGridView
    /// </summary>
    private void OnOverallSelectionChanged(object sender, EventArgs e)
    {
        if (sender != this && SelectedRows.Count != 0)
        {
            ClearSelection();
            Invalidate();
        }
    }

    protected override void OnSelectionChanged(EventArgs e)
    {
        if (ignoreSelectionChanged)
        {
            return;
        }

        if (SelectedRows.Count != 1 || SelectedRows[0] != selectedRow)
        {
            ignoreSelectionChanged = true; //Following lines cause event to be raised again
            if (selectedRow == null || selectedRow.DataGridView != this)
            {
                ClearSelection();
            }
            else
            {
                selectedRow.Selected = true; //Deny new selection
                OverallSelectionChanged?.Invoke(this, EventArgs.Empty);
            }
            ignoreSelectionChanged = false;
        }
        else
        {
            base.OnSelectionChanged(e);
            OverallSelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SelectRow(int rowIndex)
    {
        selectedRow = Rows[rowIndex];
        selectedRow.Selected = true;
        Invalidate();
    }

    #endregion Selection

    #region Selection highlighting

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_Paint_Selection(object sender, PaintEventArgs e)
    {
        if (!HighlightSelectedRow)
        {
            return;
        }

        if (selectedRow == null || selectedRow.DataGridView != this)
        {
            return;
        }

        var displayRect = GetRowDisplayRectangle(selectedRow.Index, false);
        if (displayRect.Height == 0)
        {
            return;
        }

        selectionPen.Width = SelectionWidth;
        int heightAdjust = (int)Math.Ceiling((float)SelectionWidth / 2);

        e.Graphics.DrawRectangle(
            selectionPen,
            displayRect.X - 1,
            displayRect.Y - heightAdjust,
            displayRect.Width,
            displayRect.Height + SelectionWidth - 1);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_DefaultcellStyleChanged(object sender, EventArgs e)
    {
        DefaultCellStyle.SelectionBackColor = DefaultCellStyle.BackColor;
        DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_Scroll(object sender, ScrollEventArgs e)
    {
        Invalidate();
    }

    #endregion Selection highlighting

    #region Drag-and-drop

    protected override void OnDragDrop(DragEventArgs args)
    {
        if (args.Effect == DragDropEffects.None)
        {
            return;
        }

        //Convert to coordinates within client (instead of screen-coordinates)
        var clientPoint = PointToClient(new Point(args.X, args.Y));

        //Get index of row to insert into
        var dragFromRow = (DataGridViewRow)args.Data.GetData(typeof(DataGridViewRow));
        int newRowIndex = GetNewRowIndex(clientPoint.Y);

        //Adjust index if both rows belong to same DataGridView, due to removal of row
        if (dragFromRow.DataGridView == this && dragFromRow.Index < newRowIndex)
        {
            newRowIndex--;
        }

        //Clean up
        RemoveHighlighting();
        autoScrollTimer.Enabled = false;

        //Only go through the trouble if we're actually moving the row
        if (dragFromRow.DataGridView != this || newRowIndex != dragFromRow.Index)
        {
            //Insert the row
            MoveDraggedRow(dragFromRow, newRowIndex);

            //Let everyone know the selection has changed
            SelectRow(newRowIndex);
        }
        base.OnDragDrop(args);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_DragLeave(object sender, EventArgs e1)
    {
        RemoveHighlighting();
        autoScrollTimer.Enabled = false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_DragEnter(object sender, DragEventArgs e)
    {
        e.Effect = e.Data.GetDataPresent(typeof(DataGridViewRow))
            ? DragDropEffects.Move
            : DragDropEffects.None;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_DragOver(object sender, DragEventArgs e)
    {
        if (e.Effect == DragDropEffects.None)
        {
            return;
        }

        var clientPoint = PointToClient(new Point(e.X, e.Y));

        //Note: For some reason, HitTest is failing when clientPoint.Y = dataGridView1.Height-1.
        // I have no idea why.
        // clientPoint.Y is always 0 <= clientPoint.Y < dataGridView1.Height
        if (clientPoint.Y < Height - 1)
        {
            int newRowIndex = GetNewRowIndex(clientPoint.Y);
            HighlightInsertPosition(newRowIndex);
            StartAutoscrollTimer(e);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        bool shouldDrag = DragMode == DragMode.FirstCell
            ? e.Button == MouseButtons.Left && e.RowIndex >= 0 && e.ColumnIndex == 0
            : e.Button == MouseButtons.Left && e.RowIndex >= 0;

        if (shouldDrag)
        {
            SelectRow(e.RowIndex);
            var dragObject = Rows[e.RowIndex];
            DoDragDrop(dragObject, DragDropEffects.Move);
            //TODO: Any way to make this *not* happen if they only click?
        }
    }

    /// <summary>
    /// Based on the mouse position, determines where the new row would
    /// be inserted if the user were to release the mouse-button right now
    /// </summary>
    /// <param name="clientY">
    /// The y-coordinate of the mouse, given with respectto the control
    /// (not the screen)
    /// </param>
    private int GetNewRowIndex(int clientY)
    {
        int lastRowIndex = Rows.Count - 1;

        //DataGridView has no cells
        if (Rows.Count == 0)
        {
            return 0;
        }

        //Dragged above the DataGridView
        if (clientY < GetRowDisplayRectangle(0, true).Top)
        {
            return 0;
        }

        //Dragged below the DataGridView
        int bottom = GetRowDisplayRectangle(lastRowIndex, true).Bottom;
        if (bottom > 0 && clientY >= bottom)
        {
            return lastRowIndex + 1;
        }

        //Dragged onto one of the cells.  Depending on where in cell,
        // insert before or after row.
        var hitTest = HitTest(2, clientY); //Don't care about X coordinate

        if (hitTest.RowIndex == -1)
        {
            //This should only happen when midway scrolled down the page,
            //and user drags over header-columns
            //Grab the index of the current top (displayed) row
            return FirstDisplayedScrollingRowIndex;
        }

        //If we are hovering over the upper-quarter of the row, place above;
        // otherwise below.  Experimenting shows that placing above at 1/4
        //works better than at 1/2 or always below
        return clientY < GetRowDisplayRectangle(hitTest.RowIndex, false).Top + (Rows[hitTest.RowIndex].Height / 4)
            ? hitTest.RowIndex
            : hitTest.RowIndex + 1;
    }

    private void MoveDraggedRow(DataGridViewRow dragFromRow, int newRowIndex)
    {
        dragFromRow.DataGridView.Rows.Remove(dragFromRow);
        Rows.Insert(newRowIndex, dragFromRow);
    }

    #endregion Drag-and-drop

    #region Drop-and-drop highlighting

    //Draw the actual row-divider
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void dataGridView_Paint_RowDivider(object sender, PaintEventArgs e)
    {
        if (predictedInsertIndex != null)
        {
            e.Graphics.FillRectangle(dividerBrush, GetHighlightRectangle());
        }
    }

    private Rectangle GetHighlightRectangle()
    {
        int width = DisplayRectangle.Width - 2;

        int relativeY = predictedInsertIndex > 0
            ? GetRowDisplayRectangle((int)predictedInsertIndex - 1, false).Bottom
            : Columns[0].HeaderCell.Size.Height;

        if (relativeY == 0)
        {
            relativeY = GetRowDisplayRectangle(FirstDisplayedScrollingRowIndex, true).Top;
        }

        int locationX = Location.X + 1;
        int locationY = relativeY - (int)Math.Ceiling((double)DividerHeight / 2);
        return new Rectangle(locationX, locationY, width, DividerHeight);
    }

    private void HighlightInsertPosition(int rowIndex)
    {
        if (predictedInsertIndex == rowIndex)
        {
            return;
        }

        var oldRect = GetHighlightRectangle();
        predictedInsertIndex = rowIndex;
        var newRect = GetHighlightRectangle();

        Invalidate(oldRect);
        Invalidate(newRect);
    }

    private void RemoveHighlighting()
    {
        if (predictedInsertIndex != null)
        {
            var oldRect = GetHighlightRectangle();
            predictedInsertIndex = null;
            Invalidate(oldRect);
        }
        else
        {
            Invalidate();
        }
    }

    #endregion Drop-and-drop highlighting

    #region Autoscroll

    private void SetupTimer()
    {
        autoScrollTimer = new System.Windows.Forms.Timer
        {
            Interval = 250,
            Enabled = false
        };
        autoScrollTimer.Tick += OnAutoscrollTimerTick;
    }

    private void StartAutoscrollTimer(DragEventArgs args)
    {
        var position = PointToClient(new Point(args.X, args.Y));

        if (position.Y <= Font.Height / 2 && FirstDisplayedScrollingRowIndex > 0)
        {
            //Near top, scroll up
            scrollDirection = -1;
            autoScrollTimer.Enabled = true;
        }
        else if (position.Y >= ClientSize.Height - (Font.Height / 2) && FirstDisplayedScrollingRowIndex < Rows.Count - 1)
        {
            //Near bottom, scroll down
            scrollDirection = 1;
            autoScrollTimer.Enabled = true;
        }
        else
        {
            autoScrollTimer.Enabled = false;
        }
    }

    private void OnAutoscrollTimerTick(object sender, EventArgs e)
    {
        //Scroll up/down
        FirstDisplayedScrollingRowIndex += scrollDirection;
    }

    #endregion Autoscroll

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        autoScrollTimer?.Dispose();
        dividerBrush?.Dispose();
        selectionPen?.Dispose();
    }
}

public enum DragMode : byte
{
    FullRow = 0,

    /// <summary>
    /// Useful if you have an editable grid.. make only the first cell draggable.
    /// Recommend using an image column with an icon for that purpose.
    /// </summary>
    FirstCell = 1
}