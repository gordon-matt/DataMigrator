namespace DataMigrator.Controls.Dialogs;

public partial class InputDialog : KryptonForm
{
    public InputDialog()
    {
        InitializeComponent();
    }

    public string LabelText
    {
        get => lblInput.Text;
        set => lblInput.Text = value;
    }

    public string UserInput
    {
        get => txtInput.Text;
        set => txtInput.Text = value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnCancel_Click(object sender, EventArgs e) => Close();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void btnOK_Click(object sender, EventArgs e) => Close();
}