using System;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using DataMigrator.Common;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms;
using DataMigrator.Windows.Forms.Diagnostics;
using Kore.Data.Common;

namespace DataMigrator.SqlCe3_5
{
    public partial class SqlCe3_5ConnectionControl : UserControl, IConnectionControl
    {
        private const string SQLCE_CONNECTION_STRING_FORMAT = "Data Source={0}";

        #region Public Properties

        public string Database
        {
            get { return txtDatabase.Text.Trim(); }
            set { txtDatabase.Text = value; }
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

                #endregion Checks

                return string.Format(SQLCE_CONNECTION_STRING_FORMAT, Database);
            }
        }

        public bool ValidateConnection()
        {
            using (var connection = new SqlCeConnection(ConnectionString))
            {
                return connection.Validate();
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
                //using (var connection = new SqlCeConnection(ConnectionString))
                //{
                //    isValid = connection.Validate();
                //}

                return new ConnectionDetails
                {
                    Database = this.Database,
                    ProviderName = Constants.PROVIDER_NAME,
                    ConnectionString = this.ConnectionString
                };
            }
            set
            {
                Database = value.Database;
            }
        }

        #endregion IConnectionControl Members

        public SqlCe3_5ConnectionControl()
        {
            InitializeComponent();
        }

        private void btnBrowseDatabase_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                Database = dlgOpenFile.FileName;
            }
        }
    }
}