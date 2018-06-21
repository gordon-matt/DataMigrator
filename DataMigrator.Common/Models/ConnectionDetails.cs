using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DataMigrator.Common.Models
{
    public class ConnectionDetails
    {
        [XmlAttribute]
        public string Server { get; set; }

        [XmlAttribute]
        public int Port { get; set; }

        [XmlAttribute]
        public string Database { get; set; }

        [XmlAttribute]
        public string Domain { get; set; }

        [XmlAttribute]
        public string UserName { get; set; }

        [XmlAttribute]
        public string Password { get; set; }

        [XmlAttribute]
        public bool IntegratedSecurity { get; set; }

        [XmlAttribute]
        public string ProviderName { get; set; }

        [XmlAttribute]
        public string ConnectionString { get; set; }

        [XmlArray("ExtendedProperties")]
        [XmlArrayItem("Property")]
        public ExtendedPropertyCollection ExtendedProperties { get; set; }

        public ConnectionDetails()
        {
            ExtendedProperties = new ExtendedPropertyCollection();
        }
    }

    public class ExtendedProperty
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public TValue GetValue<TValue>()
        {
            if (Value != null)
            {
                return (TValue)Value;
            }
            return default(TValue);
        }
    }

    public class ExtendedPropertyCollection : List<ExtendedProperty>
    {
        public ExtendedProperty this[string key]
        {
            get { return this.SingleOrDefault(x => x.Key == key); }
        }
        
        public void Add(string key, object value)
        {
            this.Add(new ExtendedProperty { Key = key, Value = value });
        }
    }
}