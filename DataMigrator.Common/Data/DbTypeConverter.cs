using System.Data;
using System.Linq;
using DataMigrator.Common.Models;
using Kore.Collections.Generic;

namespace DataMigrator.Common.Data
{
    public class DbTypeConverter : IFieldTypeConverter<DbType>
    {
        private static TupleList<FieldType, DbType> fieldTypes = new TupleList<FieldType, DbType>();
        private static TupleList<DbType, FieldType> dbTypes = new TupleList<DbType, FieldType>();

        static DbTypeConverter()
        {
            #region fieldTypes

            //fieldTypes.Add(FieldType.AutoNumber, DbType.Int64);
            fieldTypes.Add(FieldType.Binary, DbType.Binary);
            fieldTypes.Add(FieldType.Byte, DbType.Byte); //Cannot map exactly
            fieldTypes.Add(FieldType.Boolean, DbType.Boolean);
            fieldTypes.Add(FieldType.Char, DbType.StringFixedLength);
            fieldTypes.Add(FieldType.Choice, DbType.String);
            fieldTypes.Add(FieldType.Calculated, DbType.String);
            fieldTypes.Add(FieldType.Currency, DbType.Currency);
            fieldTypes.Add(FieldType.Date, DbType.Date);
            fieldTypes.Add(FieldType.DateTime, DbType.DateTime);
            fieldTypes.Add(FieldType.DateTimeOffset, DbType.DateTimeOffset);
            fieldTypes.Add(FieldType.Decimal, DbType.Decimal);
            fieldTypes.Add(FieldType.Double, DbType.Double);
            fieldTypes.Add(FieldType.Geometry, DbType.String);
            fieldTypes.Add(FieldType.Guid, DbType.Guid);
            fieldTypes.Add(FieldType.Int16, DbType.Int16);
            fieldTypes.Add(FieldType.Int32, DbType.Int32);
            fieldTypes.Add(FieldType.Int64, DbType.Int64);
            fieldTypes.Add(FieldType.Lookup, DbType.String);
            fieldTypes.Add(FieldType.MultiChoice, DbType.String);
            fieldTypes.Add(FieldType.MultiLookup, DbType.String);
            fieldTypes.Add(FieldType.MultiUser, DbType.String);
            fieldTypes.Add(FieldType.Object, DbType.Object);
            fieldTypes.Add(FieldType.RichText, DbType.String);
            fieldTypes.Add(FieldType.SByte, DbType.SByte);
            fieldTypes.Add(FieldType.Single, DbType.Single);
            fieldTypes.Add(FieldType.String, DbType.String);
            fieldTypes.Add(FieldType.Time, DbType.Time);
            fieldTypes.Add(FieldType.Timestamp, DbType.DateTime); //not sure if best mapping
            fieldTypes.Add(FieldType.UInt16, DbType.UInt16); //Cannot map exactly
            fieldTypes.Add(FieldType.UInt32, DbType.UInt32); //Cannot map exactly
            fieldTypes.Add(FieldType.UInt64, DbType.UInt64); //Cannot map exactly
            fieldTypes.Add(FieldType.Url, DbType.String);
            fieldTypes.Add(FieldType.User, DbType.String);
            fieldTypes.Add(FieldType.Xml, DbType.Xml);

            #endregion

            #region dbTypes

            dbTypes.Add(DbType.AnsiString, FieldType.String);
            dbTypes.Add(DbType.AnsiStringFixedLength, FieldType.Char);
            dbTypes.Add(DbType.Binary, FieldType.Binary);
            dbTypes.Add(DbType.Boolean, FieldType.Boolean);
            dbTypes.Add(DbType.Byte, FieldType.Byte);
            dbTypes.Add(DbType.Currency, FieldType.Currency);
            dbTypes.Add(DbType.Date, FieldType.DateTime);
            dbTypes.Add(DbType.DateTime, FieldType.DateTime);
            dbTypes.Add(DbType.DateTime2, FieldType.DateTime);
            dbTypes.Add(DbType.DateTimeOffset, FieldType.DateTimeOffset);
            dbTypes.Add(DbType.Decimal, FieldType.Decimal);
            dbTypes.Add(DbType.Double, FieldType.Double);
            dbTypes.Add(DbType.Guid, FieldType.Guid);
            dbTypes.Add(DbType.Int16, FieldType.Int16);
            dbTypes.Add(DbType.Int32, FieldType.Int32);
            dbTypes.Add(DbType.Int64, FieldType.Int64);
            dbTypes.Add(DbType.Object, FieldType.Object);
            dbTypes.Add(DbType.SByte, FieldType.SByte);
            dbTypes.Add(DbType.Single, FieldType.Single);
            dbTypes.Add(DbType.String, FieldType.String);
            dbTypes.Add(DbType.StringFixedLength, FieldType.Char);
            dbTypes.Add(DbType.Time, FieldType.Time);
            dbTypes.Add(DbType.UInt16, FieldType.UInt16);
            dbTypes.Add(DbType.UInt32, FieldType.UInt32);
            dbTypes.Add(DbType.UInt64, FieldType.UInt64);
            dbTypes.Add(DbType.VarNumeric, FieldType.Decimal); //not sure if best mapping
            dbTypes.Add(DbType.Xml, FieldType.Xml);

            #endregion
        }

        #region IFieldTypeConverter<DbType> Members

        public FieldType GetDataMigratorFieldType(DbType providerFieldType)
        {
            return dbTypes.First(x => x.Item1 == providerFieldType).Item2;
        }

        public DbType GetDataProviderFieldType(FieldType fieldType)
        {
            return fieldTypes.First(x => x.Item1 == fieldType).Item2;
        }

        #endregion
    }
}