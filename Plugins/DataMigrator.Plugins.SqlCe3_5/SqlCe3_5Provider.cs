using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Kore;

namespace DataMigrator.SqlCe3_5
{
    public class SqlCe3_5Provider : BaseProvider
    {
        public override string DbProviderName => "System.Data.SqlServerCe.4.0";

        public SqlCe3_5Provider(ConnectionDetails connectionDetails)
            : base(connectionDetails)
        {
        }

        public override IEnumerable<string> TableNames
        {
            get
            {
                using (var connection = new SqlCeConnection(ConnectionDetails.ConnectionString))
                {
                    connection.Open();
                    var schema = connection.GetSchema("Tables");
                    connection.Close();

                    var tableNames = new List<string>();
                    foreach (DataRow row in schema.Rows)
                    {
                        tableNames.Add(row.Field<string>("TABLE_NAME"));
                    }
                    return tableNames;
                }
            }
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
}