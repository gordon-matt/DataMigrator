using DataMigrator.Controls;

namespace DataMigrator.Views;

partial class RunJobsForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
        cancellationTokenSource?.Dispose();
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView = new DataMigrator.Controls.KryptonDataGridViewWithDraggableRows();
            this.DragColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.RunColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRun = new Krypton.Toolkit.KryptonButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.lblInfo = new Krypton.Toolkit.KryptonLabel();
            this.cbSelectAll = new Krypton.Toolkit.KryptonCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowDrop = true;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DragColumn,
            this.RunColumn,
            this.NameColumn,
            this.StatusColumn});
            this.dataGridView.DividerColor = System.Drawing.Color.LightSteelBlue;
            this.dataGridView.DividerHeight = 3;
            this.dataGridView.DragMode = DataMigrator.Controls.DragMode.FirstCell;
            this.dataGridView.Location = new System.Drawing.Point(14, 38);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.RowTemplate.Height = 32;
            this.dataGridView.SelectionColor = System.Drawing.Color.Blue;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.SelectionWidth = 0;
            this.dataGridView.Size = new System.Drawing.Size(750, 323);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            // 
            // DragColumn
            // 
            this.DragColumn.HeaderText = "";
            this.DragColumn.Name = "DragColumn";
            this.DragColumn.Width = 7;
            // 
            // RunColumn
            // 
            this.RunColumn.HeaderText = "Run";
            this.RunColumn.Name = "RunColumn";
            this.RunColumn.Width = 38;
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.Width = 68;
            // 
            // StatusColumn
            // 
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            this.StatusColumn.Width = 68;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.CornerRoundingRadius = -1F;
            this.btnRun.Location = new System.Drawing.Point(424, 407);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(167, 43);
            this.btnRun.TabIndex = 4;
            this.btnRun.Values.Image = global::DataMigrator.Resources.Play_32x32;
            this.btnRun.Values.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(14, 407);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(402, 43);
            this.progressBar.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CornerRoundingRadius = -1F;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(597, 407);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(167, 43);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Values.Image = global::DataMigrator.Resources.Cancel_32x32;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.LabelStyle = Krypton.Toolkit.LabelStyle.ToolTip;
            this.lblInfo.Location = new System.Drawing.Point(14, 365);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(649, 36);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Values.Image = global::DataMigrator.Resources.Info_32x32;
            this.lblInfo.Values.Text = "Jobs can be re-ordered by dragging and dropping them in the order you want them t" +
    "o run. Use the icon in the first column.";
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.Location = new System.Drawing.Point(14, 12);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(70, 18);
            this.cbSelectAll.TabIndex = 0;
            this.cbSelectAll.Values.Text = "Select All";
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // RunJobsForm
            // 
            this.AcceptButton = this.btnRun;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(778, 457);
            this.Controls.Add(this.cbSelectAll);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.dataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = global::DataMigrator.Resources.MigrateIcon;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "RunJobsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Run Job/s";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private KryptonDataGridViewWithDraggableRows dataGridView;
    private KryptonButton btnRun;
    private System.Windows.Forms.ProgressBar progressBar;
    private KryptonButton btnCancel;
    private KryptonLabel lblInfo;
    private KryptonCheckBox cbSelectAll;
    private DataGridViewImageColumn DragColumn;
    private DataGridViewCheckBoxColumn RunColumn;
    private DataGridViewTextBoxColumn NameColumn;
    private DataGridViewTextBoxColumn StatusColumn;
}