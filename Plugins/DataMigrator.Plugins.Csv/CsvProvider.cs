using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using Extenso;
using Extenso.Collections;
using Extenso.Data;
using Extenso.IO;

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
            var table = ReadCsv();
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
            var table = ReadCsv();
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
            var table = ReadCsv();
            return table.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
        }

        public override FieldCollection GetFields(string tableName)
        {
            var table = ReadCsv();
            var fields = new FieldCollection();

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
            int rowCount = new FileInfo(ConnectionDetails.ConnectionString).ReadAllText().ToLines().Count();
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            return hasHeaderRow ? rowCount - 1 : rowCount;
        }

        public override IEnumerator<Record> GetRecordsEnumerator(string tableName, IEnumerable<Field> fields)
        {
            var table = ReadCsv();
            foreach (DataRow row in table.Rows)
            {
                var record = new Record();
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
            var table = ReadCsv();

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

        private DataTable ReadCsv()
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            using var fileStream = File.OpenRead(ConnectionDetails.Database);
            using var streamReader = new StreamReader(fileStream);
            using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                HasHeaderRecord = hasHeaderRow
            });
            using var csvDataReader = new CsvDataReader(csvReader);

            var table = new DataTable();
            table.Load(csvDataReader);
            return table;
        }
    }
}