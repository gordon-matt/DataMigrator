using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.Office;
using Kore;
using Kore.Collections;

namespace DataMigrator.Excel
{
    public class ExcelProvider : BaseProvider
    {
        private ExcelDbTypeConverter typeConverter = new ExcelDbTypeConverter();

        public override string DbProviderName
        {
            get { return "System.Data.OleDb"; }
            //get { throw new NotSupportedException(); }
        }

        #region Constructor

        public ExcelProvider(ConnectionDetails connectionDetails)
            : base(connectionDetails)
        {
        }

        #endregion

        #region OleDb Style (Many problems)

        //public override IEnumerable<string> TableNames
        //{
        //    get
        //    {
        //        using (OleDbConnection connection = new OleDbConnection(ConnectionDetails.ConnectionString))
        //        {
        //            string[] restrictions = new string[4];
        //            restrictions[3] = "Table";

        //            connection.Open();
        //            DataTable schema = connection.GetSchema("Tables", restrictions);
        //            connection.Close();

        //            List<string> tableNames = new List<string>();
        //            foreach (DataRow row in schema.Rows)
        //            {
        //                tableNames.Add(row.Field<string>("TABLE_NAME").Replace("$", string.Empty));
        //            }
        //            return tableNames;
        //        }
        //    }
        //}
        //public override bool CreateTable(string tableName)
        //{
        //    return CreateTable(tableName, null);
        //}
        //public override bool CreateTable(string tableName, IEnumerable<Field> fields)
        //{
        //    try
        //    {
        //        string commandText = null;

        //        if (!fields.IsNullOrEmpty())
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            fields.ForEach(field =>
        //                {
        //                    if (field.Type == FieldType.String)
        //                    {
        //                        if (field.MaxLength > 255 || field.MaxLength.In(0, -1))
        //                        {
        //                            sb.AppendFormat("{0} LongText,", field.Name);
        //                        }
        //                        else { sb.AppendFormat("{0} VarChar({1}),", field.Name, field.MaxLength); }
        //                    }
        //                    else
        //                    {
        //                        sb.AppendFormat("{0} {1},", field.Name, GetDataProviderFieldType(field.Type));
        //                    }
        //                });
        //            sb.Length -= 1;
        //            commandText = string.Format("CREATE TABLE {0}({1})", tableName, sb.ToString());
        //        }
        //        else
        //        {
        //            commandText = string.Concat("CREATE TABLE ", tableName);
        //        }

        //        using (OleDbConnection connection = new OleDbConnection(ConnectionDetails.ConnectionString))
        //        {
        //            using (OleDbCommand command = connection.CreateCommand())
        //            {
        //                command.CommandType = CommandType.Text;
        //                command.CommandText = commandText;
        //                connection.Open();
        //                command.ExecuteNonQuery();
        //                connection.Close();
        //            }
        //        }
        //    }
        //    catch (OleDbException x)
        //    {
        //        TraceService.Instance.WriteException(x);
        //        return false;
        //    }
        //    catch (Exception x)
        //    {
        //        TraceService.Instance.WriteException(x);
        //        return false;
        //    }
        //    return true;
        //}

        //public override IEnumerable<string> GetFieldNames(string tableName)
        //{
        //    using (OleDbConnection connection = new OleDbConnection(ConnectionDetails.ConnectionString))
        //    {
        //        var columnInfo = connection.GetColumnData(tableName + "$");
        //        return columnInfo.Select(c => c.ColumnName);
        //    }
        //}
        //public override FieldCollection GetFields(string tableName)
        //{
        //    using (OleDbConnection connection = new OleDbConnection(ConnectionDetails.ConnectionString))
        //    {
        //        var columnInfo = connection.GetColumnData(tableName + "$");
        //        FieldCollection fields = new FieldCollection();

        //        columnInfo.ForEach(c =>
        //        {
        //            Field field = new Field
        //            {
        //                DisplayName = c.ColumnName,
        //                IsPrimaryKey = c.KeyType == KeyType.PrimaryKey,
        //                IsRequired = !c.IsNullable,
        //                MaxLength = (int)c.MaximumLength,
        //                Name = c.ColumnName,
        //                Ordinal = (int)c.OrdinalPosition,
        //                Type = AppContext.SystemTypeConverter.GetDataMigratorFieldType(c.DataType)
        //            };
        //            fields.Add(field);
        //        });

        //        return fields;
        //    }
        //}
        //public override bool CreateField(string tableName, Field field)
        //{
        //    throw new NotSupportedException();
        //}

        //protected override void CreateTable(string tableName, string pkColumnName, string pkDataType, bool pkIsIdentity)
        //{
        //    throw new NotSupportedException();
        //}

        //public override int GetRecordCount(string tableName)
        //{
        //    tableName = tableName.Append("$");
        //    return base.GetRecordCount(tableName);
        //}

        //#region Field Conversion Methods

        //public override FieldType GetDataMigratorFieldType(string providerFieldType)
        //{
        //    return typeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<ExcelType>(providerFieldType, true));
        //}
        //public override string GetDataProviderFieldType(FieldType fieldType)
        //{
        //    return typeConverter.GetDataProviderFieldType(fieldType).ToString();
        //}

        //#endregion

        #endregion

        #region OpenXml Style

        #region Table Methods

        public override IEnumerable<string> TableNames
        {
            get
            {
                using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
                {
                    return excel.GetSheetNames();
                }
            }
        }

        public override bool CreateTable(string tableName)
        {
            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                excel.AddSheet(tableName);
            }
            return true;
        }

        public override bool CreateTable(string tableName, IEnumerable<Field> fields)
        {
            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                excel.AddSheet(tableName);
                int columnIndex = 0;
                foreach (Field field in fields)
                {
                    excel.InsertText(tableName, field.Name, 1, ++columnIndex);
                }
                excel.Save();
            }
            return true;
        }

        //protected override void CreateTable(string tableName, string pkColumnName, string pkDataType, bool pkIsIdentity)
        //{
        //    throw new NotSupportedException();
        //}

        #endregion

        #region Field Methods

        public override bool CreateField(string tableName, Field field)
        {
            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                int columnCount = excel.GetColumnCount(tableName);
                excel.InsertText(tableName, field.Name, 1, columnCount + 1);
            }
            return true;
        }

        public override IEnumerable<string> GetFieldNames(string tableName)
        {
            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                return excel.ReadSheet(tableName).Columns.Cast<DataColumn>().Select(x => x.ColumnName);
            }
        }

        public override FieldCollection GetFields(string tableName)
        {
            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                FieldCollection fields = new FieldCollection();
                foreach (DataColumn column in excel.ReadSheet(tableName, true).Columns)
                {
                    Field field = new Field
                    {
                        DisplayName = column.ColumnName,
                        IsRequired = !column.AllowDBNull,
                        MaxLength = column.MaxLength,
                        Name = column.ColumnName,
                        Ordinal = column.Ordinal,
                        Type = AppContext.SystemTypeConverter.GetDataMigratorFieldType(column.DataType)
                    };
                    fields.Add(field);
                }
                return fields;
            }
        }

        #endregion

        #region Record Methods

        public override int GetRecordCount(string tableName)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                int rowCount = excel.GetRowCount(tableName);
                return hasHeaderRow ? rowCount - 1 : rowCount;
            }
        }

        public override IEnumerator<Record> GetRecordsEnumerator(string tableName, IEnumerable<Field> fields)
        {
            bool hasHeaderRow = ConnectionDetails.ExtendedProperties["HasHeaderRow"].GetValue<bool>();
            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                foreach (DataRow row in excel.ReadSheet(tableName, hasHeaderRow).Rows)
                {
                    Record record = new Record();
                    record.Fields.AddRange(fields);
                    fields.ForEach(f =>
                    {
                        record[f.Name].Value = row[f.Name];
                    });
                    yield return record;
                }
            }
        }

        public override void InsertRecords(string tableName, IEnumerable<Record> records)
        {
            RecordCollection collection = new RecordCollection();
            collection.AddRange(records);

            using (ExcelOpenXmlDocument excel = ExcelOpenXmlDocument.Load(ConnectionDetails.Database))
            {
                DataTable data = collection.ToDataTable();
                excel.Import(data, tableName, true, (uint)(excel.GetRowCount(tableName) + 1));
            }
        }

        #endregion

        #region Field Conversion Methods

        public override FieldType GetDataMigratorFieldType(string providerFieldType)
        {
            return typeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<ExcelType>(providerFieldType, true));
        }

        public override string GetDataProviderFieldType(FieldType fieldType)
        {
            return typeConverter.GetDataProviderFieldType(fieldType).ToString();
        }

        #endregion

        #endregion
    }
}