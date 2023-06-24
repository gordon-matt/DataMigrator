namespace DataMigrator.Views;

partial class TraceViewerControl
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
        this.txtTrace = new KryptonTextBox();
        this.SuspendLayout();
        // 
        // txtTrace
        // 
        this.txtTrace.BackColor = System.Drawing.Color.White;
        this.txtTrace.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtTrace.Location = new System.Drawing.Point(0, 0);
        this.txtTrace.Multiline = true;
        this.txtTrace.Name = "txtTrace";
        this.txtTrace.ReadOnly = true;
        this.txtTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtTrace.Size = new System.Drawing.Size(605, 324);
        this.txtTrace.TabIndex = 0;
        this.txtTrace.WordWrap = false;
        // 
        // TraceViewerControl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.txtTrace);
        this.Name = "TraceViewerControl";
        this.Size = new System.Drawing.Size(605, 324);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private KryptonTextBox txtTrace;
}
