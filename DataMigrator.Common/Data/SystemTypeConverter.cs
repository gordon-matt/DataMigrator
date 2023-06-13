using System.Data;
using System.Data.SqlTypes;
using DataMigrator.Common.Models;
using Extenso.Collections.Generic;

namespace DataMigrator.Common.Data;

public class SystemTypeConverter : IFieldTypeConverter<Type>
{
    private static readonly TupleList<Type, FieldType> netTypes = new();
    private static readonly TupleList<DbType, FieldType> dbTypes = new();
    private static readonly TupleList<FieldType, Type> fieldTypes = new();

    static SystemTypeConverter()
    {
        #region fieldTypes

        //fieldTypes.Add(FieldType.AutoNumber, typeof(Int32));
        fieldTypes.Add(FieldType.Binary, typeof(Byte[]));
        fieldTypes.Add(FieldType.Byte, typeof(byte));
        fieldTypes.Add(FieldType.Boolean, typeof(Boolean));
        fieldTypes.Add(FieldType.Char, typeof(Char));
        fieldTypes.Add(FieldType.Choice, typeof(String));
        fieldTypes.Add(FieldType.Calculated, typeof(String));
        fieldTypes.Add(FieldType.Currency, typeof(Decimal));
        fieldTypes.Add(FieldType.Date, typeof(DateTime));
        fieldTypes.Add(FieldType.DateTime, typeof(DateTime));
        fieldTypes.Add(FieldType.DateTimeOffset, typeof(DateTimeOffset));
        fieldTypes.Add(FieldType.Decimal, typeof(Decimal));
        fieldTypes.Add(FieldType.Double, typeof(Double));
        fieldTypes.Add(FieldType.Geometry, typeof(String));
        fieldTypes.Add(FieldType.Guid, typeof(Guid));
        fieldTypes.Add(FieldType.Int16, typeof(Int16));
        fieldTypes.Add(FieldType.Int32, typeof(Int32));
        fieldTypes.Add(FieldType.Int64, typeof(Int64));
        fieldTypes.Add(FieldType.Lookup, typeof(String));
        fieldTypes.Add(FieldType.MultiChoice, typeof(String));
        fieldTypes.Add(FieldType.MultiLookup, typeof(String));
        fieldTypes.Add(FieldType.MultiUser, typeof(String));
        fieldTypes.Add(FieldType.Object, typeof(Object));
        fieldTypes.Add(FieldType.RichText, typeof(String));
        fieldTypes.Add(FieldType.SByte, typeof(SByte));
        fieldTypes.Add(FieldType.Single, typeof(Single));
        fieldTypes.Add(FieldType.String, typeof(String));
        fieldTypes.Add(FieldType.Time, typeof(TimeSpan));
        fieldTypes.Add(FieldType.Timestamp, typeof(Byte[]));
        fieldTypes.Add(FieldType.UInt16, typeof(UInt16));
        fieldTypes.Add(FieldType.UInt32, typeof(UInt32));
        fieldTypes.Add(FieldType.UInt64, typeof(UInt64));
        fieldTypes.Add(FieldType.Url, typeof(Uri));
        fieldTypes.Add(FieldType.User, typeof(String));
        fieldTypes.Add(FieldType.Xml, typeof(SqlXml));

        #endregion fieldTypes

        #region netTypes

        netTypes.Add(typeof(Boolean), FieldType.Boolean);
        netTypes.Add(typeof(Byte), FieldType.Byte);
        netTypes.Add(typeof(Char), FieldType.Char);
        netTypes.Add(typeof(Int16), FieldType.Int16);
        netTypes.Add(typeof(Int32), FieldType.Int32);
        netTypes.Add(typeof(Int64), FieldType.Int64);
        netTypes.Add(typeof(Decimal), FieldType.Decimal);
        netTypes.Add(typeof(Double), FieldType.Double);
        netTypes.Add(typeof(DateTime), FieldType.DateTime);
        netTypes.Add(typeof(DateTimeOffset), FieldType.DateTimeOffset);
        netTypes.Add(typeof(Guid), FieldType.Guid);
        netTypes.Add(typeof(Single), FieldType.Single);
        netTypes.Add(typeof(String), FieldType.String);
        netTypes.Add(typeof(SByte), FieldType.SByte);
        netTypes.Add(typeof(TimeSpan), FieldType.Time);
        netTypes.Add(typeof(UInt16), FieldType.UInt16);
        netTypes.Add(typeof(UInt32), FieldType.UInt32);
        netTypes.Add(typeof(UInt64), FieldType.UInt64);
        netTypes.Add(typeof(Uri), FieldType.Url);

        #endregion netTypes

        #region dbTypes

        dbTypes.Add(DbType.AnsiString, FieldType.String);
        dbTypes.Add(DbType.AnsiStringFixedLength, FieldType.String);
        dbTypes.Add(DbType.Binary, FieldType.Binary);
        dbTypes.Add(DbType.Boolean, FieldType.Boolean);
        dbTypes.Add(DbType.Byte, FieldType.Byte);
        dbTypes.Add(DbType.Currency, FieldType.Decimal);
        dbTypes.Add(DbType.Date, FieldType.Date);
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
        dbTypes.Add(DbType.StringFixedLength, FieldType.String);
        dbTypes.Add(DbType.Time, FieldType.Time);
        dbTypes.Add(DbType.UInt16, FieldType.UInt16);
        dbTypes.Add(DbType.UInt32, FieldType.UInt32);
        dbTypes.Add(DbType.UInt64, FieldType.UInt64);
        dbTypes.Add(DbType.VarNumeric, FieldType.Decimal);
        dbTypes.Add(DbType.Xml, FieldType.Xml);

        #endregion dbTypes
    }

    #region IFieldTypeConverter<Type> Members

    public FieldType GetDataMigratorFieldType(Type providerFieldType)
    {
        return netTypes.First(x => x.Item1 == providerFieldType).Item2;
    }

    public FieldType GetDataMigratorFieldType(DbType dbType)
    {
        return dbTypes.First(x => x.Item1 == dbType).Item2;
    }

    public Type GetDataProviderFieldType(FieldType fieldType)
    {
        return fieldTypes.First(x => x.Item1 == fieldType).Item2;
    }

    #endregion IFieldTypeConverter<Type> Members
}