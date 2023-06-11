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
        this.lblUrl = new Krypton.Toolkit.KryptonLabel();
        this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
        this.lblPassword = new Krypton.Toolkit.KryptonLabel();
        this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
        this.lblUserName = new Krypton.Toolkit.KryptonLabel();
        this.txtDomain = new Krypton.Toolkit.KryptonTextBox();
        this.lblDomain = new Krypton.Toolkit.KryptonLabel();
        this.SuspendLayout();
        // 
        // txtUrl
        // 
        this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtUrl.Location = new System.Drawing.Point(90, 10);
        this.txtUrl.Name = "txtUrl";
        this.txtUrl.Size = new System.Drawing.Size(367, 23);
        this.txtUrl.TabIndex = 1;
        // 
        // lblUrl
        // 
        this.lblUrl.Location = new System.Drawing.Point(10, 13);
        this.lblUrl.Name = "lblUrl";
        this.lblUrl.Size = new System.Drawing.Size(29, 20);
        this.lblUrl.TabIndex = 2;
        this.lblUrl.Values.Text = "Url:";
        // 
        // txtPassword
        // 
        this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtPassword.Location = new System.Drawing.Point(314, 39);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(143, 23);
        this.txtPassword.TabIndex = 13;
        // 
        // lblPassword
        // 
        this.lblPassword.Location = new System.Drawing.Point(252, 42);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(62, 20);
        this.lblPassword.TabIndex = 12;
        this.lblPassword.Values.Text = "Password";
        // 
        // txtUserName
        // 
        this.txtUserName.Location = new System.Drawing.Point(90, 39);
        this.txtUserName.Name = "txtUserName";
        this.txtUserName.Size = new System.Drawing.Size(156, 23);
        this.txtUserName.TabIndex = 11;
        // 
        // lblUserName
        // 
        this.lblUserName.Location = new System.Drawing.Point(10, 42);
        this.lblUserName.Name = "lblUserName";
        this.lblUserName.Size = new System.Drawing.Size(74, 20);
        this.lblUserName.TabIndex = 10;
        this.lblUserName.Values.Text = "User Name:";
        // 
        // txtDomain
        // 
        this.txtDomain.Location = new System.Drawing.Point(90, 68);
        this.txtDomain.Name = "txtDomain";
        this.txtDomain.Size = new System.Drawing.Size(156, 23);
        this.txtDomain.TabIndex = 15;
        // 
        // lblDomain
        // 
        this.lblDomain.Location = new System.Drawing.Point(10, 71);
        this.lblDomain.Name = "lblDomain";
        this.lblDomain.Size = new System.Drawing.Size(56, 20);
        this.lblDomain.TabIndex = 14;
        this.lblDomain.Values.Text = "Domain:";
        // 
        // SharePointConnectionControl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
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
        this.Name = "SharePointConnectionControl";
        this.Size = new System.Drawing.Size(463, 111);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private KryptonTextBox txtUrl;
    private KryptonLabel lblUrl;
    private KryptonTextBox txtPassword;
    private KryptonLabel lblPassword;
    private KryptonTextBox txtUserName;
    private KryptonLabel lblUserName;
    private KryptonTextBox txtDomain;
    private KryptonLabel lblDomain;

}
