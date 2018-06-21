using System.Collections.Generic;

namespace DataMigrator.Office
{
    public interface IWordDocument
    {
        void Replace(string find, string replaceWith);
        void Replace(IDictionary<string, string> replacements);
        void Save();
    }
}
