using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Kore;
using Kore.Collections;

namespace DataMigrator.Office
{
    /// <summary>
    /// Helper class for filling in data forms based on Word 2007 documents.
    /// </summary>
    public static class MailMergeHelper
    {
        #region Private Members

        /// <summary>
        /// Regex used to parse MERGEFIELDs in the provided document.
        /// </summary>
        private static readonly Regex rgxInstruction =
            new Regex(
@"# This retrieves the field's name (Named Capture Group -> name)
^[\s]*MERGEFIELD[\s]+(?<name>[#\w]*){1}

# Retrieves field's format flag (Named Capture Group -> Format)
[\s]*(\\\*[\s]+(?<Format>[\w]*){1})?

# Retrieves text to display before field data (Named Capture Group -> PreText)
[\s]*(\\b[\s]+[""]?(?<PreText>[^\\]*){1})?

# Retrieves text to display after field data (Named Capture Group -> PostText)
[\s]*(\\f[\s]+[""]?(?<PostText>[^\\]*){1})?",
                        RegexOptions.Compiled |
                        RegexOptions.CultureInvariant |
                        RegexOptions.ExplicitCapture |
                        RegexOptions.IgnoreCase |
                        RegexOptions.IgnorePatternWhitespace |
                        RegexOptions.Singleline);

        #endregion Private Members

        /// <summary>
        /// Fills in the body element with the provided data.
        /// </summary>
        /// <param name="dataSet">Dataset with the datatables to use to fill the document tables with.  Table names in the dataset should match the table names in the document.</param>
        /// <param name="values">Values to fill the document.  Keys should match the MERGEFIELD names.</param>
        /// <returns>The filled-in document.</returns>
        public static void ProcessBody(WordOpenXmlDocument document, Body body, DataSet dataSet, IDictionary<string, string> values)
        {
            ConvertComplexFieldsToSimpleFields(body);

            #region Process All Tables

            string[] switches = null;
            foreach (var field in body.Descendants<SimpleField>())
            {
                string fieldName = GetFieldName(field, out switches);
                if (!string.IsNullOrEmpty(fieldName) && fieldName.StartsWith("TBL_"))
                {
                    var tableRow = GetFirstParent<TableRow>(field);
                    if (tableRow == null)
                    {
                        // can happen because table contains multiple fields, and after 1 pass, the initial row is already deleted
                        continue;
                    }

                    var table = GetFirstParent<Table>(tableRow);
                    if (table == null)
                    {
                        // can happen because table contains multiple fields, and after 1 pass, the initial row is already deleted
                        continue;
                    }

                    string tableName = GetTableNameFromFieldName(fieldName);

                    if (dataSet == null || !dataSet.Tables.Contains(tableName) || dataSet.Tables[tableName].Rows.Count == 0)
                    {
                        // don't remove table here: will be done in next pass
                        continue;
                    }

                    var dataTable = dataSet.Tables[tableName];

                    var cellPropertiesList = new List<TableCellProperties>();
                    var cellColumnNamesList = new List<string>();
                    var paragraphPropertiesList = new List<string>();
                    var cellFieldsList = new List<SimpleField>();

                    foreach (var tableCell in tableRow.Descendants<TableCell>())
                    {
                        cellPropertiesList.Add(tableCell.GetFirstChild<TableCellProperties>());
                        var paragraph = tableCell.GetFirstChild<Paragraph>();
                        if (paragraph != null)
                        {
                            var pp = paragraph.GetFirstChild<ParagraphProperties>();
                            if (pp != null)
                            {
                                paragraphPropertiesList.Add(pp.OuterXml);
                            }
                            else
                            {
                                paragraphPropertiesList.Add(null);
                            }
                        }
                        else
                        {
                            paragraphPropertiesList.Add(null);
                        }

                        string columnName = string.Empty;
                        SimpleField columnField = null;
                        foreach (var cellField in tableCell.Descendants<SimpleField>())
                        {
                            columnField = cellField;
                            columnName = GetColumnNameFromFieldName(GetFieldName(cellField, out switches));
                            break;  // supports only 1 cellfield per table
                        }

                        cellColumnNamesList.Add(columnName);
                        cellFieldsList.Add(columnField);
                    }

                    // keep reference to row properties
                    var rowProperties = tableRow.GetFirstChild<TableRowProperties>();

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        var row = new TableRow();

                        if (rowProperties != null)
                        {
                            row.Append(new TableRowProperties(rowProperties.OuterXml));
                        }

                        for (int i = 0; i < cellPropertiesList.Count; i++)
                        {
                            var cellProperties = new TableCellProperties(cellPropertiesList[i].OuterXml);
                            var cell = new TableCell();
                            cell.Append(cellProperties);
                            var p = new Paragraph(new ParagraphProperties(paragraphPropertiesList[i]));
                            cell.Append(p);   // cell must contain at minimum a paragraph !

                            if (!string.IsNullOrEmpty(cellColumnNamesList[i]))
                            {
                                if (!dataTable.Columns.Contains(cellColumnNamesList[i]))
                                {
                                    throw new Exception(string.Format("Unable to complete template: column name '{0}' is unknown in parameter tables !", cellColumnNamesList[i]));
                                }

                                if (!dataRow.IsNull(cellColumnNamesList[i]))
                                {
                                    string val = dataRow[cellColumnNamesList[i]].ToString();
                                    p.Append(GetRunElementForText(val, cellFieldsList[i]));
                                }
                            }
                            row.Append(cell);
                        }
                        table.Append(row);
                    }

                    // finally : delete template-row (and thus also the mergefields in the table)
                    tableRow.Remove();
                }
            }

            #endregion Process All Tables

            #region Clean Empty Tables

            foreach (var field in body.Descendants<SimpleField>())
            {
                string fieldName = GetFieldName(field, out switches);
                if (!string.IsNullOrEmpty(fieldName) && fieldName.StartsWith("TBL_"))
                {
                    var tableRow = GetFirstParent<TableRow>(field);
                    if (tableRow == null)
                    {
                        continue;   // can happen: is because table contains multiple fields, and after 1 pass, the initial row is already deleted
                    }

                    var table = GetFirstParent<Table>(tableRow);
                    if (table == null)
                    {
                        continue;   // can happen: is because table contains multiple fields, and after 1 pass, the initial row is already deleted
                    }

                    string tableName = GetTableNameFromFieldName(fieldName);
                    if (dataSet == null || !dataSet.Tables.Contains(tableName) || dataSet.Tables[tableName].Rows.Count == 0)
                    {
                        // if there's a 'dt' switch: delete Word-table
                        if (switches.Contains("dt"))
                        {
                            table.Remove();
                        }
                    }
                }
            }

            #endregion Clean Empty Tables

            #region Process Remaining Fields In Main Document & Save

            FillWordFieldsInElement(values, body);
            document.Save();  // save main document back in package

            #endregion Process Remaining Fields In Main Document & Save
        }

        public static void ProcessHeadersAndFooters(WordOpenXmlDocument document, IDictionary<string, string> values)
        {
            // process header(s)
            foreach (var headerPart in document.Document.MainDocumentPart.HeaderParts)
            {
                //  2010/08/01: addition
                ConvertComplexFieldsToSimpleFields(headerPart.Header);

                FillWordFieldsInElement(values, headerPart.Header);
                headerPart.Header.Save();    // save header back in package
            }

            // process footer(s)
            foreach (var footerPart in document.Document.MainDocumentPart.FooterParts)
            {
                //  2010/08/01: addition
                ConvertComplexFieldsToSimpleFields(footerPart.Footer);

                FillWordFieldsInElement(values, footerPart.Footer);
                footerPart.Footer.Save();    // save footer back in package
            }

            document.Save();
        }

        /// <summary>
        /// Applies any formatting specified to the pre and post text as
        /// well as to fieldValue.
        /// </summary>
        /// <param name="format">The format flag to apply.</param>
        /// <param name="fieldValue">The data value being inserted.</param>
        /// <param name="preText">The text to appear before fieldValue, if any.</param>
        /// <param name="postText">The text to appear after fieldValue, if any.</param>
        /// <returns>The formatted text; [0] = fieldValue, [1] = preText, [2] = postText.</returns>
        /// <exception cref="">Throw if fieldValue, preText, or postText are null.</exception>
        private static string[] ApplyFormatting(string format, string fieldValue, string preText, string postText)
        {
            string[] result = new string[3];

            if ("UPPER".Equals(format))
            {
                // Convert everything to uppercase.
                result[0] = fieldValue.ToUpper(CultureInfo.CurrentCulture);
                result[1] = preText.ToUpper(CultureInfo.CurrentCulture);
                result[2] = postText.ToUpper(CultureInfo.CurrentCulture);
            }
            else if ("LOWER".Equals(format))
            {
                // Convert everything to lowercase.
                result[0] = fieldValue.ToLower(CultureInfo.CurrentCulture);
                result[1] = preText.ToLower(CultureInfo.CurrentCulture);
                result[2] = postText.ToLower(CultureInfo.CurrentCulture);
            }
            else if ("FirstCap".Equals(format))
            {
                // Capitalize the first letter, everything else is lowercase.
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    result[0] = fieldValue.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
                    if (fieldValue.Length > 1)
                    {
                        result[0] = result[0] + fieldValue.Substring(1).ToLower(CultureInfo.CurrentCulture);
                    }
                }

                if (!string.IsNullOrEmpty(preText))
                {
                    result[1] = preText.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
                    if (fieldValue.Length > 1)
                    {
                        result[1] = result[1] + preText.Substring(1).ToLower(CultureInfo.CurrentCulture);
                    }
                }

                if (!string.IsNullOrEmpty(postText))
                {
                    result[2] = postText.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
                    if (fieldValue.Length > 1)
                    {
                        result[2] = result[2] + postText.Substring(1).ToLower(CultureInfo.CurrentCulture);
                    }
                }
            }
            else if ("Caps".Equals(format))
            {
                // Title casing: the first letter of every word should be capitalized.
                result[0] = fieldValue.ToTitleCase();
                result[1] = preText.ToTitleCase();
                result[2] = postText.ToTitleCase();
            }
            else
            {
                result[0] = fieldValue;
                result[1] = preText;
                result[2] = postText;
            }

            return result;
        }

        /// <summary>
        /// Executes the field switches on a given element.
        /// The possible switches are:
        /// <list>
        /// <li>dt : delete table</li>
        /// <li>dr : delete row</li>
        /// <li>dp : delete paragraph</li>
        /// </list>
        /// </summary>
        /// <param name="element">The element being operated on.</param>
        /// <param name="switches">The switched to be executed.</param>
        private static void ExecuteSwitches(OpenXmlElement element, string[] switches)
        {
            if (switches.IsNullOrEmpty())
            {
                return;
            }

            // check switches (switches are always lowercase)
            if (switches.Contains("dp"))
            {
                var p = GetFirstParent<Paragraph>(element);
                if (p != null)
                {
                    p.Remove();
                }
            }
            else if (switches.Contains("dr"))
            {
                var row = GetFirstParent<TableRow>(element);
                if (row != null)
                {
                    row.Remove();
                }
            }
            else if (switches.Contains("dt"))
            {
                var table = GetFirstParent<Table>(element);
                if (table != null)
                {
                    table.Remove();
                }
            }
        }

        /// <summary>
        /// Fills all the <see cref="SimpleFields"/> that are found in a given <see cref="OpenXmlElement"/>.
        /// </summary>
        /// <param name="values">The values to insert; keys should match the placeholder names, values are the data to insert.</param>
        /// <param name="element">The document element taht will contain the new values.</param>
        private static void FillWordFieldsInElement(IDictionary<string, string> values, OpenXmlElement element)
        {
            string[] switches;
            string[] options;
            string[] formattedText;

            var emptyFields = new Dictionary<SimpleField, string[]>();

            // First pass: fill in data, but do not delete empty fields.  Deletions silently break the loop.
            var fields = element.Descendants<SimpleField>().ToArray();
            foreach (var field in fields)
            {
                string fieldName = GetFieldNameWithOptions(field, out switches, out options);
                if (!string.IsNullOrEmpty(fieldName))
                {
                    if (values.ContainsKey(fieldName) && !string.IsNullOrEmpty(values[fieldName]))
                    {
                        formattedText = ApplyFormatting(options[0], values[fieldName], options[1], options[2]);

                        // Prepend any text specified to appear before the data in the MergeField
                        if (!string.IsNullOrEmpty(options[1]))
                        {
                            field.Parent.InsertBeforeSelf(GetPreOrPostParagraphToInsert(formattedText[1], field));
                        }

                        // Append any text specified to appear after the data in the MergeField
                        if (!string.IsNullOrEmpty(options[2]))
                        {
                            field.Parent.InsertAfterSelf(GetPreOrPostParagraphToInsert(formattedText[2], field));
                        }

                        // replace mergefield with text
                        field.Parent.ReplaceChild(GetRunElementForText(formattedText[0], field), field);
                    }
                    else
                    {
                        // keep track of unknown or empty fields
                        emptyFields[field] = switches;
                    }
                }
            }

            // second pass : clear empty fields
            foreach (var keyValue in emptyFields)
            {
                // if field is unknown or empty: execute switches and remove it from document !
                ExecuteSwitches(keyValue.Key, keyValue.Value);
                keyValue.Key.Remove();
            }
        }

        /// <summary>
        /// Returns the columnname from a given fieldname from a Mergefield
        /// The instruction of a table-Mergefield is formatted as TBL_tablename_columnname
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The column name.</returns>
        /// <exception cref="ArgumentException">Thrown when fieldname is not formatted as TBL_tablename_columname.</exception>
        private static string GetColumnNameFromFieldName(string fieldName)
        {
            // Column name is after the second underscore.
            int pos1 = fieldName.IndexOf('_');
            if (pos1 <= 0)
            {
                throw new ArgumentException("Error: table-MERGEFIELD should be formatted as follows: TBL_tablename_columnname.");
            }

            int pos2 = fieldName.IndexOf('_', pos1 + 1);
            if (pos2 <= 0)
            {
                throw new ArgumentException("Error: table-MERGEFIELD should be formatted as follows: TBL_tablename_columnname.");
            }

            return fieldName.Substring(pos2 + 1);
        }

        /// <summary>
        /// Returns the fieldname and switches from the given mergefield-instruction
        /// Note: the switches are always returned lowercase !
        /// </summary>
        /// <param name="field">The field being examined.</param>
        /// <param name="switches">An array of switches to apply to the field.</param>
        /// <returns>The name of the field.</returns>
        private static string GetFieldName(SimpleField field, out string[] switches)
        {
            var attribute = field.GetAttribute("instr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            switches = new string[0];
            string fieldName = string.Empty;
            string instruction = attribute.Value;

            if (!string.IsNullOrEmpty(instruction))
            {
                var match = rgxInstruction.Match(instruction);
                if (match.Success)
                {
                    fieldName = match.Groups["name"].ToString().Trim();
                    int pos = fieldName.IndexOf('#');
                    if (pos > 0)
                    {
                        // Process the switches, correct the fieldname.
                        switches = fieldName.Substring(pos + 1).ToLower().Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        fieldName = fieldName.Substring(0, pos);
                    }
                }
            }

            return fieldName;
        }

        /// <summary>
        /// Returns the fieldname and switches from the given mergefield-instruction
        /// Note: the switches are always returned lowercase !
        /// Note 2: options holds values for formatting and text to insert before and/or after the field value.
        ///         options[0] = Formatting (Upper, Lower, Caps a.k.a. title case, FirstCap)
        ///         options[1] = Text to insert before data
        ///         options[2] = Text to insert after data
        /// </summary>
        /// <param name="field">The field being examined.</param>
        /// <param name="switches">An array of switches to apply to the field.</param>
        /// <param name="options">Formatting options to apply.</param>
        /// <returns>The name of the field.</returns>
        private static string GetFieldNameWithOptions(SimpleField field, out string[] switches, out string[] options)
        {
            var attribute = field.GetAttribute("instr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            switches = new string[0];
            options = new string[3];
            string fieldName = string.Empty;
            string instruction = attribute.Value;

            if (!string.IsNullOrEmpty(instruction))
            {
                var match = rgxInstruction.Match(instruction);
                if (match.Success)
                {
                    fieldName = match.Groups["name"].ToString().Trim();
                    options[0] = match.Groups["Format"].Value.Trim();
                    options[1] = match.Groups["PreText"].Value.Trim();
                    options[2] = match.Groups["PostText"].Value.Trim();
                    int pos = fieldName.IndexOf('#');
                    if (pos > 0)
                    {
                        // Process the switches, correct the fieldname.
                        switches = fieldName.Substring(pos + 1).ToLower().Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        fieldName = fieldName.Substring(0, pos);
                    }
                }
            }

            return fieldName;
        }

        /// <summary>
        /// Returns the first parent of a given <see cref="OpenXmlElement"/> that corresponds
        /// to the given type.
        /// This methods is different from the Ancestors-method on the OpenXmlElement in the sense that
        /// this method will return only the first-parent in direct line (closest to the given element).
        /// </summary>
        /// <typeparam name="T">The type of element being searched for.</typeparam>
        /// <param name="element">The element being examined.</param>
        /// <returns>The first parent of the element of the specified type.</returns>
        private static T GetFirstParent<T>(OpenXmlElement element)
            where T : OpenXmlElement
        {
            if (element.Parent == null)
            {
                return null;
            }
            else if (element.Parent.GetType() == typeof(T))
            {
                return element.Parent as T;
            }
            else
            {
                return GetFirstParent<T>(element.Parent);
            }
        }

        /// <summary>
        /// Creates a paragraph to house text that should appear before or after the MergeField.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="fieldToMimic">The MergeField that will have its properties mimiced.</param>
        /// <returns>An OpenXml Paragraph ready to insert.</returns>
        private static Paragraph GetPreOrPostParagraphToInsert(string text, SimpleField fieldToMimic)
        {
            Run run = GetRunElementForText(text, fieldToMimic);
            Paragraph paragraph = new Paragraph();
            paragraph.Append(run);
            return paragraph;
        }

        /// <summary>
        /// Returns a <see cref="Run"/>-openxml element for the given text.
        /// Specific about this run-element is that it can describe multiple-line and tabbed-text.
        /// The <see cref="SimpleField"/> placeholder can be provided too, to allow duplicating the formatting.
        /// </summary>
        /// <param name="text">The text to be inserted.</param>
        /// <param name="placeHolder">The placeholder where the text will be inserted.</param>
        /// <returns>A new <see cref="Run"/>-openxml element containing the specified text.</returns>
        private static Run GetRunElementForText(string text, SimpleField placeHolder)
        {
            string runProperties = null;
            if (placeHolder != null)
            {
                foreach (var descendant in placeHolder.Descendants<RunProperties>())
                {
                    runProperties = descendant.OuterXml;
                    break;  // break at first
                }
            }

            Run run = new Run();
            if (!string.IsNullOrEmpty(runProperties))
            {
                run.Append(new RunProperties(runProperties));
            }

            if (!string.IsNullOrEmpty(text))
            {
                // first process line breaks
                string[] split = text.Split(new string[] { "\n" }, StringSplitOptions.None);
                bool first = true;
                foreach (string value in split)
                {
                    if (!first)
                    {
                        run.Append(new Break());
                    }

                    first = false;

                    // then process tabs
                    bool firstTab = true;
                    string[] tabSplit = value.Split(new string[] { "\t" }, StringSplitOptions.None);
                    foreach (string tabText in tabSplit)
                    {
                        if (!firstTab)
                        {
                            run.Append(new TabChar());
                        }

                        run.Append(new DocumentFormat.OpenXml.Wordprocessing.Text(tabText));
                        firstTab = false;
                    }
                }
            }

            return run;
        }

        /// <summary>
        /// Returns the table name from a given fieldname from a Mergefield.
        /// The instruction of a table-Mergefield is formatted as TBL_tablename_columnname
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The table name.</returns>
        /// <exception cref="ArgumentException">Thrown when fieldname is not formatted as TBL_tablename_columname.</exception>
        private static string GetTableNameFromFieldName(string fieldName)
        {
            int pos1 = fieldName.IndexOf('_');
            if (pos1 <= 0)
            {
                throw new ArgumentException("Error: table-MERGEFIELD should be formatted as follows: TBL_tablename_columnname.");
            }

            int pos2 = fieldName.IndexOf('_', pos1 + 1);
            if (pos2 <= 0)
            {
                throw new ArgumentException("Error: table-MERGEFIELD should be formatted as follows: TBL_tablename_columnname.");
            }

            return fieldName.Substring(pos1 + 1, pos2 - pos1 - 1);
        }

        /// <summary>
        /// Since MS Word 2010 the SimpleField element is not longer used. It has been replaced by a combination of
        /// Run elements and a FieldCode element. This method will convert the new format to the old SimpleField-compliant
        /// format.
        /// </summary>
        /// <param name="parentElement"></param>
        private static void ConvertComplexFieldsToSimpleFields(OpenXmlElement parentElement)
        {
            //  search for all the Run elements
            var runElements = parentElement.Descendants<Run>().ToArray();
            if (runElements.Length == 0)
            {
                return;
            }

            var newFields = new Dictionary<Run, Run[]>();

            int index = 0;
            do
            {
                var run = runElements[index];

                if (run.HasChildren &&
                    run.Descendants<FieldChar>().Count() > 0 &&
                    (run.Descendants<FieldChar>().First().FieldCharType & FieldCharValues.Begin) == FieldCharValues.Begin)
                {
                    var innerRuns = new List<Run>();
                    innerRuns.Add(run);

                    //  loop until we find the 'end' FieldChar
                    bool endFound = false;
                    string instruction = null;
                    RunProperties runProperties = null;
                    do
                    {
                        index++;
                        run = runElements[index];

                        innerRuns.Add(run);
                        if (run.HasChildren && run.Descendants<FieldCode>().Count() > 0)
                        {
                            instruction += run.GetFirstChild<FieldCode>().Text;
                        }
                        if (run.HasChildren && run.Descendants<FieldChar>().Count() > 0
                            && (run.Descendants<FieldChar>().First().FieldCharType & FieldCharValues.End) == FieldCharValues.End)
                        {
                            endFound = true;
                        }
                        if (run.HasChildren && run.Descendants<RunProperties>().Count() > 0)
                        {
                            runProperties = run.GetFirstChild<RunProperties>();
                        }
                    } while (!endFound && index < runElements.Length);

                    //  something went wrong : found Begin but no End. Throw exception
                    if (!endFound)
                    {
                        throw new Exception("Found a Begin FieldChar but no End !");
                    }

                    if (!string.IsNullOrEmpty(instruction))
                    {
                        //  build new Run containing a SimpleField
                        Run newRun = new Run();
                        if (runProperties != null)
                        {
                            newRun.AppendChild(runProperties.CloneNode(true));
                        }

                        newRun.AppendChild(new SimpleField { Instruction = instruction });

                        newFields.Add(newRun, innerRuns.ToArray());
                    }
                }

                index++;
            } while (index < runElements.Length);

            // Replace all FieldCodes with old-style SimpleFields
            foreach (var keyValue in newFields)
            {
                keyValue.Value[0].Parent.ReplaceChild(keyValue.Key, keyValue.Value[0]);
                for (int i = 1; i < keyValue.Value.Length; i++)
                {
                    keyValue.Value[i].Remove();
                }
            }
        }
    }
}