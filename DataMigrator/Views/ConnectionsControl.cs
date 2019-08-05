using System.Linq;
using System.Windows.Forms;
using DataMigrator.Common;
using DataMigrator.Common.Models;
using Kore.Collections;

namespace DataMigrator.Views
{
    public partial class ConnectionsControl : UserControl
    {
        private IConnectionControl sourceConnectionControl = null;
        private IConnectionControl destinationConnectionControl = null;

        public ConnectionDetails SourceConnection
        {
            get
            {
                if (sourceConnectionControl == null)
                { return null; }
                return sourceConnectionControl.ConnectionDetails;
            }
        }

        public ConnectionDetails DestinationConnection
        {
            get
            {
                if (destinationConnectionControl == null)
                { return null; }
                return destinationConnectionControl.ConnectionDetails;
            }
        }

        public string SourceConnectionType
        {
            get
            {
                if (cmbSourceConnectionType.SelectedIndex != -1)
                {
                    return cmbSourceConnectionType.SelectedItem.ToString();
                }
                return string.Empty;
            }
            set { cmbSourceConnectionType.SelectedItem = value.ToString(); }
        }

        public string DestinationConnectionType
        {
            get
            {
                if (cmbDestinationConnectionType.SelectedIndex != -1)
                {
                    return cmbDestinationConnectionType.SelectedItem.ToString();
                }
                return string.Empty;
            }
            set { cmbDestinationConnectionType.SelectedItem = value.ToString(); }
        }

        public ConnectionsControl()
        {
            InitializeComponent();

            var providerNames = Program.Plugins.Select(p => p.ProviderName).OrderBy(p => p);
            providerNames.ForEach(x => cmbSourceConnectionType.Items.Add(x));
            providerNames.ForEach(x => cmbDestinationConnectionType.Items.Add(x));

            if (Program.Configuration.SourceConnection != null)
            {
                var plugin = Program.Plugins.SingleOrDefault(p => p.ProviderName == Program.Configuration.SourceConnection.ProviderName);
                this.sourceConnectionControl = plugin.ConnectionControl;
                sourceConnectionControl.ConnectionDetails = Program.Configuration.SourceConnection;
                LoadSourceConnectionControl();
                cmbSourceConnectionType.SelectedIndexChanged -= new System.EventHandler(this.cmbSourceConnectionType_SelectedIndexChanged);
                cmbSourceConnectionType.Text = plugin.ProviderName;
                cmbSourceConnectionType.SelectedIndexChanged += new System.EventHandler(this.cmbSourceConnectionType_SelectedIndexChanged);
            }
            if (Program.Configuration.DestinationConnection != null)
            {
                var plugin = Program.Plugins.SingleOrDefault(p => p.ProviderName == Program.Configuration.DestinationConnection.ProviderName);
                this.destinationConnectionControl = plugin.ConnectionControl;
                destinationConnectionControl.ConnectionDetails = Program.Configuration.DestinationConnection;
                LoadDestinationConnectionControl();
                cmbDestinationConnectionType.SelectedIndexChanged -= new System.EventHandler(this.cmbDestinationConnectionType_SelectedIndexChanged);
                cmbDestinationConnectionType.Text = plugin.ProviderName;
                cmbDestinationConnectionType.SelectedIndexChanged -= new System.EventHandler(this.cmbDestinationConnectionType_SelectedIndexChanged);
            }
        }

        private void LoadSourceConnectionControl()
        {
            if (sourceConnectionControl != null)
            {
                pnlSourceConnection.Controls.Clear();
                var content = sourceConnectionControl.ControlContent;
                pnlSourceConnection.Controls.Add(content);
                content.Dock = DockStyle.Fill;
            }
        }

        private void LoadDestinationConnectionControl()
        {
            if (destinationConnectionControl != null)
            {
                pnlDestinationConnection.Controls.Clear();
                var content = destinationConnectionControl.ControlContent;
                pnlDestinationConnection.Controls.Add(content);
                content.Dock = DockStyle.Fill;
            }
        }

        private void cmbSourceConnectionType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            sourceConnectionControl = Controller.GetConnectionControl(SourceConnectionType);
            LoadSourceConnectionControl();
        }

        private void cmbDestinationConnectionType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            destinationConnectionControl = Controller.GetConnectionControl(DestinationConnectionType);
            LoadDestinationConnectionControl();
        }

        private void btnValidateSourceConnection_Click(object sender, System.EventArgs e)
        {
            if (sourceConnectionControl != null)
            {
                bool isValid = sourceConnectionControl.ValidateConnection();
                if (isValid)
                {
                    MessageBox.Show("Successfully connected to source data", "Successful Connection",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Could not connect to source data", "Unsuccessful Connection",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Source Connection Not Specified", "No Source Connection",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnValidateDestinationConnection_Click(object sender, System.EventArgs e)
        {
            if (destinationConnectionControl != null)
            {
                bool isValid = destinationConnectionControl.ValidateConnection();
                if (isValid)
                {
                    MessageBox.Show("Successfully connected to destination data", "Successful Connection",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Could not connect to destination data", "Unsuccessful Connection",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Destination Connection Not Specified", "No Destination Connection",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}