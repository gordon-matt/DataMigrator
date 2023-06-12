using System.Data.SqlClient;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso.Collections;
using Extenso.Data.SqlClient;
using Extenso.Windows.Forms;

namespace DataMigrator.Windows.Forms.Data;

public partial class SqlConnectionControl : UserControl
{
    private const string SQL_CONNECTION_STRING_FORMAT = "Data Source={0};Initial Catalog={1};User={2};Password={3}";
    private const string SQL_CONNECTION_STRING_FORMAT_WA = "Data Source={0};Initial Catalog={1};Integrated Security=true";

    public SqlConnectionControl()
    {
        InitializeComponent();

        if (!ShowTable)
        {
            lblTable.Visible = false;
            cmbTable.Visible = false;
        }
    }

    public delegate void DatabaseChangedEventHandler();

    public event DatabaseChangedEventHandler DatabaseChanged;

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
                TraceService.Instance.WriteMessage("ERROR: Server is invalid. Please try again.");
                return string.Empty;
            }
            if (string.IsNullOrWhiteSpace(Database))
            {
                TraceService.Instance.WriteMessage("ERROR: Database is invalid. Please try again.");
                return string.Empty;
            }
            if (!IntegratedSecurity)
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    TraceService.Instance.WriteMessage("ERROR: User Name is invalid. Please try again.");
                    return string.Empty;
                }
            }

            #endregion Checks

            if (IntegratedSecurity)
            {
                return string.Format(SQL_CONNECTION_STRING_FORMAT_WA, Server, Database);
            }
            else
            {
                return string.Format(SQL_CONNECTION_STRING_FORMAT, Server, Database, UserName, Password);
            }
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
            return "master";
        }
        set
        {
            if (cmbDatabase.Items.Count > 0)
            {
                cmbDatabase.SelectedItem = value;
            }
            else { cmbDatabase.Text = value; }
        }
    }

    public bool IntegratedSecurity
    {
        get { return cbIntegratedSecurity.Checked; }
        set { cbIntegratedSecurity.Checked = value; }
    }

    public string Password
    {
        get { return txtPassword.Text.Trim(); }
        set { txtPassword.Text = value; }
    }

    public string Server
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtServerName.Text))
            {
                return txtServerName.Text;
            }
            return string.Empty;
        }
        set
        {
            txtServerName.Text = value;
        }
    }

    public bool ShowTable { get; set; }

    public string Table
    {
        get
        {
            if (cmbTable.SelectedIndex != -1)
            {
                return cmbTable.SelectedItem.ToString();
            }
            return string.Empty;
        }
        set { cmbTable.SelectedItem = value; }
    }

    public IEnumerable<string> Tables { get; set; }

    public string UserName
    {
        get { return txtUserName.Text.Trim(); }
        set { txtUserName.Text = value; }
    }

    //private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    cmbDatabase.Items.Clear();
    //    using (SqlConnection connection = new SqlConnection(ConnectionString))
    //    {
    //        cmbDatabase.Items.AddRange(connection.GetDatabaseNames());
    //    }
    //}

    private void cbIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
    {
        //cbIntegratedSecurity.Enabled = IntegratedSecurity;
        txtUserName.Enabled = !IntegratedSecurity;
        txtPassword.Enabled = !IntegratedSecurity;
    }

    private void cmbDatabase_DropDown(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Server))
        {
            cmbDatabase.Items.Clear();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.GetDatabaseNames().ForEach(x => cmbDatabase.Items.Add(x));
            }
        }
    }

    private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbTable.Items.Clear();
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            Tables = connection.GetTableNames();
            Tables.ForEach(x => cmbTable.Items.Add(x));

            DatabaseChanged?.Invoke();
        }
    }
}