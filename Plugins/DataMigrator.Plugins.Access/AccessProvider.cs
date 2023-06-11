using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso;
using Extenso.Data;

namespace DataMigrator.Access
{
    //TODO: Test this class
    public class AccessProvider : BaseProvider
    {
        private AccessDbTypeConverter typeConverter = new AccessDbTypeConverter();

        public override string DbProviderName
        {
            //get { return "Microsoft.ACE.OLEDB.12.0"; }
            get { return "System.Data.OleDb"; }
        }

        public AccessProvider(ConnectionDetails connectionDetails)
            : base(connectionDetails)
        {
            //MaxTextFieldLength = "255";
            //MaxRichTextFieldLength = string.Empty;
        }

        public override IEnumerable<string> TableNames
        {
            get
            {
                using (var connection = new OleDbConnection(ConnectionDetails.ConnectionString))
                {
                    string[] restrictions = new string[4];
                    restrictions[3] = "Table";

                    connection.Open();
                    var schema = connection.GetSchema("Tables", restrictions);
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

        public override IEnumerable<string> GetFieldNames(string tableName)
        {
            using (var connection = new OleDbConnection(ConnectionDetails.ConnectionString))
            {
                var columnInfo = connection.GetColumnData(tableName);
                return columnInfo.Select(c => c.ColumnName);
            }
        }

        public override FieldCollection GetFields(string tableName)
        {
            using (var connection = new OleDbConnection(ConnectionDetails.ConnectionString))
            {
                var columnInfo = connection.GetColumnData(tableName);
                var fields = new FieldCollection();

                columnInfo.ForEach(c =>
                    {
                        var field = new Field
                        {
                            DisplayName = c.ColumnName,
                            IsPrimaryKey = c.KeyType == KeyType.PrimaryKey,
                            IsRequired = !c.IsNullable,
                            MaxLength = (int)c.MaximumLength,
                            Name = c.ColumnName,
                            Ordinal = (int)c.OrdinalPosition,
                            Type = AppContext.SystemTypeConverter.GetDataMigratorFieldType(c.DataType)
                        };
                        fields.Add(field);
                    });

                return fields;
            }
        }

        public override bool CreateField(string tableName, Field field)
        {
            var existingFieldNames = GetFieldNames(tableName);
            if (existingFieldNames.Contains(field.Name))
            {
                TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the table, {1}", field.Name, tableName);
                //throw new ArgumentException("etc");
                return false;
            }

            using (var connection = new OleDbConnection(ConnectionDetails.ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    string fieldType = GetDataProviderFieldType(field.Type);
                    string maxLength = string.Empty;

                    if (field.Type == FieldType.String)
                    {
                        if (field.MaxLength > 0 && field.MaxLength <= 255)
                        {
                            maxLength = string.Concat("(", field.MaxLength, ")");
                        }
                        else { maxLength = "(255)"; }
                    }

                    string isRequired = string.Empty;
                    if (field.IsRequired)
                    { isRequired = " NOT NULL"; }

                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(
                        "ALTER TABLE [{0}] ADD {1}",
                        tableName,
                        string.Concat(
                            EncloseIdentifier(field.Name), " ",
                            fieldType,
                            maxLength,
                            isRequired));
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        ////TODO: Probably dont need this since implementing SpaceEscape in base class. Just need to override it in this
        ////class' constructor and then test for Sql and for Access to make sure all working well for both
        ////without having to use this method override.
        //protected override void CreateTable(string tableName, string pkColumnName, string pkDataType, bool pkIsIdentity)
        //{
        //    using (OleDbConnection connection = new OleDbConnection(ConnectionDetails.ConnectionString))
        //    {
        //        const string CMD_CREATE_TABLE_FORMAT = "CREATE TABLE {0}({1} {2} NOT NULL CONSTRAINT PK_{0} PRIMARY KEY )";
        //        string commandText = string.Format(
        //            CMD_CREATE_TABLE_FORMAT,
        //            tableName,
        //            pkColumnName,
        //            pkDataType);

        //        using (OleDbCommand command = connection.CreateCommand())
        //        {
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = commandText;
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //    }
        //}

        public override FieldType GetDataMigratorFieldType(string providerFieldType)
        {
            return typeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<AccessDbType>(providerFieldType, true));
        }

        public override string GetDataProviderFieldType(FieldType fieldType)
        {
            return typeConverter.GetDataProviderFieldType(fieldType).ToString();
        }
    }
}