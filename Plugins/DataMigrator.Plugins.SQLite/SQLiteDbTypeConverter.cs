using System.Linq;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Kore.Collections.Generic;

namespace DataMigrator.SQLite
{
    public enum SQLiteDbType
    {
        Unknown = 0,
        Text,
        Numeric,
        Integer,
        Real,
        None
    }

    public class SQLiteDbTypeConverter : IFieldTypeConverter<SQLiteDbType>
    {
        private static TupleList<FieldType, SQLiteDbType> fieldTypes = new TupleList<FieldType, SQLiteDbType>();
        private static TupleList<SQLiteDbType, FieldType> sQLiteDbTypes = new TupleList<SQLiteDbType, FieldType>();

        static SQLiteDbTypeConverter()
        {
            #region fieldTypes

            //fieldTypes.Add(FieldType.AutoNumber, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Binary, SQLiteDbType.None);
            fieldTypes.Add(FieldType.Byte, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Boolean, SQLiteDbType.Numeric);
            fieldTypes.Add(FieldType.Char, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Choice, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Calculated, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Currency, SQLiteDbType.Real);
            fieldTypes.Add(FieldType.Date, SQLiteDbType.Numeric);
            fieldTypes.Add(FieldType.DateTime, SQLiteDbType.Numeric);
            fieldTypes.Add(FieldType.DateTimeOffset, SQLiteDbType.Numeric);
            fieldTypes.Add(FieldType.Decimal, SQLiteDbType.Real);
            fieldTypes.Add(FieldType.Double, SQLiteDbType.Real);
            fieldTypes.Add(FieldType.Geometry, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Guid, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Int16, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Int32, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Int64, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Lookup, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.MultiChoice, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.MultiLookup, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.MultiUser, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Object, SQLiteDbType.None);
            fieldTypes.Add(FieldType.RichText, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.SByte, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Single, SQLiteDbType.Real);
            fieldTypes.Add(FieldType.String, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Time, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Timestamp, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.UInt16, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.UInt32, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.UInt64, SQLiteDbType.Integer);
            fieldTypes.Add(FieldType.Url, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.User, SQLiteDbType.Text);
            fieldTypes.Add(FieldType.Xml, SQLiteDbType.Text);

            #endregion

            #region sQLiteDbTypes

            sQLiteDbTypes.Add(SQLiteDbType.Integer, FieldType.Int32);
            sQLiteDbTypes.Add(SQLiteDbType.None, FieldType.Binary);
            sQLiteDbTypes.Add(SQLiteDbType.Numeric, FieldType.Decimal);
            sQLiteDbTypes.Add(SQLiteDbType.Real, FieldType.Double);
            sQLiteDbTypes.Add(SQLiteDbType.Text, FieldType.String);

            #endregion
        }

        #region IFieldTypeConverter<SqlDbType> Members

        public FieldType GetDataMigratorFieldType(SQLiteDbType providerFieldType)
        {
            return sQLiteDbTypes.First(x => x.Item1 == providerFieldType).Item2;
        }

        public SQLiteDbType GetDataProviderFieldType(FieldType fieldType)
        {
            return fieldTypes.First(x => x.Item1 == fieldType).Item2;
        }

        #endregion
    }
}
