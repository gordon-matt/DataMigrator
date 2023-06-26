namespace DataMigrator.Common
{
    public static class TypeConvert
    {
        private static DbTypeConverter dbTypeConverter = null;
        private static SqlDbTypeConverter sqlDbTypeConverter = null;
        private static SystemTypeConverter systemTypeConverter = null;

        public static DbTypeConverter DbTypeConverter => dbTypeConverter ??= new DbTypeConverter();

        public static SqlDbTypeConverter SqlDbTypeConverter => sqlDbTypeConverter ??= new SqlDbTypeConverter();

        public static SystemTypeConverter SystemTypeConverter => systemTypeConverter ??= new SystemTypeConverter();
    }
}