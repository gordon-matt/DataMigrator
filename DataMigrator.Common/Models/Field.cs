using System.ComponentModel;

namespace DataMigrator.Common.Models;

public class Field : ICloneable
{
    public string Name { get; set; }

    [DisplayName("Display Name")]
    public string DisplayName { get; set; }

    [DisplayName("Is Primary Key")]
    public bool IsPrimaryKey { get; set; }

    [DisplayName("Is Required")]
    public bool IsRequired { get; set; }

    [DisplayName("Max Length")]
    public int MaxLength { get; set; }

    public int Ordinal { get; set; }

    public FieldType Type { get; set; } = FieldType.String;

    [Browsable(false)]
    public object Value { get; set; }

    //[Newtonsoft.Json.JsonIgnore]
    //[System.Text.Json.Serialization.JsonIgnore]
    //[Browsable(false)]
    //public bool IsNumeric =>
    //    Type.In(FieldType.Byte, FieldType.Currency, FieldType.Decimal, FieldType.Double,
    //        FieldType.Int16, FieldType.Int32, FieldType.Int64, FieldType.SByte, FieldType.Single,
    //        FieldType.UInt16, FieldType.UInt32, FieldType.UInt64);

    #region Ctor

    public Field()
    {
    }

    public Field(string name, object value)
    {
        Name = name;
        Value = value;
    }

    #endregion Ctor

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
            Value = this.Value,
            ValueShouldBeSerialized = true // For logging purposes
        };
    }

    object ICloneable.Clone() => Clone();

    #endregion ICloneable Members

    #region JSON Serialization

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [Browsable(false)]
    public bool ValueShouldBeSerialized { get; set; }

    public bool ShouldSerializeValue() => ValueShouldBeSerialized;

    #endregion
}