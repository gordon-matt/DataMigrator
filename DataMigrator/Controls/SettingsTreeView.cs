using DataMigrator.Common;

namespace DataMigrator.Controls;

public class SettingsTreeView : TreeView
{
    private readonly ImageList imageList = null;

    public SettingsTreeView()
    {
        imageList = new ImageList
        {
            ImageSize = new Size(24, 24)
        };
        imageList.Images.Add(Resources.TreeNode);
        this.ImageList = imageList;
    }

    public TreeNode AddSettingsNode(string providerName, ISettingsControl tag)
    {
        if (tag != null)
        {
            var treeNode = new TreeNode(providerName)
            {
                Tag = tag
            };
            this.Nodes.Add(treeNode);
            return treeNode;
        }
        return null;
    }
}