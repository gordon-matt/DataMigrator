using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Kore.Collections;

namespace DataMigrator.Office
{
    public class WordOpenXmlDocument : IWordDocument, IDisposable
    {
        #region Non-Public Members

        private string documentText = string.Empty;
        private string documentXml = string.Empty;

        private static XNamespace XMLNS { get { return XNamespace.Get(WORDMLNS); } }
        private const string WORDMLNS = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        #endregion Non-Public Members

        #region Public Properties

        public WordprocessingDocument Document { get; private set; }

        public string DocumentText
        {
            get
            {
                if (string.IsNullOrEmpty(documentText))
                {
                    documentText = Document.MainDocumentPart.Document.InnerText;
                }
                return documentText;
            }
        }

        //public string DocumentText
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(documentText))
        //        {
        //            using (StreamReader sr = new StreamReader(Document.MainDocumentPart.GetStream()))
        //            {
        //                documentText = sr.ReadToEnd();
        //            }
        //        }
        //        return documentText;
        //    }
        //    private set { documentText = value; }
        //}

        public string DocumentXml
        {
            get
            {
                if (string.IsNullOrEmpty(documentXml))
                {
                    using (var sr = new StreamReader(Document.MainDocumentPart.GetStream()))
                    {
                        documentXml = sr.ReadToEnd();
                    }
                }
                return documentXml;
            }
            private set { documentXml = value; }
        }

        public Body Body
        {
            get { return Document.MainDocumentPart.Document.Body; }
            set { Document.MainDocumentPart.Document.Body = value; }
        }

        #endregion Public Properties

        #region Constructors

        private WordOpenXmlDocument()
        {
        }

        #endregion Constructors

        #region Public Methods

        public static WordOpenXmlDocument Create(string filePath)
        {
            var word = new WordOpenXmlDocument();
            word.Document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);

            var mainDocumentPart = word.Document.AddMainDocumentPart();

            const string docXml =
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<w:document xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
   <w:body>
   </w:body>
</w:document>";

            using (var stream = mainDocumentPart.GetStream())
            {
                byte[] buffer = (new UTF8Encoding()).GetBytes(docXml);
                stream.Write(buffer, 0, buffer.Length);
            }

            return word;
        }

        public static WordOpenXmlDocument Create(string filePath, string templateFilePath)
        {
            if (!File.Exists(templateFilePath))
            {
                throw new FileNotFoundException("Could not find specified Word template.");
            }

            File.Copy(templateFilePath, filePath);

            return new WordOpenXmlDocument
            {
                Document = WordprocessingDocument.Open(filePath, true)
            };
        }

        public static WordOpenXmlDocument Create(Stream stream)
        {
            var word = new WordOpenXmlDocument();
            word.Document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

            var mainDocumentPart = word.Document.AddMainDocumentPart();

            const string docXml =
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<w:document xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
   <w:body>
   </w:body>
</w:document>";

            using (var stream2 = mainDocumentPart.GetStream())
            {
                byte[] buffer = (new UTF8Encoding()).GetBytes(docXml);
                stream2.Write(buffer, 0, buffer.Length);
            }

            return word;
        }

        public static WordOpenXmlDocument Load(string filePath)
        {
            return new WordOpenXmlDocument
            {
                Document = WordprocessingDocument.Open(filePath, true)
            };
        }

        public static WordOpenXmlDocument Load(Stream stream)
        {
            return new WordOpenXmlDocument
            {
                Document = WordprocessingDocument.Open(stream, true)
            };
        }

        /// <summary>
        /// Creates a new word document from the specified template and then performas a mail merge.
        /// </summary>
        /// <param name="templateFilePath">The mail merge template document to use</param>
        /// <param name="outputFilePath">The name of the new file.</param>
        /// <param name="data">The data to use for the mail merge.</param>
        /// <param name="mergeType">Specifies whether to merge to a single document or multiple documents</param>
        public static void MailMerge(string templateFilePath, string outputFilePath, DataTable data, MailMergeType mergeType)
        {
            var mappings = new List<MailMergeMapping>();
            data.Columns.Cast<DataColumn>().ForEach(column =>
            {
                mappings.Add(new MailMergeMapping
                {
                    DataColumnFieldName = column.ColumnName,
                    MailMergeFieldName = column.ColumnName
                });
            });

            MailMerge(templateFilePath, outputFilePath, data, mergeType, mappings);
        }

        /// <summary>
        /// Creates a new word document from the specified template and then performas a mail merge.
        /// </summary>
        /// <param name="templateFilePath">The mail merge template document to use</param>
        /// <param name="outputFilePath">The name of the new file.</param>
        /// <param name="data">The data to use for the mail merge.</param>
        /// <param name="mappings">The mappings to use to match Word MAILMERGE fields with Data Columns in the specified data.</param>
        /// <param name="mergeType">Specifies whether to merge to a single document or multiple documents</param>
        public static void MailMerge(string templateFilePath, string outputFilePath, DataTable data, MailMergeType mergeType, IEnumerable<MailMergeMapping> mappings)
        {
            if (mergeType == MailMergeType.MultiDocument)
            {
                foreach (DataRow row in data.Rows)
                {
                    MailMerge(templateFilePath, GetUniqueFileName(outputFilePath), row, mappings);
                }
            }
            else
            {
                MailMerge(templateFilePath, outputFilePath, data, mappings);
            }
        }

        public static MemoryStream MailMerge(Stream templateStream, DataTable data, IEnumerable<MailMergeMapping> mappings)
        {
            var stream = new MemoryStream();
            templateStream.CopyTo(stream);
            var document = Load(stream);
            DoMailMerge(document, data, mappings);
            stream.Flush();
            return stream;
        }

        public void InsertPageBreak()
        {
            this.Body.AppendChild(
                new Paragraph(
                    new Run(
                        new Break() { Type = BreakValues.Page })));
        }

        #endregion Public Methods

        #region Private Methods

        // NOTE to Developers: Call this method whenever document text changed.
        // For example, add new paragraph or replace text, etc... always call this
        // after finished text changed.
        private void ResetCachedProperties()
        {
            documentText = documentXml = string.Empty;
        }

        private static string GetUniqueFileName(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return fileName;
            }

            string directory = Path.GetDirectoryName(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            int number = 0;
            string newFileName = Path.Combine(
                directory,
                string.Concat(fileNameWithoutExtension, " (", ++number, ")", extension));

            while (File.Exists(newFileName))
            {
                newFileName = Path.Combine(
                    directory,
                    string.Concat(fileNameWithoutExtension, " (", ++number, ")", extension));
            }

            return newFileName;
        }

        /// <summary>
        /// <para>Creates a new word document from the specified template and then performas a mail merge;</para>
        /// <para>keeping all records in the same single document.</para>
        /// </summary>
        /// <param name="templateFilePath">The mail merge template document to use</param>
        /// <param name="outputFilePath">The name of the new file.</param>
        /// <param name="data">The data to use for the mail merge.</param>
        /// <returns>An instance of WordOpenXmlDocument that is the output document</returns>
        private static void MailMerge(string templateFilePath, string outputFilePath, DataTable data)
        {
            var mappings = new List<MailMergeMapping>();
            data.Columns.Cast<DataColumn>().ForEach(column =>
            {
                mappings.Add(new MailMergeMapping
                {
                    DataColumnFieldName = column.ColumnName,
                    MailMergeFieldName = column.ColumnName
                });
            });

            MailMerge(templateFilePath, outputFilePath, data, mappings);
        }

        /// <summary>
        /// <para>Creates a new word document from the specified template and then performas a mail merge;</para>
        /// <para>keeping all records in the same single document.</para>
        /// </summary>
        /// <param name="templateFilePath">The mail merge template document to use</param>
        /// <param name="outputFilePath">The name of the new file.</param>
        /// <param name="data">The data to use for the mail merge.</param>
        /// <param name="mappings">The mappings to use to match Word MAILMERGE fields with Data Columns in the specified data.</param>
        /// <returns>An instance of WordOpenXmlDocument that is the output document</returns>
        private static void MailMerge(string templateFilePath, string outputFilePath, DataTable data, IEnumerable<MailMergeMapping> mappings)
        {
            using (var document = Create(outputFilePath, templateFilePath))
            {
                DoMailMerge(document, data, mappings);
            }
        }

        /// <summary>
        /// Creates a single document from the specified document template and performs a mail merge with one row of data
        /// </summary>
        /// <param name="templateFilePath">The mail merge template document to use</param>
        /// <param name="outputFilePath">The name of the new file.</param>
        /// <param name="row">The DataRow to use for the mail merge.</param>
        /// <returns>An instance of WordOpenXmlDocument that is the output document</returns>
        private static void MailMerge(string templateFilePath, string outputFilePath, DataRow row)
        {
            var mappings = new List<MailMergeMapping>();
            row.Table.Columns.Cast<DataColumn>().ForEach(column => mappings.Add(new MailMergeMapping
            {
                DataColumnFieldName = column.ColumnName,
                MailMergeFieldName = column.ColumnName
            }));

            MailMerge(templateFilePath, outputFilePath, row, mappings);
        }

        /// <summary>
        /// Creates a single document from the specified document template and performs a mail merge with one row of data
        /// </summary>
        /// <param name="templateFilePath">The mail merge template document to use</param>
        /// <param name="outputFilePath">The name of the new file.</param>
        /// <param name="row">The DataRow to use for the mail merge.</param>
        /// <param name="mappings">The mappings to use to match Word MAILMERGE fields with Data Columns in the specified DataRow.</param>
        /// <returns>An instance of WordOpenXmlDocument that is the output document</returns>
        private static void MailMerge(string templateFilePath, string outputFilePath, DataRow row, IEnumerable<MailMergeMapping> mappings)
        {
            using (var document = Create(outputFilePath, templateFilePath))
            {
                DoMailMerge(document, row, mappings);
            }
        }

        private static void DoMailMerge(WordOpenXmlDocument document, DataTable data, IEnumerable<MailMergeMapping> mappings)
        {
            var bodyElements = document.Body.ChildElements;
            var tempBody = new Body(document.Body.OuterXml);

            //Remove original elements:
            while (bodyElements.Count() > 0)
            {
                bodyElements.ElementAt(0).Remove();
            }

            bool firstIteration = true;

            foreach (DataRow row in data.Rows)
            {
                var newBody = new Body(tempBody.OuterXml);

                var mapValues = new Dictionary<string, string>();
                mappings.ForEach(kv =>
                {
                    if (!mapValues.ContainsKey(kv.MailMergeFieldName))
                    {
                        mapValues.Add(kv.MailMergeFieldName, row.Field<string>(kv.DataColumnFieldName));
                    }
                });

                MailMergeHelper.ProcessBody(document, newBody, null, mapValues);

                if (firstIteration)
                {
                    firstIteration = false;
                }
                else
                { document.InsertPageBreak(); }

                newBody.ChildElements.ForEach(e => document.Body.AppendChild(e.CloneNode(true)));
            }

            document.Save();
        }

        private static void DoMailMerge(WordOpenXmlDocument document, DataRow row, IEnumerable<MailMergeMapping> mappings)
        {
            var bodyElements = document.Body.ChildElements;
            var tempBody = new Body(document.Body.OuterXml);

            //Remove original elements:
            while (bodyElements.Count() > 0)
            {
                bodyElements.ElementAt(0).Remove();
            }

            bool firstIteration = true;

            var newBody = new Body(tempBody.OuterXml);

            var mapValues = new Dictionary<string, string>();
            mappings.ForEach(kv =>
            {
                if (!mapValues.ContainsKey(kv.MailMergeFieldName))
                {
                    mapValues.Add(kv.MailMergeFieldName, row.Field<string>(kv.DataColumnFieldName));
                }
            });

            MailMergeHelper.ProcessBody(document, newBody, null, mapValues);

            if (firstIteration)
            {
                firstIteration = false;
            }
            else
            { document.InsertPageBreak(); }

            newBody.ChildElements.ForEach(e => document.Body.AppendChild(e.CloneNode(true)));

            document.Save();
        }

        #endregion Private Methods

        #region IWordDocument Members

        /// <summary>
        /// Finds the specified text and replaces it with the specified replacement text
        /// </summary>
        /// <param name="find">The text to find.</param>
        /// <param name="replaceWith">The replacement text.</param>
        public void Replace(string find, string replaceWith)
        {
            DocumentXml = new Regex(find).Replace(DocumentXml, replaceWith);

            using (var sw = new StreamWriter(Document.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(DocumentXml);
            }

            ResetCachedProperties();
        }

        /// <summary>
        /// <para>Finds the values from the specified IDictionary&lt;String, String&gt;'s keys collection</para>
        /// <para>in the current word document and replaces them with the values from the values</para>
        /// <para>collection</para>
        /// </summary>
        /// <param name="replacements">The IDictionary&lt;String, String&gt; of replacements</param>
        public void Replace(IDictionary<string, string> replacements)
        {
            foreach (var kv in replacements)
            {
                DocumentXml = new Regex(kv.Key).Replace(DocumentXml, kv.Value);
            }

            using (var sw = new StreamWriter(Document.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(DocumentXml);
            }

            ResetCachedProperties();
        }

        public void Save()
        {
            Document.MainDocumentPart.Document.Save();
        }

        #endregion IWordDocument Members

        #region IDisposable Members

        public void Dispose()
        {
            Document.Dispose();
        }

        #endregion IDisposable Members
    }
}