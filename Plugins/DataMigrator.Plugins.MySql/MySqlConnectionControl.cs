using DataMigrator.Common;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso.Data.Common;
using Extenso.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DataMigrator.MySql;

public partial class MySqlConnectionControl : UserControl, IConnectionControl
{
    // Setting Sql Server Mode=True no longer necessary, as have implemented "SpaceEscape" in BaseProvider.
    // Just need to set SpaceEscapeStart and SpaceEscapeEnd in constructor
    private const string MYSQL_CONNECTION_STRING_FORMAT_STANDARD = "Server={0};Database={1};Uid={2};Pwd={3};CharSet=utf8";

    private const string MYSQL_CONNECTION_STRING_FORMAT_WITH_PORT = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};CharSet=utf8";

    //private const string MYSQL_CONNECTION_STRING_FORMAT_STANDARD = "Server={0};Database={1};Uid={2};Pwd={3};Sql Server Mode=True;";
    //private const string MYSQL_CONNECTION_STRING_FORMAT_WITH_PORT = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};Sql Server Mode=True;";

    #region Public Properties

    public string Server
    {
        get { return txtServer.Text.Trim(); }
        set { txtServer.Text = value; }
    }

    public int Port
    {
        get
        {
            if (!string.IsNullOrEmpty(txtPort.Text))
            {
                return int.Parse(txtPort.Text.Trim());
            }
            return -1;
        }
        set { txtPort.Text = value.ToString(); }
    }

    public string Database
    {
        get { return txtDatabase.Text.Trim(); }
        set { txtDatabase.Text = value; }
    }

    public string UserName
    {
        get { return txtUserName.Text.Trim(); }
        set { txtUserName.Text = value; }
    }

    public string Password
    {
        get { return txtPassword.Text.Trim(); }
        set { txtPassword.Text = value; }
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

            if (Port != -1)
            {
                return string.Format(MYSQL_CONNECTION_STRING_FORMAT_WITH_PORT, Server, Port, Database, UserName, Password);
            }
            else
            {
                return string.Format(MYSQL_CONNECTION_STRING_FORMAT_STANDARD, Server, Database, UserName, Password);
            }
        }
    }

    #endregion Public Properties

    #region IConnectionControl Members

    public UserControl ControlContent
    {
        get { return this; }
    }

    public ConnectionDetails ConnectionDetails
    {
        get
        {
            //bool isValid = false;
            //using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            //{
            //    isValid = connection.Validate();
            //}

            return new ConnectionDetails
            {
                Database = this.Database,
                Password = this.Password,
                IntegratedSecurity = false,
                Port = this.Port,
                Server = this.Server,
                UserName = this.UserName,
                ProviderName = Constants.PROVIDER_NAME,
                ConnectionString = this.ConnectionString
            };
        }
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
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            return connection.Validate();
        }
    }

    #endregion IConnectionControl Members

    public MySqlConnectionControl()
    {
        InitializeComponent();
    }
}