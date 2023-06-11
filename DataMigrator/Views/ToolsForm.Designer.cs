using Krypton.Toolkit;

namespace DataMigrator.Views;

partial class ToolsForm
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
        this.components = new System.ComponentModel.Container();
        this.btnOK = new Krypton.Toolkit.KryptonButton();
        this.contentPanel = new Krypton.Toolkit.KryptonPanel();
        this.panel = new Krypton.Toolkit.KryptonPanel();
        this.btnCancel = new Krypton.Toolkit.KryptonButton();
        this.splitContainer = new System.Windows.Forms.SplitContainer();
        this.toolsTreeView = new DataMigrator.Controls.ToolsTreeView();
        ((System.ComponentModel.ISupportInitialize)(this.contentPanel)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.panel)).BeginInit();
        this.panel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
        this.splitContainer.Panel1.SuspendLayout();
        this.splitContainer.Panel2.SuspendLayout();
        this.splitContainer.SuspendLayout();
        this.SuspendLayout();
        // 
        // btnOK
        // 
        this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOK.Location = new System.Drawing.Point(267, 6);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new System.Drawing.Size(75, 28);
        this.btnOK.TabIndex = 1;
        this.btnOK.Values.Text = "OK";
        this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
        // 
        // contentPanel
        // 
        this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.contentPanel.Location = new System.Drawing.Point(0, 0);
        this.contentPanel.Name = "contentPanel";
        this.contentPanel.Size = new System.Drawing.Size(435, 433);
        this.contentPanel.TabIndex = 3;
        // 
        // panel
        // 
        this.panel.Controls.Add(this.btnOK);
        this.panel.Controls.Add(this.btnCancel);
        this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel.Location = new System.Drawing.Point(0, 433);
        this.panel.Name = "panel";
        this.panel.Size = new System.Drawing.Size(435, 37);
        this.panel.TabIndex = 2;
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(348, 6);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 28);
        this.btnCancel.TabIndex = 0;
        this.btnCancel.Values.Text = "Cancel";
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // 
        // splitContainer
        // 
        this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
        this.splitContainer.Location = new System.Drawing.Point(0, 0);
        this.splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        this.splitContainer.Panel1.Controls.Add(this.toolsTreeView);
        // 
        // splitContainer.Panel2
        // 
        this.splitContainer.Panel2.Controls.Add(this.contentPanel);
        this.splitContainer.Panel2.Controls.Add(this.panel);
        this.splitContainer.Size = new System.Drawing.Size(652, 470);
        this.splitContainer.SplitterDistance = 213;
        this.splitContainer.TabIndex = 3;
        // 
        // toolsTreeView
        // 
        this.toolsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
        this.toolsTreeView.ImageIndex = 0;
        this.toolsTreeView.Location = new System.Drawing.Point(0, 0);
        this.toolsTreeView.Name = "toolsTreeView";
        this.toolsTreeView.SelectedImageIndex = 0;
        this.toolsTreeView.Size = new System.Drawing.Size(213, 470);
        this.toolsTreeView.TabIndex = 0;
        this.toolsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.toolsTreeView_AfterSelect);
        // 
        // ToolsForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(652, 470);
        this.Controls.Add(this.splitContainer);
        this.Name = "ToolsForm";
        this.Text = "Tools";
        this.Load += new System.EventHandler(this.ToolsForm_Load);
        ((System.ComponentModel.ISupportInitialize)(this.contentPanel)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.panel)).EndInit();
        this.panel.ResumeLayout(false);
        this.splitContainer.Panel1.ResumeLayout(false);
        this.splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
        this.splitContainer.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private KryptonButton btnOK;
    private KryptonPanel contentPanel;
    private KryptonPanel panel;
    private KryptonButton btnCancel;
    private System.Windows.Forms.SplitContainer splitContainer;
    private Controls.ToolsTreeView toolsTreeView;

}