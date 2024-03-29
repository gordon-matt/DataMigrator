﻿using System.Data.Common;
using System.Net;
using System.Text;
using DataMigrator.Common;
using DataMigrator.Common.Data;
using DataMigrator.Common.Diagnostics;
using DataMigrator.Common.Models;
using DataMigrator.SharePoint.Extensions;
using Extenso;
using Extenso.Collections;
using SP = Microsoft.SharePoint.Client;

namespace DataMigrator.SharePoint;

//TODO: Test! This class not yet tested.
public class SharePointMigrationService : IMigrationService
{
    private readonly SPFieldTypeConverter typeConverter = new();

    public SharePointMigrationService(ConnectionDetails connectionDetails)
    {
        ConnectionDetails = connectionDetails;
    }

    private ConnectionDetails ConnectionDetails { get; set; }

    #region IMigrationService Members

    public DbConnection CreateDbConnection() => null;

    public async Task<IEnumerable<string>> GetTableNamesAsync()
    {
        var listNames = new List<string>();
        using (var context = GetClientContext(ConnectionDetails))
        {
            var site = context.Web;
            var lists = context.LoadQuery(site.Lists);
            await context.ExecuteQueryAsync();

            lists.ForEach(list =>
            {
                listNames.Add(list.Title);
            });
        }
        return listNames;
    }

    public async Task<string> CreateTableAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        try
        {
            using var context = GetClientContext(ConnectionDetails);
            var site = context.Web;

            // Create a list.
            var listCreationInfo = new SP.ListCreationInformation
            {
                Title = GetFullTableName(tableName, schemaName),
                TemplateType = (int)SP.ListTemplateType.GenericList,
                QuickLaunchOption = SP.QuickLaunchOptions.On
            };
            var list = site.Lists.Add(listCreationInfo);
            list.Update();
            await context.ExecuteQueryAsync();

            if (!fields.IsNullOrEmpty())
            {
                fields.ForEach(async field =>
                {
                    await CreateFieldAsync(tableName, schemaName, field);
                });
            }

            return GetFullTableName(tableName, schemaName);
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteException(x);
            return null;
        }
    }

    public async Task<FieldCollection> GetFieldsAsync(string tableName, string schemaName)
    {
        var fields = new FieldCollection();
        using (var context = GetClientContext(ConnectionDetails))
        {
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(GetFullTableName(tableName, schemaName));
            var spFields = context.LoadQuery(list.Fields);
            await context.ExecuteQueryAsync();

            spFields.ForEach(spField =>
            {
                if (!spField.Hidden && !spField.InternalName.In(Constants.SystemFields))
                {
                    var field = new Field
                    {
                        DisplayName = spField.InternalName,
                        IsPrimaryKey = false,
                        IsRequired = spField.Required,
                        Name = spField.InternalName,
                        Type = typeConverter.GetDataMigratorFieldType(spField.FieldTypeKind)
                    };

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

    public int CountRecords(string tableName, string schemaName)
    {
        using var context = GetClientContext(ConnectionDetails);
        var site = context.Web;
        var list = context.Web.Lists.GetByTitle(GetFullTableName(tableName, schemaName));
        var camlQuery = new SP.CamlQuery
        {
            // retrieve only one field, to make the query as small and quick as possible.
            ViewXml = "<View><ViewFields><FieldRef Name='Title'/></ViewFields></View>"
        };
        var listItems = list.GetItems(camlQuery);
        context.Load(listItems);
        context.ExecuteQuery();
        return listItems.Count;
    }

    public async IAsyncEnumerable<Record> GetRecordsAsync(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        using var context = GetClientContext(ConnectionDetails);
        var site = context.Web;
        var list = context.Web.Lists.GetByTitle(GetFullTableName(tableName, schemaName));
        var spFields = context.LoadQuery(list.Fields);

        const string FIELD_REF_FORMAT = "<FieldRef Name='{0}'/>";

        var sb = new StringBuilder(200);
        foreach (var field in fields)
        {
            sb.AppendFormat(FIELD_REF_FORMAT, field.Name);
        }

        var camlQuery = new SP.CamlQuery
        {
            ViewXml = string.Format("<View><ViewFields>{0}</ViewFields></View>", sb.ToString())
        };

        var listItems = list.GetItems(camlQuery);
        context.Load(listItems);
        await context.ExecuteQueryAsync();

        foreach (var item in listItems)
        {
            var record = new Record();
            record.Fields.AddRange(fields);

            foreach (var field in fields)
            {
                var spField = spFields.SingleOrDefault(f => f.InternalName == field.Name);
                if (spField != null)
                {
                    if (spField.FieldTypeKind == SP.FieldType.Text)
                    {
                        string value = item[spField.InternalName].ToString();
                        if (AppState.ConfigFile.TrimStrings)
                        {
                            value = value.Trim();
                        }

                        if (string.IsNullOrEmpty(value))
                        {
                            value = null;
                        }

                        field.Value = value;
                    }
                    else
                    {
                        field.Value = item[spField.InternalName];
                    }
                }
            }

            yield return record;
        }
    }

    public async Task InsertRecordsAsync(DbConnection connection, string tableName, string schemaName, IEnumerable<Record> records)
    {
        //ProcessBatchData not available in Client OM. Maybe can use custom solution similar to base class ADO.NET version
        //But first need to test - maybe this is already fast enough
        using var context = GetClientContext(ConnectionDetails);
        var site = context.Web;
        var list = context.Web.Lists.GetByTitle(GetFullTableName(tableName, schemaName));
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
                        value = value[..255];
                    }
                    listItem[field.Name] = field.Value.ToString();
                }
                else
                {
                    listItem[field.Name] = field.Type == FieldType.RichText ? field.Value.ToString() : field.Value;
                }
            });

            listItem.Update();
        });
        await context.ExecuteQueryAsync();
    }

    #endregion IMigrationService Members

    internal static SP.ClientContext GetClientContext(ConnectionDetails connectionDetails)
    {
        return new SP.ClientContext(connectionDetails.ConnectionString)
        {
            Credentials = connectionDetails.IntegratedSecurity
                ? CredentialCache.DefaultNetworkCredentials
                : (ICredentials)new NetworkCredential(connectionDetails.UserName, connectionDetails.Password, connectionDetails.Domain)
        };
    }

    private async Task<bool> CreateFieldAsync(string tableName, string schemaName, Field field)
    {
        string fullTableName = GetFullTableName(tableName, schemaName);

        var existingFieldNames = await GetFieldNamesAsync(tableName, schemaName);
        if (existingFieldNames.Contains(field.Name))
        {
            TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the list, {1}", field.Name, fullTableName);
            //throw new ArgumentException("etc");
            return false;
        }

        try
        {
            using var context = GetClientContext(ConnectionDetails);
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(fullTableName);

            var spField = list.Fields.Add(field.Name, typeConverter.GetDataProviderFieldType(field.Type), true);
            list.Update();
            await context.ExecuteQueryAsync();
            return true;
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteException(x);
            return false;
        }
    }

    private async Task<IEnumerable<string>> GetFieldNamesAsync(string tableName, string schemaName)
    {
        var spFieldNames = new List<string>();
        using (var context = GetClientContext(ConnectionDetails))
        {
            var site = context.Web;
            var list = context.Web.Lists.GetByTitle(GetFullTableName(tableName, schemaName));
            var fields = context.LoadQuery(list.Fields);
            await context.ExecuteQueryAsync();

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

    private static string GetFullTableName(string tableName, string schemaName) =>
        !string.IsNullOrEmpty(schemaName) ? $"{schemaName}_{tableName}" : tableName;
}