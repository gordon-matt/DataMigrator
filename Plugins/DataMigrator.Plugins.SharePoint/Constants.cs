namespace DataMigrator.SharePoint
{
    internal static class Constants
    {
        internal const string PROVIDER_NAME = "SharePoint";

        private static string[] systemFields = null;

        public static string[] SystemFields
        {
            get
            {
                if (systemFields == null)
                {
                    systemFields = new string[]
                    {
                        "Author",
                        "ContentType",
                        "Modified",
                        "Created",
                        "Editor",
                        "_UIVersionString",
                        "Attachments",
                        "Edit",
                        "LinkTitleNoMenu",
                        "LinkTitle",
                        "DocIcon",
                        "ItemChildCount",
                        "FolderChildCount"
                    };
                }
                return systemFields;
            }
        }
    }
}