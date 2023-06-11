using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Extenso.Collections;
using Kore;
using Kore.Collections;
using Kore.Data;

namespace DataMigrator.Office
{
    [CLSCompliant(false)]
    public class ExcelOpenXmlDocument : IExcelDocument, IDisposable
    {
        #region Non-Public Members

        public SpreadsheetDocument Document { get; private set; }
        private uint DateStyleIndex = 0;
        private uint DateTimeStyleIndex = 0;

        #endregion Non-Public Members

        #region Constructors

        private ExcelOpenXmlDocument()
        {
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// <para>Creates a new spreadsheet with the specified file name and returns a new instance</para>
        /// <para>of this class.</para>
        /// </summary>
        /// <param name="filePath">The file name of the new spreadsheet.</param>
        /// <returns>New instance of ExcelOpenXmlDocument class.</returns>
        public static ExcelOpenXmlDocument Create(string filePath)
        {
            var excel = new ExcelOpenXmlDocument();
            excel.Document = SpreadsheetDocument.Create(
                filePath,
                SpreadsheetDocumentType.Workbook);

            PopulateNewDocument(excel);
            excel.Intialize();

            return excel;
        }

        /// <summary>
        /// Creates a new spreadsheet based on the specified spreadsheet template.
        /// </summary>
        /// <param name="filePath">The location of the new spreadsheet document.</param>
        /// <param name="templateFilePath">The location to the template spreadsheet.</param>
        /// /// <returns>New instance of ExcelOpenXmlDocument class.</returns>
        public static ExcelOpenXmlDocument Create(string filePath, string templateFilePath)
        {
            if (!File.Exists(templateFilePath))
            {
                throw new FileNotFoundException("Could not find specified Excel template.");
            }

            File.Copy(templateFilePath, filePath);

            var excel = new ExcelOpenXmlDocument
            {
                Document = SpreadsheetDocument.Open(filePath, true)
            };
            excel.Intialize();
            return excel;
        }

        /// <summary>
        /// <para>Creates a new spreadsheet in the specified System.IO.Stream and returns a new instance</para>
        /// <para>of this class.</para>
        /// </summary>
        /// <param name="stream">The System.IO.Stream to create the spreadsheet in.</param>
        /// <returns>New instance of ExcelOpenXmlDocument class.</returns>
        public static ExcelOpenXmlDocument Create(Stream stream)
        {
            var excel = new ExcelOpenXmlDocument();
            excel.Document = SpreadsheetDocument.Create(
                stream,
                SpreadsheetDocumentType.Workbook);

            PopulateNewDocument(excel);
            excel.Intialize();

            return excel;
        }

        /// <summary>
        /// Creates a new instance based on the specified existing spreadsheet file.
        /// </summary>
        /// <param name="fileName">The location of the existing spreadsheet document.</param>
        /// <returns>New instance of ExcelOpenXmlDocument class.</returns>
        public static ExcelOpenXmlDocument Load(string filePath)
        {
            var excel = new ExcelOpenXmlDocument
            {
                Document = SpreadsheetDocument.Open(filePath, true)
            };
            excel.Intialize();
            return excel;
        }

        /// <summary>
        /// Creates a new instance based on the specified System.IO.Stream.
        /// </summary>
        /// <param name="stream">The System.IO.Stream to open the spreadsheet from.</param>
        /// <returns>New instance of ExcelOpenXmlDocument class.</returns>
        public static ExcelOpenXmlDocument Load(Stream stream)
        {
            var excel = new ExcelOpenXmlDocument
            {
                Document = SpreadsheetDocument.Open(stream, true)
            };
            excel.Intialize();
            return excel;
        }

        /// <summary>
        /// Writes data to the specified cell.
        /// </summary>
        /// <param name="sheetName">The name of the worksheet to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="rowIndex">The index of the row to write to.</param>
        /// <param name="columnIndex">The index of the column to write to.</param>
        public void InsertNumber(string sheetName, double value, uint rowIndex, int columnIndex)
        {
            var worksheetPart = GetWorksheetPartByName(sheetName);
            var cell = InsertCellInWorksheet(columnIndex, rowIndex, worksheetPart);
            var formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            var cellValue = new CellValue(value.ToString("#.#", formatProvider));
            cell.CellValue = cellValue;
            cell.DataType = CellValues.Number;

            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// Writes data to the specified cell.
        /// </summary>
        /// <param name="sheetName">The name of the worksheet to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="rowIndex">The index of the row to write to.</param>
        /// <param name="columnIndex">The index of the column to write to.</param>
        public void InsertDate(string sheetName, DateTime value, uint rowIndex, int columnIndex)
        {
            var worksheetPart = GetWorksheetPartByName(sheetName);
            var cell = InsertCellInWorksheet(columnIndex, rowIndex, worksheetPart);
            cell.StyleIndex = DateStyleIndex;
            cell.CellValue = new CellValue(value.ToOADate().ToString());

            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// Writes data to the specified cell.
        /// </summary>
        /// <param name="sheetName">The name of the worksheet to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="rowIndex">The index of the row to write to.</param>
        /// <param name="columnIndex">The index of the column to write to.</param>
        public void InsertDateTime(string sheetName, DateTime value, uint rowIndex, int columnIndex)
        {
            var worksheetPart = GetWorksheetPartByName(sheetName);
            var cell = InsertCellInWorksheet(columnIndex, rowIndex, worksheetPart);
            cell.StyleIndex = DateTimeStyleIndex;
            cell.CellValue = new CellValue(value.ToOADate().ToString());

            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// Writes data to the specified cell.
        /// </summary>
        /// <param name="sheetName">The name of the worksheet to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="rowIndex">The index of the row to write to.</param>
        /// <param name="columnIndex">The index of the column to write to.</param>
        public void InsertTime(string sheetName, DateTime value, uint rowIndex, int columnIndex)
        {
            var worksheetPart = GetWorksheetPartByName(sheetName);
            var cell = InsertCellInWorksheet(columnIndex, rowIndex, worksheetPart);
            cell.CellValue = new CellValue(value.ToString("HH:mm"));
            cell.DataType = new EnumValue<CellValues>(CellValues.Date);

            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// Writes data to the specified cell.
        /// </summary>
        /// <param name="sheetName">The name of the worksheet to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="rowIndex">The index of the row to write to.</param>
        /// <param name="columnIndex">The index of the column to write to.</param>
        public void InsertText(string sheetName, string value, uint rowIndex, int columnIndex)
        {
            SharedStringTablePart shareStringPart;
            if (Document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = Document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = Document.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }

            int index = InsertSharedStringItem(value, shareStringPart);
            var worksheetPart = GetWorksheetPartByName(sheetName);
            var cell = InsertCellInWorksheet(columnIndex, rowIndex, worksheetPart);
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// Inserts the specified string into the specified SharedStringTablePart.
        /// </summary>
        /// <param name="value">The value to enter.</param>
        /// <param name="shareStringPart">The SharedStringTablePart to enter the value into.</param>
        /// <returns>Index of new shared string.</returns>
        public int InsertSharedStringItem(string value, SharedStringTablePart shareStringPart)
        {
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;
            foreach (var item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == value)
                {
                    return i;
                }

                i++;
            }

            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(value)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        #endregion Public Methods

        #region IExcelDocument Members

        /// <summary>
        /// Adds a new worksheet to the current workbook.
        /// </summary>
        /// <param name="sheetName">The name of the new worksheet.</param>
        public void AddSheet(string sheetName)
        {
            var workbookPart = Document.WorkbookPart;

            // Add a blank WorksheetPart.
            var newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();

            // Add new sheet to main workbook part
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            var newSheet = new Sheet
            {
                Id = relationshipId,
                Name = sheetName,
                SheetId = sheetId
            };

            sheets.Append(newSheet);
            workbookPart.Workbook.Save();
        }

        //public void AddSheet(string sheetName)
        //{
        //    WorkbookPart workbookPart = Document.WorkbookPart;

        //    // Add a blank WorksheetPart.
        //    WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();

        //    newWorksheetPart.Worksheet = new Worksheet(new SheetData());

        //    // Add new sheet to main workbook part
        //    Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
        //    Sheet newSheet = new Sheet
        //    {
        //        Name = sheetName
        //    };

        //    uint sheetId = 1;
        //    if (sheets.Elements<Sheet>().Count() > 0)
        //    {
        //        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
        //    }

        //    newSheet.Id = workbookPart.GetIdOfPart(newWorksheetPart);
        //    newSheet.SheetId = sheetId;
        //    sheets.Append(newSheet);

        //    newWorksheetPart.Worksheet.Save();
        //    workbookPart.Workbook.Save();
        //}

        /// <summary>
        /// Makes a copy of the specified worksheet.
        /// </summary>
        /// <param name="source">The worksheet to copy.</param>
        /// <param name="destination">The name of the new worksheet.</param>
        public void CopySheet(string source, string destination)
        {
            var workbookPart = Document.WorkbookPart;
            // Get the source sheet to be copied
            var sourceWorksheetPart = workbookPart.GetWorksheetPart(source);
            // Make clone
            var tempWorksheetPart = Document.DeepCloneWorksheetPart(sourceWorksheetPart);
            // Add cloned sheet and all associated parts to workbook
            var clonedWorksheetPart = workbookPart.AddPart<WorksheetPart>(tempWorksheetPart);

            // Table definition parts are somewhat special and need unique ids...so let's make an id based on count
            int tableDefinitionPartCount = sourceWorksheetPart.GetPartsCountOfType<TableDefinitionPart>();

            // Clean up table definition parts (tables need unique ids)
            if (tableDefinitionPartCount != 0)
            {
                FixupTableParts(clonedWorksheetPart, (uint)tableDefinitionPartCount);
            }
            // There should only be one sheet that has focus
            CleanView(clonedWorksheetPart);

            // Add new sheet to main workbook part
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            var copiedSheet = new Sheet();
            copiedSheet.Name = destination;
            copiedSheet.Id = workbookPart.GetIdOfPart(clonedWorksheetPart);
            copiedSheet.SheetId = (uint)sheets.ChildElements.Count + 1;
            sheets.Append(copiedSheet);

            workbookPart.Workbook.Save();
        }

        /// <summary>
        /// Deletes the specified worksheet.
        /// </summary>
        /// <param name="sheetName">The name of the sheet to delete.</param>
        public void DeleteSheet(string sheetName)
        {
            //From: http://blogs.msdn.com/b/vsod/archive/2010/02/05/how-to-delete-a-worksheet-from-excel-using-open-xml-sdk-2-0.aspx

            string SheetID = string.Empty;
            var workbookPart = Document.WorkbookPart;

            // Get the pivot Table Parts
            var pvtTableCacheParts = workbookPart.PivotTableCacheDefinitionParts;
            var pvtTableCacheDefinationPart = new Dictionary<PivotTableCacheDefinitionPart, string>();
            foreach (var Item in pvtTableCacheParts)
            {
                var pvtCacheDef = Item.PivotCacheDefinition;
                // Check if this CacheSource is linked to SheetToDelete
                var pvtCahce = pvtCacheDef.Descendants<CacheSource>().Where(s => s.WorksheetSource.Sheet == sheetName);
                if (pvtCahce.Count() > 0)
                {
                    pvtTableCacheDefinationPart.Add(Item, Item.ToString());
                }
            }
            foreach (var Item in pvtTableCacheDefinationPart)
            {
                workbookPart.DeletePart(Item.Key);
            }
            // Get the SheetToDelete from workbook.xml
            var theSheet = workbookPart.Workbook.Descendants<Sheet>()
                .Where(s => s.Name == sheetName)
                .FirstOrDefault();

            if (theSheet == null)
            {
                // The specified sheet doesn't exist.
            }
            // Store the SheetID for the reference
            SheetID = theSheet.SheetId;

            // Remove the sheet reference from the workbook.
            var worksheetPart = (WorksheetPart)(workbookPart.GetPartById(theSheet.Id));
            theSheet.Remove();

            // Delete the worksheet part.
            workbookPart.DeletePart(worksheetPart);

            // Get the DefinedNames
            var definedNames = workbookPart.Workbook.Descendants<DefinedNames>().FirstOrDefault();
            if (definedNames != null)
            {
                foreach (DefinedName Item in definedNames)
                {
                    // This condition checks to delete only those names which are part of Sheet in question
                    if (Item.Text.Contains(sheetName + "!"))
                    {
                        Item.Remove();
                    }
                }
            }
            // Get the CalculationChainPart
            // Note: An instance of this part type contains an ordered set of references to all cells in all worksheets in the
            // workbook whose value is calculated from any formula

            CalculationChainPart calChainPart;
            calChainPart = workbookPart.CalculationChainPart;
            if (calChainPart != null)
            {
                var calChainEntries = calChainPart.CalculationChain.Descendants<CalculationCell>()
                    .Where(c => c.SheetId == SheetID);

                foreach (var Item in calChainEntries)
                {
                    Item.Remove();
                }
                if (calChainPart.CalculationChain.Count() == 0)
                {
                    workbookPart.DeletePart(calChainPart);
                }
            }

            workbookPart.Workbook.Save();
        }

        public int GetColumnCount(string sheetName)
        {
            var worksheetPart = GetWorksheetPartByName(sheetName);
            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            var rows = sheetData.Descendants<Row>();
            return rows.ElementAt(0).Count();
        }

        public int GetRowCount(string sheetName)
        {
            var worksheetPart = GetWorksheetPartByName(sheetName);
            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            return sheetData.Descendants<Row>().Count();
        }

        /// <summary>
        /// <para>Exports the specified System.Data.DataSet to the current workbook.</para>
        /// <para>A worksheet will be created for each System.Data.DataTable. The name of the </para>
        /// <para>DataTables will be used as the sheet names. If a sheet name already exists, a new</para>
        /// <para>name will be generated.</para>
        /// </summary>
        /// <param name="dataSet">The System.Data.DataSet to export</param>
        public void Import(DataSet dataSet)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                Import(table);
            }
        }

        /// <summary>
        /// <para>Exports the specified System.Data.DataTable to the current workbook.</para>
        /// <para>A worksheet will be created for based on the name of the DataTable.</para>
        /// <para>If the sheet name already exists, a new name will be generated.</para>
        /// </summary>
        /// <param name="table">The System.Data.DataTable to export.</param>
        public void Import(DataTable table)
        {
            int sheetNameID = 1;

            #region Generate Sheet Name

            string tableName = table.TableName;
            if (!string.IsNullOrEmpty(tableName))
            {
                while (Document.WorkbookPart.GetWorksheetPart(tableName) != null)
                {
                    tableName = string.Concat(table.TableName, '_', sheetNameID++);
                }
            }
            else
            {
                tableName = "Table_" + sheetNameID++;
                while (Document.WorkbookPart.GetWorksheetPart(tableName) != null)
                {
                    tableName = "Table_" + sheetNameID++;
                }
            }

            #endregion Generate Sheet Name

            AddSheet(tableName);
            Import(table, tableName);
        }

        /// <summary>
        /// Exports the specified System.Data.DataTable to the specified worksheet.
        /// </summary>
        /// <param name="table">The System.Data.DataTable to export.</param>
        /// <param name="sheetName">The name of the worksheet to export to.</param>
        public void Import(DataTable table, string sheetName)
        {
            Import(table, sheetName, true, 1);
        }

        public void Import(DataTable table, string sheetName, bool outputColumnNames, uint startRowIndex)
        {
            var workbookPart = Document.WorkbookPart;
            var worksheetPart = workbookPart.GetWorksheetPart(sheetName);
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            #region Change Enum Columns To String Columns

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].DataType.IsEnum)
                {
                    table.Columns[i].ChangeDataType<string>();
                }
            }

            #endregion Change Enum Columns To String Columns

            #region Add column names to the first row

            if (outputColumnNames)
            {
                foreach (DataColumn column in table.Columns)
                {
                    InsertText(sheetName, column.ColumnName, 1, table.Columns.IndexOf(column) + 1);
                }
            }

            #endregion Add column names to the first row

            //loop through each data row
            DataRow contentRow;

            if (outputColumnNames)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    contentRow = table.Rows[i];
                    CreateContentRow(sheetName, contentRow, (uint)(i + startRowIndex));
                }
            }
            else
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    contentRow = table.Rows[i];
                    CreateContentRow(sheetName, contentRow, (uint)(i + startRowIndex + 1));
                }
            }

            worksheetPart.Worksheet.Save();
            workbookPart.Workbook.Save();
        }

        /// <summary>
        /// <para>Exports the specified IEnumerable&lt;T&gt; to the current workbook.</para>
        /// <para>A sheet name will be generated.</para>
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="enumerable">The IEnumerable&lt;T&gt; to export.</param>
        public void Import<T>(IEnumerable<T> enumerable)
        {
            Import(enumerable.ToDataTable());
        }

        /// <summary>
        /// Exports the specified IEnumerable&lt;T&gt; to the specified worksheet.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="enumerable">The IEnumerable&lt;T&gt; to export.</param>
        /// <param name="sheetName">The name of the worksheet to export to.</param>
        public void Import<T>(IEnumerable<T> enumerable, string sheetName)
        {
            Import(enumerable.ToDataTable(), sheetName);
        }

        /// <summary>
        /// Returns an IEnumerable&lt;String&gt; containing the names of all worksheets in this workbook.
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt; of available worksheets.</returns>
        public IEnumerable<string> GetSheetNames()
        {
            var sheetNames = new List<string>();
            foreach (var sheet in Document.WorkbookPart.Workbook.Descendants<Sheet>())
            {
                sheetNames.Add(sheet.Name);
            }
            return sheetNames;
        }

        public DataTable ReadSheet(string sheetName)
        {
            return ReadSheet(sheetName, false);
        }

        /// <summary>
        /// Gets data for the specified worksheet.
        /// </summary>
        /// <param name="sheetName">The name of the sheet to get data from.</param>
        /// <param name="firstRowContainsColumnNames">Set true to remove first row from data.</param>
        /// <returns>System.Data.DataTable containing specified worksheet's data.</returns>
        public DataTable ReadSheet(string sheetName, bool firstRowContainsColumnNames)
        {
            var table = new DataTable(sheetName);

            //WorkbookPart workbookPart = Document.WorkbookPart;
            var sheets = Document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

            string relationshipId = sheets.SingleOrDefault(x => x.Name == sheetName).Id.Value;
            var worksheetPart = (WorksheetPart)Document.WorkbookPart.GetPartById(relationshipId);
            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            var rows = sheetData.Descendants<Row>();

            if (firstRowContainsColumnNames)
            {
                foreach (Cell cell in rows.ElementAt(0))
                {
                    table.Columns.Add(GetCellValue(cell));
                }
            }
            else
            {
                for (int i = 0; i < rows.ElementAt(0).Count(); i++)
                {
                    table.Columns.Add(string.Concat("Column", i + 1));
                }
            }

            foreach (var row in rows)
            {
                var dataRow = table.NewRow();

                for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                {
                    dataRow[i] = GetCellValue(row.Descendants<Cell>().ElementAt(i));
                }

                table.Rows.Add(dataRow);
            }

            if (firstRowContainsColumnNames)
            {
                table.Rows.RemoveAt(0); // Remove header row
            }
            return table;
        }

        /// <summary>
        /// Gets data from all worksheets in the workbook.
        /// </summary>
        /// <returns>System.Data.DataSet containing all workbook data.</returns>
        public DataSet ReadWorkbook()
        {
            var dataSet = new DataSet("Data");

            foreach (string sheetName in GetSheetNames())
            {
                dataSet.Tables.Add(ReadSheet(sheetName, false));
            }

            return dataSet;
        }

        public void Save()
        {
            Document.WorkbookPart.Workbook.Save();
        }

        /// <summary>
        /// Makes the specified worksheet the active worksheet.
        /// </summary>
        /// <param name="sheetName">The name of the worksheet to set as the active sheet.</param>
        public void SetActiveSheet(string sheetName)
        {
            var existingSheet = Document.WorkbookPart.Workbook.Descendants<Sheet>().SingleOrDefault(s => s.Name == sheetName);

            if (existingSheet == default(Sheet)) //if null
            {
                return;
            }

            //Loop through all sheets
            var sheets = Document.WorkbookPart.Workbook.Descendants<Sheet>();
            byte tabIndex = 0;
            byte idxCount = 0;
            foreach (var sheet in sheets)
            {
                var worksheetPart = (WorksheetPart)Document.WorkbookPart.GetPartById(sheet.Id);
                var sheetViews = worksheetPart.Worksheet.GetFirstChild<SheetViews>();
                var sheetView = sheetViews.GetFirstChild<SheetView>();

                if (sheet.Name == sheetName)
                {
                    sheetView.TabSelected = true;
                    sheetView.TopLeftCell = "A1"; // Scroll to top left
                    tabIndex = idxCount;
                }
                else
                {
                    sheetView.TabSelected = null;
                }
                idxCount++;
            }

            //Select the tab
            var workbook = Document.WorkbookPart.Workbook;
            var bookViews = workbook.GetFirstChild<BookViews>();
            var workbookView = bookViews.GetFirstChild<WorkbookView>();
            workbookView.ActiveTab = (UInt32Value)tabIndex;
        }

        #endregion IExcelDocument Members

        #region Private Methods

        private static void PopulateNewDocument(ExcelOpenXmlDocument excel)
        {
            // create the workbook
            excel.Document.AddWorkbookPart();

            AddStylesParts(excel);

            excel.Document.WorkbookPart.Workbook = new Workbook();     // create the worksheet
            excel.Document.WorkbookPart.AddNewPart<WorksheetPart>();
            excel.Document.WorkbookPart.WorksheetParts.First().Worksheet = new Worksheet();

            // create sheet data
            excel.Document.WorkbookPart.WorksheetParts.First().Worksheet.AppendChild(new SheetData());

            // save worksheet
            excel.Document.WorkbookPart.WorksheetParts.First().Worksheet.Save();

            // create the worksheet to workbook relation
            excel.Document.WorkbookPart.Workbook.AppendChild(new Sheets());

            #region Not Needed Anymore (Only use if want to create default sheet)

            //excel.Document.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet()
            //{
            //    Id = excel.Document.WorkbookPart.GetIdOfPart(excel.Document.WorkbookPart.WorksheetParts.First()),
            //    SheetId = 1,
            //    Name = firstSheetName
            //});

            #endregion Not Needed Anymore (Only use if want to create default sheet)

            excel.Document.WorkbookPart.Workbook.Save();
        }

        private static void AddStylesParts(ExcelOpenXmlDocument excel)
        {
            var part = excel.Document.WorkbookPart.AddNewPart<WorkbookStylesPart>();

            new Stylesheet(
                new Fonts(
                    new Font(
                        new FontSize { Val = 11D },
                        new Color { Theme = (UInt32Value)1U },
                        new FontName { Val = "Calibri" },
                        new FontFamilyNumbering { Val = 2 },
                        new FontScheme { Val = FontSchemeValues.Minor })
                )
                { Count = (UInt32Value)1U },
                new Fills(
                    new Fill(
                        new PatternFill { PatternType = PatternValues.None }),
                    new Fill(
                        new PatternFill { PatternType = PatternValues.Gray125 })
                )
                { Count = (UInt32Value)2U },
                new Borders(
                    new Border(
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder())
                )
                { Count = (UInt32Value)1U },
                new CellStyleFormats(
                    new CellFormat { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U }
                )
                { Count = (UInt32Value)1U },
                new CellFormats(
                    new CellFormat { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U }
                )
                { Count = (UInt32Value)1U },
                new CellStyles(
                    new CellStyle { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U }
                )
                { Count = (UInt32Value)1U },
                new DifferentialFormats { Count = (UInt32Value)0U },
                new TableStyles { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium9", DefaultPivotStyle = "PivotStyleLight16" }
            ).Save(part);
        }

        private WorksheetPart GetWorksheetPartByName(string sheetName)
        {
            var sheets = Document.WorkbookPart.Workbook
                .GetFirstChild<Sheets>()
                .Elements<Sheet>()
                .Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            var worksheetPart = (WorksheetPart)Document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;
        }

        private Cell InsertCellInWorksheet(int columnIndex, uint rowIndex, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = GetColumnName(columnIndex) + rowIndex;

            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            if (row.Elements<Cell>().Where(c => c.CellReference.Value == GetColumnName(columnIndex) + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                var newCell = new Cell { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        private Row CreateContentRow(string sheetName, DataRow dataRow, uint rowIndex)
        {
            var row = new Row { RowIndex = (uint)rowIndex };

            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                Type type = dataRow[i].GetType();

                if (type == typeof(DateTime))
                {
                    InsertDateTime(sheetName, Convert.ToDateTime(dataRow[i]), rowIndex, i + 1);
                }
                else if (type.In(
                    typeof(Byte),
                    typeof(Int16),
                    typeof(Int32),
                    typeof(Int64),
                    typeof(Decimal),
                    typeof(Double),
                    typeof(Single),
                    typeof(SByte),
                    typeof(UInt16),
                    typeof(UInt32),
                    typeof(UInt64)))
                {
                    InsertNumber(sheetName, Convert.ToDouble(dataRow[i]), rowIndex, i + 1);
                }
                else
                {
                    InsertText(sheetName, dataRow[i].ToString(), rowIndex, i + 1);
                }
            }
            return row;
        }

        private string GetCellValue(Cell cell)
        {
            var sharedStringTablePart = Document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return sharedStringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
            }
            else { return value; }
        }

        private static string GetColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = string.Empty;
            int modifier;

            while (dividend > 0)
            {
                modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = (dividend - modifier) / 26;
            }

            return columnName;
        }

        // For CopySheet
        private static void CleanView(WorksheetPart worksheetPart)
        {
            //There can only be one sheet that has focus
            var sheetViews = worksheetPart.Worksheet.GetFirstChild<SheetViews>();

            if (sheetViews != null)
            {
                sheetViews.Remove();
                worksheetPart.Worksheet.Save();
            }
        }

        private static void FixupTableParts(WorksheetPart worksheetPart, uint tableID)
        {
            //Every table needs a unique id and name
            foreach (var tableDefPart in worksheetPart.TableDefinitionParts)
            {
                tableDefPart.Table.Id = tableID + 1;
                string tableName = string.Concat("CopiedTable", tableID, 1);
                tableDefPart.Table.DisplayName = tableName;
                tableDefPart.Table.Name = tableName;
                tableDefPart.Table.Save();
            }
        }

        private UInt32Value CreateCellFormat(
            Stylesheet styleSheet,
            UInt32Value fontIndex,
            UInt32Value fillIndex,
            UInt32Value numberFormatId)
        {
            var cellFormat = new CellFormat();

            if (fontIndex != null)
            {
                cellFormat.FontId = fontIndex;
            }

            if (fillIndex != null)
            {
                cellFormat.FillId = fillIndex;
            }

            if (numberFormatId != null)
            {
                cellFormat.NumberFormatId = numberFormatId;
                cellFormat.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            }

            styleSheet.CellFormats.Append(cellFormat);

            var result = styleSheet.CellFormats.Count;
            styleSheet.CellFormats.Count++;
            return result;
        }

        private void Intialize()
        {
            DateStyleIndex = CreateCellFormat(
                Document.WorkbookPart.WorkbookStylesPart.Stylesheet,
                null,
                null,
                UInt32Value.FromUInt32(14));

            DateTimeStyleIndex = CreateCellFormat(
                Document.WorkbookPart.WorkbookStylesPart.Stylesheet,
                null,
                null,
                UInt32Value.FromUInt32(22));
        }

        #endregion Private Methods

        #region IDisposable Members

        public void Dispose()
        {
            Document.Dispose();
        }

        #endregion IDisposable Members
    }
}