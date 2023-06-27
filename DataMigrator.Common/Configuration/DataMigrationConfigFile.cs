using Extenso.IO;
using Newtonsoft.Json;

namespace DataMigrator.Common.Configuration;

public class DataMigrationConfigFile
{
    public int BatchSize { get; set; } = 10000;

    public bool TrimStrings { get; set; } = true;

    public ConnectionDetails SourceConnection { get; set; }

    public ConnectionDetails DestinationConnection { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string FileName { get; private set; }

    public JobCollection Jobs { get; set; } = new JobCollection();

    public static DataMigrationConfigFile Load(string fileName)
    {
        var configFile = new FileInfo(fileName).ReadAllText().JsonDeserialize<DataMigrationConfigFile>();
        configFile.FileName = fileName;
        return configFile;
    }

    public void Save()
    {
        if (!string.IsNullOrEmpty(FileName))
        {
            SaveAs(FileName);
        }
        else
        {
            using var dlgSaveFile = new SaveFileDialog();
            dlgSaveFile.Filter = "Data Migrator Files|*.dmf";
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                SaveAs(dlgSaveFile.FileName);
            }
        }
    }

    public void SaveAs(string fileName)
    {
        this.JsonSerialize(new JsonSerializerSettings { Formatting = Formatting.Indented }).ToFile(fileName);
        FileName = fileName;
    }
}