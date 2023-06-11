using System.Linq;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso.Collections.Generic;
using SPFieldType = Microsoft.SharePoint.Client.FieldType;

namespace DataMigrator.SharePoint
{
    public class SPFieldTypeConverter : IFieldTypeConverter<SPFieldType>
    {
        private static TupleList<FieldType, SPFieldType> fieldTypes = new TupleList<FieldType, SPFieldType>();
        private static TupleList<SPFieldType, FieldType> providerFieldTypes = new TupleList<SPFieldType, FieldType>();

        static SPFieldTypeConverter()
        {
            #region fieldTypes

            //fieldTypes.Add(FieldType.AutoNumber, SPFieldType.Counter);
            fieldTypes.Add(FieldType.Binary, SPFieldType.File); //not sure if best mapping
            fieldTypes.Add(FieldType.Byte, SPFieldType.Integer);
            fieldTypes.Add(FieldType.Boolean, SPFieldType.Boolean);
            fieldTypes.Add(FieldType.Char, SPFieldType.Text);
            fieldTypes.Add(FieldType.Choice, SPFieldType.Choice);
            fieldTypes.Add(FieldType.Calculated, SPFieldType.Calculated);
            fieldTypes.Add(FieldType.Currency, SPFieldType.Currency);
            fieldTypes.Add(FieldType.Date, SPFieldType.DateTime);
            fieldTypes.Add(FieldType.DateTime, SPFieldType.DateTime);
            fieldTypes.Add(FieldType.DateTimeOffset, SPFieldType.Text);
            fieldTypes.Add(FieldType.Decimal, SPFieldType.Number);
            fieldTypes.Add(FieldType.Double, SPFieldType.Number);
            fieldTypes.Add(FieldType.Geometry, SPFieldType.Text);
            fieldTypes.Add(FieldType.Guid, SPFieldType.Guid);
            fieldTypes.Add(FieldType.Int16, SPFieldType.Integer);
            fieldTypes.Add(FieldType.Int32, SPFieldType.Integer);
            fieldTypes.Add(FieldType.Int64, SPFieldType.Integer);
            fieldTypes.Add(FieldType.Lookup, SPFieldType.Lookup);
            fieldTypes.Add(FieldType.MultiChoice, SPFieldType.MultiChoice);
            fieldTypes.Add(FieldType.MultiLookup, SPFieldType.Lookup);
            fieldTypes.Add(FieldType.MultiUser, SPFieldType.User);
            fieldTypes.Add(FieldType.Object, SPFieldType.File); //not sure if best mapping
            fieldTypes.Add(FieldType.RichText, SPFieldType.Note);
            fieldTypes.Add(FieldType.SByte, SPFieldType.Integer);
            fieldTypes.Add(FieldType.Single, SPFieldType.Number);
            fieldTypes.Add(FieldType.String, SPFieldType.Text);
            fieldTypes.Add(FieldType.Time, SPFieldType.DateTime); //not sure if best mapping
            fieldTypes.Add(FieldType.Timestamp, SPFieldType.Text);
            fieldTypes.Add(FieldType.UInt16, SPFieldType.Integer);
            fieldTypes.Add(FieldType.UInt32, SPFieldType.Integer);
            fieldTypes.Add(FieldType.UInt64, SPFieldType.Integer);
            fieldTypes.Add(FieldType.Url, SPFieldType.URL);
            fieldTypes.Add(FieldType.User, SPFieldType.User);
            fieldTypes.Add(FieldType.Xml, SPFieldType.Text);

            #endregion fieldTypes

            #region providerFieldTypes

            providerFieldTypes.Add(SPFieldType.Boolean, FieldType.Boolean);
            providerFieldTypes.Add(SPFieldType.Calculated, FieldType.Calculated);
            providerFieldTypes.Add(SPFieldType.Choice, FieldType.Choice);
            //providerFieldTypes.Add(SPFieldType.Counter, FieldType.AutoNumber);
            providerFieldTypes.Add(SPFieldType.Computed, FieldType.Calculated);
            providerFieldTypes.Add(SPFieldType.Currency, FieldType.Currency);
            providerFieldTypes.Add(SPFieldType.DateTime, FieldType.DateTime);
            providerFieldTypes.Add(SPFieldType.File, FieldType.Binary);
            providerFieldTypes.Add(SPFieldType.Guid, FieldType.Guid);
            providerFieldTypes.Add(SPFieldType.Integer, FieldType.Int32);//Maybe Int64?
            providerFieldTypes.Add(SPFieldType.Lookup, FieldType.Lookup);
            providerFieldTypes.Add(SPFieldType.MultiChoice, FieldType.MultiChoice);
            providerFieldTypes.Add(SPFieldType.Note, FieldType.RichText);
            providerFieldTypes.Add(SPFieldType.Number, FieldType.Decimal);//Maybe Double?
            providerFieldTypes.Add(SPFieldType.Text, FieldType.String);
            providerFieldTypes.Add(SPFieldType.URL, FieldType.Url);
            providerFieldTypes.Add(SPFieldType.User, FieldType.User);

            #endregion providerFieldTypes
        }

        #region IFieldTypeConverter<DbType> Members

        public FieldType GetDataMigratorFieldType(SPFieldType providerFieldType)
        {
            return providerFieldTypes.First(x => x.Item1 == providerFieldType).Item2;
        }

        public SPFieldType GetDataProviderFieldType(FieldType fieldType)
        {
            return fieldTypes.First(x => x.Item1 == fieldType).Item2;
        }

        #endregion IFieldTypeConverter<DbType> Members
    }
}