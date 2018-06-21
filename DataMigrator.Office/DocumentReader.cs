using System.IO;

namespace DataMigrator.Office
{
    public class DocumentReader : AbstractReader
    {
        //Private constructor 
        private DocumentReader()
        {
        }

        /// <summary>
        /// Returns a new spreadsheet document as a stream from the blank spreadsheet template.
        ///</summary>
        public static MemoryStream Create()
        {
            return GetEmbeddedResourceStream("Templates\\blank.docx");
        }
    }
}