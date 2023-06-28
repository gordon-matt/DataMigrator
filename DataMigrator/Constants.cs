using System.Drawing.Imaging;

namespace DataMigrator;

internal static class Constants
{
    internal const string SQL_PROVIDER_NAME = "Sql Server";

    internal static class TreeView
    {
        internal const string ROOT_NODE_TEXT = "Data Migrator";

        internal const string CONNECTIONS_NODE_TEXT = "Connections";

        internal const string JOBS_NODE_TEXT = "Jobs";

        internal const string SETTINGS_NODE_TEXT = "Settings";
    }

    internal static class ImageBytes
    {
        internal static readonly byte[] Script;

        internal static readonly byte[] Script_24x24;

        static ImageBytes()
        {
            Script = LoadImage(Resources.CS_Script);
            Script_24x24 = LoadImage(Resources.CS_Script_24x24);
        }

        private static byte[] LoadImage(Image image)
        {
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Bmp);
            return memoryStream.ToArray();
        }
    }
}