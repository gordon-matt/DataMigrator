using DataMigrator.Controls.Dialogs;

namespace DataMigrator.Controls;

public class DataMigratorTreeView : TreeView
{
    private readonly ImageList imageList = null;
    private readonly ContextMenuStrip mnuContextJobs = null;
    private readonly ToolStripMenuItem mnuContextJobsNewJob;
    private readonly ContextMenuStrip mnuContextJobsJob = null;
    private readonly ToolStripMenuItem mnuContextJobsJobRename;
    private readonly ToolStripMenuItem mnuContextJobsJobDelete;

    private TreeNode RootNode { get; set; }
    private TreeNode ConnectionsNode { get; set; }
    private TreeNode JobsNode { get; set; }
    private TreeNode SettingsNode { get; set; }

    public delegate void TreeViewChangedHandler();

    public event TreeViewChangedHandler TreeViewChanged;

    public DataMigratorTreeView()
    {
        imageList = new ImageList();
        imageList.Images.Add(Resources.Transfer);
        imageList.Images.Add(Resources.DatabaseConfig);
        imageList.Images.Add(Resources.List);
        imageList.Images.Add(Resources.Settings);
        imageList.Images.Add(Resources.Table);
        imageList.ImageSize = new Size(32, 32);
        this.ImageList = imageList;

        mnuContextJobs = new ContextMenuStrip
        {
            Name = "mnuContextJobs"
        };

        mnuContextJobsNewJob = new ToolStripMenuItem("New Job") { Name = "mnuContextJobsNewJob" };
        mnuContextJobsNewJob.Click += new EventHandler(mnuContextJobsNewJob_Click);
        mnuContextJobs.Items.Add(mnuContextJobsNewJob);

        mnuContextJobsJob = new ContextMenuStrip { Name = "mnuContextJobsJob" };
        mnuContextJobsJobRename = new ToolStripMenuItem("Rename") { Name = "mnuContextJobsJobRename" };
        mnuContextJobsJobRename.Click += new EventHandler(mnuContextJobsJobRename_Click);
        mnuContextJobsJob.Items.Add(mnuContextJobsJobRename);

        mnuContextJobsJobDelete = new ToolStripMenuItem("Delete") { Name = "mnuContextJobsJobDelete" };
        mnuContextJobsJobDelete.Click += new EventHandler(mnuContextJobsJobDelete_Click);
        mnuContextJobsJob.Items.Add(mnuContextJobsJobDelete);
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

    private void mnuContextJobsJobRename_Click(object sender, EventArgs e)
    {
        using var dlgInput = new InputDialog
        {
            Text = "Rename Job",
            LabelText = "Enter job name:"
        };
        if (dlgInput.ShowDialog() == DialogResult.OK)
        {
            string newJobName = dlgInput.UserInput;

            string currentJobName = this.SelectedNode.Text;
            if (AppState.CurrentJob.Name == currentJobName)
            {
                AppState.CurrentJob.Name = newJobName;
            }
            else { AppState.ConfigFile.Jobs[currentJobName].Name = newJobName; }

            this.SelectedNode.Text = newJobName;
        }
    }

    private void mnuContextJobsJobDelete_Click(object sender, EventArgs e)
    {
        string jobName = this.SelectedNode.Text;
        var selectedJob = AppState.ConfigFile.Jobs[jobName];
        AppState.ConfigFile.Jobs.Remove(selectedJob);

        JobsNode.Nodes.Clear();
        foreach (var job in AppState.ConfigFile.Jobs.OrderBy(j => j.Name))
        {
            AddJob(job);
        }
        TreeViewChanged?.Invoke();
    }

    private void mnuContextJobsNewJob_Click(object sender, EventArgs e)
    {
        using var dlgInput = new InputDialog
        {
            Text = "Add New Job",
            LabelText = "Enter job name:"
        };
        if (dlgInput.ShowDialog() == DialogResult.OK)
        {
            string jobName = dlgInput.UserInput;

            if (AppState.ConfigFile.Jobs.Any(x => x.Name == jobName))
            {
                MessageBox.Show(
                    "There's already a job with that name. Try again.",
                    "Duplicate Job Name",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            AppState.CurrentJob = AddJob(jobName).Tag as Job;
            TreeViewChanged?.Invoke();
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

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        imageList?.Dispose();
        mnuContextJobsNewJob?.Dispose();
        mnuContextJobsJobRename?.Dispose();
        mnuContextJobsJob?.Dispose();
        mnuContextJobs?.Dispose();
    }
}