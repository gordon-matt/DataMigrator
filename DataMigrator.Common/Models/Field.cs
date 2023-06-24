using System.ComponentModel;

namespace DataMigrator.Common.Models;

public class Field : ICloneable
{
    [XmlAttribute]
    public string Name { get; set; }

    [XmlAttribute]
    [DisplayName("Display Name")]
    public string DisplayName { get; set; }

    [XmlAttribute]
    [DisplayName("Is Primary Key")]
    public bool IsPrimaryKey { get; set; }

    [XmlAttribute]
    [DisplayName("Is Required")]
    public bool IsRequired { get; set; }

    [XmlAttribute]
    [DisplayName("Max Length")]
    public int MaxLength { get; set; }

    [XmlAttribute]
    public int Ordinal { get; set; }

    [XmlAttribute]
    public FieldType Type { get; set; } = FieldType.String;

    [XmlIgnore]
    [Browsable(false)]
    public object Value { get; set; }

    [XmlIgnore]
    [Browsable(false)]
    public bool IsNumeric =>
        Type.In(FieldType.Byte, FieldType.Currency, FieldType.Decimal, FieldType.Double,
            FieldType.Int16, FieldType.Int32, FieldType.Int64, FieldType.SByte, FieldType.Single,
            FieldType.UInt16, FieldType.UInt32, FieldType.UInt64);

    public Field()
    {
    }

    public Field(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public T GetValue<T>()
    {
        return (T)Value;
    }

    public override string ToString()
    {
        return Value != null ? string.Concat(Name, ": ", Value) : Name;
    }

    #region ICloneable Members

    public Field Clone()
    {
        return new Field
        {
            DisplayName = this.DisplayName,
            IsPrimaryKey = this.IsPrimaryKey,
            IsRequired = this.IsRequired,
            MaxLength = this.MaxLength,
            Name = this.Name,
            Ordinal = this.Ordinal,
            Type = this.Type,
            Value = this.Value
        };
    }

    object ICloneable.Clone()
    {
        return Clone();
    }

    #endregion ICloneable Members
}