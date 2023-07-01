using DataMigrator.Common.Data;
using DataMigrator.Common.Models;

namespace DataMigrator.Access;

public class AccessDbTypeConverter : IFieldTypeConverter<AccessDbType>
{
    private static readonly List<(FieldType FieldType, AccessDbType AccessDbType)> fieldTypes = new();
    private static readonly List<(AccessDbType AccessDbType, FieldType FieldType)> accessDbTypes = new();

    static AccessDbTypeConverter()
    {
        #region fieldTypes

        //fieldTypes.Add((FieldType.AutoNumber, AccessDbType.AutoNumber));
        fieldTypes.Add((FieldType.Binary, AccessDbType.OleObject));
        fieldTypes.Add((FieldType.Byte, AccessDbType.Number));
        fieldTypes.Add((FieldType.Boolean, AccessDbType.YesNo));
        fieldTypes.Add((FieldType.Char, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Choice, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Calculated, AccessDbType.Calculated));
        fieldTypes.Add((FieldType.Currency, AccessDbType.Currency));
        fieldTypes.Add((FieldType.Date, AccessDbType.DateTime));
        fieldTypes.Add((FieldType.DateTime, AccessDbType.DateTime));
        fieldTypes.Add((FieldType.DateTimeOffset, AccessDbType.Number));
        fieldTypes.Add((FieldType.Decimal, AccessDbType.LargeNumber));
        fieldTypes.Add((FieldType.Double, AccessDbType.LargeNumber));
        fieldTypes.Add((FieldType.Geometry, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Guid, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Int16, AccessDbType.Number));
        fieldTypes.Add((FieldType.Int32, AccessDbType.Number));
        fieldTypes.Add((FieldType.Int64, AccessDbType.LargeNumber));
        fieldTypes.Add((FieldType.Json, AccessDbType.LongText));
        fieldTypes.Add((FieldType.Lookup, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.MultiChoice, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.MultiLookup, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.MultiUser, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Object, AccessDbType.OleObject));
        fieldTypes.Add((FieldType.RichText, AccessDbType.LongText));
        fieldTypes.Add((FieldType.SByte, AccessDbType.Number));
        fieldTypes.Add((FieldType.Single, AccessDbType.Number));
        fieldTypes.Add((FieldType.String, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Time, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Timestamp, AccessDbType.DateTime));
        fieldTypes.Add((FieldType.UInt16, AccessDbType.Number));
        fieldTypes.Add((FieldType.UInt32, AccessDbType.Number));
        fieldTypes.Add((FieldType.UInt64, AccessDbType.LargeNumber));
        fieldTypes.Add((FieldType.Url, AccessDbType.Hyperlink));
        fieldTypes.Add((FieldType.User, AccessDbType.ShortText));
        fieldTypes.Add((FieldType.Xml, AccessDbType.LongText));

        #endregion fieldTypes

        #region accessDbTypes

        accessDbTypes.Add((AccessDbType.Attachment, FieldType.Binary));
        accessDbTypes.Add((AccessDbType.AutoNumber, FieldType.Int32));
        accessDbTypes.Add((AccessDbType.Calculated, FieldType.Calculated));
        accessDbTypes.Add((AccessDbType.Currency, FieldType.Currency));
        accessDbTypes.Add((AccessDbType.DateTime, FieldType.DateTime));
        accessDbTypes.Add((AccessDbType.DateTimeExtended, FieldType.DateTime));
        accessDbTypes.Add((AccessDbType.Hyperlink, FieldType.Url));
        accessDbTypes.Add((AccessDbType.LargeNumber, FieldType.Int64));
        accessDbTypes.Add((AccessDbType.LongText, FieldType.RichText));
        accessDbTypes.Add((AccessDbType.Number, FieldType.Double));
        accessDbTypes.Add((AccessDbType.OleObject, FieldType.Binary));
        accessDbTypes.Add((AccessDbType.ShortText, FieldType.String));
        accessDbTypes.Add((AccessDbType.YesNo, FieldType.Boolean));

        #endregion accessDbTypes
    }

    #region IFieldTypeConverter<AccessDbType> Members

    public FieldType GetDataMigratorFieldType(AccessDbType providerFieldType) => accessDbTypes.First(x => x.Item1 == providerFieldType).Item2;

    public AccessDbType GetDataProviderFieldType(FieldType fieldType) => fieldTypes.First(x => x.Item1 == fieldType).Item2;

    #endregion IFieldTypeConverter<AccessDbType> Members
}