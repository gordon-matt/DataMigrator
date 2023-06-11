using System.Data;
using System.Linq;
using DataMigrator.Common.Models;
using Extenso.Collections.Generic;

namespace DataMigrator.Common.Data
{
    public class SqlDbTypeConverter : IFieldTypeConverter<SqlDbType>
    {
        private static TupleList<FieldType, SqlDbType> fieldTypes = new TupleList<FieldType, SqlDbType>();
        private static TupleList<SqlDbType, FieldType> sqlDbTypes = new TupleList<SqlDbType, FieldType>();

        static SqlDbTypeConverter()
        {
            #region fieldTypes

            //fieldTypes.Add(FieldType.AutoNumber, SqlDbType.Int);
            fieldTypes.Add(FieldType.Binary, SqlDbType.Binary);
            fieldTypes.Add(FieldType.Byte, SqlDbType.TinyInt); //Cannot map exactly
            fieldTypes.Add(FieldType.Boolean, SqlDbType.Bit);
            fieldTypes.Add(FieldType.Char, SqlDbType.Char);
            fieldTypes.Add(FieldType.Choice, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.Calculated, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.Currency, SqlDbType.Money);
            fieldTypes.Add(FieldType.Date, SqlDbType.Date);
            fieldTypes.Add(FieldType.DateTime, SqlDbType.DateTime);
            fieldTypes.Add(FieldType.DateTimeOffset, SqlDbType.DateTimeOffset);
            fieldTypes.Add(FieldType.Decimal, SqlDbType.Decimal);
            fieldTypes.Add(FieldType.Double, SqlDbType.Float);
            fieldTypes.Add(FieldType.Geometry, SqlDbType.VarChar);
            fieldTypes.Add(FieldType.Guid, SqlDbType.UniqueIdentifier);
            fieldTypes.Add(FieldType.Int16, SqlDbType.SmallInt);
            fieldTypes.Add(FieldType.Int32, SqlDbType.Int);
            fieldTypes.Add(FieldType.Int64, SqlDbType.BigInt);
            fieldTypes.Add(FieldType.Lookup, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.MultiChoice, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.MultiLookup, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.MultiUser, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.Object, SqlDbType.Variant); //Binary?
            fieldTypes.Add(FieldType.RichText, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.SByte, SqlDbType.TinyInt);
            fieldTypes.Add(FieldType.Single, SqlDbType.Float);
            fieldTypes.Add(FieldType.String, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.Time, SqlDbType.Time);
            fieldTypes.Add(FieldType.Timestamp, SqlDbType.Timestamp);
            fieldTypes.Add(FieldType.UInt16, SqlDbType.SmallInt); //Cannot map exactly
            fieldTypes.Add(FieldType.UInt32, SqlDbType.Int); //Cannot map exactly
            fieldTypes.Add(FieldType.UInt64, SqlDbType.BigInt); //Cannot map exactly
            fieldTypes.Add(FieldType.Url, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.User, SqlDbType.NVarChar);
            fieldTypes.Add(FieldType.Xml, SqlDbType.Xml);

            #endregion fieldTypes

            #region sqlDbTypes

            sqlDbTypes.Add(SqlDbType.BigInt, FieldType.Int64);
            sqlDbTypes.Add(SqlDbType.Binary, FieldType.Binary);
            sqlDbTypes.Add(SqlDbType.Bit, FieldType.Boolean);
            sqlDbTypes.Add(SqlDbType.Char, FieldType.Char);
            sqlDbTypes.Add(SqlDbType.Date, FieldType.Date);
            sqlDbTypes.Add(SqlDbType.DateTime, FieldType.DateTime);
            sqlDbTypes.Add(SqlDbType.DateTime2, FieldType.DateTime);
            sqlDbTypes.Add(SqlDbType.DateTimeOffset, FieldType.DateTimeOffset);
            sqlDbTypes.Add(SqlDbType.Decimal, FieldType.Decimal);
            sqlDbTypes.Add(SqlDbType.Float, FieldType.Double);
            sqlDbTypes.Add(SqlDbType.Int, FieldType.Int32);
            sqlDbTypes.Add(SqlDbType.Money, FieldType.Currency);
            sqlDbTypes.Add(SqlDbType.NChar, FieldType.Char);
            sqlDbTypes.Add(SqlDbType.NText, FieldType.String);
            sqlDbTypes.Add(SqlDbType.NVarChar, FieldType.String);
            sqlDbTypes.Add(SqlDbType.Real, FieldType.Decimal);
            sqlDbTypes.Add(SqlDbType.SmallDateTime, FieldType.DateTime);
            sqlDbTypes.Add(SqlDbType.SmallInt, FieldType.Int16);
            sqlDbTypes.Add(SqlDbType.SmallMoney, FieldType.Currency);
            sqlDbTypes.Add(SqlDbType.Structured, FieldType.Binary);
            sqlDbTypes.Add(SqlDbType.Text, FieldType.String);
            sqlDbTypes.Add(SqlDbType.Time, FieldType.Time);
            sqlDbTypes.Add(SqlDbType.Timestamp, FieldType.Timestamp);
            sqlDbTypes.Add(SqlDbType.TinyInt, FieldType.Int16);
            sqlDbTypes.Add(SqlDbType.Udt, FieldType.Binary);
            sqlDbTypes.Add(SqlDbType.UniqueIdentifier, FieldType.Guid);
            sqlDbTypes.Add(SqlDbType.VarBinary, FieldType.Binary);
            sqlDbTypes.Add(SqlDbType.VarChar, FieldType.String);
            sqlDbTypes.Add(SqlDbType.Variant, FieldType.Object);
            sqlDbTypes.Add(SqlDbType.Xml, FieldType.Xml);

            #endregion sqlDbTypes
        }

        #region IFieldTypeConverter<SqlDbType> Members

        public FieldType GetDataMigratorFieldType(SqlDbType providerFieldType)
        {
            return sqlDbTypes.First(x => x.Item1 == providerFieldType).Item2;
        }

        public SqlDbType GetDataProviderFieldType(FieldType fieldType)
        {
            return fieldTypes.First(x => x.Item1 == fieldType).Item2;
        }

        #endregion IFieldTypeConverter<SqlDbType> Members
    }
}