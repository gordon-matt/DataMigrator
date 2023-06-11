using System.Net;
using System.Text;
using DataMigrator.Common.Data;
using DataMigrator.Common.Models;
using DataMigrator.SharePoint.Extensions;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso;
using Extenso.Collections;
using SP = Microsoft.SharePoint.Client;

namespace DataMigrator.SharePoint;

//TODO: Test! This class not yet tested.
public class SharePointProvider : BaseProvider
{
    private SPFieldTypeConverter typeConverter = new SPFieldTypeConverter();

    public override string DbProviderName => throw new NotSupportedException();

    public SharePointProvider(ConnectionDetails connectionDetails)
        : base(connectionDetails)
    {
    }

    internal static SP.ClientContext GetClientContext(ConnectionDetails connectionDetails)
    {
        var context = new SP.ClientContext(connectionDetails.ConnectionString);
        if (connectionDetails.IntegratedSecurity)
        {
            context.Credentials = CredentialCache.DefaultNetworkCredentials;
        }
        else
        {
            context.Credentials = new NetworkCredential(
                connectionDetails.UserName,
                connectionDetails.Password,
                connectionDetails.Domain);
        }
        return context;
    }

    public override IEnumerable<string> TableNames
    {
        get
        {
            var listNames = new List<string>();
            using (var context = GetClientContext(ConnectionDetails))
            {
                var site = context.Web;
                var lists = context.LoadQuery(site.Lists);
                context.ExecuteQuery();

                lists.ForEach(list =>
                {
                    listNames.Add(list.Title);
                });
            }
            return listNames;
        }
    }

    public override bool CreateTable(string tableName) => CreateTable(tableName, null);

    public override bool CreateTable(string tableName, IEnumerable<Field> fields)
    {
        try
        {
            using (var context = GetClientContext(ConnectionDetails))
            {
                var site = context.Web;

                // Create a list.
                var listCreationInfo = new SP.ListCreationInformation();
                listCreationInfo.Title = tableName;
                listCreationInfo.TemplateType = (int)SP.ListTemplateType.GenericList;
                listCreationInfo.QuickLaunchOption = SP.QuickLaunchOptions.On;
                var list = site.Lists.Add(listCreationInfo);
                list.Update();
                context.ExecuteQuery();

                if (!fields.IsNullOrEmpty())
                {
                    fields.ForEach(field =>
                    {
                        CreateField(tableName, field);
                    });
                }

                return true;
            }
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteException(x);
            return false;
        }
    }

    public override bool CreateField(string tableName, Field field)
    {
        var existingFieldNames = GetFieldNames(tableName);
        if (existingFieldNames.Contains(field.Name))
        {
            TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the list, {1}", field.Name, tableName);
            //throw new ArgumentException("etc");
            return false;
        }

        try
        {
            using var context = GetClientContext(ConnectionDetails);
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(tableName);

            var spField = list.Fields.Add(field.Name, typeConverter.GetDataProviderFieldType(field.Type), true);
            list.Update();
            context.ExecuteQuery();
            return true;
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteException(x);
            return false;
        }
    }

    public override IEnumerable<string> GetFieldNames(string tableName)
    {
        var spFieldNames = new List<string>();
        using (var context = GetClientContext(ConnectionDetails))
        {
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(tableName);
            var fields = context.LoadQuery(list.Fields);
            context.ExecuteQuery();

            fields.ForEach(spField =>
            {
                if (!spField.Hidden && !spField.InternalName.In(Constants.SystemFields))
                {
                    spFieldNames.Add(spField.InternalName);
                }
            });
        }
        return spFieldNames;
    }

    public override FieldCollection GetFields(string tableName)
    {
        var fields = new FieldCollection();
        using (var context = GetClientContext(ConnectionDetails))
        {
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(tableName);
            var spFields = context.LoadQuery(list.Fields);
            context.ExecuteQuery();

            spFields.ForEach(spField =>
            {
                if (!spField.Hidden && !spField.InternalName.In(Constants.SystemFields))
                {
                    var field = new Field
                    {
                        DisplayName = spField.InternalName,
                        IsPrimaryKey = false,
                        IsRequired = spField.Required,
                        Name = spField.InternalName
                    };

                    field.Type = typeConverter.GetDataMigratorFieldType(spField.FieldTypeKind);

                    if (spField.FieldTypeKind == SP.FieldType.Text)
                    {
                        field.MaxLength = ((SP.FieldText)spField).MaxLength;
                    }

                    fields.Add(field);
                }
            });
        }
        return fields;
    }

    public override int GetRecordCount(string tableName)
    {
        using var context = GetClientContext(ConnectionDetails);
        var site = context.Web;
        var list = context.Web.Lists.GetByTitle(tableName);
        var camlQuery = new SP.CamlQuery();
        // retrieve only one field, to make the query as small and quick as possible.
        camlQuery.ViewXml = "<View><ViewFields><FieldRef Name='Title'/></ViewFields></View>";
        var listItems = list.GetItems(camlQuery);
        context.Load(listItems);
        context.ExecuteQuery();
        return listItems.Count;
    }

    public override IEnumerator<Record> GetRecordsEnumerator(string tableName)
    {
        return GetRecordsEnumeratorInternal(tableName, GetFields(tableName));
    }

    public override IEnumerator<Record> GetRecordsEnumerator(string tableName, IEnumerable<Field> fields)
    {
        return GetRecordsEnumeratorInternal(tableName, fields);
    }

    public override void InsertRecords(string tableName, IEnumerable<Record> records)
    {
        //ProcessBatchData not available in Client OM. Maybe can use custom solution similar to base class ADO.NET version
        //But first need to test - maybe this is already fast enough
        using (var context = GetClientContext(ConnectionDetails))
        {
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(tableName);
            var itemCreateInfo = new SP.ListItemCreationInformation();

            records.ForEach(record =>
            {
                var listItem = list.AddItem(itemCreateInfo);

                record.Fields.ForEach(field =>
                {
                    if (field.Type == FieldType.DateTime)
                    {
                        listItem[field.Name] = field.GetValue<DateTime>().ToISO8601DateString();
                    }
                    else if (field.Type == FieldType.String)
                    {
                        string value = field.Value.ToString();
                        if (value.Length > 255)
                        {
                            value = value.Substring(0, 255);
                        }
                        listItem[field.Name] = field.Value.ToString();
                    }
                    else if (field.Type == FieldType.RichText)
                    {
                        listItem[field.Name] = field.Value.ToString();
                    }
                    else
                    {
                        listItem[field.Name] = field.Value;
                    }
                });

                listItem.Update();
            });
            context.ExecuteQuery();
        }
    }

    private IEnumerator<Record> GetRecordsEnumeratorInternal(string tableName, IEnumerable<Field> fields)
    {
        using (var context = GetClientContext(ConnectionDetails))
        {
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(tableName);
            var spFields = context.LoadQuery(list.Fields);

            const string FIELD_REF_FORMAT = "<FieldRef Name='{0}'/>";

            var sb = new StringBuilder(200);
            foreach (var field in fields)
            {
                sb.AppendFormat(FIELD_REF_FORMAT, field.Name);
            }

            var camlQuery = new SP.CamlQuery();
            camlQuery.ViewXml = string.Format("<View><ViewFields>{0}</ViewFields></View>", sb.ToString());

            var listItems = list.GetItems(camlQuery);
            context.Load(listItems);
            context.ExecuteQuery();

            foreach (var item in listItems)
            {
                var record = new Record();
                record.Fields.AddRange(fields);

                foreach (var field in fields)
                {
                    var spField = spFields.SingleOrDefault(f => f.InternalName == field.Name);
                    if (spField != null)
                    {
                        field.Value = item[spField.InternalName];
                    }
                }

                yield return record;
            }
        }
    }

    public override FieldType GetDataMigratorFieldType(string providerFieldType)
    {
        return typeConverter.GetDataMigratorFieldType(EnumExtensions.ToEnum<SP.FieldType>(providerFieldType, true));
    }

    public override string GetDataProviderFieldType(FieldType fieldType)
    {
        return typeConverter.GetDataProviderFieldType(fieldType).ToString();
    }
}