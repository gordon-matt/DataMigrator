using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DataMigrator.Views;

internal partial class AboutForm : KryptonForm
{
    public AboutForm()
    {
        InitializeComponent();
        this.Text = string.Format("About {0}", AssemblyTitle);
        this.lblProductName.Text = AssemblyProduct;
        this.lblVersion.Text = string.Format("Version {0}", AssemblyVersion);
        this.lblCopyright.Text = AssemblyCopyright;
        this.lblCompanyName.Text = AssemblyCompany;
        this.rtbDescription.Text = AssemblyDescription;
    }

    #region Assembly Attribute Accessors

    public static string AssemblyTitle
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != string.Empty)
                {
                    return titleAttribute.Title;
                }
            }
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        }
    }

    public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

    public static string AssemblyDescription
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }

    public static string AssemblyProduct
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyProductAttribute)attributes[0]).Product;
        }
    }

    public static string AssemblyCopyright
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    public static string AssemblyCompany
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }

    #endregion Assembly Attribute Accessors

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Acceptable for WinForms event handlers")]
    private void rtbDescription_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        LaunchBrowser(e.LinkText);
    }

    /// <summary>
    /// https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
    /// </summary>
    /// <param name="url"></param>
    public static void LaunchBrowser(string url)
    {
        try
        {
            using var process = Process.Start(url);
        }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                using var process = Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                using var process = Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                using var process = Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }
    }
}