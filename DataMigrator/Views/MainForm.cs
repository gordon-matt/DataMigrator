using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Autofac;

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
        treeView.TreeViewChanged += TreeView_TreeViewChanged;

        if (!this.IsInWinDesignMode())
        {
            string pluginsPath = Path.Combine(Application.StartupPath, "Plugins");
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }

            using var mainAssemblyCatalog = new AssemblyCatalog(typeof(Program).Assembly);
            using var pluginsDirectoryCatalog = new DirectoryCatalog(pluginsPath);
            using var aggregateCatalog = new AggregateCatalog(mainAssemblyCatalog, pluginsDirectoryCatalog);
            using var container = new CompositionContainer(aggregateCatalog);
            container.ComposeParts(this);

            Program.Plugins = this.plugins;
            this.plugins = null;

            //Initialize here or get an error: "Window Handle not yet created"
            LoadUserControl<TraceViewerControl>();
        }
    }

    #endregion Constructor

    #region Private Methods

    public void HideTraceViewer()
    {
        ClearControls();
        mnuMainToolsShowTraceViewer.Checked = false;
    }

    public void ShowTraceViewer()
    {
        ClearControls();
        var traceViewer = userControls[typeof(TraceViewerControl)];
        currentControl = traceViewer;
        panelMain.Controls.Add(traceViewer);
        traceViewer.Dock = DockStyle.Fill;
        mnuMainToolsShowTraceViewer.Checked = true;

        bool isTraceViewerSelected = treeView.SelectedNode is not null && treeView.SelectedNode.Level == 0 && treeView.SelectedNode.Index == 0;
        if (!isTraceViewerSelected)
        {
            treeView.AfterSelect -= new TreeViewEventHandler(treeView_AfterSelect);
            treeView.SelectedNode = treeView.Nodes[0];
            treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
        }
    }

    private DialogResult CheckSaveChanges()
    {
        if (AppState.ConfigFile.IsNew)
        {
            return DialogResult.OK;
        }

        var dialogResult = MessageBox.Show(
            "Do you want to save the current file?",
            "Save Changes?",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question);

        if (dialogResult == DialogResult.OK)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                AppState.ConfigFile.SaveAs(dlgSaveFile.FileName);
            }
        }

        return dialogResult;
    }

    internal void ClearControls(bool fullReset = false)
    {
        panelMain.Controls.Clear();
        currentControl = null;

        if (fullReset)
        {
            foreach (var control in userControls)
            {
                control.Value?.Dispose();
            }
            userControls.Clear();
            LoadUserControl<TraceViewerControl>(); // Won't loop, because LoadUserControl() does not use "fullReset"
        }
    }

    private void LoadUserControl<T>() where T : UserControl
    {
        UserControl control;
        if (!typeof(T).GetInterfaces().Contains(typeof(ITransientControl)))
        {
            if (userControls.ContainsKey(typeof(T)))
            {
                control = userControls[typeof(T)];
            }
            else
            {
                control = GetControl<T>();
                userControls.Add(typeof(T), control);
            }
        }
        else
        {
            control = GetControl<T>();
        }

        ClearControls();
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

    private static T GetControl<T>() where T : UserControl
    {
        //T control;
        //if (Program.Container.TryResolve(out control))
        //{
        //    return control;
        //}

        return Activator.CreateInstance<T>();
    }

    private void NewFile()
    {
        CheckSaveChanges();
        AppState.ConfigFile = new DataMigrationConfigFile();
        ClearControls(fullReset: true);
        treeView.Reset();
        HideTraceViewer();
    }

    private void OpenFile()
    {
        if (CheckSaveChanges() == DialogResult.Cancel) { return; }

        if (dlgOpenFile.ShowDialog() == DialogResult.OK)
        {
            AppState.ConfigFile = DataMigrationConfigFile.Load(dlgOpenFile.FileName);
            ClearControls(fullReset: true);
            treeView.Reset();
            foreach (var job in AppState.ConfigFile.Jobs.OrderBy(j => j.Name))
            {
                treeView.AddJob(job);
            }
        }
    }

    private void SaveCurrentControl()
    {
        if (currentControl is not null && currentControl is IConfigControl)
        {
            (currentControl as IConfigControl).Save();
        }
    }

    private void SaveFile()
    {
        SaveCurrentControl();
        //ClearControls();
        AppState.ConfigFile.Save();
    }

    #endregion Private Methods

    #region Control Event Handlers

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnNew_Click(object sender, EventArgs e) => NewFile();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnOpen_Click(object sender, EventArgs e) => OpenFile();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnRun_Click(object sender, EventArgs e)
    {
        ShowTraceViewer();

        using var form = Program.Container.Resolve<RunJobsForm>();
        form.ShowDialog();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnSave_Click(object sender, EventArgs e) => SaveFile();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        SaveCurrentControl();

        if (e.Node.Level == 2) // Job Node
        {
            AppState.CurrentJob = e.Node.Tag as Job;
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
                        using var form = Program.Container.Resolve<SettingsForm>();
                        form.ShowDialog();
                        treeView.SelectedNode = treeView.Nodes[0];
                    }
                    break;

                default: break;
            }
        }
    }

    #region Main Menu

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainFileExit_Click(object sender, EventArgs e) => Close();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainFileNew_Click(object sender, EventArgs e) => NewFile();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainFileOpen_Click(object sender, EventArgs e) => OpenFile();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainFileSave_Click(object sender, EventArgs e) => SaveFile();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainFileSaveAs_Click(object sender, EventArgs e)
    {
        if (dlgSaveFile.ShowDialog() == DialogResult.OK)
        {
            SaveCurrentControl();
            ClearControls();
            AppState.ConfigFile.SaveAs(dlgSaveFile.FileName);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainHelpAbout_Click(object sender, EventArgs e)
    {
        using var form = Program.Container.Resolve<AboutForm>();
        form.ShowDialog();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainToolsOptions_Click(object sender, EventArgs e)
    {
        using var form = Program.Container.Resolve<SettingsForm>();
        form.ShowDialog();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void mnuMainToolsPluginTools_Click(object sender, EventArgs e)
    {
        using var form = Program.Container.Resolve<ToolsForm>();
        form.ShowDialog();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
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

    private void TreeView_TreeViewChanged()
    {
        ShowTraceViewer();
    }
}