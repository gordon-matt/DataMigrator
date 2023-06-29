namespace DataMigrator.Sql;

partial class SqlConnectionControl
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
            this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
            this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
            this.lblServer = new Krypton.Toolkit.KryptonLabel();
            this.txtServer = new Krypton.Toolkit.KryptonTextBox();
            this.grpConnectDatabase = new System.Windows.Forms.GroupBox();
            this.lblDatabase = new Krypton.Toolkit.KryptonLabel();
            this.cmbDatabase = new Krypton.Toolkit.KryptonComboBox();
            this.grpLogOnServer = new System.Windows.Forms.GroupBox();
            this.lblPassword = new Krypton.Toolkit.KryptonLabel();
            this.lblUserName = new Krypton.Toolkit.KryptonLabel();
            this.rbUseSqlServerAuthentication = new Krypton.Toolkit.KryptonRadioButton();
            this.rbUseWindowsAuthentication = new Krypton.Toolkit.KryptonRadioButton();
            this.grpConnectDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase)).BeginInit();
            this.grpLogOnServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(114, 113);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(302, 23);
            this.txtPassword.TabIndex = 5;
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(114, 84);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(302, 23);
            this.txtUserName.TabIndex = 3;
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
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(19, 40);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(434, 23);
            this.txtServer.TabIndex = 1;
            // 
            // grpConnectDatabase
            // 
            this.grpConnectDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConnectDatabase.Controls.Add(this.lblDatabase);
            this.grpConnectDatabase.Controls.Add(this.cmbDatabase);
            this.grpConnectDatabase.Location = new System.Drawing.Point(19, 229);
            this.grpConnectDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpConnectDatabase.Name = "grpConnectDatabase";
            this.grpConnectDatabase.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpConnectDatabase.Size = new System.Drawing.Size(434, 71);
            this.grpConnectDatabase.TabIndex = 3;
            this.grpConnectDatabase.TabStop = false;
            this.grpConnectDatabase.Text = "Connect to a database";
            // 
            // lblDatabase
            // 
            this.lblDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDatabase.Location = new System.Drawing.Point(14, 31);
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
            this.cmbDatabase.Location = new System.Drawing.Point(82, 28);
            this.cmbDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(334, 21);
            this.cmbDatabase.Sorted = true;
            this.cmbDatabase.TabIndex = 1;
            this.cmbDatabase.DropDown += new System.EventHandler(this.cmbDatabase_DropDown);
            // 
            // grpLogOnServer
            // 
            this.grpLogOnServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLogOnServer.Controls.Add(this.lblPassword);
            this.grpLogOnServer.Controls.Add(this.lblUserName);
            this.grpLogOnServer.Controls.Add(this.rbUseSqlServerAuthentication);
            this.grpLogOnServer.Controls.Add(this.txtPassword);
            this.grpLogOnServer.Controls.Add(this.rbUseWindowsAuthentication);
            this.grpLogOnServer.Controls.Add(this.txtUserName);
            this.grpLogOnServer.Location = new System.Drawing.Point(19, 69);
            this.grpLogOnServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpLogOnServer.Name = "grpLogOnServer";
            this.grpLogOnServer.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpLogOnServer.Size = new System.Drawing.Size(434, 153);
            this.grpLogOnServer.TabIndex = 2;
            this.grpLogOnServer.TabStop = false;
            this.grpLogOnServer.Text = "Log on to the server";
            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassword.Enabled = false;
            this.lblPassword.Location = new System.Drawing.Point(38, 119);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(62, 20);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Values.Text = "Password";
            // 
            // lblUserName
            // 
            this.lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserName.Enabled = false;
            this.lblUserName.Location = new System.Drawing.Point(38, 89);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(71, 20);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Values.Text = "User Name";
            // 
            // rbUseSqlServerAuthentication
            // 
            this.rbUseSqlServerAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbUseSqlServerAuthentication.Location = new System.Drawing.Point(17, 59);
            this.rbUseSqlServerAuthentication.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbUseSqlServerAuthentication.Name = "rbUseSqlServerAuthentication";
            this.rbUseSqlServerAuthentication.Size = new System.Drawing.Size(189, 20);
            this.rbUseSqlServerAuthentication.TabIndex = 1;
            this.rbUseSqlServerAuthentication.Values.Text = "Use SQL Server Authentication";
            this.rbUseSqlServerAuthentication.CheckedChanged += new System.EventHandler(this.rbIntegratedSecurity_CheckedChanged);
            // 
            // rbUseWindowsAuthentication
            // 
            this.rbUseWindowsAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbUseWindowsAuthentication.Checked = true;
            this.rbUseWindowsAuthentication.Location = new System.Drawing.Point(17, 32);
            this.rbUseWindowsAuthentication.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbUseWindowsAuthentication.Name = "rbUseWindowsAuthentication";
            this.rbUseWindowsAuthentication.Size = new System.Drawing.Size(180, 20);
            this.rbUseWindowsAuthentication.TabIndex = 0;
            this.rbUseWindowsAuthentication.Values.Text = "Use Windows Authentication";
            this.rbUseWindowsAuthentication.CheckedChanged += new System.EventHandler(this.rbIntegratedSecurity_CheckedChanged);
            // 
            // SqlConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.grpConnectDatabase);
            this.Controls.Add(this.grpLogOnServer);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SqlConnectionControl";
            this.Size = new System.Drawing.Size(472, 317);
            this.grpConnectDatabase.ResumeLayout(false);
            this.grpConnectDatabase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase)).EndInit();
            this.grpLogOnServer.ResumeLayout(false);
            this.grpLogOnServer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private KryptonTextBox txtPassword;
    private KryptonTextBox txtUserName;
    private KryptonLabel lblServer;
    private KryptonTextBox txtServer;
    private GroupBox grpConnectDatabase;
    private KryptonLabel lblDatabase;
    private KryptonComboBox cmbDatabase;
    private GroupBox grpLogOnServer;
    private KryptonLabel lblPassword;
    private KryptonLabel lblUserName;
    private KryptonRadioButton rbUseSqlServerAuthentication;
    private KryptonRadioButton rbUseWindowsAuthentication;
}
