using System.Data;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso;
using AppContext = DataMigrator.Common.AppContext;

namespace DataMigrator.Sql;

public class SqlProvider : BaseProvider
{
    public override string DbProviderName
    {
        get { return "System.Data.SqlClient"; }
    }

    public SqlProvider(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    public override FieldType GetDataMigratorFieldType(string providerFieldType)
    {
        return AppContext.SqlDbTypeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<SqlDbType>(providerFieldType, true));
    }

    public override string GetDataProviderFieldType(FieldType fieldType)
    {
        return AppContext.SqlDbTypeConverter.GetDataProviderFieldType(fieldType).ToString();
    }
}