using Microsoft.SharePoint.Client;

namespace DataMigrator.SharePoint.Extensions
{
    public static class FieldCollectionExtensions
    {
        public static Field Add(this FieldCollection fields, string name, FieldType fieldType, bool addToDefaultView)
        {
            return fields.AddFieldAsXml(
                string.Format("<Field Name='{0}' DisplayName='{0}' Type='{1}' />", name, fieldType),
                addToDefaultView,
                AddFieldOptions.DefaultValue);
        }
    }
}