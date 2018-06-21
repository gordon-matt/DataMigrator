using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataMigrator.Common.Models
{
    public class RecordCollection : List<Record>
    {
        public void ReMapFields(IEnumerable<FieldMapping> mappings)
        {
            // Get Fields To Remap (Quicker than passing all items in job.FieldMappings)
            var fieldToReMap = mappings.Where(x => !x.DestinationField.Name.Equals(
                x.SourceField.Name,
                StringComparison.InvariantCultureIgnoreCase));

            var fieldTypesToReMap = mappings.Where(x => x.DestinationField.Type != x.SourceField.Type);

            foreach (Record record in this)
            {
                record.ReMapFieldTypes(fieldTypesToReMap);
                record.ReMapFields(fieldToReMap);
            }
        }

        public DataTable ToDataTable()
        {
            DataTable table = new DataTable();

            if (this.Count == 0)
            {
                return table;
            }

            Record record1 = this[0];
            record1.Fields.ForEach(field =>
                {
                    table.Columns.Add(
                        field.Name,
                        AppContext.SystemTypeConverter.GetDataProviderFieldType(field.Type));
                });

            this.ForEach(record =>
            {
                DataRow row = table.NewRow();
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