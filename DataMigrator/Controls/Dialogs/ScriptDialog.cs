namespace DataMigrator.Controls.Dialogs;

public partial class ScriptDialog : KryptonForm
{
    public string Script
    {
        get => fctScript.Text;
        set => fctScript.Text = value;
    }

    public ScriptDialog()
    {
        InitializeComponent();
    }

    private void ScriptDialog_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Script))
        {
            Script =
@"public object Transform(object value)
{
    // Modify the value here.
    return value;
}";
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnCancel_Click(object sender, EventArgs e) => Close();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnOK_Click(object sender, EventArgs e) => Close();
}