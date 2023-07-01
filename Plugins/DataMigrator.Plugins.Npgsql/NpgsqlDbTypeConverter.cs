using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso;
using NpgsqlTypes;

namespace DataMigrator.Plugins.Npgsql;

public class NpgsqlDbTypeConverter : IFieldTypeConverter<NpgsqlDbType>
{
    private static readonly List<(FieldType FieldType, NpgsqlDbType NpgsqlDbType)> fieldTypes = new();
    private static readonly List<(NpgsqlDbType NpgsqlDbType, FieldType FieldType)> npgsqlDbTypes = new();

    static NpgsqlDbTypeConverter()
    {
        #region fieldTypes

        fieldTypes.Add((FieldType.Binary, NpgsqlDbType.Bytea));
        fieldTypes.Add((FieldType.Byte, NpgsqlDbType.Smallint));
        fieldTypes.Add((FieldType.Boolean, NpgsqlDbType.Boolean));
        fieldTypes.Add((FieldType.Char, NpgsqlDbType.Char));
        fieldTypes.Add((FieldType.Choice, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.Calculated, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.Currency, NpgsqlDbType.Numeric));
        fieldTypes.Add((FieldType.Date, NpgsqlDbType.Date));
        fieldTypes.Add((FieldType.DateTime, NpgsqlDbType.Timestamp));
        fieldTypes.Add((FieldType.DateTimeOffset, NpgsqlDbType.TimestampTz));
        fieldTypes.Add((FieldType.Decimal, NpgsqlDbType.Numeric));
        fieldTypes.Add((FieldType.Double, NpgsqlDbType.Double));
        fieldTypes.Add((FieldType.Geometry, NpgsqlDbType.Geometry));
        fieldTypes.Add((FieldType.Guid, NpgsqlDbType.Uuid));
        fieldTypes.Add((FieldType.Int16, NpgsqlDbType.Smallint));
        fieldTypes.Add((FieldType.Int32, NpgsqlDbType.Integer));
        fieldTypes.Add((FieldType.Int64, NpgsqlDbType.Bigint));
        fieldTypes.Add((FieldType.Json, NpgsqlDbType.Json));
        fieldTypes.Add((FieldType.Lookup, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.MultiChoice, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.MultiLookup, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.MultiUser, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.Object, NpgsqlDbType.Unknown));
        fieldTypes.Add((FieldType.RichText, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.SByte, NpgsqlDbType.Smallint));
        fieldTypes.Add((FieldType.Single, NpgsqlDbType.Real));
        fieldTypes.Add((FieldType.String, NpgsqlDbType.Varchar));
        fieldTypes.Add((FieldType.Time, NpgsqlDbType.Time));
        fieldTypes.Add((FieldType.Timestamp, NpgsqlDbType.Timestamp));
        fieldTypes.Add((FieldType.UInt16, NpgsqlDbType.Smallint));
        fieldTypes.Add((FieldType.UInt32, NpgsqlDbType.Integer));
        fieldTypes.Add((FieldType.UInt64, NpgsqlDbType.Bigint));
        fieldTypes.Add((FieldType.Url, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.User, NpgsqlDbType.Text));
        fieldTypes.Add((FieldType.Xml, NpgsqlDbType.Xml));

        #endregion fieldTypes

        #region npgsqlDbTypes

        npgsqlDbTypes.Add((NpgsqlDbType.Array, FieldType.Binary));
        npgsqlDbTypes.Add((NpgsqlDbType.Bigint, FieldType.Int64));
        npgsqlDbTypes.Add((NpgsqlDbType.Bit, FieldType.Binary));
        npgsqlDbTypes.Add((NpgsqlDbType.Boolean, FieldType.Boolean));
        npgsqlDbTypes.Add((NpgsqlDbType.Box, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Bytea, FieldType.Binary));
        npgsqlDbTypes.Add((NpgsqlDbType.Char, FieldType.String));
        npgsqlDbTypes.Add((NpgsqlDbType.Cid, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Cidr, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Circle, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Citext, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Date, FieldType.Date));
        npgsqlDbTypes.Add((NpgsqlDbType.Double, FieldType.Double));
        npgsqlDbTypes.Add((NpgsqlDbType.Geometry, FieldType.Geometry));
        npgsqlDbTypes.Add((NpgsqlDbType.Hstore, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Inet, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Int2Vector, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Integer, FieldType.Int32));
        npgsqlDbTypes.Add((NpgsqlDbType.InternalChar, FieldType.String));
        npgsqlDbTypes.Add((NpgsqlDbType.Interval, FieldType.Time));
        npgsqlDbTypes.Add((NpgsqlDbType.Json, FieldType.Json));
        npgsqlDbTypes.Add((NpgsqlDbType.Jsonb, FieldType.Binary));
        npgsqlDbTypes.Add((NpgsqlDbType.JsonPath, FieldType.String));
        npgsqlDbTypes.Add((NpgsqlDbType.Line, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.LSeg, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.MacAddr, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Money, FieldType.Currency));
        npgsqlDbTypes.Add((NpgsqlDbType.Name, FieldType.String));
        npgsqlDbTypes.Add((NpgsqlDbType.Numeric, FieldType.Decimal));
        npgsqlDbTypes.Add((NpgsqlDbType.Oid, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Oidvector, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Path, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Point, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Polygon, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Range, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Real, FieldType.Single));
        npgsqlDbTypes.Add((NpgsqlDbType.Refcursor, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Regtype, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Smallint, FieldType.Int16));
        npgsqlDbTypes.Add((NpgsqlDbType.Text, FieldType.String));
        npgsqlDbTypes.Add((NpgsqlDbType.Tid, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Time, FieldType.Time));
        npgsqlDbTypes.Add((NpgsqlDbType.Timestamp, FieldType.Timestamp));
        npgsqlDbTypes.Add((NpgsqlDbType.TimestampTz, FieldType.Timestamp));
        npgsqlDbTypes.Add((NpgsqlDbType.TimeTz, FieldType.Time));
        npgsqlDbTypes.Add((NpgsqlDbType.TsVector, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Unknown, FieldType.Unknown));
        npgsqlDbTypes.Add((NpgsqlDbType.Uuid, FieldType.Guid));
        npgsqlDbTypes.Add((NpgsqlDbType.Varbit, FieldType.Binary));
        npgsqlDbTypes.Add((NpgsqlDbType.Varchar, FieldType.String));
        npgsqlDbTypes.Add((NpgsqlDbType.Xid, FieldType.Object));
        npgsqlDbTypes.Add((NpgsqlDbType.Xml, FieldType.Xml));

        #endregion npgsqlDbTypes
    }

    #region IFieldTypeConverter<NpgsqlDbType> Members

    public FieldType GetDataMigratorFieldType(NpgsqlDbType providerFieldType) =>
        npgsqlDbTypes.First(x => x.NpgsqlDbType == providerFieldType).FieldType;

    public NpgsqlDbType GetDataProviderFieldType(FieldType fieldType) =>
        fieldTypes.First(x => x.FieldType == fieldType).NpgsqlDbType;

    #endregion IFieldTypeConverter<NpgsqlDbType> Members

    public static string GetNpgsqlDataTypeStringValue(NpgsqlDbType npgsqlDbType)
    {
        return npgsqlDbType switch
        {
            NpgsqlDbType.Char => "character",
            NpgsqlDbType.Varchar or NpgsqlDbType.Text => "character varying",
            NpgsqlDbType.Timestamp => "timestamp without time zone",
            NpgsqlDbType.TimestampTz => "timestamp with time zone",
            NpgsqlDbType.Time => "time without time zone",
            NpgsqlDbType.TimeTz => "time with time zone",
            _ => npgsqlDbType.ToString().ToLowerInvariant(),
        };

        //return npgsqlDbTypes2.First(x => x.Item1 == npgsqlDbType).Item2;
    }

    public static NpgsqlDbType GetNpgsqlDataType(string npgsqlDbType)
    {
        string dataType = npgsqlDbType.ToLowerInvariant();

        return dataType switch
        {
            "character" => NpgsqlDbType.Char,
            "character varying" => NpgsqlDbType.Varchar,
            "timestamp with time zone" => NpgsqlDbType.TimestampTz,
            "timestamp without time zone" => NpgsqlDbType.Timestamp,
            "time with time zone" => NpgsqlDbType.TimeTz,
            "time without time zone" => NpgsqlDbType.Time,
            _ => EnumExtensions.Parse<NpgsqlDbType>(npgsqlDbType),
        };
    }
}