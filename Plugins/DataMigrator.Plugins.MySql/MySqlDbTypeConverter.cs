using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso;
using MySql.Data.MySqlClient;

namespace DataMigrator.MySql;

public class MySqlDbTypeConverter : IFieldTypeConverter<MySqlDbType>
{
    private static readonly List<(FieldType FieldType, MySqlDbType MySqlDbType)> fieldTypes = new();
    private static readonly List<(MySqlDbType MySqlDbType, FieldType FieldType)> mySqlDbTypes = new();
    private static readonly List<(MySqlDbType EnumValue, string StringValue)> mySqlDbTypes2 = new();

    static MySqlDbTypeConverter()
    {
        #region fieldTypes

        //fieldTypes.Add((FieldType.AutoNumber, MySqlDbType.Int32));
        fieldTypes.Add((FieldType.Binary, MySqlDbType.Binary));
        fieldTypes.Add((FieldType.Byte, MySqlDbType.UByte));
        fieldTypes.Add((FieldType.Boolean, MySqlDbType.Bit));
        fieldTypes.Add((FieldType.Char, MySqlDbType.TinyText));
        fieldTypes.Add((FieldType.Choice, MySqlDbType.Text));
        fieldTypes.Add((FieldType.Calculated, MySqlDbType.Text));
        fieldTypes.Add((FieldType.Currency, MySqlDbType.Decimal));
        fieldTypes.Add((FieldType.Date, MySqlDbType.Date));
        fieldTypes.Add((FieldType.DateTime, MySqlDbType.DateTime));
        fieldTypes.Add((FieldType.DateTimeOffset, MySqlDbType.Text));
        fieldTypes.Add((FieldType.Decimal, MySqlDbType.Decimal));
        fieldTypes.Add((FieldType.Double, MySqlDbType.Double));
        fieldTypes.Add((FieldType.Geometry, MySqlDbType.Geometry));
        fieldTypes.Add((FieldType.Guid, MySqlDbType.Guid));
        fieldTypes.Add((FieldType.Int16, MySqlDbType.Int16));
        fieldTypes.Add((FieldType.Int32, MySqlDbType.Int32));
        fieldTypes.Add((FieldType.Int64, MySqlDbType.Int64));
        fieldTypes.Add((FieldType.Json, MySqlDbType.JSON));
        fieldTypes.Add((FieldType.Lookup, MySqlDbType.Text));
        fieldTypes.Add((FieldType.MultiChoice, MySqlDbType.Text));
        fieldTypes.Add((FieldType.MultiLookup, MySqlDbType.Text));
        fieldTypes.Add((FieldType.MultiUser, MySqlDbType.Text));
        fieldTypes.Add((FieldType.Object, MySqlDbType.Blob));
        fieldTypes.Add((FieldType.RichText, MySqlDbType.LongText));
        fieldTypes.Add((FieldType.SByte, MySqlDbType.Byte));
        fieldTypes.Add((FieldType.Single, MySqlDbType.Float));
        fieldTypes.Add((FieldType.String, MySqlDbType.Text));
        fieldTypes.Add((FieldType.Time, MySqlDbType.Time));
        fieldTypes.Add((FieldType.Timestamp, MySqlDbType.Timestamp));
        fieldTypes.Add((FieldType.UInt16, MySqlDbType.UInt16));
        fieldTypes.Add((FieldType.UInt32, MySqlDbType.UInt32));
        fieldTypes.Add((FieldType.UInt64, MySqlDbType.UInt64));
        fieldTypes.Add((FieldType.Url, MySqlDbType.Text));
        fieldTypes.Add((FieldType.User, MySqlDbType.Text));
        fieldTypes.Add((FieldType.Xml, MySqlDbType.Text));

        #endregion fieldTypes

        #region mySqlDbTypes

        mySqlDbTypes.Add((MySqlDbType.Binary, FieldType.Binary));
        mySqlDbTypes.Add((MySqlDbType.Bit, FieldType.Boolean));
        mySqlDbTypes.Add((MySqlDbType.Blob, FieldType.Object));//not sure if best mapping
        mySqlDbTypes.Add((MySqlDbType.Byte, FieldType.SByte));
        mySqlDbTypes.Add((MySqlDbType.Date, FieldType.Date));
        mySqlDbTypes.Add((MySqlDbType.DateTime, FieldType.DateTime));
        mySqlDbTypes.Add((MySqlDbType.Decimal, FieldType.Decimal));
        mySqlDbTypes.Add((MySqlDbType.Double, FieldType.Double));
        mySqlDbTypes.Add((MySqlDbType.Enum, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.Float, FieldType.Single));
        mySqlDbTypes.Add((MySqlDbType.Geometry, FieldType.Geometry));
        mySqlDbTypes.Add((MySqlDbType.Guid, FieldType.Guid));
        mySqlDbTypes.Add((MySqlDbType.Int16, FieldType.Int16));
        mySqlDbTypes.Add((MySqlDbType.Int24, FieldType.Int32));
        mySqlDbTypes.Add((MySqlDbType.Int32, FieldType.Int32));
        mySqlDbTypes.Add((MySqlDbType.Int64, FieldType.Int64));
        mySqlDbTypes.Add((MySqlDbType.JSON, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.LongBlob, FieldType.Object));
        mySqlDbTypes.Add((MySqlDbType.LongText, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.MediumBlob, FieldType.Object));
        mySqlDbTypes.Add((MySqlDbType.MediumText, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.Newdate, FieldType.DateTime));
        mySqlDbTypes.Add((MySqlDbType.NewDecimal, FieldType.Decimal));
        mySqlDbTypes.Add((MySqlDbType.Set, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.String, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.Text, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.Time, FieldType.Time));
        mySqlDbTypes.Add((MySqlDbType.Timestamp, FieldType.Timestamp));
        mySqlDbTypes.Add((MySqlDbType.TinyBlob, FieldType.Object));
        mySqlDbTypes.Add((MySqlDbType.TinyText, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.UByte, FieldType.Byte));
        mySqlDbTypes.Add((MySqlDbType.UInt16, FieldType.UInt16));
        mySqlDbTypes.Add((MySqlDbType.UInt24, FieldType.UInt32));
        mySqlDbTypes.Add((MySqlDbType.UInt32, FieldType.UInt32));
        mySqlDbTypes.Add((MySqlDbType.UInt64, FieldType.UInt64));
        mySqlDbTypes.Add((MySqlDbType.VarBinary, FieldType.Binary));
        mySqlDbTypes.Add((MySqlDbType.VarChar, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.VarString, FieldType.String));
        mySqlDbTypes.Add((MySqlDbType.Year, FieldType.Int16));
        #endregion mySqlDbTypes

        #region mySqlDbTypes2

        mySqlDbTypes2.Add((MySqlDbType.Binary, "BINARY"));
        mySqlDbTypes2.Add((MySqlDbType.Bit, "BIT"));
        mySqlDbTypes2.Add((MySqlDbType.Blob, "BLOB"));
        mySqlDbTypes2.Add((MySqlDbType.Byte, "TINYINT"));
        mySqlDbTypes2.Add((MySqlDbType.Date, "DATE"));
        mySqlDbTypes2.Add((MySqlDbType.DateTime, "DATETIME"));
        mySqlDbTypes2.Add((MySqlDbType.Decimal, "DECIMAL"));
        mySqlDbTypes2.Add((MySqlDbType.Double, "DOUBLE"));
        mySqlDbTypes2.Add((MySqlDbType.Enum, "ENUM"));
        mySqlDbTypes2.Add((MySqlDbType.Float, "FLOAT"));
        mySqlDbTypes2.Add((MySqlDbType.Geometry, "GEOMETRY"));
        mySqlDbTypes2.Add((MySqlDbType.Guid, "CHAR(36)"));
        mySqlDbTypes2.Add((MySqlDbType.Int16, "SMALLINT"));
        mySqlDbTypes2.Add((MySqlDbType.Int24, "MEDIUMINT"));
        mySqlDbTypes2.Add((MySqlDbType.Int32, "INT"));
        mySqlDbTypes2.Add((MySqlDbType.Int64, "BIGINT"));
        mySqlDbTypes2.Add((MySqlDbType.JSON, "JSON"));
        mySqlDbTypes2.Add((MySqlDbType.LongBlob, "LONGBLOB"));
        mySqlDbTypes2.Add((MySqlDbType.LongText, "LONGTEXT"));
        mySqlDbTypes2.Add((MySqlDbType.MediumBlob, "MEDIUMBLOB"));
        mySqlDbTypes2.Add((MySqlDbType.MediumText, "MEDIUMTEXT"));
        mySqlDbTypes2.Add((MySqlDbType.Newdate, "NEWDATE"));
        mySqlDbTypes2.Add((MySqlDbType.NewDecimal, "DECIMAL"));
        mySqlDbTypes2.Add((MySqlDbType.Set, "SET"));
        mySqlDbTypes2.Add((MySqlDbType.String, "CHAR"));
        mySqlDbTypes2.Add((MySqlDbType.Text, "TEXT"));
        mySqlDbTypes2.Add((MySqlDbType.Time, "TIME"));
        mySqlDbTypes2.Add((MySqlDbType.Timestamp, "TIMESTAMP"));
        mySqlDbTypes2.Add((MySqlDbType.TinyBlob, "TINYBLOB"));
        mySqlDbTypes2.Add((MySqlDbType.TinyText, "TINYTEXT"));
        mySqlDbTypes2.Add((MySqlDbType.UByte, "TINYINT UNSIGNED"));
        mySqlDbTypes2.Add((MySqlDbType.UInt16, "SMALLINT UNSIGNED"));
        mySqlDbTypes2.Add((MySqlDbType.UInt24, "MEDIUMINT UNSIGNED"));
        mySqlDbTypes2.Add((MySqlDbType.UInt32, "INTEGER UNSIGNED"));
        mySqlDbTypes2.Add((MySqlDbType.UInt64, "BIGINT UNSIGNED"));
        mySqlDbTypes2.Add((MySqlDbType.VarBinary, "VARBINARY"));
        mySqlDbTypes2.Add((MySqlDbType.VarChar, "VARCHAR"));
        mySqlDbTypes2.Add((MySqlDbType.VarString, "VARSTRING"));
        mySqlDbTypes2.Add((MySqlDbType.Year, "YEAR"));

        #endregion mySqlDbTypes2
    }

    #region IFieldTypeConverter<MySqlDbType> Members

    public FieldType GetDataMigratorFieldType(MySqlDbType providerFieldType) =>
        mySqlDbTypes.First(x => x.MySqlDbType == providerFieldType).FieldType;

    public MySqlDbType GetDataProviderFieldType(FieldType fieldType) =>
        fieldTypes.First(x => x.FieldType == fieldType).MySqlDbType;

    #endregion IFieldTypeConverter<MySqlDbType> Members

    public static string GetMySqlDataTypeStringValue(MySqlDbType mySqlDbType) =>
        mySqlDbTypes2.First(x => x.EnumValue == mySqlDbType).StringValue;

    public static MySqlDbType GetMySqlDataType(string mySqlDbType)
    {
        string dataType = mySqlDbType.Contains('(')
            ? mySqlDbType.LeftOf('(')
            : mySqlDbType;

        return mySqlDbType.Equals("INTEGER", StringComparison.InvariantCultureIgnoreCase)
            ? MySqlDbType.Int32
            : mySqlDbTypes2.First(x => x.StringValue == dataType.ToUpperInvariant()).EnumValue;
    }
}