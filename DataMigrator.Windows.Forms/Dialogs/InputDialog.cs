namespace DataMigrator.Windows.Forms;

public partial class InputDialog : Form
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

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        Close();
    }
}