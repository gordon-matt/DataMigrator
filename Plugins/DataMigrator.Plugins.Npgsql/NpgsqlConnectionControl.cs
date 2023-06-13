using DataMigrator.Common;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso.Data.Common;
using Extenso.Windows.Forms;
using Npgsql;

namespace DataMigrator.Plugins.Npgsql;

public partial class NpgsqlConnectionControl : UserControl, IConnectionControl
{
    private const string PGSQL_CONNECTION_STRING_FORMAT = "Server={0};port={1};Database={2};User Id={3};Password={4};";

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
        get => txtDatabase.Text.Trim();
        set => txtDatabase.Text = value;
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

    public string Schema
    {
        get => txtSchema.Text.Trim();
        set => txtSchema.Text = value;
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

            return string.Format(PGSQL_CONNECTION_STRING_FORMAT, Server, Port, Database, UserName, Password);
        }
    }

    #endregion Public Properties

    #region IConnectionControl Members

    public UserControl ControlContent => this;

    public ConnectionDetails ConnectionDetails
    {
        get
        {
            //bool isValid = false;
            //using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            //{
            //    isValid = connection.Validate();
            //}

            var connectionDetails = new ConnectionDetails
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
            connectionDetails.ExtendedProperties.Add("Schema", Schema);
            return connectionDetails;
        }
        set
        {
            Database = value.Database;
            Password = value.Password;
            Port = value.Port;
            Server = value.Server;
            UserName = value.UserName;
            Schema = ConnectionDetails.ExtendedProperties["Schema"].GetValue<string>();
        }
    }

    public bool ValidateConnection()
    {
        using var connection = new NpgsqlConnection(ConnectionString);
        return connection.Validate();
    }

    #endregion IConnectionControl Members

    public NpgsqlConnectionControl()
    {
        InitializeComponent();
    }
}