using DataMigrator.Common.Configuration;

namespace DataMigrator.Common;

public static class AppState
{
    public static DataMigrationConfigFile ConfigFile { get; set; } = new DataMigrationConfigFile();

    public static Job CurrentJob { get; set; }
}