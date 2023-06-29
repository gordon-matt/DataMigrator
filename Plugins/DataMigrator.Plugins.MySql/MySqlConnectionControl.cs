using DataMigrator.Common;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso.Collections;
using Extenso.Data.Common;
using Extenso.Data.MySql;
using Extenso.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DataMigrator.MySql;

public partial class MySqlConnectionControl : UserControl, IConnectionControl
{
    // Setting Sql Server Mode=True no longer necessary, as have implemented "SpaceEscape" in BaseProvider.
    // Just need to set SpaceEscapeStart and SpaceEscapeEnd in constructor
    private const string MYSQL_CONNECTION_STRING_FORMAT_STANDARD = "Server={0};Database={1};Uid={2};Pwd={3};CharSet=utf8";
    private const string MYSQL_CONNECTION_STRING_FORMAT_WITH_PORT = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};CharSet=utf8";

    public MySqlConnectionControl()
    {
        InitializeComponent();
    }

    #region Public Properties

    public string Server
    {
        get => txtServer.Text.Trim();
        set => txtServer.Text = value;
    }

    public int Port
    {
        get => !string.IsNullOrEmpty(txtPort.Text) ? int.Parse(txtPort.Text.Trim()) : -1;
        set => txtPort.Text = value.ToString();
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
            return string.Empty;
        }
        set => cmbDatabase.Text = value;
    }

    public string UserName
    {
        get => txtUserName.Text.Trim();
        set => txtUserName.Text = value;
    }

    public string Password
    {
        get => txtPassword.Text.Trim();
        set => txtPassword.Text = value;
    }

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

            //if (string.IsNullOrEmpty(Database))
            //{
            //    TraceService.Instance.WriteMessage(TraceEvent.Error, "Database is invalid. Please try again.");
            //    return string.Empty;
            //}

            if (string.IsNullOrEmpty(UserName))
            {
                TraceService.Instance.WriteMessage(TraceEvent.Error, "User Name is invalid. Please try again.");
                return string.Empty;
            }

            #endregion Checks

            return Port != -1
                ? string.Format(MYSQL_CONNECTION_STRING_FORMAT_WITH_PORT, Server, Port, Database, UserName, Password)
                : string.Format(MYSQL_CONNECTION_STRING_FORMAT_STANDARD, Server, Database, UserName, Password);
        }
    }

    #endregion Public Properties

    #region IConnectionControl Members

    public UserControl ControlContent => this;

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

    public bool ValidateConnection()
    {
        using var connection = new MySqlConnection(ConnectionString);
        return connection.Validate();
    }

    #endregion IConnectionControl Members

    private void cmbDatabase_DropDown(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Server) && string.IsNullOrEmpty(cmbDatabase.Text))
        {
            cmbDatabase.Items.Clear();
            using var connection = new MySqlConnection(ConnectionString);
            connection.GetDatabaseNames().ForEach(x => cmbDatabase.Items.Add(x));
        }
    }
}