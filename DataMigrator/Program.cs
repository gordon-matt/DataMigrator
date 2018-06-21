using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataMigrator.Common;
using DataMigrator.Common.Configuration;
using DataMigrator.Views;

namespace DataMigrator
{
    static class Program
    {
        public static IEnumerable<IMigrationPlugin> Plugins { get; set; }

        public static DataMigrationConfigFile Configuration { get; set; }

        public static Job CurrentJob { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Configuration = new DataMigrationConfigFile();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
