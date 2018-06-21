using ComponentFactory.Krypton.Toolkit;

namespace DataMigrator.SqlCe3_5
{
    partial class SqlCe3_5ConnectionControl
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
            this.lblDatabase = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.btnBrowseDatabase = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtDatabase = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lblDatabase
            // 
            this.lblDatabase.Location = new System.Drawing.Point(3, 9);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(64, 20);
            this.lblDatabase.TabIndex = 0;
            this.lblDatabase.Values.Text = "Database:";
            // 
            // btnBrowseDatabase
            // 
            this.btnBrowseDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDatabase.Location = new System.Drawing.Point(367, 6);
            this.btnBrowseDatabase.Name = "btnBrowseDatabase";
            this.btnBrowseDatabase.Size = new System.Drawing.Size(75, 28);
            this.btnBrowseDatabase.TabIndex = 2;
            this.btnBrowseDatabase.Values.Text = "Browse";
            this.btnBrowseDatabase.Click += new System.EventHandler(this.btnBrowseDatabase_Click);
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(73, 9);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(288, 23);
            this.txtDatabase.TabIndex = 1;
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.Filter = "SqlCe Database Files|*.sdf";
            // 
            // SqlCe3_5ConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.btnBrowseDatabase);
            this.Controls.Add(this.lblDatabase);
            this.Name = "SqlCe3_5ConnectionControl";
            this.Size = new System.Drawing.Size(454, 108);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KryptonLabel lblDatabase;
        private KryptonButton btnBrowseDatabase;
        private KryptonTextBox txtDatabase;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
    }
}
