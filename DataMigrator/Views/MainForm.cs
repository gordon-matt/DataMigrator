﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using DataMigrator.Common;
using DataMigrator.Common.Configuration;
using DataMigrator.Windows.Forms;

namespace DataMigrator.Views
{
    public partial class MainForm : KryptonForm
    {
        private TraceViewerControl traceViewer = new TraceViewerControl();
        private UserControl currentControl = null;

        [ImportMany(typeof(IMigrationPlugin))]
        private IEnumerable<IMigrationPlugin> plugins = null;

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
            treeView.LoadDefaultNodes();

            if (!this.IsInWinDesignMode())
            {
                ShowTraceViewer();//Initialize here or get an error: "Window Handle not yet created"
            }

            using (AggregateCatalog aggregateCatalog = new AggregateCatalog(
                new AssemblyCatalog(typeof(Program).Assembly),
                new DirectoryCatalog(Path.Combine(Application.StartupPath, "Plugins"))))
            {
                using (CompositionContainer container = new CompositionContainer(aggregateCatalog))
                {
                    container.ComposeParts(this);
                }
            }

            Program.Plugins = this.plugins;
            this.plugins = null;
        }

        #endregion

        #region Private Methods

        private DialogResult CheckSaveChanges()
        {
            DialogResult dialogResult = MessageBox.Show(
                "Do you want to save the current file?",
                "Save Changes?",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.OK)
            {
                if (dlgSaveFile.ShowDialog() == DialogResult.OK)
                {
                    Program.Configuration.SaveAs(dlgSaveFile.FileName);
                }
            }

            return dialogResult;
        }

        private void SaveCurrentControl()
        {
            if (panelMain.Controls.Count < 1)
            { return; }

            //Control currentControl = panelMain.Controls[0];
            if (currentControl != null)
            {
                if (currentControl is ConnectionsControl)
                {
                    ConnectionsControl control = currentControl as ConnectionsControl;

                    if (control.SourceConnection != null)
                    { Program.Configuration.SourceConnection = control.SourceConnection; }

                    if (control.DestinationConnection != null)
                    { Program.Configuration.DestinationConnection = control.DestinationConnection; }
                }
                else if (currentControl is TableMappingControl)
                {
                    TableMappingControl control = currentControl as TableMappingControl;
                    Program.CurrentJob.SourceTable = control.SourceTable;
                    Program.CurrentJob.DestinationTable = control.DestinationTable;

                    Program.CurrentJob.FieldMappings.Clear();
                    Program.CurrentJob.FieldMappings.AddRange(control.FieldMappings);

                    Job existingJob = Program.Configuration.Jobs[Program.CurrentJob.Name];

                    if (existingJob == null)
                    {
                        Program.Configuration.Jobs.Add(Program.CurrentJob);
                    }
                }
            }
        }

        private T LoadUserControl<T>() where T : UserControl
        {
            //SaveCurrentControl();
            T control = Activator.CreateInstance<T>();
            panelMain.Controls.Clear();
            panelMain.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            if (control.MinimumSize != new Size(0, 0))
            {
                this.Width = this.Width - panelMain.Width + control.MinimumSize.Width;
                this.Height = this.Height - panelMain.Height + control.MinimumSize.Height;
            }

            currentControl = control;
            mnuMainToolsShowTraceViewer.Checked = false;
            return control;
        }

        public void HideTraceViewer()
        {
            panelMain.Controls.Clear();
            if (currentControl != null)
            {
                panelMain.Controls.Add(currentControl);
                currentControl.Dock = DockStyle.Fill;
            }
            mnuMainToolsShowTraceViewer.Checked = false;
        }

        public void ShowTraceViewer()
        {
            panelMain.Controls.Clear();
            panelMain.Controls.Add(traceViewer);
            traceViewer.Dock = DockStyle.Fill;
            mnuMainToolsShowTraceViewer.Checked = true;
        }

        private void NewFile()
        {
            CheckSaveChanges();
            Program.Configuration = new DataMigrationConfigFile();
            treeView.ClearJobs();
        }

        private void OpenFile()
        {
            DialogResult dialogResult = CheckSaveChanges();
            if (dialogResult == DialogResult.Cancel)
            { return; }

            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                Program.Configuration = DataMigrationConfigFile.Load(dlgOpenFile.FileName);
                treeView.ClearJobs();
                foreach (Job job in Program.Configuration.Jobs.OrderBy(j => j.Name))
                {
                    treeView.AddJob(job);
                }
            }
        }

        private void SaveFile()
        {
            SaveCurrentControl();
            panelMain.Controls.Clear();
            Program.Configuration.Save();
        }

        #endregion

        #region Control Event Handlers

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            RunJobsForm form = new RunJobsForm();
            form.ShowDialog();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SaveCurrentControl();

            if (e.Node.Level == 2) // Job Node
            {
                Program.CurrentJob = e.Node.Tag as Job;
                LoadUserControl<TableMappingControl>();
            }
            else
            {
                switch (e.Node.Text)
                {
                    case Constants.TreeView.ROOT_NODE_TEXT: ShowTraceViewer(); break;
                    case Constants.TreeView.CONNECTIONS_NODE_TEXT: LoadUserControl<ConnectionsControl>(); break;
                    case Constants.TreeView.SETTINGS_NODE_TEXT:
                        {
                            new SettingsForm().ShowDialog();
                            treeView.SelectedNode = treeView.Nodes[0];
                        }
                        break;
                    default: break;
                }
            }
        }

        #region Main Menu

        private void mnuMainFileNew_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void mnuMainFileOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void mnuMainFileSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void mnuMainFileSaveAs_Click(object sender, EventArgs e)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                SaveCurrentControl();
                panelMain.Controls.Clear();
                Program.Configuration.SaveAs(dlgSaveFile.FileName);
            }
        }

        private void mnuMainFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuMainToolsShowTraceViewer_Click(object sender, EventArgs e)
        {
            if (mnuMainToolsShowTraceViewer.Checked)
            {
                HideTraceViewer();
            }
            else
            {
                ShowTraceViewer();
            }
        }

        private void mnuMainToolsOptions_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void mnuMainHelpAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void mnuMainToolsPluginTools_Click(object sender, EventArgs e)
        {
            new ToolsForm().ShowDialog();
        }

        #endregion

        #endregion
    }
}