namespace DataMigrator.Common.Models;

public class FieldCollection : List<Field>
{
    public Field this[string name] => this.SingleOrDefault(x => x.Name == name);

    public FieldCollection()
    {
    }

    public FieldCollection(IEnumerable<Field> fields)
    {
        foreach (var field in fields)
        {
            this.Add(field.Clone());
        }
    }
}