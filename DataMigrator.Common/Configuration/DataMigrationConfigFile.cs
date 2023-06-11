using System.Xml.Serialization;
using DataMigrator.Common.Models;
using Extenso;
using Extenso.IO;

namespace DataMigrator.Common.Configuration;

public class DataMigrationConfigFile
{
    [XmlAttribute]
    public int BatchSize { get; set; }

    public ConnectionDetails SourceConnection { get; set; }

    public ConnectionDetails DestinationConnection { get; set; }

    [XmlIgnore]
    public string FileName { get; private set; }

    [XmlArray]
    [XmlArrayItem("Job")]
    public JobCollection Jobs { get; set; }

    public DataMigrationConfigFile()
    {
        Jobs = new JobCollection();
        BatchSize = 10000;
    }

    public static DataMigrationConfigFile Load(string fileName)
    {
        DataMigrationConfigFile configFile = new FileInfo(fileName).XmlDeserialize<DataMigrationConfigFile>();
        configFile.FileName = fileName;
        return configFile;
    }

    public void Save()
    {
        if (!string.IsNullOrEmpty(FileName))
        {
            this.SaveAs(FileName);
        }
        else
        {
            using SaveFileDialog dlgSaveFile = new SaveFileDialog();
            dlgSaveFile.Filter = "Data Migrator Files|*.dmf";
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                this.SaveAs(dlgSaveFile.FileName);
            }
        }
    }

    public void SaveAs(string fileName)
    {
        this.XmlSerialize(fileName);
        this.FileName = fileName;
    }
}