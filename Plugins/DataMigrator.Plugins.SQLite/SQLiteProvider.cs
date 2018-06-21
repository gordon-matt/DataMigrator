using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Kore;
using Kore.Text;

namespace DataMigrator.SQLite
{//TODO: Test! This class not yet tested.
    public class SQLiteProvider : BaseProvider
    {
        private SQLiteDbTypeConverter typeConverter = new SQLiteDbTypeConverter();

        public SQLiteProvider(ConnectionDetails connectionDetails)
            : base(connectionDetails)
        {
        }

        public override string DbProviderName
        {
            get { return "System.Data.SQLite"; }
        }

        #region Table Methods

        public override IEnumerable<string> TableNames
        {
            get
            {
                List<string> tables = new List<string>();
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionDetails.ConnectionString))
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT tbl_name FROM sqlite_master WHERE type = 'table'";

                        connection.Open();

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tables.Add(reader.GetString(0));
                            }
                        }

                        connection.Close();
                    }
                }
                return tables;
            }
        }

        //public override bool CreateTable(string tableName)
        //{
        //    throw new NotSupportedException();
        //}

        //TODO: Maybe no need to override this. Override other overload only for data type
        //TEST first and decide if this override needed or not

        public override bool CreateTable(string tableName, IEnumerable<Field> fields)
        {
            try
            {
                if (!File.Exists(ConnectionDetails.Database))
                {
                    SQLiteConnection.CreateFile(ConnectionDetails.Database);
                }

                #region Create Table Text

                const string CMD_CREATE_TABLE_FORMAT =
    @"CREATE TABLE {0}(
{1}
)";
                StringBuilder sbColumns = new StringBuilder();
                for (int i = 0; i < fields.Count(); i++)
                {
                    Field column = fields.ElementAt(i);
                    sbColumns.Append(column.Name, " ");
                    sbColumns.Append(Common.AppContext.SqlDbTypeConverter.GetDataProviderFieldType(column.Type).ToString().ToUpperInvariant());
                    if (i != fields.Count() - 1)
                    {
                        sbColumns.Append(",", Environment.NewLine);
                    }
                }

                #endregion Create Table Text

                using (SQLiteConnection connection = new SQLiteConnection(ConnectionDetails.ConnectionString))
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format(CMD_CREATE_TABLE_FORMAT, tableName, sbColumns.ToString());

                        connection.Open();
                        command.ExecuteNonQuery(); // CREATE TABLE
                        connection.Close();
                    }
                }

                return true;
            }
            catch (Exception x)
            {
                TraceService.Instance.WriteException(x);
                return false;
            }
        }

        #endregion Table Methods

        #region Field Methods

        public override bool CreateField(string tableName, Field field)
        {
            const string cmdAddColumn = "ALTER TABLE {0} ADD ( {1} );";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionDetails.ConnectionString))
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    string fieldType = typeConverter.GetDataProviderFieldType(field.Type).ToString();
                    string maxLength = string.Empty;
                    if (field.MaxLength > 0 && field.Type.In(FieldType.String, FieldType.RichText))
                    {
                        maxLength = string.Concat("(", field.MaxLength, ")");
                    }
                    string isRequired = string.Empty;
                    if (field.IsRequired)
                    { isRequired = " NOT NULL"; }

                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(
                        cmdAddColumn,
                        tableName,
                        string.Concat(field.Name, " ", fieldType, maxLength, isRequired));
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
        }

        public override IEnumerable<string> GetFieldNames(string tableName)
        {
            const string COLUMN_INFO_QUERY_FORMAT = "PRAGMA table_info('{0}');";
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionDetails.ConnectionString))
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(COLUMN_INFO_QUERY_FORMAT, tableName);
                    List<string> fields = new List<string>();

                    connection.Open();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fields.Add(reader.GetString(1));
                        }
                    }
                    connection.Close();
                    return fields;
                }
            }
        }

        public override FieldCollection GetFields(string tableName)
        {
            const string COLUMN_INFO_QUERY_FORMAT = "PRAGMA table_info('{0}');";
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionDetails.ConnectionString))
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(COLUMN_INFO_QUERY_FORMAT, tableName);
                    FieldCollection fields = new FieldCollection();

                    connection.Open();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Field field = new Field();
                            field.Name = reader.GetString(1);
                            if (!reader.IsDBNull(0))
                            { field.Ordinal = reader.GetInt32(0); }
                            if (!reader.IsDBNull(2))
                            { field.Type = typeConverter.GetDataMigratorFieldType(reader.GetString(2).ToEnum<SQLiteDbType>(true)); }
                            if (!reader.IsDBNull(3))
                            { field.IsRequired = reader.GetBoolean(3) == true; } //TODO: Test
                            fields.Add(field);
                        }
                    }
                    connection.Close();
                    return fields;
                }
            }
        }

        #endregion Field Methods

        public override FieldType GetDataMigratorFieldType(string providerFieldType)
        {
            return typeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<SQLiteDbType>(providerFieldType, true));
        }

        public override string GetDataProviderFieldType(FieldType fieldType)
        {
            return typeConverter.GetDataProviderFieldType(fieldType).ToString();
        }
    }
}