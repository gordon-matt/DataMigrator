namespace DataMigrator.Controls;

public class ToolsTreeView : TreeView
{
    private readonly ImageList imageList = null;

    public ToolsTreeView()
    {
        imageList = new ImageList
        {
            ImageSize = new Size(24, 24)
        };
        imageList.Images.Add(Resources.TreeNode);
        ImageList = imageList;
    }

    public TreeNode AddToolsNode(string providerName, IEnumerable<IMigrationTool> tools)
    {
        if (!tools.IsNullOrEmpty())
        {
            var providerNode = new TreeNode(providerName);

            foreach (var tool in tools)
            {
                TreeNode toolNode;

                if (tool.Icon == null)
                {
                    toolNode = new TreeNode(tool.Name, 0, 0);
                }
                else
                {
                    imageList.Images.Add(tool.Icon);
                    int imageIndex = imageList.Images.Count - 1;
                    toolNode = new TreeNode(tool.Name, imageIndex, imageIndex);
                }

                toolNode.ToolTipText = tool.Description;
                toolNode.Tag = tool.ControlContent;
                providerNode.Nodes.Add(toolNode);
            }

            Nodes.Add(providerNode);
            return providerNode;
        }
        return null;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        imageList?.Dispose();
    }
}