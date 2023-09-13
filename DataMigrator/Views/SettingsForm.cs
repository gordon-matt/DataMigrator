namespace DataMigrator.Views;

public partial class SettingsForm : KryptonForm
{
    private readonly DataMigratorSettingsControl dataMigratorSettingsControl;

    public SettingsForm(DataMigratorSettingsControl dataMigratorSettingsControl)
    {
        InitializeComponent();
        this.dataMigratorSettingsControl = dataMigratorSettingsControl;
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
        settingsTreeView.AddSettingsNode("General", dataMigratorSettingsControl);

        Program.Plugins.OrderBy(p => p.ProviderName).ForEach(plugin =>
        {
            settingsTreeView.AddSettingsNode(plugin.ProviderName, plugin.SettingsControl);
        });
    }

    private void SaveCurrentControl()
    {
        if (contentPanel.HasChildren)
        {
            var currentControl = contentPanel.Controls[0] as ISettingsControl;
            currentControl?.Save();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnOK_Click(object sender, EventArgs e)
    {
        SaveCurrentControl();
        this.Close();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnCancel_Click(object sender, EventArgs e)
    {
        SaveCurrentControl();
        this.Close();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void settingsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        SaveCurrentControl();

        if (e.Node.Tag == null)
        { return; }

        var settingsControl = e.Node.Tag as ISettingsControl;
        var control = settingsControl.ControlContent;
        contentPanel.Controls.Clear();
        contentPanel.Controls.Add(control);
        control.Dock = DockStyle.Fill;
    }
}