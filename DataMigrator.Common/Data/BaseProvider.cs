using System.Data;
using System.Data.Common;
using System.Text;
using DataMigrator.Common.Models;
using DataMigrator.Windows.Forms.Diagnostics;
using Extenso;
using Extenso.Data;
using Extenso.Collections;
using Extenso.Data.Common;

namespace DataMigrator.Common.Data;

public abstract class BaseProvider : IProvider
{
    #region Public Properties

    public abstract string DbProviderName { get; }

    protected ConnectionDetails ConnectionDetails { get; set; }

    /// <summary>
    /// Used in T-SQL queries for escaping spaces and reserved words
    /// </summary>
    protected virtual string EscapeIdentifierStart { get; set; } = "[";

    protected virtual string EscapeIdentifierEnd { get; set; } = "]";

    #endregion Public Properties

    protected string EncloseIdentifier(string value) => $"{EscapeIdentifierStart}{value}{EscapeIdentifierEnd}";

    #region Constructor

    public BaseProvider(ConnectionDetails connectionDetails)
    {
        ConnectionDetails = connectionDetails;
    }

    #endregion Constructor

    #region Table Methods

    public virtual IEnumerable<string> TableNames
    {
        get
        {
            using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
            string[] restrictions = new string[4];
            restrictions[3] = "Base Table";

            connection.Open();
            var schema = connection.GetSchema("Tables", restrictions);
            connection.Close();

            var tableNames = new List<string>();
            foreach (DataRow row in schema.Rows)
            {
                string schemaName = row.Field<string>("TABLE_SCHEMA");
                string tableName = row.Field<string>("TABLE_NAME");

                tableNames.Add($"{schemaName}.{tableName}");
            }
            return tableNames;
        }
    }

    public virtual bool CreateTable(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        bool ok = CreateTable(tableName, schemaName);

        if (!ok)
        { return false; }

        foreach (var field in fields)
        {
            CreateField(tableName, schemaName, field);
        }
        return true;
    }

    protected virtual bool CreateTable(string tableName, string schemaName)
    {
        try
        {
            CreateTable(tableName, schemaName, "Id", GetDataProviderFieldType(FieldType.Int32), true);
        }
        catch (DbException x)
        {
            TraceService.Instance.WriteException(x);
            return false;
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteException(x);
            return false;
        }

        return true;
    }

    #endregion Table Methods

    #region Field Methods

    public virtual FieldCollection GetFields(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(Constants.Data.CMD_SELECT_INFO_SCHEMA_COLUMNS, tableName);

        if (!string.IsNullOrEmpty(schemaName))
        {
            command.CommandText = $"{command.CommandText} AND TABLE_SCHEMA = '{schemaName}'";
        }

        var fields = new FieldCollection();

        connection.Open();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var field = new Field
                {
                    Name = reader.GetString(0)
                };
                if (!reader.IsDBNull(1))
                { field.Ordinal = reader.GetInt32(1); }
                if (!reader.IsDBNull(2))
                { field.Type = GetDataMigratorFieldType(reader.GetString(2)); }
                if (!reader.IsDBNull(3))
                { field.IsRequired = reader.GetString(3) == "NO"; }
                if (!reader.IsDBNull(4))
                { field.MaxLength = reader.GetInt32(4); }
                fields.Add(field);
            }
        }
        connection.Close();

        try
        {
            command.CommandText = string.Format(Constants.Data.CMD_IS_PRIMARY_KEY_FORMAT, tableName);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string pkColumn = reader.GetString(0);
                    var match = fields.SingleOrDefault(f => f.Name == pkColumn);
                    if (match != null)
                    {
                        match.IsPrimaryKey = true;
                    }
                }
            }

            connection.Close();
        }
        catch (Exception x)
        {
            TraceService.Instance.WriteConcat(TraceEvent.Error, "Error: Could not get primary key info - ", x.Message);
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        return fields;
    }

    protected virtual bool CreateField(string tableName, string schemaName, Field field)
    {
        var existingFieldNames = GetFieldNames(tableName, schemaName);
        if (existingFieldNames.Contains(field.Name))
        {
            TraceService.Instance.WriteFormat(TraceEvent.Error, "The field, '{0}', already exists in the table, {1}", field.Name, GetFullTableName(tableName, schemaName));
            //throw new ArgumentException("etc");
            return false;
        }

        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();
        string fieldType = GetDataProviderFieldType(field.Type);
        string maxLength = string.Empty;
        if (field.Type.In(FieldType.String, FieldType.RichText, FieldType.Char))
        {
            if (field.MaxLength is > 0 and <= 8000)
            {
                maxLength = $"({field.MaxLength})";
            }
            else
            {
                if (field.Type.In(FieldType.String, FieldType.RichText)) //Not supported for CHAR
                {
                    maxLength = "(MAX)";
                }
            }
        }
        string isRequired = string.Empty;
        if (field.IsRequired)
        { isRequired = " NOT NULL"; }

        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(
            Constants.Data.CMD_ADD_COLUMN,
            GetFullTableName(tableName, schemaName),
            string.Concat(
                EncloseIdentifier(field.Name), " ",
                fieldType,
                maxLength,
                isRequired));
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
        return true;
    }

    protected virtual IEnumerable<string> GetFieldNames(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format(Constants.Data.CMD_SELECT_INFO_SCHEMA_COLUMN_NAMES, tableName);

        if (!string.IsNullOrEmpty(schemaName))
        {
            command.CommandText = $"{command.CommandText} AND TABLE_SCHEMA = '{schemaName}'";
        }

        var columns = new List<string>();

        connection.Open();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                columns.Add(reader.GetString(0));
            }
        }
        connection.Close();
        return columns;
    }

    #endregion Field Methods

    #region Record Methods

    public virtual int GetRecordCount(string tableName, string schemaName)
    {
        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        return connection.ExecuteScalar($"SELECT COUNT(*) FROM {GetFullTableName(tableName, schemaName)}");
    }

    public virtual IEnumerator<Record> GetRecordsEnumerator(string tableName, string schemaName, IEnumerable<Field> fields)
    {
        //Query query = new Query();
        //fields.ForEach(f => { query.Select(f.Name); });
        //query.From(tableName);

        var sb = new StringBuilder();
        sb.Append("SELECT ");
        sb.Append(fields.Select(f => f.Name).Join(
            string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart))
            .Prepend(EscapeIdentifierStart)
            .Append(EscapeIdentifierEnd));
        sb.Append(" FROM ");
        sb.Append(GetFullTableName(tableName, schemaName));

        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = sb.ToString();

        connection.Open();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var record = new Record();
                record.Fields.AddRange(fields);
                fields.ForEach(f =>
                {
                    record[f.Name].Value = reader[f.Name];
                });
                yield return record;
            }
        }
        connection.Close();
    }

    // TODO: See if can improve performance.
    public virtual void InsertRecords(string tableName, string schemaName, IEnumerable<Record> records)
    {
        const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";

        var parameterNames = CreateParameterNames(records.ElementAt(0).Fields.Select(f => f.Name));
        string fieldNames = parameterNames.Keys.Join(",");

        fieldNames = fieldNames
            .Replace(",", string.Concat(EscapeIdentifierEnd, ",", EscapeIdentifierStart)) // "],["
            .Prepend(EscapeIdentifierStart) // "["
            .Append(EscapeIdentifierEnd); // "]"

        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = string.Format(INSERT_INTO_FORMAT, GetFullTableName(tableName, schemaName), fieldNames, parameterNames.Values.Join(","));

                records.ElementAt(0).Fields.ForEach(field =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = parameterNames[field.Name];
                    parameter.DbType = AppContext.DbTypeConverter.GetDataProviderFieldType(field.Type);
                    command.Parameters.Add(parameter);
                });

                records.ForEach(record =>
                {
                    record.Fields.ForEach(field =>
                    {
                        command.Parameters[parameterNames[field.Name]].Value = field.Value;
                    });

                    command.ExecuteNonQuery();
                });
            }
            transaction.Commit();
        }
        connection.Close();
    }

    protected static IDictionary<string, string> CreateParameterNames(IEnumerable<string> fieldNames)
    {
        var parameterNames = new Dictionary<string, string>();
        fieldNames.ForEach(f =>
        {
            string parameterName = f;
            "¬`!\"£$%^&*()-=+{}[]:;@'~#|<>,.?/ ".ToCharArray().ForEach(c => { parameterName = parameterName.Replace(c, '_'); });
            parameterNames.Add(f, parameterName.ToPascalCase().Prepend("@"));
        });
        return parameterNames;
    }

    #endregion Record Methods

    #region Public Static Methods

    protected virtual DbConnection CreateDbConnection(string providerName, string connectionString)
    {
        // Assume failure.
        DbConnection connection = null;

        // Create the DbProviderFactory and DbConnection.
        if (connectionString != null)
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(providerName);

                connection = factory.CreateConnection();
                connection.ConnectionString = connectionString;
            }
            catch (Exception ex)
            {
                // Set the connection to null if it was created.
                if (connection != null)
                {
                    connection = null;
                }
                Console.WriteLine(ex.Message);
            }
        }
        // Return the connection.
        return connection;
    }

    #endregion Public Static Methods

    protected virtual void CreateTable(string tableName, string schemaName, string pkColumnName, string pkDataType, bool pkIsIdentity)
    {
        using var connection = CreateDbConnection(DbProviderName, ConnectionDetails.ConnectionString);

        string commandText =
$@"CREATE TABLE {GetFullTableName(tableName, schemaName)}
(
    {EncloseIdentifier(pkColumnName)} {pkDataType} {(pkIsIdentity ? "IDENTITY(1,1)" : string.Empty)} NOT NULL
        CONSTRAINT {EncloseIdentifier("PK_" + tableName)} PRIMARY KEY
)";

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = commandText;
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    protected virtual string GetFullTableName(string tableName, string schemaName) =>
        !string.IsNullOrEmpty(schemaName)
            ? $"{EncloseIdentifier(schemaName)}.{EncloseIdentifier(tableName)}"
            : EncloseIdentifier(tableName);

    #region Field Conversion Methods

    protected abstract FieldType GetDataMigratorFieldType(string providerFieldType);

    protected abstract string GetDataProviderFieldType(FieldType fieldType);

    #endregion Field Conversion Methods
}