using System.Linq;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso.Collections.Generic;

namespace DataMigrator.Access
{
    public enum AccessDbType
    {
        Text,
        Memo,
        Number,
        DateTime,
        Currency,
        AutoNumber,
        YesNo,
        OleObject,
        Hyperlink,
        Attachment,
        Calculated
    }

    public class AccessDbTypeConverter : IFieldTypeConverter<AccessDbType>
    {
        private static TupleList<FieldType, AccessDbType> fieldTypes = new TupleList<FieldType, AccessDbType>();
        private static TupleList<AccessDbType, FieldType> accessDbTypes = new TupleList<AccessDbType, FieldType>();

        static AccessDbTypeConverter()
        {
            #region fieldTypes

            //fieldTypes.Add(FieldType.AutoNumber, AccessDbType.AutoNumber);
            fieldTypes.Add(FieldType.Binary, AccessDbType.OleObject);
            fieldTypes.Add(FieldType.Byte, AccessDbType.Number);
            fieldTypes.Add(FieldType.Boolean, AccessDbType.YesNo);
            fieldTypes.Add(FieldType.Char, AccessDbType.Text);
            fieldTypes.Add(FieldType.Choice, AccessDbType.Text);
            fieldTypes.Add(FieldType.Calculated, AccessDbType.Calculated);
            fieldTypes.Add(FieldType.Currency, AccessDbType.Currency);
            fieldTypes.Add(FieldType.Date, AccessDbType.DateTime);
            fieldTypes.Add(FieldType.DateTime, AccessDbType.DateTime);
            fieldTypes.Add(FieldType.DateTimeOffset, AccessDbType.Number);
            fieldTypes.Add(FieldType.Decimal, AccessDbType.Number);
            fieldTypes.Add(FieldType.Double, AccessDbType.Number);
            fieldTypes.Add(FieldType.Geometry, AccessDbType.Text);
            fieldTypes.Add(FieldType.Guid, AccessDbType.Text);
            fieldTypes.Add(FieldType.Int16, AccessDbType.Number);
            fieldTypes.Add(FieldType.Int32, AccessDbType.Number);
            fieldTypes.Add(FieldType.Int64, AccessDbType.Number);
            fieldTypes.Add(FieldType.Lookup, AccessDbType.Text);
            fieldTypes.Add(FieldType.MultiChoice, AccessDbType.Text);
            fieldTypes.Add(FieldType.MultiLookup, AccessDbType.Text);
            fieldTypes.Add(FieldType.MultiUser, AccessDbType.Text);
            fieldTypes.Add(FieldType.Object, AccessDbType.OleObject);
            fieldTypes.Add(FieldType.RichText, AccessDbType.Memo);
            fieldTypes.Add(FieldType.SByte, AccessDbType.Number);
            fieldTypes.Add(FieldType.Single, AccessDbType.Number);
            fieldTypes.Add(FieldType.String, AccessDbType.Text);
            fieldTypes.Add(FieldType.Time, AccessDbType.Text);
            fieldTypes.Add(FieldType.Timestamp, AccessDbType.Text);
            fieldTypes.Add(FieldType.UInt16, AccessDbType.Number);
            fieldTypes.Add(FieldType.UInt32, AccessDbType.Number);
            fieldTypes.Add(FieldType.UInt64, AccessDbType.Number);
            fieldTypes.Add(FieldType.Url, AccessDbType.Hyperlink);
            fieldTypes.Add(FieldType.User, AccessDbType.Text);
            fieldTypes.Add(FieldType.Xml, AccessDbType.Memo);

            #endregion fieldTypes

            #region accessDbTypes

            accessDbTypes.Add(AccessDbType.Attachment, FieldType.Object);
            //accessDbTypes.Add(AccessDbType.AutoNumber, FieldType.AutoNumber);
            accessDbTypes.Add(AccessDbType.Calculated, FieldType.Calculated);
            accessDbTypes.Add(AccessDbType.Currency, FieldType.Currency);
            accessDbTypes.Add(AccessDbType.DateTime, FieldType.DateTime);
            accessDbTypes.Add(AccessDbType.Hyperlink, FieldType.Url);
            accessDbTypes.Add(AccessDbType.Memo, FieldType.RichText);
            accessDbTypes.Add(AccessDbType.Number, FieldType.Double);
            accessDbTypes.Add(AccessDbType.OleObject, FieldType.Object);
            accessDbTypes.Add(AccessDbType.Text, FieldType.String);
            accessDbTypes.Add(AccessDbType.YesNo, FieldType.Boolean);

            #endregion accessDbTypes
        }

        #region IFieldTypeConverter<AccessDbType> Members

        public FieldType GetDataMigratorFieldType(AccessDbType providerFieldType)
        {
            return accessDbTypes.First(x => x.Item1 == providerFieldType).Item2;
        }

        public AccessDbType GetDataProviderFieldType(FieldType fieldType)
        {
            return fieldTypes.First(x => x.Item1 == fieldType).Item2;
        }

        #endregion IFieldTypeConverter<AccessDbType> Members
    }
}