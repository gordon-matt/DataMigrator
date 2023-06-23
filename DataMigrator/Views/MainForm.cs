using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using DataMigrator.Common;
using DataMigrator.Common.Configuration;
using Extenso.Windows.Forms;
using Krypton.Toolkit;

namespace DataMigrator.Views;

public partial class MainForm : KryptonForm
{
    private UserControl currentControl = null;

    [ImportMany(typeof(IMigrationPlugin))]
    private readonly ICollection<IMigrationPlugin> plugins = new List<IMigrationPlugin>();

    private Dictionary<Type, UserControl> userControls = new();

    #region Constructor

    public MainForm()
    {
        InitializeComponent();
        treeView.LoadDefaultNodes();

        if (!this.IsInWinDesignMode())
        {
            //Initialize here or get an error: "Window Handle not yet created"
            LoadUserControl<TraceViewerControl>();
        }

        using var mainAssemblyCatalog = new AssemblyCatalog(typeof(Program).Assembly);
        using var pluginsDirectoryCatalog = new DirectoryCatalog(Path.Combine(Application.StartupPath, "Plugins"));
        using var aggregateCatalog = new AggregateCatalog(mainAssemblyCatalog, pluginsDirectoryCatalog);
        using var container = new CompositionContainer(aggregateCatalog);
        container.ComposeParts(this);

        Program.Plugins = this.plugins;
        this.plugins = null;
    }

    #endregion Constructor

    #region Private Methods

    public void HideTraceViewer()
    {
        panelMain.Controls.Clear();
        if (currentControl is not null and not TraceViewerControl)
        {
            panelMain.Controls.Add(currentControl);
            currentControl.Dock = DockStyle.Fill;
        }
        mnuMainToolsShowTraceViewer.Checked = false;
    }

    public void ShowTraceViewer()
    {
        if (currentControl is TraceViewerControl)
        {
            return;
        }

        panelMain.Controls.Clear();
        var traceViewer = userControls[typeof(TraceViewerControl)];
        currentControl = traceViewer;
        panelMain.Controls.Add(traceViewer);
        traceViewer.Dock = DockStyle.Fill;
        mnuMainToolsShowTraceViewer.Checked = true;

        bool isTraceViewerSelected = treeView.SelectedNode.Level == 0 && treeView.SelectedNode.Index == 0;
        if (!isTraceViewerSelected)
        {
            treeView.AfterSelect -= new TreeViewEventHandler(treeView_AfterSelect);
            treeView.SelectedNode = treeView.Nodes[0];
            treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
        }
    }

    private DialogResult CheckSaveChanges()
    {
        var dialogResult = MessageBox.Show(
            "Do you want to save the current file?",
            "Save Changes?",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question);

        if (dialogResult == DialogResult.OK)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                Program.Configuration.SaveAs(dlgSaveFile.FileName);
            }
        }

        return dialogResult;
    }

    private void LoadUserControl<T>() where T : UserControl
    {
        UserControl control;
        if (userControls.ContainsKey(typeof(T)))
        {
            control = userControls[typeof(T)];
        }
        else
        {
            control = Activator.CreateInstance<T>();
            userControls.Add(typeof(T), control);
        }

        panelMain.Controls.Clear();
        panelMain.Controls.Add(control);
        control.Dock = DockStyle.Fill;
        if (control.MinimumSize != new Size(0, 0))
        {
            this.Width = this.Width - panelMain.Width + control.MinimumSize.Width;
            this.Height = this.Height - panelMain.Height + control.MinimumSize.Height;
        }

        currentControl = control;
        mnuMainToolsShowTraceViewer.Checked = currentControl is TraceViewerControl;
    }

    private void NewFile()
    {
        CheckSaveChanges();
        Program.Configuration = new DataMigrationConfigFile();
        panelMain.Controls.Clear();
        treeView.Reset();
        HideTraceViewer();
    }

    private void OpenFile()
    {
        var dialogResult = CheckSaveChanges();
        if (dialogResult == DialogResult.Cancel)
        { return; }

        if (dlgOpenFile.ShowDialog() == DialogResult.OK)
        {
            Program.Configuration = DataMigrationConfigFile.Load(dlgOpenFile.FileName);
            panelMain.Controls.Clear();
            treeView.Reset();
            foreach (var job in Program.Configuration.Jobs.OrderBy(j => j.Name))
            {
                treeView.AddJob(job);
            }
        }
    }

    private void SaveCurrentControl()
    {
        if (panelMain.Controls.Count == 0)
        {
            return;
        }

        if (currentControl is not null && currentControl is IConfigControl)
        {
            (currentControl as IConfigControl).Save();
        }
    }

    private void SaveFile()
    {
        SaveCurrentControl();
        panelMain.Controls.Clear();
        Program.Configuration.Save();
    }

    #endregion Private Methods

    #region Control Event Handlers

    private void btnNew_Click(object sender, EventArgs e) => NewFile();

    private void btnOpen_Click(object sender, EventArgs e) => OpenFile();

    private void btnRun_Click(object sender, EventArgs e)
    {
        ShowTraceViewer();

        using var form = new RunJobsForm();
        form.ShowDialog();
    }

    private void btnSave_Click(object sender, EventArgs e) => SaveFile();

    private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        SaveCurrentControl();

        if (e.Node.Level == 2) // Job Node
        {
            Program.CurrentJob = e.Node.Tag as Job;
            LoadUserControl<TableMappingControl>();
        }
        else
        {
            switch (e.Node.Text)
            {
                case Constants.TreeView.ROOT_NODE_TEXT: ShowTraceViewer(); break;
                case Constants.TreeView.CONNECTIONS_NODE_TEXT: LoadUserControl<ConnectionsControl>(); break;
                case Constants.TreeView.SETTINGS_NODE_TEXT:
                    {
                        using var form = new SettingsForm();
                        form.ShowDialog();
                        treeView.SelectedNode = treeView.Nodes[0];
                    }
                    break;

                default: break;
            }
        }
    }

    #region Main Menu

    private void mnuMainFileExit_Click(object sender, EventArgs e) => Close();

    private void mnuMainFileNew_Click(object sender, EventArgs e) => NewFile();

    private void mnuMainFileOpen_Click(object sender, EventArgs e) => OpenFile();

    private void mnuMainFileSave_Click(object sender, EventArgs e) => SaveFile();

    private void mnuMainFileSaveAs_Click(object sender, EventArgs e)
    {
        if (dlgSaveFile.ShowDialog() == DialogResult.OK)
        {
            SaveCurrentControl();
            panelMain.Controls.Clear();
            Program.Configuration.SaveAs(dlgSaveFile.FileName);
        }
    }

    private void mnuMainHelpAbout_Click(object sender, EventArgs e)
    {
        using var form = new AboutForm();
        form.ShowDialog();
    }

    private void mnuMainToolsOptions_Click(object sender, EventArgs e)
    {
        using var form = new SettingsForm();
        form.ShowDialog();
    }

    private void mnuMainToolsPluginTools_Click(object sender, EventArgs e)
    {
        using var form = new ToolsForm();
        form.ShowDialog();
    }

    private void mnuMainToolsShowTraceViewer_Click(object sender, EventArgs e)
    {
        if (mnuMainToolsShowTraceViewer.Checked)
        {
            HideTraceViewer();
        }
        else
        {
            ShowTraceViewer();
        }
    }

    #endregion Main Menu

    #endregion Control Event Handlers
}