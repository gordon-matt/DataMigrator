using System.Data;
using Extenso.Collections;

namespace DataMigrator.Common.Models
{
    public static class Extensions
    {
        public static DataTable ToDataTable(this IEnumerable<Record> records)
        {
            var table = new DataTable();

            if (records.IsNullOrEmpty())
            {
                return table;
            }

            var record1 = records.First();
            record1.Fields.ForEach(field =>
            {
                table.Columns.Add(
                    field.Name,
                    AppContext.SystemTypeConverter.GetDataProviderFieldType(field.Type));
            });

            records.ForEach(record =>
            {
                var row = table.NewRow();
                record.Fields.ForEach(field =>
                {
                    row[field.Name] = field.Value;
                });
                table.Rows.Add(row);
            });

            return table;
        }
    }
}