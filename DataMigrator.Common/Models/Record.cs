using Extenso;

namespace DataMigrator.Common.Models;

public class Record : ICloneable
{
    public FieldCollection Fields { get; set; } = new FieldCollection();

    public Field this[string name] => Fields.SingleOrDefault(f => f.Name == name);

    #region ICloneable Members

    public Record Clone() => new() { Fields = new FieldCollection(Fields) };

    object ICloneable.Clone() => Clone();

    #endregion ICloneable Members

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

    public void ReMapFieldTypes(IEnumerable<FieldMapping> mappings)
    {
        foreach (var mapping in mappings)
        {
            var field = this[mapping.SourceField.Name];
            if (field != null)
            {
                if (field.Value == null || field.Value == DBNull.Value)
                { continue; }

                var newType = AppContext.SystemTypeConverter.GetDataProviderFieldType(mapping.DestinationField.Type);
                field.Value = field.Value.ConvertTo(newType);
                field.Type = mapping.DestinationField.Type;
            }
        }
    }

    public override string ToString() => $"Fields: {Fields.Count}";
}