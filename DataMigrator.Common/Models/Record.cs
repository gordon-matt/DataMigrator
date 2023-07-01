using DataMigrator.Common.Diagnostics;

namespace DataMigrator.Common.Models;

public class Record : ICloneable
{
    public FieldCollection Fields { get; set; } = new FieldCollection();

    public Field this[string name] => Fields.SingleOrDefault(f => f.Name == name);

    #region ICloneable Members

    public Record Clone() => new() { Fields = new FieldCollection(Fields) };

    object ICloneable.Clone() => Clone();

    #endregion ICloneable Members

    public void RunScripts(IEnumerable<(string SourceFieldName, ITransform Script)> scripts)
    {
        foreach (var entry in scripts)
        {
            var field = this[entry.SourceFieldName];
            if (field is not null && field.Value is not null)
            {
                field.Value = entry.Script.Transform(field.Value);
            }
        }
    }

    public void ReMapFieldTypes(IEnumerable<FieldMapping> mappings)
    {
        foreach (var mapping in mappings)
        {
            try
            {
                var field = this[mapping.SourceField.Name];
                if (field != null)
                {
                    // This should always happen, even if no value to convert..
                    field.Type = mapping.DestinationField.Type;

                    if (field.Value == null || field.Value == DBNull.Value || (field.Value is string && string.IsNullOrEmpty(field.Value as string)))
                    { continue; }

                    var newType = TypeConvert.SystemTypeConverter.GetDataProviderFieldType(mapping.DestinationField.Type);
                    if (newType == typeof(object))
                    {
                        // Not much we can do here.. just return as-is
                        continue;
                    }

                    if (mapping.SourceField.Type == FieldType.Guid)
                    {
                        // Guid does not implement IConvertible.. so let's convert it to a string first..
                        //  and then let the rest of the process carry itself out
                        field.Value = field.Value.ToString();
                    }

                    field.Value = field.Value.ConvertTo(newType);
                }
            }
            catch
            {
                TraceService.Instance.WriteMessage(
$@"Error during type conversion:
    Source field: '{mapping.SourceField.Name}' of type, '{mapping.SourceField.Type}'
    Destination field: '{mapping.DestinationField.Name}' of type, '{mapping.DestinationField.Type}'
    Value: {this[mapping.SourceField.Name]?.Value}");

                throw;
            }
        }
    }

    public void ReMapFields(IEnumerable<FieldMapping> mappings)
    {
        foreach (var mapping in mappings)
        {
            var field = this[mapping.SourceField.Name];
            if (field != null)
            {
                field.Name = mapping.DestinationField.Name;
            }
        }
    }

    public override string ToString() => $"Fields: {Fields.Count}";
}