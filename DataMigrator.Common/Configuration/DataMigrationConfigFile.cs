using Extenso.IO;

namespace DataMigrator.Common.Configuration;

public class DataMigrationConfigFile
{
    [XmlAttribute]
    public int BatchSize { get; set; } = 10000;

    [XmlAttribute]
    public bool TrimStrings { get; set; } = true;

    public ConnectionDetails SourceConnection { get; set; }

    public ConnectionDetails DestinationConnection { get; set; }

    [XmlIgnore]
    public string FileName { get; private set; }

    [XmlArray]
    [XmlArrayItem("Job")]
    public JobCollection Jobs { get; set; } = new JobCollection();

    public static DataMigrationConfigFile Load(string fileName)
    {
        var configFile = new FileInfo(fileName).XmlDeserialize<DataMigrationConfigFile>();
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
        this.XmlSerialize(fileName);
        FileName = fileName;
    }
}