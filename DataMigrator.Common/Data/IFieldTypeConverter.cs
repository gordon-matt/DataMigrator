using DataMigrator.Common.Models;

namespace DataMigrator.Common.Data
{
    public interface IFieldTypeConverter<T>
    {
        FieldType GetDataMigratorFieldType(T providerFieldType);

        T GetDataProviderFieldType(FieldType fieldType);
    }
}