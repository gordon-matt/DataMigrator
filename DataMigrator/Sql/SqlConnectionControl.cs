using Extenso.Data.Common;
using Extenso.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace DataMigrator.Sql;

public partial class SqlConnectionControl : UserControl, IConnectionControl
{
    private const string SQL_CONNECTION_STRING_FORMAT = "Data Source={0};Initial Catalog={1};User={2};Password={3};TrustServerCertificate=True";
    private const string SQL_CONNECTION_STRING_FORMAT_WA = "Data Source={0};Initial Catalog={1};Integrated Security=true;TrustServerCertificate=True";

    public SqlConnectionControl()
    {
        InitializeComponent();
    }

    #region Public Properties

    public string Server
    {
        get => !string.IsNullOrWhiteSpace(txtServer.Text) ? txtServer.Text : string.Empty;
        set => txtServer.Text = value;
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
            return "master";
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

    public bool IntegratedSecurity
    {
        get => rbUseWindowsAuthentication.Checked;
        set => rbUseWindowsAuthentication.Checked = value;
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

            if (string.IsNullOrWhiteSpace(Database))
            {
                TraceService.Instance.WriteMessage(TraceEvent.Error, "Database is invalid. Please try again.");
                return string.Empty;
            }

            if (!IntegratedSecurity)
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    TraceService.Instance.WriteMessage(TraceEvent.Error, "User Name is invalid. Please try again.");
                    return string.Empty;
                }
            }

            #endregion Checks

            return IntegratedSecurity
                ? string.Format(SQL_CONNECTION_STRING_FORMAT_WA, Server, Database)
                : string.Format(SQL_CONNECTION_STRING_FORMAT, Server, Database, UserName, Password);
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
            IntegratedSecurity = IntegratedSecurity,
            Password = Password,
            Server = Server,
            UserName = UserName,
            ProviderName = Constants.SQL_PROVIDER_NAME,
            ConnectionString = ConnectionString
        };
        set
        {
            Server = value.Server;
            IntegratedSecurity = value.IntegratedSecurity;
            Password = value.Password;
            UserName = value.UserName;
            Database = value.Database;
        }
    }

    public bool ValidateConnection()
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Validate();
    }

    #endregion IConnectionControl Members

    private void rbIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
    {
        txtUserName.Enabled = !IntegratedSecurity;
        txtPassword.Enabled = !IntegratedSecurity;
    }

    private void cmbDatabase_DropDown(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Server) && string.IsNullOrEmpty(cmbDatabase.Text))
        {
            cmbDatabase.Items.Clear();
            using var connection = new SqlConnection(ConnectionString);
            connection.GetDatabaseNames().ForEach(x => cmbDatabase.Items.Add(x));
        }
    }
}