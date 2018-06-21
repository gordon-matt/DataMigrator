using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Kore;
using System.IO;
using Kore.IO;
using System.Data;
using Kore.Collections;
using Kore.Data;

namespace DataMigrator.Csv
{
    public class CsvProvider : BaseProvider
    {
        public override string DbProviderName
        {
            get { throw new NotSupportedException(); }
        }

        public CsvProvider(ConnectionDetails connectionDetails)
            : base(connectionDetails)
        {
        }

        public override bool CreateField(string tableName, Field field)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            DataTable table = new FileInfo(ConnectionDetails.Database).ReadCsv(hasHeaderRow, ",");
            table.Columns.Add(field.Name);
            table.ToCsv(ConnectionDetails.Database, true);
            return true;
        }

        public override bool CreateTable(string tableName)
        {
            throw new NotSupportedException();
        }

        public override bool CreateTable(string tableName, IEnumerable<Field> fields)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            DataTable table = new FileInfo(ConnectionDetails.Database).ReadCsv(hasHeaderRow, ",");
            fields.ForEach(field => table.Columns.Add(field.Name));
            table.ToCsv(ConnectionDetails.Database, true);
            return true;
        }

        protected override void CreateTable(string tableName, string pkColumnName, string pkDataType, bool pkIsIdentity)
        {
            throw new NotSupportedException();
        }

        public override IEnumerable<string> GetFieldNames(string tableName)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            DataTable table = new FileInfo(ConnectionDetails.Database).ReadCsv(hasHeaderRow, ",");
            return table.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
        }

        public override FieldCollection GetFields(string tableName)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            DataTable table = new FileInfo(ConnectionDetails.Database).ReadCsv(hasHeaderRow, ",");
            FieldCollection fields = new FieldCollection();
            table.Columns.Cast<DataColumn>().ForEach(c => fields.Add(new Field
            {
                Name = c.ColumnName,
                DisplayName = c.ColumnName,
                IsRequired = !c.AllowDBNull,
                MaxLength = c.ColumnLength(),
                Ordinal = c.Ordinal,
                Type = FieldType.String
            }));
            return fields;
        }

        public override int GetRecordCount(string tableName)
        {
            int rowCount = new FileInfo(ConnectionDetails.ConnectionString).GetText().ToLines().Count();
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            return hasHeaderRow ? rowCount - 1 : rowCount;
        }

        public override IEnumerator<Record> GetRecordsEnumerator(string tableName, IEnumerable<Field> fields)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            DataTable table = new FileInfo(ConnectionDetails.Database).ReadCsv(hasHeaderRow, ",");
            foreach (DataRow row in table.Rows)
            {
                Record record = new Record();
                record.Fields.AddRange(fields);
                fields.ForEach(f =>
                {
                    record[f.Name].Value = row.Field<string>(f.Name);
                });
                yield return record;
            }
        }

        public override void InsertRecords(string tableName, IEnumerable<Record> records)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            DataTable table = new FileInfo(ConnectionDetails.Database).ReadCsv(hasHeaderRow, ",");

            records.ForEach(record =>
                {
                    DataRow row = table.NewRow();
                    record.Fields.ForEach(field =>
                        {
                            row[field.Name] = field.Value.ToString();
                        });
                    table.Rows.Add(row);
                });

            table.ToCsv(ConnectionDetails.Database, true);
        }

        public override IEnumerable<string> TableNames
        {
            get { return new string[] { Path.GetFileNameWithoutExtension(ConnectionDetails.Database) }; }
        }

        public override FieldType GetDataMigratorFieldType(string providerFieldType)
        {
            return FieldType.String;
        }

        public override string GetDataProviderFieldType(FieldType fieldType)
        {
            return typeof(string).ToString();
        }
    }
}