using ComponentFactory.Krypton.Toolkit;

namespace DataMigrator.Excel
{
    partial class ExcelConnectionControl
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
            this.btnBrowse = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtWorkbook = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lblWorkbook = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.cbHasHeaderRow = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(492, 8);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(72, 28);
            this.btnBrowse.TabIndex = 23;
            this.btnBrowse.Values.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtWorkbook
            // 
            this.txtWorkbook.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkbook.Location = new System.Drawing.Point(85, 11);
            this.txtWorkbook.Name = "txtWorkbook";
            this.txtWorkbook.Size = new System.Drawing.Size(401, 23);
            this.txtWorkbook.TabIndex = 22;
            // 
            // lblWorkbook
            // 
            this.lblWorkbook.Location = new System.Drawing.Point(9, 11);
            this.lblWorkbook.Name = "lblWorkbook";
            this.lblWorkbook.Size = new System.Drawing.Size(70, 20);
            this.lblWorkbook.TabIndex = 21;
            this.lblWorkbook.Values.Text = "Workbook:";
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.Filter = "Microsoft Excel Spreadsheets|*.xlsx";
            // 
            // cbHasHeaderRow
            // 
            this.cbHasHeaderRow.Checked = true;
            this.cbHasHeaderRow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHasHeaderRow.Location = new System.Drawing.Point(85, 40);
            this.cbHasHeaderRow.Name = "cbHasHeaderRow";
            this.cbHasHeaderRow.Size = new System.Drawing.Size(114, 20);
            this.cbHasHeaderRow.TabIndex = 24;
            this.cbHasHeaderRow.Values.Text = "Has Header Row";
            // 
            // ExcelConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbHasHeaderRow);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtWorkbook);
            this.Controls.Add(this.lblWorkbook);
            this.Name = "ExcelConnectionControl";
            this.Size = new System.Drawing.Size(577, 110);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KryptonButton btnBrowse;
        private KryptonTextBox txtWorkbook;
        private KryptonLabel lblWorkbook;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private KryptonCheckBox cbHasHeaderRow;
    }
}
