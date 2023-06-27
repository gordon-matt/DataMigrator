namespace DataMigrator.Common.Models;

public class Record : ICloneable
{
    public FieldCollection Fields { get; set; } = new FieldCollection();

    public Field this[string name] => Fields.SingleOrDefault(f => f.Name == name);

    #region ICloneable Members

    public Record Clone() => new() { Fields = new FieldCollection(Fields) };

    object ICloneable.Clone() => Clone();

    #endregion ICloneable Members

    public void RunScripts(List<(string SourceFieldName, ITransform Script)> scripts)
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
                    if (field.Value == null || field.Value == DBNull.Value || (field.Value is string && string.IsNullOrEmpty(field.Value as string)))
                    { continue; }

                    var newType = TypeConvert.SystemTypeConverter.GetDataProviderFieldType(mapping.DestinationField.Type);
                    field.Value = field.Value.ConvertTo(newType);
                    field.Type = mapping.DestinationField.Type;
                }
            }
            catch
            {
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