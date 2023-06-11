using DataMigrator.Common.Data;

namespace DataMigrator.Common;

public static class AppContext
{
    private static DbTypeConverter dbTypeConverter = null;

    public static DbTypeConverter DbTypeConverter
    {
        get
        {
            if (dbTypeConverter == null)
            {
                dbTypeConverter = new DbTypeConverter();
            }
            return dbTypeConverter;
        }
    }

    private static SqlDbTypeConverter sqlDbTypeConverter = null;

    public static SqlDbTypeConverter SqlDbTypeConverter
    {
        get
        {
            if (sqlDbTypeConverter == null)
            {
                sqlDbTypeConverter = new SqlDbTypeConverter();
            }
            return sqlDbTypeConverter;
        }
    }

    private static SystemTypeConverter systemTypeConverter = null;

    public static SystemTypeConverter SystemTypeConverter
    {
        get
        {
            if (systemTypeConverter == null)
            {
                systemTypeConverter = new SystemTypeConverter();
            }
            return systemTypeConverter;
        }
    }
}