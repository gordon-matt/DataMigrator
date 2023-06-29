namespace DataMigrator.Views;

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
            this.lblBatchSize = new System.Windows.Forms.Label();
            this.nudBatchSize = new Krypton.Toolkit.KryptonNumericUpDown();
            this.cbTrimStrings = new Krypton.Toolkit.KryptonCheckBox();
            this.SuspendLayout();
            // 
            // lblBatchSize
            // 
            this.lblBatchSize.Location = new System.Drawing.Point(18, 15);
            this.lblBatchSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblBatchSize.Name = "lblBatchSize";
            this.lblBatchSize.Size = new System.Drawing.Size(68, 20);
            this.lblBatchSize.TabIndex = 0;
            this.lblBatchSize.Text = "Batch Size:";
            // 
            // nudBatchSize
            // 
            this.nudBatchSize.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBatchSize.Location = new System.Drawing.Point(104, 15);
            this.nudBatchSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
            this.nudBatchSize.Size = new System.Drawing.Size(140, 22);
            this.nudBatchSize.TabIndex = 1;
            this.nudBatchSize.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // cbTrimStrings
            // 
            this.cbTrimStrings.Checked = true;
            this.cbTrimStrings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrimStrings.Location = new System.Drawing.Point(104, 43);
            this.cbTrimStrings.Name = "cbTrimStrings";
            this.cbTrimStrings.Size = new System.Drawing.Size(89, 20);
            this.cbTrimStrings.TabIndex = 2;
            this.cbTrimStrings.Values.Text = "Trim Strings";
            // 
            // DataMigratorSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbTrimStrings);
            this.Controls.Add(this.nudBatchSize);
            this.Controls.Add(this.lblBatchSize);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DataMigratorSettingsControl";
            this.Size = new System.Drawing.Size(574, 487);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private Label lblBatchSize;
    private KryptonNumericUpDown nudBatchSize;
    private KryptonCheckBox cbTrimStrings;
}
