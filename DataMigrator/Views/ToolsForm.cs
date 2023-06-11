using Extenso.Collections;
using Krypton.Toolkit;

namespace DataMigrator.Views
{
    public partial class ToolsForm : KryptonForm
    {
        public ToolsForm()
        {
            InitializeComponent();
        }

        private void ToolsForm_Load(object sender, EventArgs e)
        {
            Program.Plugins.OrderBy(p => p.ProviderName).ForEach(plugin =>
            {
                toolsTreeView.AddToolsNode(plugin.ProviderName, plugin.Tools);
            });

            if (toolsTreeView.Nodes.Count == 0)
            {
                MessageBox.Show(
                    "None of the plugins have any tools to use.",
                    "No Tools Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                this.Dispose();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            { return; }

            if (e.Node.Tag == null)
            { return; }

            var control = e.Node.Tag as UserControl;
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }
    }
}