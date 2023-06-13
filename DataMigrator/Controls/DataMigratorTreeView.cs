using DataMigrator.Common.Configuration;
using DataMigrator.Windows.Forms;

namespace DataMigrator.Controls;

public class DataMigratorTreeView : TreeView
{
    private readonly ImageList imageList = null;
    private readonly ContextMenuStrip mnuContextJobs = null;
    private readonly ContextMenuStrip mnuContextJobsJob = null;

    private TreeNode RootNode { get; set; }
    private TreeNode ConnectionsNode { get; set; }
    private TreeNode JobsNode { get; set; }
    private TreeNode SettingsNode { get; set; }

    public DataMigratorTreeView()
    {
        imageList = new ImageList();
        imageList.Images.Add(Resources.Migrate);
        imageList.Images.Add(Resources.Connection);
        imageList.Images.Add(Resources.Mapping32x32);
        imageList.Images.Add(Resources.Options32x32);
        imageList.Images.Add(Resources.Table);
        imageList.ImageSize = new Size(24, 24);
        this.ImageList = imageList;

        mnuContextJobs = new ContextMenuStrip
        {
            Name = "mnuContextJobs"
        };

        var mnuContextJobsNewJob = new ToolStripMenuItem("New Job")
        {
            Name = "mnuContextJobsNewJob"
        };
        mnuContextJobsNewJob.Click += new System.EventHandler(mnuContextJobsNewJob_Click);
        mnuContextJobs.Items.Add(mnuContextJobsNewJob);

        mnuContextJobsJob = new ContextMenuStrip
        {
            Name = "mnuContextJobsJob"
        };

        var mnuContextJobsJobRename = new ToolStripMenuItem("Rename")
        {
            Name = "mnuContextJobsJobRename"
        };
        mnuContextJobsJobRename.Click += new System.EventHandler(mnuContextJobsJobRename_Click);
        mnuContextJobsJob.Items.Add(mnuContextJobsJobRename);
    }

    public void LoadDefaultNodes()
    {
        RootNode = new TreeNode(Constants.TreeView.ROOT_NODE_TEXT, 0, 0);
        ConnectionsNode = new TreeNode(Constants.TreeView.CONNECTIONS_NODE_TEXT, 1, 1);
        JobsNode = new TreeNode(Constants.TreeView.JOBS_NODE_TEXT, 2, 2);
        SettingsNode = new TreeNode(Constants.TreeView.SETTINGS_NODE_TEXT, 3, 3);

        RootNode.Nodes.Add(ConnectionsNode);
        RootNode.Nodes.Add(JobsNode);
        RootNode.Nodes.Add(SettingsNode);

        this.Nodes.Add(RootNode);
        RootNode.ExpandAll();
    }

    public TreeNode AddJob(string jobName)
    {
        var jobNode = new TreeNode(jobName, 4, 4)
        {
            Tag = new Job { Name = jobName }
        };
        JobsNode.Nodes.Add(jobNode);

        JobsNode.Expand();

        return jobNode;
    }

    public TreeNode AddJob(Job job)
    {
        var jobNode = new TreeNode(job.Name, 4, 4)
        {
            Tag = job
        };
        JobsNode.Nodes.Add(jobNode);

        JobsNode.Expand();

        return jobNode;
    }

    //public void ClearJobs()
    //{
    //    JobsNode.Nodes.Clear();
    //}

    public void Reset()
    {
        this.Nodes.Clear();
        LoadDefaultNodes();
    }

    private void mnuContextJobsJobRename_Click(object sender, System.EventArgs e)
    {
        var dlgInput = new InputDialog
        {
            Text = "Rename Job",
            LabelText = "Enter job name:"
        };
        if (dlgInput.ShowDialog() == DialogResult.OK)
        {
            string newJobName = dlgInput.UserInput;

            string currentJobName = this.SelectedNode.Text;
            if (Program.CurrentJob.Name == currentJobName)
            {
                Program.CurrentJob.Name = newJobName;
            }
            else { Program.Configuration.Jobs[currentJobName].Name = newJobName; }

            this.SelectedNode.Text = newJobName;
        }
    }

    private void mnuContextJobsNewJob_Click(object sender, System.EventArgs e)
    {
        var dlgInput = new InputDialog
        {
            Text = "Add New Job",
            LabelText = "Enter job name:"
        };
        if (dlgInput.ShowDialog() == DialogResult.OK)
        {
            string jobName = dlgInput.UserInput;
            Program.CurrentJob = AddJob(jobName).Tag as Job;
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            this.SelectedNode = this.GetNodeAt(e.X, e.Y);

            if (this.SelectedNode != null)
            {
                switch (SelectedNode.Level)
                {
                    case 2: mnuContextJobsJob.Show(this, e.Location); break;
                    case 1:
                        {
                            if (SelectedNode.Text == Constants.TreeView.JOBS_NODE_TEXT)
                            {
                                mnuContextJobs.Show(this, e.Location);
                            }
                        }
                        break;
                }
            }
        }

        base.OnMouseUp(e);
    }
}