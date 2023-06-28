using CSScriptLib;

namespace DataMigrator.Common.Models;

public class RecordCollection : List<Record>
{
    public void ReMapFields(IEnumerable<FieldMapping> mappings)
    {
        // Get Fields To Remap (Quicker than passing all items in job.FieldMappings)
        var fieldToReMap = mappings.Where(x => !x.DestinationField.Name.Equals(x.SourceField.Name));

        var cachedScripts = mappings.Where(x => !string.IsNullOrEmpty(x.TransformScript))
            .Select(x => (x.SourceField.Name, CSScript.Evaluator.LoadMethod<ITransform>(x.TransformScript)))
            .ToList();

        foreach (var record in this)
        {
            // First process any scripts..
            record.RunScripts(cachedScripts);

            // Then change data types..
            record.ReMapFieldTypes(mappings.Where(x => x.DestinationField.Type != x.SourceField.Type));

            // Then rename the fields
            record.ReMapFields(fieldToReMap);
        }
    }
}