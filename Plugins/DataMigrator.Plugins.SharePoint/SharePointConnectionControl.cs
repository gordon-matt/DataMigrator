using System;
using System.Windows.Forms;
using DataMigrator.Common;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;

namespace DataMigrator.SharePoint
{
    public partial class SharePointConnectionControl : UserControl, IConnectionControl
    {
        public string Url
        {
            get { return txtUrl.Text; }
            set { txtUrl.Text = value; }
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

        public string Domain
        {
            get { return txtDomain.Text.Trim(); }
            set { txtDomain.Text = value; }
        }

        public SharePointConnectionControl()
        {
            InitializeComponent();
        }

        #region IConnectionControl Members

        public ConnectionDetails ConnectionDetails
        {
            get
            {
                return new ConnectionDetails
                {
                    Server = this.Url,
                    IntegratedSecurity = false,
                    Domain = this.Domain,
                    Password = this.Password,
                    UserName = this.UserName,
                    ConnectionString = Url,
                    ProviderName = Constants.PROVIDER_NAME
                };
            }
            set
            {
                Url = value.Server;
                Domain = value.Domain;
                UserName = value.UserName;
                Password = value.Password;
            }
        }

        public UserControl ControlContent
        {
            get { return this; }
        }

        public bool ValidateConnection()
        {
            try
            {
                using (var context = SharePointProvider.GetClientContext(ConnectionDetails))
                {
                    var site = context.Web;
                    var lists = context.LoadQuery(site.Lists);
                    context.ExecuteQuery(); //If can run this line, then credentials OK
                    return true;
                }
            }
            catch (Exception x)
            {
                TraceService.Instance.WriteException(x);
                return false;
            }
        }

        #endregion IConnectionControl Members
    }
}