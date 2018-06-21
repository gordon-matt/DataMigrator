using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Kore;
using Kore.Collections;
using Kore.Data.PostgreSql;
using Npgsql;
using NpgsqlTypes;

namespace DataMigrator.Plugins.Npgsql
{
    public class NpgsqlProvider : BaseProvider
    {
        private NpgsqlDbTypeConverter typeConverter = new NpgsqlDbTypeConverter();

        public override string DbProviderName
        {
            get { return "Npgsql"; }
        }

        public NpgsqlProvider(ConnectionDetails connectionDetails)
            : base(connectionDetails)
        {
            EscapeIdentifierStart = "\"";
            EscapeIdentifierEnd = "\"";
        }

        public string Schema
        {
            get
            {
                string schema = "public";
                if (!ConnectionDetails.ExtendedProperties.IsNullOrEmpty())
                {
                    string value = ConnectionDetails.ExtendedProperties["Schema"].GetValue<string>();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        schema = value;
                    }
                }
                return schema;
            }
        }

        public override IEnumerable<string> TableNames
        {
            get
            {
                using (var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString))
                {
                    return connection.GetTableNames(schema: Schema);
                }
            }
        }

        protected override DbConnection CreateDbConnection(string providerName, string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public override bool CreateTable(string tableName)
        {
            using (var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(
                    @"CREATE TABLE {0}.""{1}""()",
                    Schema,
                    tableName);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return true;
        }

        protected override void CreateTable(string tableName, string pkColumnName, string pkDataType, bool pkIsIdentity)
        {
            throw new NotSupportedException();
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

            using (var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                string fieldType = GetDataProviderFieldType(field.Type);
                string maxLength = string.Empty;
                if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char))
                {
                    if (field.MaxLength > 0)
                    {
                        maxLength = string.Concat("(", field.MaxLength, ")");
                    }
                    else
                    {
                        fieldType = "text";
                    }
                }
                string isRequired = string.Empty;
                if (field.IsRequired)
                { isRequired = " NOT NULL"; }

                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(
                    @"ALTER TABLE {0}.""{1}"" ADD {2}",
                    Schema,
                    tableName,
                    string.Concat(EncloseIdentifier(field.Name), " ", fieldType, maxLength, isRequired));
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }

        public override int GetRecordCount(string tableName)
        {
            using (var connection = new NpgsqlConnection(ConnectionDetails.ConnectionString))
            {
                return connection.GetRowCount(Schema, tableName);
            }
        }

        public override IEnumerator<Record> GetRecordsEnumerator(string tableName, IEnumerable<Field> fields)
        {
            var sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(fields.Select(f => f.Name).Join(
                string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart))
                .Prepend(EscapeIdentifierStart)
                .Append(EscapeIdentifierEnd));
            sb.Append(" FROM ");
            sb.Append(Schema);
            sb.Append('.');
            sb.Append(EncloseIdentifier(tableName));

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
                        var record = new Record();
                        record.Fields.AddRange(fields);
                        fields.ForEach(f =>
                        {
                            var value = reader[f.Name];
                            //if (f.Type.In(FieldType.Date, FieldType.DateTime, FieldType.DateTimeOffset, FieldType.Time, FieldType.Timestamp))
                            //{
                            //    if (value.ToString() == string.Empty)
                            //    {
                            //        value = DBNull.Value;
                            //    }
                            //}
                            record[f.Name].Value = value;
                        });
                        yield return record;
                    }
                }
                connection.Close();
            }
        }

        public override void InsertRecords(string tableName, IEnumerable<Record> records)
        {
            const string INSERT_INTO_FORMAT = @"INSERT INTO {0}.""{1}""({2}) VALUES({3})";

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
                        command.CommandText = string.Format(INSERT_INTO_FORMAT, Schema, tableName, fieldNames, parameterNames.Values.Join(","));

                        records.ElementAt(0).Fields.ForEach(field =>
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = parameterNames[field.Name];
                            parameter.DbType = Common.AppContext.DbTypeConverter.GetDataProviderFieldType(field.Type);
                            command.Parameters.Add(parameter);
                        });

                        records.ForEach(record =>
                        {
                            record.Fields.ForEach(field =>
                            {
                                command.Parameters[parameterNames[field.Name]].Value = field.Value;
                            });

                            command.ExecuteNonQuery();
                        });
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
        }

        public override FieldType GetDataMigratorFieldType(string providerFieldType)
        {
            NpgsqlDbType npgsqlType = NpgsqlDbTypeConverter.GetNpgsqlDataType(providerFieldType);
            return typeConverter.GetDataMigratorFieldType(npgsqlType);
        }

        public override string GetDataProviderFieldType(FieldType fieldType)
        {
            NpgsqlDbType mySqlType = typeConverter.GetDataProviderFieldType(fieldType);
            return NpgsqlDbTypeConverter.GetNpgsqlDataTypeStringValue(mySqlType);
        }
    }
}