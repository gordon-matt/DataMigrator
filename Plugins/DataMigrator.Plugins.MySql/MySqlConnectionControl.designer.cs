using Krypton.Toolkit;

namespace DataMigrator.MySql;

partial class MySqlConnectionControl
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.grpLogOnServer = new System.Windows.Forms.GroupBox();
            this.lblPassword = new Krypton.Toolkit.KryptonLabel();
            this.lblUserName = new Krypton.Toolkit.KryptonLabel();
            this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
            this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
            this.txtPort = new Krypton.Toolkit.KryptonTextBox();
            this.lblPort = new Krypton.Toolkit.KryptonLabel();
            this.txtServer = new Krypton.Toolkit.KryptonTextBox();
            this.lblServer = new Krypton.Toolkit.KryptonLabel();
            this.grpConnectDatabase = new System.Windows.Forms.GroupBox();
            this.lblDatabase = new Krypton.Toolkit.KryptonLabel();
            this.cmbDatabase = new Krypton.Toolkit.KryptonComboBox();
            this.grpLogOnServer.SuspendLayout();
            this.grpConnectDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase)).BeginInit();
            this.SuspendLayout();
            // 
            // grpLogOnServer
            // 
            this.grpLogOnServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLogOnServer.Controls.Add(this.lblPassword);
            this.grpLogOnServer.Controls.Add(this.lblUserName);
            this.grpLogOnServer.Controls.Add(this.txtPassword);
            this.grpLogOnServer.Controls.Add(this.txtUserName);
            this.grpLogOnServer.Location = new System.Drawing.Point(19, 124);
            this.grpLogOnServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpLogOnServer.Name = "grpLogOnServer";
            this.grpLogOnServer.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpLogOnServer.Size = new System.Drawing.Size(434, 95);
            this.grpLogOnServer.TabIndex = 4;
            this.grpLogOnServer.TabStop = false;
            this.grpLogOnServer.Text = "Log on to the server";
            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassword.Enabled = false;
            this.lblPassword.Location = new System.Drawing.Point(13, 57);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(62, 20);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Values.Text = "Password";
            // 
            // lblUserName
            // 
            this.lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserName.Enabled = false;
            this.lblUserName.Location = new System.Drawing.Point(13, 27);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(71, 20);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Values.Text = "User Name";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(89, 51);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(337, 23);
            this.txtPassword.TabIndex = 3;
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Location = new System.Drawing.Point(89, 22);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(337, 23);
            this.txtUserName.TabIndex = 1;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(19, 95);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(135, 23);
            this.txtPort.TabIndex = 3;
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(19, 69);
            this.lblPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(36, 20);
            this.lblPort.TabIndex = 2;
            this.lblPort.Values.Text = "Port:";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(19, 40);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(434, 23);
            this.txtServer.TabIndex = 1;
            // 
            // lblServer
            // 
            this.lblServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServer.Location = new System.Drawing.Point(19, 14);
            this.lblServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(47, 20);
            this.lblServer.TabIndex = 0;
            this.lblServer.Values.Text = "Server:";
            // 
            // grpConnectDatabase
            // 
            this.grpConnectDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConnectDatabase.Controls.Add(this.lblDatabase);
            this.grpConnectDatabase.Controls.Add(this.cmbDatabase);
            this.grpConnectDatabase.Location = new System.Drawing.Point(19, 225);
            this.grpConnectDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpConnectDatabase.Name = "grpConnectDatabase";
            this.grpConnectDatabase.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpConnectDatabase.Size = new System.Drawing.Size(434, 71);
            this.grpConnectDatabase.TabIndex = 5;
            this.grpConnectDatabase.TabStop = false;
            this.grpConnectDatabase.Text = "Connect to a database";
            // 
            // lblDatabase
            // 
            this.lblDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDatabase.Location = new System.Drawing.Point(16, 31);
            this.lblDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(61, 20);
            this.lblDatabase.TabIndex = 0;
            this.lblDatabase.Values.Text = "Database";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDatabase.CornerRoundingRadius = -1F;
            this.cmbDatabase.DropDownWidth = 334;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.IntegralHeight = false;
            this.cmbDatabase.Location = new System.Drawing.Point(89, 28);
            this.cmbDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(337, 21);
            this.cmbDatabase.Sorted = true;
            this.cmbDatabase.TabIndex = 1;
            this.cmbDatabase.DropDown += new System.EventHandler(this.cmbDatabase_DropDown);
            // 
            // MySqlConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.grpLogOnServer);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.grpConnectDatabase);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MySqlConnectionControl";
            this.Size = new System.Drawing.Size(472, 315);
            this.grpLogOnServer.ResumeLayout(false);
            this.grpLogOnServer.PerformLayout();
            this.grpConnectDatabase.ResumeLayout(false);
            this.grpConnectDatabase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private GroupBox grpLogOnServer;
    private KryptonLabel lblPassword;
    private KryptonLabel lblUserName;
    private KryptonTextBox txtPassword;
    private KryptonTextBox txtUserName;
    private KryptonTextBox txtPort;
    private KryptonLabel lblPort;
    private KryptonTextBox txtServer;
    private KryptonLabel lblServer;
    private GroupBox grpConnectDatabase;
    private KryptonLabel lblDatabase;
    private KryptonComboBox cmbDatabase;
}
