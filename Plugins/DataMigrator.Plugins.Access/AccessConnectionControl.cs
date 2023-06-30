using System.Data.Odbc;
using DataMigrator.Common;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using Extenso.Data.Common;

namespace DataMigrator.Access;

public partial class AccessConnectionControl : UserControl, IConnectionControl
{
    private const string ACCESS_CONNECTION_STRING_FORMAT_STANDARD = @"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};Dbq={0};Uid={1};Pwd={2};";
    private const string ACCESS_CONNECTION_STRING_FORMAT_NO_USER = @"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};Dbq={0};";

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
            #region Checks

            if (string.IsNullOrEmpty(Database))
            {
                TraceService.Instance.WriteMessage(TraceEvent.Error, "Database is invalid. Please try again.");
                return string.Empty;
            }

            #endregion Checks

            if (string.IsNullOrWhiteSpace(UserName))
            {
                return string.Format(ACCESS_CONNECTION_STRING_FORMAT_NO_USER, Database);
            }
            else
            {
                return string.Format(ACCESS_CONNECTION_STRING_FORMAT_STANDARD, Database, UserName, Password);
            }
        }
    }

    public AccessConnectionControl()
    {
        InitializeComponent();
    }

    #region IConnectionControl Members

    public UserControl ControlContent => this;

    public ConnectionDetails ConnectionDetails
    {
        get
        {
            return new ConnectionDetails
            {
                Database = Database,
                Password = Password,
                IntegratedSecurity = false,
                Server = string.Empty,
                UserName = UserName,
                ProviderName = Constants.PROVIDER_NAME,
                ConnectionString = ConnectionString
            };
        }
        set
        {
            Database = value.Database;
            Password = value.Password;
            UserName = value.UserName;
        }
    }

    public bool ValidateConnection()
    {
        using var connection = new OdbcConnection(ConnectionString);
        return connection.Validate();
    }

    #endregion IConnectionControl Members

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        if (dlgOpenFile.ShowDialog() == DialogResult.OK)
        {
            Database = dlgOpenFile.FileName;
        }
    }
}