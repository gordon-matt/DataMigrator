using System.Drawing;
using System.Windows.Forms;
using DataMigrator.Common;
using DataMigrator.Properties;

namespace DataMigrator.Controls
{
    public class SettingsTreeView : TreeView
    {
        private ImageList imageList = null;

        public SettingsTreeView()
        {
            imageList = new System.Windows.Forms.ImageList();
            imageList.ImageSize = new Size(24, 24);
            imageList.Images.Add(Resources.TreeNode);
            this.ImageList = imageList;
        }

        public TreeNode AddSettingsNode(string providerName, ISettingsControl tag)
        {
            if (tag != null)
            {
                TreeNode treeNode = new TreeNode(providerName);
                treeNode.Tag = tag;
                this.Nodes.Add(treeNode);
                return treeNode;
            }
            return null;
        }
    }
}