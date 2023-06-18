using DataMigrator.Common;
using DataMigrator.Common.Configuration;
using DataMigrator.Views;

namespace DataMigrator;

internal static class Program
{
    public static IEnumerable<IMigrationPlugin> Plugins { get; set; }

    public static DataMigrationConfigFile Configuration { get; set; } = new DataMigrationConfigFile();

    public static Job CurrentJob { get; set; }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        using var form = new MainForm();
        Application.Run(form);
    }
}