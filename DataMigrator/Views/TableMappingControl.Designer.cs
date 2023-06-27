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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvSource = new Krypton.Toolkit.KryptonDataGridView();
            this.dgvMappings = new Krypton.Toolkit.KryptonDataGridView();
            this.dgvMappings_Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMappings_Destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMappings_Script = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvDestination = new Krypton.Toolkit.KryptonDataGridView();
            this.btnAutoMap = new Krypton.Toolkit.KryptonButton();
            this.btnAdd = new Krypton.Toolkit.KryptonButton();
            this.btnRemove = new Krypton.Toolkit.KryptonButton();
            this.cmbSourceTable = new Krypton.Toolkit.KryptonComboBox();
            this.cmbDestinationTable = new Krypton.Toolkit.KryptonComboBox();
            this.btnCreateTable = new Krypton.Toolkit.KryptonButton();
            this.pGridSource = new System.Windows.Forms.PropertyGrid();
            this.pGridDestination = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1_2 = new System.Windows.Forms.Panel();
            this.panel1_1 = new System.Windows.Forms.Panel();
            this.panel1_3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel2_2 = new System.Windows.Forms.Panel();
            this.panel2_3 = new System.Windows.Forms.Panel();
            this.btnAddEditScript = new Krypton.Toolkit.KryptonButton();
            this.panel2_1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel3_2 = new System.Windows.Forms.Panel();
            this.panel3_3 = new System.Windows.Forms.Panel();
            this.panel3_1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDestination)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationTable)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel1_2.SuspendLayout();
            this.panel1_1.SuspendLayout();
            this.panel1_3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel2_2.SuspendLayout();
            this.panel2_3.SuspendLayout();
            this.panel2_1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel3_2.SuspendLayout();
            this.panel3_3.SuspendLayout();
            this.panel3_1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSource
            // 
            this.dgvSource.AllowUserToAddRows = false;
            this.dgvSource.AllowUserToDeleteRows = false;
            this.dgvSource.AllowUserToResizeRows = false;
            this.dgvSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSource.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSource.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSource.Location = new System.Drawing.Point(0, 0);
            this.dgvSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvSource.MultiSelect = false;
            this.dgvSource.Name = "dgvSource";
            this.dgvSource.RowHeadersVisible = false;
            this.dgvSource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSource.Size = new System.Drawing.Size(261, 443);
            this.dgvSource.TabIndex = 1;
            this.dgvSource.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSource_CellMouseClick);
            this.dgvSource.SelectionChanged += new System.EventHandler(this.dgvSource_SelectionChanged);
            // 
            // dgvMappings
            // 
            this.dgvMappings.AllowUserToAddRows = false;
            this.dgvMappings.AllowUserToDeleteRows = false;
            this.dgvMappings.AllowUserToResizeRows = false;
            this.dgvMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMappings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvMappings_Source,
            this.dgvMappings_Destination,
            this.dgvMappings_Script});
            this.dgvMappings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMappings.Location = new System.Drawing.Point(0, 0);
            this.dgvMappings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvMappings.MultiSelect = false;
            this.dgvMappings.Name = "dgvMappings";
            this.dgvMappings.RowHeadersVisible = false;
            this.dgvMappings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMappings.Size = new System.Drawing.Size(529, 555);
            this.dgvMappings.TabIndex = 2;
            // 
            // dgvMappings_Source
            // 
            this.dgvMappings_Source.DataPropertyName = "Source";
            this.dgvMappings_Source.HeaderText = "Source";
            this.dgvMappings_Source.Name = "dgvMappings_Source";
            this.dgvMappings_Source.Width = 72;
            // 
            // dgvMappings_Destination
            // 
            this.dgvMappings_Destination.DataPropertyName = "Destination";
            this.dgvMappings_Destination.HeaderText = "Destination";
            this.dgvMappings_Destination.Name = "dgvMappings_Destination";
            this.dgvMappings_Destination.Width = 96;
            // 
            // dgvMappings_Script
            // 
            this.dgvMappings_Script.DataPropertyName = "Script";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = null;
            this.dgvMappings_Script.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMappings_Script.HeaderText = "Script";
            this.dgvMappings_Script.Name = "dgvMappings_Script";
            this.dgvMappings_Script.Width = 47;
            // 
            // dgvDestination
            // 
            this.dgvDestination.AllowUserToAddRows = false;
            this.dgvDestination.AllowUserToDeleteRows = false;
            this.dgvDestination.AllowUserToResizeRows = false;
            this.dgvDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDestination.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDestination.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDestination.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDestination.Location = new System.Drawing.Point(0, 0);
            this.dgvDestination.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvDestination.MultiSelect = false;
            this.dgvDestination.Name = "dgvDestination";
            this.dgvDestination.RowHeadersVisible = false;
            this.dgvDestination.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDestination.Size = new System.Drawing.Size(263, 443);
            this.dgvDestination.TabIndex = 4;
            this.dgvDestination.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDestination_CellMouseClick);
            this.dgvDestination.SelectionChanged += new System.EventHandler(this.dgvDestination_SelectionChanged);
            // 
            // btnAutoMap
            // 
            this.btnAutoMap.CornerRoundingRadius = -1F;
            this.btnAutoMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoMap.Location = new System.Drawing.Point(4, 49);
            this.btnAutoMap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAutoMap.Name = "btnAutoMap";
            this.btnAutoMap.Size = new System.Drawing.Size(256, 41);
            this.btnAutoMap.TabIndex = 5;
            this.btnAutoMap.Values.Text = "Auto Map";
            this.btnAutoMap.Click += new System.EventHandler(this.btnAutoMap_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.CornerRoundingRadius = -1F;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Location = new System.Drawing.Point(4, 3);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(256, 40);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Values.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.CornerRoundingRadius = -1F;
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.Location = new System.Drawing.Point(268, 3);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(257, 40);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Values.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // cmbSourceTable
            // 
            this.cmbSourceTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSourceTable.CornerRoundingRadius = -1F;
            this.cmbSourceTable.DropDownWidth = 240;
            this.cmbSourceTable.FormattingEnabled = true;
            this.cmbSourceTable.IntegralHeight = false;
            this.cmbSourceTable.Location = new System.Drawing.Point(0, 0);
            this.cmbSourceTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSourceTable.Name = "cmbSourceTable";
            this.cmbSourceTable.Size = new System.Drawing.Size(261, 21);
            this.cmbSourceTable.Sorted = true;
            this.cmbSourceTable.TabIndex = 0;
            this.cmbSourceTable.SelectedIndexChanged += new System.EventHandler(this.cmbSourceTable_SelectedIndexChanged);
            // 
            // cmbDestinationTable
            // 
            this.cmbDestinationTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDestinationTable.CornerRoundingRadius = -1F;
            this.cmbDestinationTable.DropDownWidth = 240;
            this.cmbDestinationTable.FormattingEnabled = true;
            this.cmbDestinationTable.IntegralHeight = false;
            this.cmbDestinationTable.Location = new System.Drawing.Point(0, 0);
            this.cmbDestinationTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDestinationTable.Name = "cmbDestinationTable";
            this.cmbDestinationTable.Size = new System.Drawing.Size(263, 21);
            this.cmbDestinationTable.Sorted = true;
            this.cmbDestinationTable.TabIndex = 3;
            this.cmbDestinationTable.SelectedIndexChanged += new System.EventHandler(this.cmbDestinationTable_SelectedIndexChanged);
            // 
            // btnCreateTable
            // 
            this.btnCreateTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateTable.CornerRoundingRadius = -1F;
            this.btnCreateTable.Location = new System.Drawing.Point(0, 0);
            this.btnCreateTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCreateTable.Name = "btnCreateTable";
            this.btnCreateTable.Size = new System.Drawing.Size(529, 33);
            this.btnCreateTable.TabIndex = 8;
            this.btnCreateTable.Values.Text = "Create Destination Table";
            this.btnCreateTable.Click += new System.EventHandler(this.btnCreateTable_Click);
            // 
            // pGridSource
            // 
            this.pGridSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pGridSource.HelpVisible = false;
            this.pGridSource.Location = new System.Drawing.Point(0, 0);
            this.pGridSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pGridSource.Name = "pGridSource";
            this.pGridSource.Size = new System.Drawing.Size(261, 205);
            this.pGridSource.TabIndex = 9;
            this.pGridSource.ToolbarVisible = false;
            // 
            // pGridDestination
            // 
            this.pGridDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pGridDestination.HelpVisible = false;
            this.pGridDestination.Location = new System.Drawing.Point(0, 0);
            this.pGridDestination.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pGridDestination.Name = "pGridDestination";
            this.pGridDestination.Size = new System.Drawing.Size(263, 205);
            this.pGridDestination.TabIndex = 10;
            this.pGridDestination.ToolbarVisible = false;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1071, 687);
            this.tableLayoutPanel.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel1_2);
            this.panel1.Controls.Add(this.panel1_1);
            this.panel1.Controls.Add(this.panel1_3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(261, 681);
            this.panel1.TabIndex = 0;
            // 
            // panel1_2
            // 
            this.panel1_2.Controls.Add(this.dgvSource);
            this.panel1_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1_2.Location = new System.Drawing.Point(0, 33);
            this.panel1_2.Name = "panel1_2";
            this.panel1_2.Size = new System.Drawing.Size(261, 443);
            this.panel1_2.TabIndex = 12;
            // 
            // panel1_1
            // 
            this.panel1_1.Controls.Add(this.cmbSourceTable);
            this.panel1_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1_1.Location = new System.Drawing.Point(0, 0);
            this.panel1_1.Name = "panel1_1";
            this.panel1_1.Size = new System.Drawing.Size(261, 33);
            this.panel1_1.TabIndex = 11;
            // 
            // panel1_3
            // 
            this.panel1_3.Controls.Add(this.pGridSource);
            this.panel1_3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1_3.Location = new System.Drawing.Point(0, 476);
            this.panel1_3.Name = "panel1_3";
            this.panel1_3.Size = new System.Drawing.Size(261, 205);
            this.panel1_3.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel2_2);
            this.panel2.Controls.Add(this.panel2_3);
            this.panel2.Controls.Add(this.panel2_1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(270, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(529, 681);
            this.panel2.TabIndex = 1;
            // 
            // panel2_2
            // 
            this.panel2_2.Controls.Add(this.dgvMappings);
            this.panel2_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2_2.Location = new System.Drawing.Point(0, 33);
            this.panel2_2.Name = "panel2_2";
            this.panel2_2.Size = new System.Drawing.Size(529, 555);
            this.panel2_2.TabIndex = 11;
            // 
            // panel2_3
            // 
            this.panel2_3.Controls.Add(this.tableLayoutPanel1);
            this.panel2_3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2_3.Location = new System.Drawing.Point(0, 588);
            this.panel2_3.Name = "panel2_3";
            this.panel2_3.Size = new System.Drawing.Size(529, 93);
            this.panel2_3.TabIndex = 10;
            // 
            // btnAddEditScript
            // 
            this.btnAddEditScript.CornerRoundingRadius = -1F;
            this.btnAddEditScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddEditScript.Location = new System.Drawing.Point(268, 49);
            this.btnAddEditScript.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAddEditScript.Name = "btnAddEditScript";
            this.btnAddEditScript.Size = new System.Drawing.Size(257, 41);
            this.btnAddEditScript.TabIndex = 8;
            this.btnAddEditScript.Values.Text = "Add/Edit Script";
            this.btnAddEditScript.Click += new System.EventHandler(this.btnAddEditScript_Click);
            // 
            // panel2_1
            // 
            this.panel2_1.Controls.Add(this.btnCreateTable);
            this.panel2_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2_1.Location = new System.Drawing.Point(0, 0);
            this.panel2_1.Name = "panel2_1";
            this.panel2_1.Size = new System.Drawing.Size(529, 33);
            this.panel2_1.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel3_2);
            this.panel3.Controls.Add(this.panel3_3);
            this.panel3.Controls.Add(this.panel3_1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(805, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(263, 681);
            this.panel3.TabIndex = 2;
            // 
            // panel3_2
            // 
            this.panel3_2.Controls.Add(this.dgvDestination);
            this.panel3_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3_2.Location = new System.Drawing.Point(0, 33);
            this.panel3_2.Name = "panel3_2";
            this.panel3_2.Size = new System.Drawing.Size(263, 443);
            this.panel3_2.TabIndex = 13;
            // 
            // panel3_3
            // 
            this.panel3_3.Controls.Add(this.pGridDestination);
            this.panel3_3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3_3.Location = new System.Drawing.Point(0, 476);
            this.panel3_3.Name = "panel3_3";
            this.panel3_3.Size = new System.Drawing.Size(263, 205);
            this.panel3_3.TabIndex = 12;
            // 
            // panel3_1
            // 
            this.panel3_1.Controls.Add(this.cmbDestinationTable);
            this.panel3_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3_1.Location = new System.Drawing.Point(0, 0);
            this.panel3_1.Name = "panel3_1";
            this.panel3_1.Size = new System.Drawing.Size(263, 33);
            this.panel3_1.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddEditScript, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRemove, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAutoMap, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(529, 93);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // TableMappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(886, 588);
            this.Name = "TableMappingControl";
            this.Size = new System.Drawing.Size(1071, 687);
            this.Load += new System.EventHandler(this.TableMappingControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDestination)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationTable)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1_2.ResumeLayout(false);
            this.panel1_1.ResumeLayout(false);
            this.panel1_3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2_2.ResumeLayout(false);
            this.panel2_3.ResumeLayout(false);
            this.panel2_1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3_2.ResumeLayout(false);
            this.panel3_3.ResumeLayout(false);
            this.panel3_1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
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
    private TableLayoutPanel tableLayoutPanel;
    private Panel panel1;
    private Panel panel2;
    private Panel panel3;
    private Panel panel1_2;
    private Panel panel1_1;
    private Panel panel1_3;
    private Panel panel2_2;
    private Panel panel2_3;
    private Panel panel2_1;
    private Panel panel3_2;
    private Panel panel3_3;
    private Panel panel3_1;
    private DataGridViewTextBoxColumn dgvMappings_Source;
    private DataGridViewTextBoxColumn dgvMappings_Destination;
    private DataGridViewImageColumn dgvMappings_Script;
    private KryptonButton btnAddEditScript;
    private TableLayoutPanel tableLayoutPanel1;
}
