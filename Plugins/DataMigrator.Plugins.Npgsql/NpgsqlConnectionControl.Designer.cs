using Krypton.Toolkit;

namespace DataMigrator.Plugins.Npgsql
{
    partial class NpgsqlConnectionControl
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
            this.txtDatabase = new Krypton.Toolkit.KryptonTextBox();
            this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
            this.lblPassword = new Krypton.Toolkit.KryptonLabel();
            this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
            this.lblUserName = new Krypton.Toolkit.KryptonLabel();
            this.lblDatabase = new Krypton.Toolkit.KryptonLabel();
            this.txtServer = new Krypton.Toolkit.KryptonTextBox();
            this.lblServer = new Krypton.Toolkit.KryptonLabel();
            this.txtPort = new Krypton.Toolkit.KryptonTextBox();
            this.lblPort = new Krypton.Toolkit.KryptonLabel();
            this.txtSchema = new Krypton.Toolkit.KryptonTextBox();
            this.lblSchema = new Krypton.Toolkit.KryptonLabel();
            this.SuspendLayout();
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(313, 6);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(214, 23);
            this.txtDatabase.TabIndex = 13;
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(313, 64);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(214, 23);
            this.txtPassword.TabIndex = 19;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(245, 67);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(62, 20);
            this.lblPassword.TabIndex = 18;
            this.lblPassword.Values.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(83, 64);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(156, 23);
            this.txtUserName.TabIndex = 17;
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(3, 67);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(74, 20);
            this.lblUserName.TabIndex = 16;
            this.lblUserName.Values.Text = "User Name:";
            // 
            // lblDatabase
            // 
            this.lblDatabase.Location = new System.Drawing.Point(245, 9);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(64, 20);
            this.lblDatabase.TabIndex = 12;
            this.lblDatabase.Values.Text = "Database:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(83, 6);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(156, 23);
            this.txtServer.TabIndex = 11;
            // 
            // lblServer
            // 
            this.lblServer.Location = new System.Drawing.Point(3, 9);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(47, 20);
            this.lblServer.TabIndex = 10;
            this.lblServer.Values.Text = "Server:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(83, 35);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(156, 23);
            this.txtPort.TabIndex = 15;
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(3, 38);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(36, 20);
            this.lblPort.TabIndex = 14;
            this.lblPort.Values.Text = "Port:";
            // 
            // txtSchema
            // 
            this.txtSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSchema.Location = new System.Drawing.Point(313, 35);
            this.txtSchema.Name = "txtSchema";
            this.txtSchema.Size = new System.Drawing.Size(214, 23);
            this.txtSchema.TabIndex = 21;
            // 
            // lblSchema
            // 
            this.lblSchema.Location = new System.Drawing.Point(245, 38);
            this.lblSchema.Name = "lblSchema";
            this.lblSchema.Size = new System.Drawing.Size(56, 20);
            this.lblSchema.TabIndex = 20;
            this.lblSchema.Values.Text = "Schema:";
            // 
            // NpgsqlConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtSchema);
            this.Controls.Add(this.lblSchema);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Name = "NpgsqlConnectionControl";
            this.Size = new System.Drawing.Size(540, 122);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KryptonTextBox txtDatabase;
        private KryptonTextBox txtPassword;
        private KryptonLabel lblPassword;
        private KryptonTextBox txtUserName;
        private KryptonLabel lblUserName;
        private KryptonLabel lblDatabase;
        private KryptonTextBox txtServer;
        private KryptonLabel lblServer;
        private KryptonTextBox txtPort;
        private KryptonLabel lblPort;
        private KryptonTextBox txtSchema;
        private KryptonLabel lblSchema;
    }
}
