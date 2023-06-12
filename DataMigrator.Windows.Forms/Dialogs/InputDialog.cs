namespace DataMigrator.Windows.Forms;

public partial class InputDialog : Form
{
    public InputDialog()
    {
        InitializeComponent();
    }

    public string LabelText
    {
        get { return lblInput.Text; }
        set { lblInput.Text = value; }
    }

    public string UserInput
    {
        get { return txtInput.Text; }
        set { txtInput.Text = value; }
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