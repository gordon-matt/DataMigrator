namespace DataMigrator.Views;

partial class MainForm
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

        ClearControls(fullReset: true);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        this.panelMain = new Krypton.Toolkit.KryptonPanel();
        this.splitContainer = new System.Windows.Forms.SplitContainer();
        this.treeView = new DataMigrator.Controls.DataMigratorTreeView();
        this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
        this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
        this.btnNew = new System.Windows.Forms.ToolStripButton();
        this.btnOpen = new System.Windows.Forms.ToolStripButton();
        this.btnSave = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.btnRun = new System.Windows.Forms.ToolStripButton();
        this.toolStrip = new System.Windows.Forms.ToolStrip();
        this.mnuMain = new System.Windows.Forms.MenuStrip();
        this.mnuMainFile = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainFileNew = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainFileOpen = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainFileSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.mnuMainFileSave = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainFileSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.mnuMainFileExit = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainTools = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainToolsPluginTools = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainToolsSeparator = new System.Windows.Forms.ToolStripSeparator();
        this.mnuMainToolsShowTraceViewer = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainHelp = new System.Windows.Forms.ToolStripMenuItem();
        this.mnuMainHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
        this.kryptonManager = new Krypton.Toolkit.KryptonManager(this.components);
        ((System.ComponentModel.ISupportInitialize)(this.panelMain)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
        this.splitContainer.Panel1.SuspendLayout();
        this.splitContainer.Panel2.SuspendLayout();
        this.splitContainer.SuspendLayout();
        this.toolStrip.SuspendLayout();
        this.mnuMain.SuspendLayout();
        this.SuspendLayout();
        // 
        // panelMain
        // 
        this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelMain.Location = new System.Drawing.Point(0, 0);
        this.panelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.panelMain.Name = "panelMain";
        this.panelMain.Size = new System.Drawing.Size(926, 615);
        this.panelMain.TabIndex = 0;
        // 
        // splitContainer
        // 
        this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
        this.splitContainer.Location = new System.Drawing.Point(0, 63);
        this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        this.splitContainer.Panel1.Controls.Add(this.treeView);
        // 
        // splitContainer.Panel2
        // 
        this.splitContainer.Panel2.Controls.Add(this.panelMain);
        this.splitContainer.Size = new System.Drawing.Size(1135, 615);
        this.splitContainer.SplitterDistance = 204;
        this.splitContainer.SplitterWidth = 5;
        this.splitContainer.TabIndex = 2;
        // 
        // treeView
        // 
        this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
        this.treeView.ImageIndex = 0;
        this.treeView.Location = new System.Drawing.Point(0, 0);
        this.treeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.treeView.Name = "treeView";
        this.treeView.SelectedImageIndex = 0;
        this.treeView.Size = new System.Drawing.Size(204, 615);
        this.treeView.TabIndex = 0;
        this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
        // 
        // dlgOpenFile
        // 
        this.dlgOpenFile.Filter = "Data Migrator Files|*.dmf";
        // 
        // dlgSaveFile
        // 
        this.dlgSaveFile.Filter = "Data Migrator Files|*.dmf";
        // 
        // btnNew
        // 
        this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
        this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnNew.Name = "btnNew";
        this.btnNew.Size = new System.Drawing.Size(36, 36);
        this.btnNew.Text = "New";
        this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
        // 
        // btnOpen
        // 
        this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.btnOpen.Image = global::DataMigrator.Resources.Open;
        this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnOpen.Name = "btnOpen";
        this.btnOpen.Size = new System.Drawing.Size(36, 36);
        this.btnOpen.Text = "Open";
        this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
        // 
        // btnSave
        // 
        this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.btnSave.Image = global::DataMigrator.Resources.Save;
        this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(36, 36);
        this.btnSave.Text = "Save";
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
        // 
        // btnRun
        // 
        this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.btnRun.Image = global::DataMigrator.Resources.Run;
        this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnRun.Name = "btnRun";
        this.btnRun.Size = new System.Drawing.Size(36, 36);
        this.btnRun.Text = "Run";
        this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
        // 
        // toolStrip
        // 
        this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
        this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
        this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.btnNew,
        this.btnOpen,
        this.btnSave,
        this.toolStripSeparator1,
        this.btnRun});
        this.toolStrip.Location = new System.Drawing.Point(0, 24);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new System.Drawing.Size(1135, 39);
        this.toolStrip.TabIndex = 1;
        this.toolStrip.Text = "toolStrip1";
        // 
        // mnuMain
        // 
        this.mnuMain.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
        this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.mnuMainFile,
        this.mnuMainTools,
        this.mnuMainHelp});
        this.mnuMain.Location = new System.Drawing.Point(0, 0);
        this.mnuMain.Name = "mnuMain";
        this.mnuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
        this.mnuMain.Size = new System.Drawing.Size(1135, 24);
        this.mnuMain.TabIndex = 0;
        this.mnuMain.Text = "menuStrip1";
        // 
        // mnuMainFile
        // 
        this.mnuMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.mnuMainFileNew,
        this.mnuMainFileOpen,
        this.mnuMainFileSeparator1,
        this.mnuMainFileSave,
        this.mnuMainFileSaveAs,
        this.mnuMainFileSeparator2,
        this.mnuMainFileExit});
        this.mnuMainFile.Name = "mnuMainFile";
        this.mnuMainFile.Size = new System.Drawing.Size(37, 20);
        this.mnuMainFile.Text = "&File";
        // 
        // mnuMainFileNew
        // 
        this.mnuMainFileNew.Image = ((System.Drawing.Image)(resources.GetObject("mnuMainFileNew.Image")));
        this.mnuMainFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.mnuMainFileNew.Name = "mnuMainFileNew";
        this.mnuMainFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
        this.mnuMainFileNew.Size = new System.Drawing.Size(146, 22);
        this.mnuMainFileNew.Text = "&New";
        this.mnuMainFileNew.Click += new System.EventHandler(this.mnuMainFileNew_Click);
        // 
        // mnuMainFileOpen
        // 
        this.mnuMainFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("mnuMainFileOpen.Image")));
        this.mnuMainFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.mnuMainFileOpen.Name = "mnuMainFileOpen";
        this.mnuMainFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
        this.mnuMainFileOpen.Size = new System.Drawing.Size(146, 22);
        this.mnuMainFileOpen.Text = "&Open";
        this.mnuMainFileOpen.Click += new System.EventHandler(this.mnuMainFileOpen_Click);
        // 
        // mnuMainFileSeparator1
        // 
        this.mnuMainFileSeparator1.Name = "mnuMainFileSeparator1";
        this.mnuMainFileSeparator1.Size = new System.Drawing.Size(143, 6);
        // 
        // mnuMainFileSave
        // 
        this.mnuMainFileSave.Image = ((System.Drawing.Image)(resources.GetObject("mnuMainFileSave.Image")));
        this.mnuMainFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.mnuMainFileSave.Name = "mnuMainFileSave";
        this.mnuMainFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
        this.mnuMainFileSave.Size = new System.Drawing.Size(146, 22);
        this.mnuMainFileSave.Text = "&Save";
        this.mnuMainFileSave.Click += new System.EventHandler(this.mnuMainFileSave_Click);
        // 
        // mnuMainFileSaveAs
        // 
        this.mnuMainFileSaveAs.Name = "mnuMainFileSaveAs";
        this.mnuMainFileSaveAs.Size = new System.Drawing.Size(146, 22);
        this.mnuMainFileSaveAs.Text = "Save &As";
        this.mnuMainFileSaveAs.Click += new System.EventHandler(this.mnuMainFileSaveAs_Click);
        // 
        // mnuMainFileSeparator2
        // 
        this.mnuMainFileSeparator2.Name = "mnuMainFileSeparator2";
        this.mnuMainFileSeparator2.Size = new System.Drawing.Size(143, 6);
        // 
        // mnuMainFileExit
        // 
        this.mnuMainFileExit.Name = "mnuMainFileExit";
        this.mnuMainFileExit.Size = new System.Drawing.Size(146, 22);
        this.mnuMainFileExit.Text = "E&xit";
        this.mnuMainFileExit.Click += new System.EventHandler(this.mnuMainFileExit_Click);
        // 
        // mnuMainTools
        // 
        this.mnuMainTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.mnuMainToolsOptions,
        this.mnuMainToolsPluginTools,
        this.mnuMainToolsSeparator,
        this.mnuMainToolsShowTraceViewer});
        this.mnuMainTools.Name = "mnuMainTools";
        this.mnuMainTools.Size = new System.Drawing.Size(46, 20);
        this.mnuMainTools.Text = "&Tools";
        // 
        // mnuMainToolsOptions
        // 
        this.mnuMainToolsOptions.Name = "mnuMainToolsOptions";
        this.mnuMainToolsOptions.Size = new System.Drawing.Size(171, 22);
        this.mnuMainToolsOptions.Text = "&Options";
        this.mnuMainToolsOptions.Click += new System.EventHandler(this.mnuMainToolsOptions_Click);
        // 
        // mnuMainToolsPluginTools
        // 
        this.mnuMainToolsPluginTools.Name = "mnuMainToolsPluginTools";
        this.mnuMainToolsPluginTools.Size = new System.Drawing.Size(171, 22);
        this.mnuMainToolsPluginTools.Text = "Plugin Tools";
        this.mnuMainToolsPluginTools.Click += new System.EventHandler(this.mnuMainToolsPluginTools_Click);
        // 
        // mnuMainToolsSeparator
        // 
        this.mnuMainToolsSeparator.Name = "mnuMainToolsSeparator";
        this.mnuMainToolsSeparator.Size = new System.Drawing.Size(168, 6);
        // 
        // mnuMainToolsShowTraceViewer
        // 
        this.mnuMainToolsShowTraceViewer.Name = "mnuMainToolsShowTraceViewer";
        this.mnuMainToolsShowTraceViewer.Size = new System.Drawing.Size(171, 22);
        this.mnuMainToolsShowTraceViewer.Text = "Show Trace Viewer";
        this.mnuMainToolsShowTraceViewer.Click += new System.EventHandler(this.mnuMainToolsShowTraceViewer_Click);
        // 
        // mnuMainHelp
        // 
        this.mnuMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.mnuMainHelpAbout});
        this.mnuMainHelp.Name = "mnuMainHelp";
        this.mnuMainHelp.Size = new System.Drawing.Size(44, 20);
        this.mnuMainHelp.Text = "&Help";
        // 
        // mnuMainHelpAbout
        // 
        this.mnuMainHelpAbout.Name = "mnuMainHelpAbout";
        this.mnuMainHelpAbout.Size = new System.Drawing.Size(116, 22);
        this.mnuMainHelpAbout.Text = "&About...";
        this.mnuMainHelpAbout.Click += new System.EventHandler(this.mnuMainHelpAbout_Click);
        // 
        // kryptonManager
        // 
        this.kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteModeManager.ProfessionalSystem;
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1135, 678);
        this.Controls.Add(this.splitContainer);
        this.Controls.Add(this.toolStrip);
        this.Controls.Add(this.mnuMain);
        this.Icon = global::DataMigrator.Resources.MigrateIcon;
        this.MainMenuStrip = this.mnuMain;
        this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.MinimumSize = new System.Drawing.Size(819, 439);
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Data Migrator";
        ((System.ComponentModel.ISupportInitialize)(this.panelMain)).EndInit();
        this.splitContainer.Panel1.ResumeLayout(false);
        this.splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
        this.splitContainer.ResumeLayout(false);
        this.toolStrip.ResumeLayout(false);
        this.toolStrip.PerformLayout();
        this.mnuMain.ResumeLayout(false);
        this.mnuMain.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private KryptonPanel panelMain;
    private System.Windows.Forms.SplitContainer splitContainer;
    private Controls.DataMigratorTreeView treeView;
    private System.Windows.Forms.OpenFileDialog dlgOpenFile;
    private System.Windows.Forms.SaveFileDialog dlgSaveFile;
    private System.Windows.Forms.ToolStripButton btnNew;
    private System.Windows.Forms.ToolStripButton btnOpen;
    private System.Windows.Forms.ToolStripButton btnSave;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton btnRun;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.MenuStrip mnuMain;
    private System.Windows.Forms.ToolStripMenuItem mnuMainFile;
    private System.Windows.Forms.ToolStripMenuItem mnuMainFileNew;
    private System.Windows.Forms.ToolStripMenuItem mnuMainFileOpen;
    private System.Windows.Forms.ToolStripSeparator mnuMainFileSeparator1;
    private System.Windows.Forms.ToolStripMenuItem mnuMainFileSave;
    private System.Windows.Forms.ToolStripMenuItem mnuMainFileSaveAs;
    private System.Windows.Forms.ToolStripSeparator mnuMainFileSeparator2;
    private System.Windows.Forms.ToolStripMenuItem mnuMainFileExit;
    private System.Windows.Forms.ToolStripMenuItem mnuMainTools;
    private System.Windows.Forms.ToolStripMenuItem mnuMainToolsOptions;
    private System.Windows.Forms.ToolStripMenuItem mnuMainHelp;
    private System.Windows.Forms.ToolStripMenuItem mnuMainHelpAbout;
    private System.Windows.Forms.ToolStripMenuItem mnuMainToolsShowTraceViewer;
    private System.Windows.Forms.ToolStripMenuItem mnuMainToolsPluginTools;
    private System.Windows.Forms.ToolStripSeparator mnuMainToolsSeparator;
    private KryptonManager kryptonManager;
}

