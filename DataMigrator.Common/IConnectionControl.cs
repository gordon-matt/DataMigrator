using System.Windows.Forms;
using DataMigrator.Common.Models;

namespace DataMigrator.Common
{
    public interface IConnectionControl
    {
        ConnectionDetails ConnectionDetails { get; set; }

        UserControl ControlContent { get; }

        bool ValidateConnection();
    }
}