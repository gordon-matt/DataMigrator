namespace DataMigrator.Views;

public partial class ConnectionsControl : UserControl, IConfigControl
{
    private IConnectionControl sourceConnectionControl = null;
    private IConnectionControl destinationConnectionControl = null;

    public ConnectionDetails SourceConnection => sourceConnectionControl?.ConnectionDetails;

    public ConnectionDetails DestinationConnection => destinationConnectionControl?.ConnectionDetails;

    public string SourceConnectionType
    {
        get => cmbSourceConnectionType.SelectedIndex != -1 ? cmbSourceConnectionType.SelectedItem.ToString() : string.Empty;
        set => cmbSourceConnectionType.SelectedItem = value.ToString();
    }

    public string DestinationConnectionType
    {
        get => cmbDestinationConnectionType.SelectedIndex != -1 ? cmbDestinationConnectionType.SelectedItem.ToString() : string.Empty;
        set => cmbDestinationConnectionType.SelectedItem = value.ToString();
    }

    public ConnectionsControl()
    {
        InitializeComponent();

        var providerNames = Program.Plugins.Select(p => p.ProviderName).OrderBy(p => p);
        providerNames.ForEach(x => cmbSourceConnectionType.Items.Add(x));
        providerNames.ForEach(x => cmbDestinationConnectionType.Items.Add(x));

        if (AppState.ConfigFile.SourceConnection != null)
        {
            var plugin = Controller.GetPlugin(AppState.ConfigFile.SourceConnection.ProviderName);
            sourceConnectionControl = plugin.ConnectionControl;
            sourceConnectionControl.ConnectionDetails = AppState.ConfigFile.SourceConnection;
            LoadSourceConnectionControl();
            cmbSourceConnectionType.SelectedIndexChanged -= new EventHandler(cmbSourceConnectionType_SelectedIndexChanged);
            cmbSourceConnectionType.Text = plugin.ProviderName;
            cmbSourceConnectionType.SelectedIndexChanged += new EventHandler(cmbSourceConnectionType_SelectedIndexChanged);
        }
        if (AppState.ConfigFile.DestinationConnection != null)
        {
            var plugin = Controller.GetPlugin(AppState.ConfigFile.DestinationConnection.ProviderName);
            destinationConnectionControl = plugin.ConnectionControl;
            destinationConnectionControl.ConnectionDetails = AppState.ConfigFile.DestinationConnection;
            LoadDestinationConnectionControl();
            cmbDestinationConnectionType.SelectedIndexChanged -= new EventHandler(cmbDestinationConnectionType_SelectedIndexChanged);
            cmbDestinationConnectionType.Text = plugin.ProviderName;
            cmbDestinationConnectionType.SelectedIndexChanged -= new EventHandler(cmbDestinationConnectionType_SelectedIndexChanged);
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void cmbSourceConnectionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        sourceConnectionControl = Controller.GetPlugin(SourceConnectionType).ConnectionControl;
        LoadSourceConnectionControl();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void cmbDestinationConnectionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        destinationConnectionControl = Controller.GetPlugin(DestinationConnectionType).ConnectionControl;
        LoadDestinationConnectionControl();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnValidateSourceConnection_Click(object sender, EventArgs e)
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnValidateDestinationConnection_Click(object sender, EventArgs e)
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

    public void Save()
    {
        if (SourceConnection != null)
        {
            AppState.ConfigFile.SourceConnection = SourceConnection;
        }

        if (DestinationConnection != null)
        {
            AppState.ConfigFile.DestinationConnection = DestinationConnection;
        }
    }
}