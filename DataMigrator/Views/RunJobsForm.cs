namespace DataMigrator.Views;

public partial class RunJobsForm : KryptonForm
{
    private const string COLUMN_RUN = "Run";
    private const string COLUMN_NAME = "Name";
    private const string COLUMN_STATUS = "Status";

    private CancellationTokenSource cancellationTokenSource;

    public RunJobsForm()
    {
        InitializeComponent();

        foreach (var job in AppState.ConfigFile.Jobs.OrderBy(x => x.Order))
        {
            dataGridView.Rows.Add(Constants.ImageBytes.MoveGrabber_32x32, false, job.Name, "Pending");
        }
    }

    private async void btnRun_Click(object sender, EventArgs e)
    {
        btnRun.Enabled = false;
        btnCancel.Enabled = true;

        progressBar.Value = 0;
        await DoWorkAsync();

        TraceService.Instance.WriteMessage(TraceEvent.Information, "Completed");
        progressBar.Value = 0;
        btnRun.Enabled = true;
        btnCancel.Enabled = false;

        cancellationTokenSource?.Dispose();
    }

    private async Task DoWorkAsync()
    {
        cancellationTokenSource = new CancellationTokenSource();

        var progressHandler = new Progress<int>(value =>
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.BeginInvoke(
                    (MethodInvoker)delegate ()
                    {
                        progressBar.Value = value;
                        progressBar.Refresh();
                    });
            }
            else
            {
                progressBar.Value = value;
                progressBar.Refresh();
            }
        });

        AppState.ConfigFile.Save(); // Ensure any reordering is persisted..

        //foreach (DataGridViewRow row in dataGridView.Rows.OfType<DataGridViewRow>().OrderBy(x => x.Index)) // Shouldn't be necessary to manually order
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            if (bool.Parse(row.Cells[RunColumn.Index].Value.ToString()))
            {
                string jobName = row.Cells[NameColumn.Index].Value.ToString();
                var job = AppState.ConfigFile.Jobs[jobName];

                if (job == null)
                {
                    TraceService.Instance.WriteConcat(TraceEvent.Warning, "Could not find job, '", jobName, "'");
                    continue;
                }

                try
                {
                    row.Cells[StatusColumn.Index].Value = "Running";

                    await Controller.RunJobAsync(job, progressHandler, cancellationTokenSource.Token);

                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        row.Cells[StatusColumn.Index].Value = "Cancelled";
                        TraceService.Instance.WriteConcat(TraceEvent.Information, "User cancelled job");
                        return;
                    }
                    else
                    {
                        row.Cells[StatusColumn.Index].Value = "Completed";
                    }
                }
                catch (Exception x)
                {
                    row.Cells[StatusColumn.Index].Value = "Error";
                    TraceService.Instance.WriteException(x, string.Concat("Job Name: ", jobName));
                }
            }
        }
    }

    private void btnCancel_Click(object sender, EventArgs e) => cancellationTokenSource.Cancel();

    private void dataGridView_DragDrop(object sender, DragEventArgs e)
    {
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            string jobName = row.Cells[NameColumn.Index].Value.ToString();
            var job = AppState.ConfigFile.Jobs[jobName];
            job.Order = row.Index;
        }
    }

    private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            var cell = (DataGridViewCheckBoxCell)row.Cells[RunColumn.Index];
            cell.Value = cbSelectAll.Checked;
        }
    }

    //#region Drag-Drop

    //// Based on: https://stackoverflow.com/questions/1620947/how-could-i-drag-and-drop-datagridview-rows-under-each-other

    //private Rectangle mouseDownDragBox;
    //private int sourceRowIndex;
    //private int destinationRowIndex;

    //private int lastDragOverIndex;

    //private void dataGridView_DragOver(object sender, DragEventArgs e)
    //{
    //    e.Effect = DragDropEffects.Move;
    //}

    //private void dataGridView_DragDrop(object sender, DragEventArgs e)
    //{
    //    // The mouse locations are relative to the screen, so they must be
    //    // converted to client coordinates.
    //    var clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));

    //    // Get the row index of the item the mouse is below.
    //    destinationRowIndex = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

    //    // If the drag operation was a move then remove and insert the row.
    //    if (e.Effect == DragDropEffects.Move)
    //    {
    //        if (destinationRowIndex < 0) { return; }

    //        var rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
    //        rowToMove.DefaultCellStyle.BackColor = Color.Empty;

    //        dataGridView.Rows.RemoveAt(sourceRowIndex);
    //        dataGridView.Rows.Insert(destinationRowIndex, rowToMove);
    //    }
    //}

    //private void dataGridView_MouseMove(object sender, MouseEventArgs e)
    //{
    //    if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
    //    {
    //        // If the mouse moves outside the rectangle, start the drag.
    //        if (mouseDownDragBox != Rectangle.Empty && !mouseDownDragBox.Contains(e.X, e.Y))
    //        {
    //            // Proceed with the drag and drop, passing in the list item.
    //            dataGridView.Rows[sourceRowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
    //            dataGridView.DoDragDrop(dataGridView.Rows[sourceRowIndex], DragDropEffects.Move);
    //        }
    //    }
    //    dataGridView.Invalidate();
    //}

    //private void dataGridView_MouseDown(object sender, MouseEventArgs e)
    //{
    //    // Get the index of the item the mouse is below.
    //    sourceRowIndex = dataGridView.HitTest(e.X, e.Y).RowIndex;
    //    if (sourceRowIndex != -1)
    //    {
    //        // Remember the point where the mouse down occurred.
    //        // The DragSize indicates the size that the mouse can move
    //        // before a drag event should be started.
    //        var dragSize = SystemInformation.DragSize;

    //        // Create a rectangle using the DragSize, with the mouse position being
    //        // at the center of the rectangle.
    //        mouseDownDragBox = new Rectangle(
    //            new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
    //    }
    //    else
    //    {
    //        // Reset the rectangle if the mouse is not over an item in the ListBox.
    //        mouseDownDragBox = Rectangle.Empty;
    //    }
    //}

    //#endregion Drag-Drop
}