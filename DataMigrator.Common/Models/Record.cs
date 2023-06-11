using System;
using System.Collections.Generic;
using System.Linq;
using Extenso;

namespace DataMigrator.Common.Models
{
    public class Record : ICloneable
    {
        public FieldCollection Fields { get; set; }

        public Field this[string name] => Fields.SingleOrDefault(f => f.Name == name);

        public Record()
        {
            Fields = new FieldCollection();
        }

        #region ICloneable Members

        public Record Clone()
        {
            return new Record
            {
                Fields = new FieldCollection(this.Fields)
            };
        }

        object ICloneable.Clone() => Clone();

        #endregion ICloneable Members

        public void ReMapFields(IEnumerable<FieldMapping> mappings)
        {
            foreach (var mapping in mappings)
            {
                var field = this[mapping.SourceField.Name];
                if (field != null)
                {
                    field.Name = mapping.DestinationField.Name;
                }
            }
        }

        public void ReMapFieldTypes(IEnumerable<FieldMapping> mappings)
        {
            foreach (var mapping in mappings)
            {
                var field = this[mapping.SourceField.Name];
                if (field != null)
                {
                    if (field.Value == null || field.Value == DBNull.Value)
                    { continue; }

                    var newType = AppContext.SystemTypeConverter.GetDataProviderFieldType(mapping.DestinationField.Type);
                    field.Value = field.Value.ConvertTo(newType);
                    field.Type = mapping.DestinationField.Type;
                }
            }
        }

        public override string ToString() => string.Concat("Fields: ", Fields.Count);
    }
}