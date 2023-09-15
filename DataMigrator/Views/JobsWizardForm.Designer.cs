namespace DataMigrator.Views
{
    partial class JobsWizardForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
            cbSelectAll = new KryptonCheckBox();
            btnCancel = new KryptonButton();
            btnOK = new KryptonButton();
            dataGridView = new KryptonDataGridView();
            SelectColumn = new DataGridViewCheckBoxColumn();
            SourceColumn = new DataGridViewTextBoxColumn();
            DestinationColumn = new DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // cbSelectAll
            // 
            cbSelectAll.Location = new Point(14, 9);
            cbSelectAll.Name = "cbSelectAll";
            cbSelectAll.Size = new Size(73, 20);
            cbSelectAll.TabIndex = 6;
            cbSelectAll.Values.Text = "Select All";
            cbSelectAll.CheckedChanged += cbSelectAll_CheckedChanged;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.CornerRoundingRadius = -1F;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(597, 404);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(167, 43);
            btnCancel.TabIndex = 9;
            btnCancel.Values.Image = Resources.Cancel_32x32;
            btnCancel.Values.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.CornerRoundingRadius = -1F;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(424, 404);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(167, 43);
            btnOK.TabIndex = 8;
            btnOK.Values.Image = Resources.OK_32x32;
            btnOK.Values.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // dataGridView
            // 
            dataGridView.AllowDrop = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToOrderColumns = true;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { SelectColumn, SourceColumn, DestinationColumn });
            dataGridView.Location = new Point(14, 35);
            dataGridView.Margin = new Padding(4, 3, 4, 3);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.RowTemplate.Height = 32;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(750, 363);
            dataGridView.TabIndex = 7;
            // 
            // SelectColumn
            // 
            SelectColumn.HeaderText = "";
            SelectColumn.Name = "SelectColumn";
            SelectColumn.Width = 7;
            // 
            // SourceColumn
            // 
            SourceColumn.HeaderText = "Source";
            SourceColumn.Name = "SourceColumn";
            SourceColumn.ReadOnly = true;
            SourceColumn.Width = 72;
            // 
            // DestinationColumn
            // 
            DestinationColumn.HeaderText = "Destination";
            DestinationColumn.Name = "DestinationColumn";
            DestinationColumn.Width = 77;
            // 
            // JobsWizardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 457);
            Controls.Add(cbSelectAll);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(dataGridView);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "JobsWizardForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Jobs Wizard";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private KryptonCheckBox cbSelectAll;
        private KryptonButton btnCancel;
        private KryptonButton btnOK;
        private KryptonDataGridView dataGridView;
        private DataGridViewCheckBoxColumn SelectColumn;
        private DataGridViewTextBoxColumn SourceColumn;
        private DataGridViewComboBoxColumn DestinationColumn;
    }
}