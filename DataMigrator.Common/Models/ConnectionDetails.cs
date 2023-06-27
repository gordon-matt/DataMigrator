namespace DataMigrator.Common.Models;

public class ConnectionDetails
{
    public string Server { get; set; }

    public int Port { get; set; }

    public string Database { get; set; }

    public string Domain { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool IntegratedSecurity { get; set; }

    public string ProviderName { get; set; }

    public string ConnectionString { get; set; }

    public ExtendedPropertyCollection ExtendedProperties { get; set; } = new ExtendedPropertyCollection();
}

public class ExtendedProperty
{
    public string Key { get; set; }

    public object Value { get; set; }

    public TValue GetValue<TValue>()
    {
        return Value != null ? Value.ConvertTo<TValue>() : default;
    }
}

public class ExtendedPropertyCollection : List<ExtendedProperty>
{
    public ExtendedProperty this[string key] => this.SingleOrDefault(x => x.Key == key);

    public void Add(string key, object value)
    {
        this.Add(new ExtendedProperty { Key = key, Value = value });
    }
}