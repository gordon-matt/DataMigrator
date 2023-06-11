using Krypton.Toolkit;

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
        this.lblPassword = new Krypton.Toolkit.KryptonLabel();
        this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
        this.lblUserName = new Krypton.Toolkit.KryptonLabel();
        this.cbIntegratedSecurity = new Krypton.Toolkit.KryptonCheckBox();
        this.lblDatabase = new Krypton.Toolkit.KryptonLabel();
        this.cmbDatabase = new Krypton.Toolkit.KryptonComboBox();
        this.lblServer = new Krypton.Toolkit.KryptonLabel();
        this.txtServer = new Krypton.Toolkit.KryptonTextBox();
        ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase)).BeginInit();
        this.SuspendLayout();
        // 
        // txtPassword
        // 
        this.txtPassword.Enabled = false;
        this.txtPassword.Location = new System.Drawing.Point(97, 102);
        this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(182, 23);
        this.txtPassword.TabIndex = 7;
        // 
        // lblPassword
        // 
        this.lblPassword.Location = new System.Drawing.Point(4, 104);
        this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(62, 20);
        this.lblPassword.TabIndex = 6;
        this.lblPassword.Values.Text = "Password";
        // 
        // txtUserName
        // 
        this.txtUserName.Enabled = false;
        this.txtUserName.Location = new System.Drawing.Point(97, 68);
        this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.txtUserName.Name = "txtUserName";
        this.txtUserName.Size = new System.Drawing.Size(182, 23);
        this.txtUserName.TabIndex = 5;
        // 
        // lblUserName
        // 
        this.lblUserName.Location = new System.Drawing.Point(4, 69);
        this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.lblUserName.Name = "lblUserName";
        this.lblUserName.Size = new System.Drawing.Size(74, 20);
        this.lblUserName.TabIndex = 4;
        this.lblUserName.Values.Text = "User Name:";
        // 
        // cbIntegratedSecurity
        // 
        this.cbIntegratedSecurity.Checked = true;
        this.cbIntegratedSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
        this.cbIntegratedSecurity.Location = new System.Drawing.Point(97, 38);
        this.cbIntegratedSecurity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.cbIntegratedSecurity.Name = "cbIntegratedSecurity";
        this.cbIntegratedSecurity.Size = new System.Drawing.Size(127, 20);
        this.cbIntegratedSecurity.TabIndex = 3;
        this.cbIntegratedSecurity.Values.Text = "Integrated Security";
        this.cbIntegratedSecurity.CheckedChanged += new System.EventHandler(this.cbIntegratedSecurity_CheckedChanged);
        // 
        // lblDatabase
        // 
        this.lblDatabase.Location = new System.Drawing.Point(4, 136);
        this.lblDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.lblDatabase.Name = "lblDatabase";
        this.lblDatabase.Size = new System.Drawing.Size(64, 20);
        this.lblDatabase.TabIndex = 8;
        this.lblDatabase.Values.Text = "Database:";
        // 
        // cmbDatabase
        // 
        this.cmbDatabase.CornerRoundingRadius = -1F;
        this.cmbDatabase.DropDownWidth = 280;
        this.cmbDatabase.FormattingEnabled = true;
        this.cmbDatabase.IntegralHeight = false;
        this.cmbDatabase.Location = new System.Drawing.Point(97, 135);
        this.cmbDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.cmbDatabase.Name = "cmbDatabase";
        this.cmbDatabase.Size = new System.Drawing.Size(468, 21);
        this.cmbDatabase.TabIndex = 9;
        this.cmbDatabase.DropDown += new System.EventHandler(this.cmbDatabase_DropDown);
        // 
        // lblServer
        // 
        this.lblServer.Location = new System.Drawing.Point(4, 10);
        this.lblServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.lblServer.Name = "lblServer";
        this.lblServer.Size = new System.Drawing.Size(47, 20);
        this.lblServer.TabIndex = 0;
        this.lblServer.Values.Text = "Server:";
        // 
        // txtServer
        // 
        this.txtServer.Location = new System.Drawing.Point(97, 7);
        this.txtServer.Name = "txtServer";
        this.txtServer.Size = new System.Drawing.Size(468, 23);
        this.txtServer.TabIndex = 10;
        this.txtServer.Text = "kryptonTextBox1";
        // 
        // SqlConnectionControl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.Controls.Add(this.txtServer);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtUserName);
        this.Controls.Add(this.lblUserName);
        this.Controls.Add(this.cbIntegratedSecurity);
        this.Controls.Add(this.lblDatabase);
        this.Controls.Add(this.cmbDatabase);
        this.Controls.Add(this.lblServer);
        this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.Name = "SqlConnectionControl";
        this.Size = new System.Drawing.Size(587, 210);
        ((System.ComponentModel.ISupportInitialize)(this.cmbDatabase)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion
    private KryptonTextBox txtPassword;
    private KryptonLabel lblPassword;
    private KryptonTextBox txtUserName;
    private KryptonLabel lblUserName;
    private KryptonCheckBox cbIntegratedSecurity;
    private KryptonLabel lblDatabase;
    private KryptonComboBox cmbDatabase;
    private KryptonLabel lblServer;
    private KryptonTextBox txtServer;
}
