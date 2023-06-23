using Krypton.Toolkit;

namespace DataMigrator.Csv;

partial class CsvConnectionControl
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
            this.btnBrowse = new Krypton.Toolkit.KryptonButton();
            this.txtFile = new Krypton.Toolkit.KryptonTextBox();
            this.lblFile = new Krypton.Toolkit.KryptonLabel();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.cbHasHeaderRow = new Krypton.Toolkit.KryptonCheckBox();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.CornerRoundingRadius = -1F;
            this.btnBrowse.Location = new System.Drawing.Point(382, 9);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(78, 23);
            this.btnBrowse.TabIndex = 19;
            this.btnBrowse.Values.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(55, 9);
            this.txtFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(320, 23);
            this.txtFile.TabIndex = 18;
            // 
            // lblFile
            // 
            this.lblFile.Location = new System.Drawing.Point(10, 13);
            this.lblFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(32, 20);
            this.lblFile.TabIndex = 17;
            this.lblFile.Values.Text = "File:";
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.Filter = "CSV Files|*.csv";
            // 
            // cbHasHeaderRow
            // 
            this.cbHasHeaderRow.Checked = true;
            this.cbHasHeaderRow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHasHeaderRow.Location = new System.Drawing.Point(55, 43);
            this.cbHasHeaderRow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbHasHeaderRow.Name = "cbHasHeaderRow";
            this.cbHasHeaderRow.Size = new System.Drawing.Size(114, 20);
            this.cbHasHeaderRow.TabIndex = 25;
            this.cbHasHeaderRow.Values.Text = "Has Header Row";
            // 
            // CsvConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbHasHeaderRow);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.lblFile);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CsvConnectionControl";
            this.Size = new System.Drawing.Size(472, 127);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private KryptonButton btnBrowse;
    private KryptonTextBox txtFile;
    private KryptonLabel lblFile;
    private System.Windows.Forms.OpenFileDialog dlgOpenFile;
    private KryptonCheckBox cbHasHeaderRow;
}
