using System.Collections.Generic;
using System.Xml.Serialization;
using DataMigrator.Common.Models;

namespace DataMigrator.Common.Configuration
{
    public class Job
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string SourceTable { get; set; }

        [XmlAttribute]
        public string DestinationTable { get; set; }

        public List<FieldMapping> FieldMappings { get; set; }

        public Job()
        {
            FieldMappings = new List<FieldMapping>();
        }
    }
}