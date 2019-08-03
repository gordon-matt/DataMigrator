using System;
using System.Windows.Forms;
using DataMigrator.Common;
using DataMigrator.Common.Models;
using DataMigrator.Office;

namespace DataMigrator.Excel
{
    public partial class ExcelConnectionControl : UserControl, IConnectionControl
    {
        private const string EXCEL_CONNECTION_STRING_FORMAT_NO_USER = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES"";";

        public bool HasHeaderRow
        {
            get { return cbHasHeaderRow.Checked; }
            set { cbHasHeaderRow.Checked = value; }
        }

        public string Workbook
        {
            get { return txtWorkbook.Text.Trim(); }
            set { txtWorkbook.Text = value; }
        }

        public string ConnectionString => Workbook;

        //public string ConnectionString
        //{
        //    get
        //    {
        //        #region Checks

        //        if (string.IsNullOrEmpty(Workbook))
        //        {
        //            TraceService.Instance.WriteMessage(TraceEvent.Error, "Workbook is invalid. Please try again.");
        //            return string.Empty;
        //        }

        //        #endregion

        //        return string.Format(EXCEL_CONNECTION_STRING_FORMAT_NO_USER, Workbook);
        //    }
        //}

        public ExcelConnectionControl()
        {
            InitializeComponent();
        }

        #region IConnectionControl Members

        public UserControl ControlContent => this;

        public ConnectionDetails ConnectionDetails
        {
            get
            {
                var connectionDetails = new ConnectionDetails
                {
                    Database = this.Workbook,
                    ProviderName = Constants.PROVIDER_NAME,
                    ConnectionString = this.ConnectionString
                };
                connectionDetails.ExtendedProperties.Add("HasHeaderRow", HasHeaderRow);
                return connectionDetails;
            }
            set
            {
                Workbook = value.Database;
                HasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            }
        }

        public bool ValidateConnection()
        {
            try
            {
                //using (OleDbConnection connection = new OleDbConnection(ConnectionString))
                //{
                //    return connection.Validate();
                //}
                using (var excel = ExcelOpenXmlDocument.Load(ConnectionString))
                {
                    return true;
                }
            }
            catch { return false; }
        }

        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                Workbook = dlgOpenFile.FileName;
            }
        }
    }
}
