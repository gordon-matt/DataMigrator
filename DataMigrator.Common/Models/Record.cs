using System;
using System.Collections.Generic;
using System.Linq;
using Kore;

namespace DataMigrator.Common.Models
{
    public class Record : ICloneable
    {
        public FieldCollection Fields { get; set; }

        public Field this[string name]
        {
            get { return Fields.SingleOrDefault(f => f.Name == name); }
        }

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

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        public void ReMapFields(IEnumerable<FieldMapping> mappings)
        {
            foreach (FieldMapping mapping in mappings)
            {
                Field field = this[mapping.SourceField.Name];
                if (field != null)
                {
                    field.Name = mapping.DestinationField.Name;
                }
            }
        }

        public void ReMapFieldTypes(IEnumerable<FieldMapping> mappings)
        {
            foreach (FieldMapping mapping in mappings)
            {
                Field field = this[mapping.SourceField.Name];
                if (field != null)
                {
                    if (field.Value == null || field.Value == DBNull.Value)
                    { continue; }

                    Type newType = AppContext.SystemTypeConverter.GetDataProviderFieldType(mapping.DestinationField.Type);
                    field.Value = field.Value.ConvertTo(newType);
                    field.Type = mapping.DestinationField.Type;
                }
            }
        }

        public override string ToString()
        {
            return string.Concat("Fields: ", Fields.Count);
        }
    }
}