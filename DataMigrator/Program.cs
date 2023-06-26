using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataMigrator.Views;
using Microsoft.Extensions.Hosting;

namespace DataMigrator;

internal static class Program
{
    //public static IConfiguration Configuration { get; private set; }

    public static IContainer Container { get; private set; }

    //public static IServiceProvider ServiceProvider { get; private set; }

    public static IEnumerable<IMigrationPlugin> Plugins { get; set; }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        //Configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //    .Build();

        using var host = CreateHostBuilder().Build();
        //ServiceProvider = host.Services;

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        using var form = Container.Resolve<MainForm>();
        Application.Run(form);
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                //services.AddLogging(configure => configure.AddConsole());

                var builder = new ContainerBuilder();
                builder.Populate(services);

                // Forms
                builder.RegisterType<MainForm>();
                builder.RegisterType<AboutForm>();
                builder.RegisterType<RunJobsForm>();
                builder.RegisterType<SettingsForm>();
                builder.RegisterType<ToolsForm>();

                builder.RegisterType<ConnectionsControl>();
                builder.RegisterType<DataMigratorSettingsControl>();
                builder.RegisterType<TableMappingControl>();
                //builder.RegisterType<TraceViewerControl>();

                Container = builder.Build();
            });
    }
}