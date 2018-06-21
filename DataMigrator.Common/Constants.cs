
namespace DataMigrator.Common
{
    internal static class Constants
    {
        internal static class Data
        {
            internal const string CMD_SELECT_INFO_SCHEMA_COLUMNS =
@"SELECT COLUMN_NAME, ORDINAL_POSITION, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}'";
            internal const string CMD_SELECT_INFO_SCHEMA_COLUMN_NAMES =
    @"SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{0}'";

            internal const string CMD_ADD_COLUMN = "ALTER TABLE [{0}] ADD {1}";

            internal const string CMD_IS_PRIMARY_KEY_FORMAT =
    @"SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_NAME), 'IsPrimaryKey') = 1
AND TABLE_NAME = '{0}'";
        }
    }
}
