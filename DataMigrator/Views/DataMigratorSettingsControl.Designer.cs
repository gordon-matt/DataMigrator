using ComponentFactory.Krypton.Toolkit;

namespace DataMigrator.Views
{
    partial class DataMigratorSettingsControl
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
            this.lblBatchSize = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.nudBatchSize = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.SuspendLayout();
            // 
            // lblBatchSize
            // 
            this.lblBatchSize.Location = new System.Drawing.Point(15, 13);
            this.lblBatchSize.Name = "lblBatchSize";
            this.lblBatchSize.Size = new System.Drawing.Size(68, 20);
            this.lblBatchSize.TabIndex = 0;
            this.lblBatchSize.Values.Text = "Batch Size:";
            // 
            // nudBatchSize
            // 
            this.nudBatchSize.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBatchSize.Location = new System.Drawing.Point(89, 13);
            this.nudBatchSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudBatchSize.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudBatchSize.Name = "nudBatchSize";
            this.nudBatchSize.Size = new System.Drawing.Size(120, 22);
            this.nudBatchSize.TabIndex = 2;
            this.nudBatchSize.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // DataMigratorSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.nudBatchSize);
            this.Controls.Add(this.lblBatchSize);
            this.Name = "DataMigratorSettingsControl";
            this.Size = new System.Drawing.Size(492, 422);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KryptonLabel lblBatchSize;
        private KryptonNumericUpDown nudBatchSize;
    }
}
