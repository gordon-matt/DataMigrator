using System.Linq;
using System.Windows.Forms;

namespace DataMigrator.Windows.Forms
{
    public static class TreeViewExtensions
    {
        #region TreeView

        public static TreeNode GetNodeByName(this TreeView treeView, string nodeName)
        {
            return treeView.Nodes.Cast<TreeNode>().Where(x => x.Name == nodeName).SingleOrDefault();
        }

        public static TreeNode GetNodeByFullPath(this TreeView treeView, string path)
        {
            return treeView.Nodes.Cast<TreeNode>().Where(x => x.FullPath == path).SingleOrDefault();
        }

        public static TreeNode GetNodeByText(this TreeView treeView, string nodeText)
        {
            return treeView.Nodes.Cast<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();
        }

        #endregion TreeView

        #region TreeNode

        public static TreeNode GetNodeByText(this TreeNode treeNode, string nodeText)
        {
            return treeNode.Nodes.Cast<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();
        }

        #endregion TreeNode
    }
}