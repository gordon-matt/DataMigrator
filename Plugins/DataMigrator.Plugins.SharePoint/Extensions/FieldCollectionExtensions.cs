using Microsoft.SharePoint.Client;

namespace DataMigrator.SharePoint.Extensions
{
    public static class FieldCollectionExtensions
    {
        public static Field Add(this FieldCollection fields, string name, FieldType fieldType, bool addToDefaultView)
        {
            return fields.AddFieldAsXml(
                $"<Field Name='{name}' DisplayName='{name}' Type='{fieldType}' />",
                addToDefaultView,
                AddFieldOptions.DefaultValue);
        }
    }
}