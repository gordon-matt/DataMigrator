using DataMigrator.Common.Data;

namespace DataMigrator.Common;

public static class AppContext
{
    private static DbTypeConverter dbTypeConverter = null;

    private static SqlDbTypeConverter sqlDbTypeConverter = null;

    private static SystemTypeConverter systemTypeConverter = null;

    public static DbTypeConverter DbTypeConverter
    {
        get
        {
            dbTypeConverter ??= new DbTypeConverter();
            return dbTypeConverter;
        }
    }

    public static SqlDbTypeConverter SqlDbTypeConverter
    {
        get
        {
            sqlDbTypeConverter ??= new SqlDbTypeConverter();
            return sqlDbTypeConverter;
        }
    }

    public static SystemTypeConverter SystemTypeConverter
    {
        get
        {
            systemTypeConverter ??= new SystemTypeConverter();
            return systemTypeConverter;
        }
    }
}