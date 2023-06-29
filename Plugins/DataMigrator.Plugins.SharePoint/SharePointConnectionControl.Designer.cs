using Krypton.Toolkit;

namespace DataMigrator.SharePoint;

partial class SharePointConnectionControl
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
            this.txtUrl = new Krypton.Toolkit.KryptonTextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtDomain = new Krypton.Toolkit.KryptonTextBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(105, 12);
            this.txtUrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(347, 23);
            this.txtUrl.TabIndex = 1;
            // 
            // lblUrl
            // 
            this.lblUrl.Location = new System.Drawing.Point(12, 15);
            this.lblUrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 20);
            this.lblUrl.TabIndex = 0;
            this.lblUrl.Text = "Url:";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(105, 70);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(347, 23);
            this.txtPassword.TabIndex = 5;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(12, 73);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(62, 20);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Location = new System.Drawing.Point(105, 41);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(347, 23);
            this.txtUserName.TabIndex = 3;
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(12, 44);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(74, 20);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "User Name:";
            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Location = new System.Drawing.Point(105, 99);
            this.txtDomain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(347, 23);
            this.txtDomain.TabIndex = 7;
            // 
            // lblDomain
            // 
            this.lblDomain.Location = new System.Drawing.Point(12, 103);
            this.lblDomain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(56, 20);
            this.lblDomain.TabIndex = 6;
            this.lblDomain.Text = "Domain:";
            // 
            // SharePointConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.lblDomain);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.txtUrl);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SharePointConnectionControl";
            this.Size = new System.Drawing.Size(472, 174);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private KryptonTextBox txtUrl;
    private Label lblUrl;
    private KryptonTextBox txtPassword;
    private Label lblPassword;
    private KryptonTextBox txtUserName;
    private Label lblUserName;
    private KryptonTextBox txtDomain;
    private Label lblDomain;

}
