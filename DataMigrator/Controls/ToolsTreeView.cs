using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DataMigrator.Common;
using Extenso.Collections;

namespace DataMigrator.Controls
{
    public class ToolsTreeView : TreeView
    {
        private ImageList imageList = null;

        public ToolsTreeView()
        {
            imageList = new ImageList();
            imageList.ImageSize = new Size(24, 24);
            imageList.Images.Add(Resources.TreeNode);
            this.ImageList = imageList;
        }

        public TreeNode AddToolsNode(string providerName, IEnumerable<IMigrationTool> tools)
        {
            if (!tools.IsNullOrEmpty())
            {
                var providerNode = new TreeNode(providerName);

                foreach (var tool in tools)
                {
                    TreeNode toolNode = null;

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

                this.Nodes.Add(providerNode);
                return providerNode;
            }
            return null;
        }
    }
}