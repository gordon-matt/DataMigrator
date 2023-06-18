using System.Data;

namespace DataMigrator.Common.Models;

public class RecordCollection : List<Record>
{
    public void ReMapFields(IEnumerable<FieldMapping> mappings)
    {
        // Get Fields To Remap (Quicker than passing all items in job.FieldMappings)
        var fieldToReMap = mappings.Where(x => !x.DestinationField.Name.Equals(
            x.SourceField.Name,
            StringComparison.InvariantCultureIgnoreCase));

        var fieldTypesToReMap = mappings.Where(x => x.DestinationField.Type != x.SourceField.Type);

        foreach (var record in this)
        {
            record.ReMapFieldTypes(fieldTypesToReMap);
            record.ReMapFields(fieldToReMap);
        }
    }
}