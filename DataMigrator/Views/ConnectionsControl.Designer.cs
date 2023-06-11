using Krypton.Toolkit;

namespace DataMigrator.Views;

partial class ConnectionsControl
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
        this.grpSourceConnection = new System.Windows.Forms.GroupBox();
        this.btnValidateSourceConnection = new Krypton.Toolkit.KryptonButton();
        this.pnlSourceConnection = new Krypton.Toolkit.KryptonPanel();
        this.cmbSourceConnectionType = new Krypton.Toolkit.KryptonComboBox();
        this.grpDestinationConnection = new System.Windows.Forms.GroupBox();
        this.btnValidateDestinationConnection = new Krypton.Toolkit.KryptonButton();
        this.pnlDestinationConnection = new Krypton.Toolkit.KryptonPanel();
        this.cmbDestinationConnectionType = new Krypton.Toolkit.KryptonComboBox();
        this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
        this.grpSourceConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pnlSourceConnection)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbSourceConnectionType)).BeginInit();
        this.grpDestinationConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pnlDestinationConnection)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationConnectionType)).BeginInit();
        this.SuspendLayout();
        // 
        // grpSourceConnection
        // 
        this.grpSourceConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.grpSourceConnection.Controls.Add(this.btnValidateSourceConnection);
        this.grpSourceConnection.Controls.Add(this.pnlSourceConnection);
        this.grpSourceConnection.Controls.Add(this.cmbSourceConnectionType);
        this.grpSourceConnection.Location = new System.Drawing.Point(13, 13);
        this.grpSourceConnection.Name = "grpSourceConnection";
        this.grpSourceConnection.Size = new System.Drawing.Size(589, 200);
        this.grpSourceConnection.TabIndex = 0;
        this.grpSourceConnection.TabStop = false;
        this.grpSourceConnection.Text = "Source Connection Details";
        // 
        // btnValidateSourceConnection
        // 
        this.btnValidateSourceConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnValidateSourceConnection.Location = new System.Drawing.Point(508, 12);
        this.btnValidateSourceConnection.Name = "btnValidateSourceConnection";
        this.btnValidateSourceConnection.Size = new System.Drawing.Size(75, 28);
        this.btnValidateSourceConnection.TabIndex = 2;
        this.btnValidateSourceConnection.Values.Text = "Validate";
        this.btnValidateSourceConnection.Click += new System.EventHandler(this.btnValidateSourceConnection_Click);
        // 
        // pnlSourceConnection
        // 
        this.pnlSourceConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.pnlSourceConnection.Location = new System.Drawing.Point(6, 46);
        this.pnlSourceConnection.Name = "pnlSourceConnection";
        this.pnlSourceConnection.Size = new System.Drawing.Size(577, 148);
        this.pnlSourceConnection.TabIndex = 1;
        // 
        // cmbSourceConnectionType
        // 
        this.cmbSourceConnectionType.DropDownWidth = 158;
        this.cmbSourceConnectionType.FormattingEnabled = true;
        this.cmbSourceConnectionType.Location = new System.Drawing.Point(6, 19);
        this.cmbSourceConnectionType.Name = "cmbSourceConnectionType";
        this.cmbSourceConnectionType.Size = new System.Drawing.Size(158, 21);
        this.cmbSourceConnectionType.TabIndex = 0;
        this.cmbSourceConnectionType.SelectedIndexChanged += new System.EventHandler(this.cmbSourceConnectionType_SelectedIndexChanged);
        // 
        // grpDestinationConnection
        // 
        this.grpDestinationConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.grpDestinationConnection.Controls.Add(this.btnValidateDestinationConnection);
        this.grpDestinationConnection.Controls.Add(this.pnlDestinationConnection);
        this.grpDestinationConnection.Controls.Add(this.cmbDestinationConnectionType);
        this.grpDestinationConnection.Location = new System.Drawing.Point(13, 219);
        this.grpDestinationConnection.Name = "grpDestinationConnection";
        this.grpDestinationConnection.Size = new System.Drawing.Size(589, 200);
        this.grpDestinationConnection.TabIndex = 1;
        this.grpDestinationConnection.TabStop = false;
        this.grpDestinationConnection.Text = "Destination Connection Details";
        // 
        // btnValidateDestinationConnection
        // 
        this.btnValidateDestinationConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnValidateDestinationConnection.Location = new System.Drawing.Point(508, 12);
        this.btnValidateDestinationConnection.Name = "btnValidateDestinationConnection";
        this.btnValidateDestinationConnection.Size = new System.Drawing.Size(75, 28);
        this.btnValidateDestinationConnection.TabIndex = 3;
        this.btnValidateDestinationConnection.Values.Text = "Validate";
        this.btnValidateDestinationConnection.Click += new System.EventHandler(this.btnValidateDestinationConnection_Click);
        // 
        // pnlDestinationConnection
        // 
        this.pnlDestinationConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.pnlDestinationConnection.Location = new System.Drawing.Point(6, 46);
        this.pnlDestinationConnection.Name = "pnlDestinationConnection";
        this.pnlDestinationConnection.Size = new System.Drawing.Size(577, 148);
        this.pnlDestinationConnection.TabIndex = 2;
        // 
        // cmbDestinationConnectionType
        // 
        this.cmbDestinationConnectionType.DropDownWidth = 158;
        this.cmbDestinationConnectionType.FormattingEnabled = true;
        this.cmbDestinationConnectionType.Location = new System.Drawing.Point(6, 19);
        this.cmbDestinationConnectionType.Name = "cmbDestinationConnectionType";
        this.cmbDestinationConnectionType.Size = new System.Drawing.Size(158, 21);
        this.cmbDestinationConnectionType.TabIndex = 1;
        this.cmbDestinationConnectionType.SelectedIndexChanged += new System.EventHandler(this.cmbDestinationConnectionType_SelectedIndexChanged);
        // 
        // dlgOpenFile
        // 
        this.dlgOpenFile.Filter = "Xml Files|*.xml";
        // 
        // ConnectionsControl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.Controls.Add(this.grpDestinationConnection);
        this.Controls.Add(this.grpSourceConnection);
        this.Name = "ConnectionsControl";
        this.Size = new System.Drawing.Size(616, 429);
        this.grpSourceConnection.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pnlSourceConnection)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbSourceConnectionType)).EndInit();
        this.grpDestinationConnection.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pnlDestinationConnection)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationConnectionType)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpSourceConnection;
    private System.Windows.Forms.GroupBox grpDestinationConnection;
    private System.Windows.Forms.OpenFileDialog dlgOpenFile;
    private KryptonPanel pnlSourceConnection;
    private KryptonComboBox cmbSourceConnectionType;
    private KryptonPanel pnlDestinationConnection;
    private KryptonComboBox cmbDestinationConnectionType;
    private KryptonButton btnValidateSourceConnection;
    private KryptonButton btnValidateDestinationConnection;
}
