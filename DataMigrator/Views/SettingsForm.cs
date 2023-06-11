using DataMigrator.Common;
using Extenso.Collections;
using Krypton.Toolkit;

namespace DataMigrator.Views;

public partial class SettingsForm : KryptonForm
{
    public SettingsForm()
    {
        InitializeComponent();
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
        settingsTreeView.AddSettingsNode("General", new DataMigratorSettingsControl());

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
            if (currentControl != null)
            {
                currentControl.Save();
            }
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        SaveCurrentControl();
        this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        SaveCurrentControl();
        this.Close();
    }

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