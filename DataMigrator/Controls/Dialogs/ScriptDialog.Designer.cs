namespace DataMigrator.Controls.Dialogs;

partial class ScriptDialog
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptDialog));
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOK = new Krypton.Toolkit.KryptonButton();
            this.fctScript = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fctScript)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CornerRoundingRadius = -1F;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(362, 504);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(167, 43);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.CornerRoundingRadius = -1F;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(189, 504);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(167, 43);
            this.btnOK.TabIndex = 4;
            this.btnOK.Values.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fctScript
            // 
            this.fctScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fctScript.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctScript.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:" +
    "]*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.fctScript.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.fctScript.BackBrush = null;
            this.fctScript.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.fctScript.CharHeight = 14;
            this.fctScript.CharWidth = 8;
            this.fctScript.DefaultMarkerSize = 8;
            this.fctScript.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctScript.IsReplaceMode = false;
            this.fctScript.Language = FastColoredTextBoxNS.Language.CSharp;
            this.fctScript.LeftBracket = '(';
            this.fctScript.LeftBracket2 = '{';
            this.fctScript.Location = new System.Drawing.Point(12, 12);
            this.fctScript.Name = "fctScript";
            this.fctScript.Paddings = new System.Windows.Forms.Padding(0);
            this.fctScript.RightBracket = ')';
            this.fctScript.RightBracket2 = '}';
            this.fctScript.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctScript.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctScript.ServiceColors")));
            this.fctScript.Size = new System.Drawing.Size(517, 486);
            this.fctScript.TabIndex = 6;
            this.fctScript.Zoom = 100;
            // 
            // ScriptDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 559);
            this.Controls.Add(this.fctScript);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ScriptDialog";
            this.Text = "ScriptsForm";
            this.Load += new System.EventHandler(this.ScriptDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fctScript)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private KryptonButton btnCancel;
    private KryptonButton btnOK;
    private FastColoredTextBoxNS.FastColoredTextBox fctScript;
}