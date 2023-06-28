namespace DataMigrator.Common.Configuration;

public class Job
{
    public string Name { get; set; }

    public string SourceTable { get; set; }

    public string DestinationTable { get; set; }

    public int Order { get; set; }

    public List<FieldMapping> FieldMappings { get; set; } = new List<FieldMapping>();
}