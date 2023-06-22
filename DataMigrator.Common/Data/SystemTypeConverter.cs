using System.Data;
using System.Data.SqlTypes;
using DataMigrator.Common.Models;

namespace DataMigrator.Common.Data;

public class SystemTypeConverter : IFieldTypeConverter<Type>
{
    private static readonly List<(Type SystemType, FieldType FieldType)> netTypes = new();
    //private static readonly List<(DbType DbType, FieldType FieldType)> dbTypes = new();
    private static readonly List<(FieldType FieldType, Type SystemType)> fieldTypes = new();

    static SystemTypeConverter()
    {
        #region fieldTypes

        //fieldTypes.Add((FieldType.AutoNumber, typeof(Int32)));
        fieldTypes.Add((FieldType.Binary, typeof(byte[])));
        fieldTypes.Add((FieldType.Byte, typeof(byte)));
        fieldTypes.Add((FieldType.Boolean, typeof(bool)));
        fieldTypes.Add((FieldType.Char, typeof(char)));
        fieldTypes.Add((FieldType.Choice, typeof(string)));
        fieldTypes.Add((FieldType.Calculated, typeof(string)));
        fieldTypes.Add((FieldType.Currency, typeof(decimal)));
        fieldTypes.Add((FieldType.Date, typeof(DateOnly)));
        fieldTypes.Add((FieldType.DateTime, typeof(DateTime)));
        fieldTypes.Add((FieldType.DateTimeOffset, typeof(DateTimeOffset)));
        fieldTypes.Add((FieldType.Decimal, typeof(decimal)));
        fieldTypes.Add((FieldType.Double, typeof(double)));
        fieldTypes.Add((FieldType.Geometry, typeof(string)));
        fieldTypes.Add((FieldType.Guid, typeof(Guid)));
        fieldTypes.Add((FieldType.Int16, typeof(short)));
        fieldTypes.Add((FieldType.Int32, typeof(int)));
        fieldTypes.Add((FieldType.Int64, typeof(long)));
        fieldTypes.Add((FieldType.Lookup, typeof(string)));
        fieldTypes.Add((FieldType.MultiChoice, typeof(string)));
        fieldTypes.Add((FieldType.MultiLookup, typeof(string)));
        fieldTypes.Add((FieldType.MultiUser, typeof(string)));
        fieldTypes.Add((FieldType.Object, typeof(object)));
        fieldTypes.Add((FieldType.RichText, typeof(string)));
        fieldTypes.Add((FieldType.SByte, typeof(sbyte)));
        fieldTypes.Add((FieldType.Single, typeof(float)));
        fieldTypes.Add((FieldType.String, typeof(string)));
        fieldTypes.Add((FieldType.Time, typeof(TimeOnly)));
        fieldTypes.Add((FieldType.Timestamp, typeof(byte[])));
        fieldTypes.Add((FieldType.UInt16, typeof(ushort)));
        fieldTypes.Add((FieldType.UInt32, typeof(uint)));
        fieldTypes.Add((FieldType.UInt64, typeof(ulong)));
        fieldTypes.Add((FieldType.Url, typeof(Uri)));
        fieldTypes.Add((FieldType.User, typeof(string)));
        fieldTypes.Add((FieldType.Xml, typeof(SqlXml)));

        #endregion fieldTypes

        #region netTypes

        netTypes.Add((typeof(bool), FieldType.Boolean));
        netTypes.Add((typeof(byte), FieldType.Byte));
        netTypes.Add((typeof(char), FieldType.Char));
        netTypes.Add((typeof(short), FieldType.Int16));
        netTypes.Add((typeof(int), FieldType.Int32));
        netTypes.Add((typeof(long), FieldType.Int64));
        netTypes.Add((typeof(decimal), FieldType.Decimal));
        netTypes.Add((typeof(double), FieldType.Double));
        netTypes.Add((typeof(DateOnly), FieldType.Date));
        netTypes.Add((typeof(DateTime), FieldType.DateTime));
        netTypes.Add((typeof(DateTimeOffset), FieldType.DateTimeOffset));
        netTypes.Add((typeof(Guid), FieldType.Guid));
        netTypes.Add((typeof(float), FieldType.Single));
        netTypes.Add((typeof(string), FieldType.String));
        netTypes.Add((typeof(sbyte), FieldType.SByte));
        netTypes.Add((typeof(TimeOnly), FieldType.Time));
        netTypes.Add((typeof(ushort), FieldType.UInt16));
        netTypes.Add((typeof(uint), FieldType.UInt32));
        netTypes.Add((typeof(ulong), FieldType.UInt64));
        netTypes.Add((typeof(Uri), FieldType.Url));

        #endregion netTypes

        #region dbTypes

        //dbTypes.Add((DbType.AnsiString, FieldType.String));
        //dbTypes.Add((DbType.AnsiStringFixedLength, FieldType.String));
        //dbTypes.Add((DbType.Binary, FieldType.Binary));
        //dbTypes.Add((DbType.Boolean, FieldType.Boolean));
        //dbTypes.Add((DbType.Byte, FieldType.Byte));
        //dbTypes.Add((DbType.Currency, FieldType.Decimal));
        //dbTypes.Add((DbType.Date, FieldType.Date));
        //dbTypes.Add((DbType.DateTime, FieldType.DateTime));
        //dbTypes.Add((DbType.DateTime2, FieldType.DateTime));
        //dbTypes.Add((DbType.DateTimeOffset, FieldType.DateTimeOffset));
        //dbTypes.Add((DbType.Decimal, FieldType.Decimal));
        //dbTypes.Add((DbType.Double, FieldType.Double));
        //dbTypes.Add((DbType.Guid, FieldType.Guid));
        //dbTypes.Add((DbType.Int16, FieldType.Int16));
        //dbTypes.Add((DbType.Int32, FieldType.Int32));
        //dbTypes.Add((DbType.Int64, FieldType.Int64));
        //dbTypes.Add((DbType.Object, FieldType.Object));
        //dbTypes.Add((DbType.SByte, FieldType.SByte));
        //dbTypes.Add((DbType.Single, FieldType.Single));
        //dbTypes.Add((DbType.String, FieldType.String));
        //dbTypes.Add((DbType.StringFixedLength, FieldType.String));
        //dbTypes.Add((DbType.Time, FieldType.Time));
        //dbTypes.Add((DbType.UInt16, FieldType.UInt16));
        //dbTypes.Add((DbType.UInt32, FieldType.UInt32));
        //dbTypes.Add((DbType.UInt64, FieldType.UInt64));
        //dbTypes.Add((DbType.VarNumeric, FieldType.Decimal));
        //dbTypes.Add((DbType.Xml, FieldType.Xml));

        #endregion dbTypes
    }

    #region IFieldTypeConverter<Type> Members

    public FieldType GetDataMigratorFieldType(Type providerFieldType) => netTypes.First(x => x.SystemType == providerFieldType).FieldType;

    //public FieldType GetDataMigratorFieldType(DbType dbType) => dbTypes.First(x => x.DbType == dbType).FieldType;

    public Type GetDataProviderFieldType(FieldType fieldType) => fieldTypes.First(x => x.FieldType == fieldType).SystemType;

    #endregion IFieldTypeConverter<Type> Members
}