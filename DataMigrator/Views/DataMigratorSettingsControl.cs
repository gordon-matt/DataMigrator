using DataMigrator.Common;

namespace DataMigrator.Views
{
    public partial class DataMigratorSettingsControl : UserControl, ISettingsControl
    {
        public int BatchSize
        {
            get { return (int)nudBatchSize.Value; }
            set { nudBatchSize.Value = value; }
        }

        public DataMigratorSettingsControl()
        {
            InitializeComponent();
            BatchSize = Program.Configuration.BatchSize;
        }

        #region ISettingsControl Members

        public UserControl ControlContent => this;

        public void Save()
        {
            Program.Configuration.BatchSize = this.BatchSize;
        }

        #endregion ISettingsControl Members
    }
}