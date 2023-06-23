using DataMigrator.Common;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso.Collections;
using Extenso.Data.Common;
using Extenso.Data.Npgsql;
using Extenso.Windows.Forms;
using Npgsql;

namespace DataMigrator.Plugins.Npgsql;

public partial class NpgsqlConnectionControl : UserControl, IConnectionControl
{
    private const string PGSQL_CONNECTION_STRING_FORMAT = "Server={0};port={1};Database={2};User Id={3};Password={4};";

    public NpgsqlConnectionControl()
    {
        InitializeComponent();
    }

    #region Public Properties

    public string ConnectionString
    {
        get
        {
            if (this.IsInWinDesignMode())
            {
                return string.Empty;
            }

            #region Checks

            if (string.IsNullOrWhiteSpace(Server))
            {
                TraceService.Instance.WriteMessage(TraceEvent.Error, "Server is invalid. Please try again.");
                return string.Empty;
            }

            if (string.IsNullOrEmpty(Database))
            {
                TraceService.Instance.WriteMessage(TraceEvent.Error, "Database is invalid. Please try again.");
                return string.Empty;
            }

            if (string.IsNullOrEmpty(UserName))
            {
                TraceService.Instance.WriteMessage(TraceEvent.Error, "User Name is invalid. Please try again.");
                return string.Empty;
            }

            #endregion Checks

            return string.Format(PGSQL_CONNECTION_STRING_FORMAT, Server, Port, Database, UserName, Password);
        }
    }

    public string Database
    {
        get
        {
            if (cmbDatabase.SelectedIndex != -1)
            {
                return cmbDatabase.SelectedItem.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(cmbDatabase.Text))
            {
                return cmbDatabase.Text;
            }
            return "postgres";
        }
        set => cmbDatabase.Text = value;
    }

    public string Password
    {
        get => txtPassword.Text.Trim();
        set => txtPassword.Text = value;
    }

    public int Port
    {
        get => !string.IsNullOrEmpty(txtPort.Text) ? int.Parse(txtPort.Text.Trim()) : -1;
        set => txtPort.Text = value.ToString();
    }

    public string Server
    {
        get => txtServer.Text.Trim();
        set => txtServer.Text = value;
    }

    public string UserName
    {
        get => txtUserName.Text.Trim();
        set => txtUserName.Text = value;
    }

    #endregion Public Properties

    #region IConnectionControl Members

    public ConnectionDetails ConnectionDetails
    {
        get => new()
        {
            Database = Database,
            Password = Password,
            IntegratedSecurity = false,
            Port = Port,
            Server = Server,
            UserName = UserName,
            ProviderName = Constants.PROVIDER_NAME,
            ConnectionString = ConnectionString
        };
        set
        {
            Database = value.Database;
            Password = value.Password;
            Port = value.Port;
            Server = value.Server;
            UserName = value.UserName;
        }
    }

    public UserControl ControlContent => this;

    public bool ValidateConnection()
    {
        using var connection = new NpgsqlConnection(ConnectionString);
        return connection.Validate();
    }

    #endregion IConnectionControl Members

    private void cmbDatabase_DropDown(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Server) && string.IsNullOrEmpty(cmbDatabase.Text))
        {
            cmbDatabase.Items.Clear();
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.GetDatabaseNames().ForEach(x => cmbDatabase.Items.Add(x));
        }
    }
}