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
        sourceConnectionControl?.Dispose();
        destinationConnectionControl?.Dispose();
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.grpSourceConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSourceConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceConnectionType)).BeginInit();
            this.grpDestinationConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlDestinationConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationConnectionType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSourceConnection
            // 
            this.grpSourceConnection.Controls.Add(this.btnValidateSourceConnection);
            this.grpSourceConnection.Controls.Add(this.pnlSourceConnection);
            this.grpSourceConnection.Controls.Add(this.cmbSourceConnectionType);
            this.grpSourceConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceConnection.Location = new System.Drawing.Point(0, 0);
            this.grpSourceConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpSourceConnection.Name = "grpSourceConnection";
            this.grpSourceConnection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpSourceConnection.Size = new System.Drawing.Size(440, 617);
            this.grpSourceConnection.TabIndex = 0;
            this.grpSourceConnection.TabStop = false;
            this.grpSourceConnection.Text = "Source Connection Details";
            // 
            // btnValidateSourceConnection
            // 
            this.btnValidateSourceConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidateSourceConnection.CornerRoundingRadius = -1F;
            this.btnValidateSourceConnection.Location = new System.Drawing.Point(338, 14);
            this.btnValidateSourceConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnValidateSourceConnection.Name = "btnValidateSourceConnection";
            this.btnValidateSourceConnection.Size = new System.Drawing.Size(88, 32);
            this.btnValidateSourceConnection.TabIndex = 2;
            this.btnValidateSourceConnection.Values.Text = "Validate";
            this.btnValidateSourceConnection.Click += new System.EventHandler(this.btnValidateSourceConnection_Click);
            // 
            // pnlSourceConnection
            // 
            this.pnlSourceConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSourceConnection.Location = new System.Drawing.Point(7, 53);
            this.pnlSourceConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlSourceConnection.Name = "pnlSourceConnection";
            this.pnlSourceConnection.Size = new System.Drawing.Size(418, 557);
            this.pnlSourceConnection.TabIndex = 1;
            // 
            // cmbSourceConnectionType
            // 
            this.cmbSourceConnectionType.CornerRoundingRadius = -1F;
            this.cmbSourceConnectionType.DropDownWidth = 158;
            this.cmbSourceConnectionType.FormattingEnabled = true;
            this.cmbSourceConnectionType.IntegralHeight = false;
            this.cmbSourceConnectionType.Location = new System.Drawing.Point(7, 22);
            this.cmbSourceConnectionType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSourceConnectionType.Name = "cmbSourceConnectionType";
            this.cmbSourceConnectionType.Size = new System.Drawing.Size(184, 21);
            this.cmbSourceConnectionType.TabIndex = 0;
            this.cmbSourceConnectionType.SelectedIndexChanged += new System.EventHandler(this.cmbSourceConnectionType_SelectedIndexChanged);
            // 
            // grpDestinationConnection
            // 
            this.grpDestinationConnection.Controls.Add(this.btnValidateDestinationConnection);
            this.grpDestinationConnection.Controls.Add(this.pnlDestinationConnection);
            this.grpDestinationConnection.Controls.Add(this.cmbDestinationConnectionType);
            this.grpDestinationConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDestinationConnection.Location = new System.Drawing.Point(0, 0);
            this.grpDestinationConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpDestinationConnection.Name = "grpDestinationConnection";
            this.grpDestinationConnection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpDestinationConnection.Size = new System.Drawing.Size(442, 617);
            this.grpDestinationConnection.TabIndex = 1;
            this.grpDestinationConnection.TabStop = false;
            this.grpDestinationConnection.Text = "Destination Connection Details";
            // 
            // btnValidateDestinationConnection
            // 
            this.btnValidateDestinationConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidateDestinationConnection.CornerRoundingRadius = -1F;
            this.btnValidateDestinationConnection.Location = new System.Drawing.Point(340, 14);
            this.btnValidateDestinationConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnValidateDestinationConnection.Name = "btnValidateDestinationConnection";
            this.btnValidateDestinationConnection.Size = new System.Drawing.Size(88, 32);
            this.btnValidateDestinationConnection.TabIndex = 3;
            this.btnValidateDestinationConnection.Values.Text = "Validate";
            this.btnValidateDestinationConnection.Click += new System.EventHandler(this.btnValidateDestinationConnection_Click);
            // 
            // pnlDestinationConnection
            // 
            this.pnlDestinationConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDestinationConnection.Location = new System.Drawing.Point(7, 53);
            this.pnlDestinationConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlDestinationConnection.Name = "pnlDestinationConnection";
            this.pnlDestinationConnection.Size = new System.Drawing.Size(420, 557);
            this.pnlDestinationConnection.TabIndex = 2;
            // 
            // cmbDestinationConnectionType
            // 
            this.cmbDestinationConnectionType.CornerRoundingRadius = -1F;
            this.cmbDestinationConnectionType.DropDownWidth = 158;
            this.cmbDestinationConnectionType.FormattingEnabled = true;
            this.cmbDestinationConnectionType.IntegralHeight = false;
            this.cmbDestinationConnectionType.Location = new System.Drawing.Point(7, 22);
            this.cmbDestinationConnectionType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDestinationConnectionType.Name = "cmbDestinationConnectionType";
            this.cmbDestinationConnectionType.Size = new System.Drawing.Size(184, 21);
            this.cmbDestinationConnectionType.TabIndex = 1;
            this.cmbDestinationConnectionType.SelectedIndexChanged += new System.EventHandler(this.cmbDestinationConnectionType_SelectedIndexChanged);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.grpSourceConnection);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpDestinationConnection);
            this.splitContainer.Size = new System.Drawing.Size(886, 617);
            this.splitContainer.SplitterDistance = 440;
            this.splitContainer.TabIndex = 2;
            // 
            // ConnectionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ConnectionsControl";
            this.Size = new System.Drawing.Size(886, 617);
            this.grpSourceConnection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSourceConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceConnectionType)).EndInit();
            this.grpDestinationConnection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlDestinationConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDestinationConnectionType)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpSourceConnection;
    private System.Windows.Forms.GroupBox grpDestinationConnection;
    private KryptonPanel pnlSourceConnection;
    private KryptonComboBox cmbSourceConnectionType;
    private KryptonPanel pnlDestinationConnection;
    private KryptonComboBox cmbDestinationConnectionType;
    private KryptonButton btnValidateSourceConnection;
    private KryptonButton btnValidateDestinationConnection;
    private SplitContainer splitContainer;
}
