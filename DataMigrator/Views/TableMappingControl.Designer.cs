namespace DataMigrator.Views;

partial class TableMappingControl
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
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.dgvSource = new Krypton.Toolkit.KryptonDataGridView();
        this.dgvMappings = new Krypton.Toolkit.KryptonDataGridView();
        this.dgvDestination = new Krypton.Toolkit.KryptonDataGridView();
        this.btnAutoMap = new Krypton.Toolkit.KryptonButton();
        this.btnAdd = new Krypton.Toolkit.KryptonButton();
        this.btnRemove = new Krypton.Toolkit.KryptonButton();
        this.cmbSourceTable = new Krypton.Toolkit.KryptonComboBox();
        this.cmbDestinationTable = new Krypton.Toolkit.KryptonComboBox();
        this.btnCreateTable = new Krypton.Toolkit.KryptonButton();
        this.pGridSource = new System.Windows.Forms.PropertyGrid();
        this.pGridDestination = new System.Windows.Forms.PropertyGrid();
        ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvDestination)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbSourceTable)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationTable)).BeginInit();
        this.SuspendLayout();
        // 
        // dgvSource
        // 
        this.dgvSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left)));
        this.dgvSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvSource.Location = new System.Drawing.Point(13, 40);
        this.dgvSource.Name = "dgvSource";
        this.dgvSource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvSource.Size = new System.Drawing.Size(240, 351);
        this.dgvSource.TabIndex = 1;
        this.dgvSource.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSource_CellMouseClick);
        this.dgvSource.SelectionChanged += new System.EventHandler(this.dgvSource_SelectionChanged);
        // 
        // dgvMappings
        // 
        this.dgvMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvMappings.Location = new System.Drawing.Point(259, 46);
        this.dgvMappings.Name = "dgvMappings";
        this.dgvMappings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvMappings.Size = new System.Drawing.Size(399, 420);
        this.dgvMappings.TabIndex = 2;
        // 
        // dgvDestination
        // 
        this.dgvDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.dgvDestination.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvDestination.Location = new System.Drawing.Point(664, 40);
        this.dgvDestination.Name = "dgvDestination";
        this.dgvDestination.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvDestination.Size = new System.Drawing.Size(240, 351);
        this.dgvDestination.TabIndex = 4;
        this.dgvDestination.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDestination_CellMouseClick);
        this.dgvDestination.SelectionChanged += new System.EventHandler(this.dgvDestination_SelectionChanged);
        // 
        // btnAutoMap
        // 
        this.btnAutoMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.btnAutoMap.Location = new System.Drawing.Point(259, 472);
        this.btnAutoMap.Name = "btnAutoMap";
        this.btnAutoMap.Size = new System.Drawing.Size(399, 29);
        this.btnAutoMap.TabIndex = 5;
        this.btnAutoMap.Values.Text = "Auto Map";
        this.btnAutoMap.Click += new System.EventHandler(this.btnAutoMap_Click);
        // 
        // btnAdd
        // 
        this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.btnAdd.Location = new System.Drawing.Point(259, 507);
        this.btnAdd.Name = "btnAdd";
        this.btnAdd.Size = new System.Drawing.Size(399, 29);
        this.btnAdd.TabIndex = 6;
        this.btnAdd.Values.Text = "Add";
        this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        // 
        // btnRemove
        // 
        this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.btnRemove.Location = new System.Drawing.Point(259, 542);
        this.btnRemove.Name = "btnRemove";
        this.btnRemove.Size = new System.Drawing.Size(399, 29);
        this.btnRemove.TabIndex = 7;
        this.btnRemove.Values.Text = "Remove";
        this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
        // 
        // cmbSourceTable
        // 
        this.cmbSourceTable.DropDownWidth = 240;
        this.cmbSourceTable.FormattingEnabled = true;
        this.cmbSourceTable.Location = new System.Drawing.Point(13, 13);
        this.cmbSourceTable.Name = "cmbSourceTable";
        this.cmbSourceTable.Size = new System.Drawing.Size(240, 21);
        this.cmbSourceTable.Sorted = true;
        this.cmbSourceTable.TabIndex = 0;
        this.cmbSourceTable.SelectedIndexChanged += new System.EventHandler(this.cmbSourceTable_SelectedIndexChanged);
        // 
        // cmbDestinationTable
        // 
        this.cmbDestinationTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.cmbDestinationTable.DropDownWidth = 240;
        this.cmbDestinationTable.FormattingEnabled = true;
        this.cmbDestinationTable.Location = new System.Drawing.Point(664, 13);
        this.cmbDestinationTable.Name = "cmbDestinationTable";
        this.cmbDestinationTable.Size = new System.Drawing.Size(240, 21);
        this.cmbDestinationTable.Sorted = true;
        this.cmbDestinationTable.TabIndex = 3;
        this.cmbDestinationTable.SelectedIndexChanged += new System.EventHandler(this.cmbDestinationTable_SelectedIndexChanged);
        // 
        // btnCreateTable
        // 
        this.btnCreateTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCreateTable.Location = new System.Drawing.Point(259, 11);
        this.btnCreateTable.Name = "btnCreateTable";
        this.btnCreateTable.Size = new System.Drawing.Size(399, 29);
        this.btnCreateTable.TabIndex = 8;
        this.btnCreateTable.Values.Text = "Create Destination Table";
        this.btnCreateTable.Click += new System.EventHandler(this.btnCreateTable_Click);
        // 
        // pGridSource
        // 
        this.pGridSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.pGridSource.HelpVisible = false;
        this.pGridSource.Location = new System.Drawing.Point(13, 397);
        this.pGridSource.Name = "pGridSource";
        this.pGridSource.Size = new System.Drawing.Size(240, 190);
        this.pGridSource.TabIndex = 9;
        this.pGridSource.ToolbarVisible = false;
        // 
        // pGridDestination
        // 
        this.pGridDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.pGridDestination.HelpVisible = false;
        this.pGridDestination.Location = new System.Drawing.Point(664, 397);
        this.pGridDestination.Name = "pGridDestination";
        this.pGridDestination.Size = new System.Drawing.Size(240, 190);
        this.pGridDestination.TabIndex = 10;
        this.pGridDestination.ToolbarVisible = false;
        // 
        // TableMappingControl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.pGridDestination);
        this.Controls.Add(this.pGridSource);
        this.Controls.Add(this.btnCreateTable);
        this.Controls.Add(this.cmbDestinationTable);
        this.Controls.Add(this.cmbSourceTable);
        this.Controls.Add(this.btnRemove);
        this.Controls.Add(this.btnAdd);
        this.Controls.Add(this.btnAutoMap);
        this.Controls.Add(this.dgvDestination);
        this.Controls.Add(this.dgvMappings);
        this.Controls.Add(this.dgvSource);
        this.MinimumSize = new System.Drawing.Size(759, 510);
        this.Name = "TableMappingControl";
        this.Size = new System.Drawing.Size(918, 595);
        ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvDestination)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbSourceTable)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationTable)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private KryptonDataGridView dgvSource;
    private KryptonDataGridView dgvMappings;
    private KryptonDataGridView dgvDestination;
    private KryptonButton btnAutoMap;
    private KryptonButton btnAdd;
    private KryptonButton btnRemove;
    private KryptonComboBox cmbSourceTable;
    private KryptonComboBox cmbDestinationTable;
    private KryptonButton btnCreateTable;
    private System.Windows.Forms.PropertyGrid pGridSource;
    private System.Windows.Forms.PropertyGrid pGridDestination;
}
