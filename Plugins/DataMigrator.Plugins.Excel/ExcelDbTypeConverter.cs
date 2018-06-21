using System.Linq;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Kore.Collections.Generic;

namespace DataMigrator.Excel
{
    public enum ExcelType
    {
        Unknown = 0,
        Short,
        Long,
        Single,
        Double,
        Currency,
        DateTime,
        Bit,
        Byte,
        GUID,
        BigBinary,
        LongBinary,
        VarBinary,
        LongText,
        VarChar,
        Decimal,
    }
    //public enum ExcelType
    //{
    //    Unknown = 0,
    //    Boolean,
    //    Number,
    //    Date,
    //    DateTime,
    //    Time,
    //    Text
    //}

    public class ExcelDbTypeConverter : IFieldTypeConverter<ExcelType>
    {
        private static TupleList<FieldType, ExcelType> fieldTypes = new TupleList<FieldType, ExcelType>();
        private static TupleList<ExcelType, FieldType> excelTypes = new TupleList<ExcelType, FieldType>();

        static ExcelDbTypeConverter()
        {
            #region fieldTypes

            //fieldTypes.Add(FieldType.AutoNumber, ExcelType.Long);
            fieldTypes.Add(FieldType.Binary, ExcelType.VarBinary);
            fieldTypes.Add(FieldType.Byte, ExcelType.Byte);
            fieldTypes.Add(FieldType.Boolean, ExcelType.Bit);
            fieldTypes.Add(FieldType.Char, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Choice, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Calculated, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Currency, ExcelType.Currency);
            fieldTypes.Add(FieldType.Date, ExcelType.DateTime);
            fieldTypes.Add(FieldType.DateTime, ExcelType.DateTime);
            fieldTypes.Add(FieldType.DateTimeOffset, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Decimal, ExcelType.Decimal);
            fieldTypes.Add(FieldType.Double, ExcelType.Double);
            fieldTypes.Add(FieldType.Geometry, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Guid, ExcelType.GUID);
            fieldTypes.Add(FieldType.Int16, ExcelType.Short);
            fieldTypes.Add(FieldType.Int32, ExcelType.Long);
            fieldTypes.Add(FieldType.Int64, ExcelType.Long);
            fieldTypes.Add(FieldType.Lookup, ExcelType.VarChar);
            fieldTypes.Add(FieldType.MultiChoice, ExcelType.VarChar);
            fieldTypes.Add(FieldType.MultiLookup, ExcelType.VarChar);
            fieldTypes.Add(FieldType.MultiUser, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Object, ExcelType.VarBinary);
            fieldTypes.Add(FieldType.RichText, ExcelType.LongText);
            fieldTypes.Add(FieldType.SByte, ExcelType.Byte);
            fieldTypes.Add(FieldType.Single, ExcelType.Single);
            fieldTypes.Add(FieldType.String, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Time, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Timestamp, ExcelType.VarChar);
            fieldTypes.Add(FieldType.UInt16, ExcelType.Short);
            fieldTypes.Add(FieldType.UInt32, ExcelType.Long);
            fieldTypes.Add(FieldType.UInt64, ExcelType.Long);
            fieldTypes.Add(FieldType.Url, ExcelType.VarChar);
            fieldTypes.Add(FieldType.User, ExcelType.VarChar);
            fieldTypes.Add(FieldType.Xml, ExcelType.VarChar);

            #endregion

            #region excelTypes

            excelTypes.Add(ExcelType.BigBinary, FieldType.Binary);
            excelTypes.Add(ExcelType.Bit, FieldType.Boolean);
            excelTypes.Add(ExcelType.Byte, FieldType.Byte);
            excelTypes.Add(ExcelType.Currency, FieldType.Currency);
            excelTypes.Add(ExcelType.DateTime, FieldType.DateTime);
            excelTypes.Add(ExcelType.Decimal, FieldType.Decimal);
            excelTypes.Add(ExcelType.Double, FieldType.Double);
            excelTypes.Add(ExcelType.GUID, FieldType.Guid);
            excelTypes.Add(ExcelType.Long, FieldType.Int64);
            excelTypes.Add(ExcelType.LongBinary, FieldType.Binary);
            excelTypes.Add(ExcelType.LongText, FieldType.RichText);
            excelTypes.Add(ExcelType.Short, FieldType.Int16);
            excelTypes.Add(ExcelType.Single, FieldType.Single);
            excelTypes.Add(ExcelType.VarBinary, FieldType.Binary);
            excelTypes.Add(ExcelType.VarChar, FieldType.String);

            #endregion
        }

        #region IFieldTypeConverter<ExcelType> Members

        public FieldType GetDataMigratorFieldType(ExcelType providerFieldType)
        {
            return excelTypes.First(x => x.Item1 == providerFieldType).Item2;
        }

        public ExcelType GetDataProviderFieldType(FieldType fieldType)
        {
            return fieldTypes.First(x => x.Item1 == fieldType).Item2;
        }

        #endregion
    }
}