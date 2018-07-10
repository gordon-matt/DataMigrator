using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Kore;
using Kore.Collections;

namespace DataMigrator.Common.Data
{
    public abstract class BaseProvider
    {
        #region Public Properties

        public abstract string DbProviderName { get; }

        protected ConnectionDetails ConnectionDetails { get; set; }

        /// <summary>
        /// Used in T-SQL queries for escaping spaces and reserved words
        /// </summary>
        protected virtual string EscapeIdentifierStart { get; set; }

        protected virtual string EscapeIdentifierEnd { get; set; }

        #endregion

        protected string EncloseIdentifier(string value)
        {
            return string.Concat(EscapeIdentifierStart, value, EscapeIdentifierEnd);
        }

        #region Constructor

        public BaseProvider(ConnectionDetails connectionDetails)
        {
            this.ConnectionDetails = connectionDetails;
            EscapeIdentifierStart = "[";
            EscapeIdentifierEnd = "]";
            //MaxTextFieldLength = "MAX";
            //MaxRichTextFieldLength = "MAX";
        }

        #endregion

        #region Table Methods

        public virtual IEnumerable<string> TableNames
        {
            get
            {
                using (DbConnection connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
                {
                    string[] restrictions = new string[4];
                    restrictions[3] = "Base Table";

                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables", restrictions);
                    connection.Close();

                    List<string> tableNames = new List<string>();
                    foreach (DataRow row in schema.Rows)
                    {
                        tableNames.Add(row.Field<string>("TABLE_NAME"));
                    }
                    return tableNames;
                }
            }
        }

        public virtual bool CreateTable(string tableName)
        {
            try
            {
                CreateTable(tableName, "Id", GetDataProviderFieldType(FieldType.Int32), true);
            }
            catch (DbException x)
            {
                TraceService.Instance.WriteException(x);
                return false;
            }
            catch (Exception x)
            {
                TraceService.Instance.WriteException(x);
                return false;
            }

            return true;
        }

        public virtual bool CreateTable(string tableName, IEnumerable<Field> fields)
        {
            bool ok = CreateTable(tableName);

            if (!ok)
            { return false; }

            foreach (Field field in fields)
            {
                CreateField(tableName, field);
            }
            return true;
        }

        #endregion

        #region Field Methods

        public virtual bool CreateField(string tableName, Field field)
        {
            var existingFieldNames = GetFieldNames(tableName);
            if (existingFieldNames.Contains(field.Name))
            {
                TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the table, {1}", field.Name, tableName);
                //throw new ArgumentException("etc");
                return false;
            }

            using (DbConnection connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    string fieldType = GetDataProviderFieldType(field.Type);
                    string maxLength = string.Empty;
                    if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char))
                    {
                        if (field.MaxLength > 0 && field.MaxLength <= 8000)
                        {
                            maxLength = string.Concat("(", field.MaxLength, ")");
                        }
                        else
                        {
                            if (field.Type.In(FieldType.String, FieldType.RichText)) //Not supported for CHAR
                            {
                                maxLength = "(MAX)";
                            }
                        }
                    }
                    string isRequired = string.Empty;
                    if (field.IsRequired)
                    { isRequired = " NOT NULL"; }

                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(
                        Constants.Data.CMD_ADD_COLUMN,
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

        public virtual IEnumerable<string> GetFieldNames(string tableName)
        {
            using (DbConnection connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(Constants.Data.CMD_SELECT_INFO_SCHEMA_COLUMN_NAMES, tableName);
                    List<string> columns = new List<string>();

                    connection.Open();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader.GetString(0));
                        }
                    }
                    connection.Close();
                    return columns;
                }
            }
        }

        public virtual FieldCollection GetFields(string tableName)
        {
            using (var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(Constants.Data.CMD_SELECT_INFO_SCHEMA_COLUMNS, tableName);
                var fields = new FieldCollection();

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var field = new Field();
                        field.Name = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                        { field.Ordinal = reader.GetInt32(1); }
                        if (!reader.IsDBNull(2))
                        { field.Type = GetDataMigratorFieldType(reader.GetString(2)); }
                        if (!reader.IsDBNull(3))
                        { field.IsRequired = reader.GetString(3) == "NO"; }
                        if (!reader.IsDBNull(4))
                        { field.MaxLength = reader.GetInt32(4); }
                        fields.Add(field);
                    }
                }
                connection.Close();

                try
                {
                    command.CommandText = string.Format(Constants.Data.CMD_IS_PRIMARY_KEY_FORMAT, tableName);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string pkColumn = reader.GetString(0);
                            var match = fields.SingleOrDefault(f => f.Name == pkColumn);
                            if (match != null)
                            {
                                match.IsPrimaryKey = true;
                            }
                        }
                    }

                    connection.Close();
                }
                catch (Exception x)
                {
                    TraceService.Instance.WriteConcat(TraceEvent.Error, "Error: Could not get primary key info - ", x.Message);
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }

                return fields;
            }
        }

        #endregion

        #region Record Methods

        public virtual int GetRecordCount(string tableName)
        {
            using (DbConnection connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format("SELECT COUNT(*) FROM {0}{1}{2}", EscapeIdentifierStart, tableName, EscapeIdentifierEnd);
                    //command.CommandText = new Query().SelectCountAll().From(tableName).ToString();

                    connection.Open();
                    int rowCount = (int)command.ExecuteScalar();
                    connection.Close();
                    return rowCount;
                }
            }
        }

        public virtual IEnumerator<Record> GetRecordsEnumerator(string tableName)
        {
            return GetRecordsEnumerator(tableName, GetFields(tableName));
        }

        public virtual IEnumerator<Record> GetRecordsEnumerator(string tableName, IEnumerable<Field> fields)
        {
            //Query query = new Query();
            //fields.ForEach(f => { query.Select(f.Name); });
            //query.From(tableName);

            var sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(fields.Select(f => f.Name).Join(
                string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart))
                .Prepend(EscapeIdentifierStart)
                .Append(EscapeIdentifierEnd));
            sb.Append(" FROM ");
            sb.Append(tableName);

            using (var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = sb.ToString();

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Record record = new Record();
                        record.Fields.AddRange(fields);
                        fields.ForEach(f =>
                        {
                            record[f.Name].Value = reader[f.Name];
                        });
                        yield return record;
                    }
                }
                connection.Close();
            }
        }

        protected IDictionary<string, string> CreateParameterNames(IEnumerable<string> fieldNames)
        {
            var parameterNames = new Dictionary<string, string>();
            fieldNames.ForEach(f =>
            {
                string parameterName = f;
                "¬`!\"£$%^&*()-=+{}[]:;@'~#|<>,.?/ ".ToCharArray().ForEach(c => { parameterName = parameterName.Replace(c, '_'); });
                parameterNames.Add(f, parameterName.ToPascalCase().Prepend("@"));
            });
            return parameterNames;
        }

        // TODO: See if can improve performance.
        public virtual void InsertRecords(string tableName, IEnumerable<Record> records)
        {
            const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
            //string fieldNames = records.ElementAt(0).Fields.Select(f => f.Name).Join(",");
            //string parameterNames = fieldNames.Replace(",", ",@").Prepend("@");

            var parameterNames = CreateParameterNames(records.ElementAt(0).Fields.Select(f => f.Name));
            string fieldNames = parameterNames.Keys.Join(",");

            fieldNames = fieldNames
                .Replace(",", string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart)) // "],["
                .Prepend(EscapeIdentifierStart) // "["
                .Append(EscapeIdentifierEnd); // "]"

            using (var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = string.Format(INSERT_INTO_FORMAT, tableName, fieldNames, parameterNames.Values.Join(","));
                        //command.CommandText = string.Format(INSERT_INTO_FORMAT, tableName, fieldNames, parameterNames);

                        records.ElementAt(0).Fields.ForEach(field =>
                        {
                            var parameter = command.CreateParameter();
                            //parameter.ParameterName = string.Concat("@", field.Name);
                            parameter.ParameterName = parameterNames[field.Name];
                            parameter.DbType = AppContext.DbTypeConverter.GetDataProviderFieldType(field.Type);
                            command.Parameters.Add(parameter);
                        });

                        records.ForEach(record =>
                        {
                            record.Fields.ForEach(field =>
                                {
                                    command.Parameters[parameterNames[field.Name]].Value = field.Value;
                                    //command.Parameters[string.Concat("@", field.Name)].Value = field.Value.ToString();
                                });

                            command.ExecuteNonQuery();
                        });
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
        }

        #endregion

        #region Public Static Methods

        protected virtual DbConnection CreateDbConnection(string providerName, string connectionString)
        {
            // Assume failure.
            DbConnection connection = null;

            // Create the DbProviderFactory and DbConnection.
            if (connectionString != null)
            {
                try
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                    connection = factory.CreateConnection();
                    connection.ConnectionString = connectionString;
                }
                catch (Exception ex)
                {
                    // Set the connection to null if it was created.
                    if (connection != null)
                    {
                        connection = null;
                    }
                    Console.WriteLine(ex.Message);
                }
            }
            // Return the connection.
            return connection;
        }

        #endregion

        protected virtual void CreateTable(string tableName, string pkColumnName, string pkDataType, bool pkIsIdentity)
        {
            using (var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString))
            {
                const string CMD_CREATE_TABLE_FORMAT = "CREATE TABLE {0}({1} {2} {3} NOT NULL CONSTRAINT {4} PRIMARY KEY)";
                string commandText = string.Format(
                    CMD_CREATE_TABLE_FORMAT,
                    EncloseIdentifier(tableName),
                    pkColumnName,
                    EncloseIdentifier(pkDataType),
                    pkIsIdentity ? "IDENTITY(1,1)" : string.Empty,
                    EncloseIdentifier("PK_" + tableName));

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = commandText;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        #region Field Conversion Methods

        public abstract FieldType GetDataMigratorFieldType(string providerFieldType);

        public abstract string GetDataProviderFieldType(FieldType fieldType);

        #endregion
    }
}