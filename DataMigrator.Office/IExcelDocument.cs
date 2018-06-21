using System;
using System.Collections.Generic;
using System.Data;

namespace DataMigrator.Office
{
    [CLSCompliant(false)]
    public interface IExcelDocument
    {
        void AddSheet(string sheetName);
        void CopySheet(string source, string destination);
        void DeleteSheet(string sheetName);
        void Import(DataSet dataSet);
        int GetColumnCount(string sheetName);
        int GetRowCount(string sheetName);
        void Import(DataTable table);
        void Import(DataTable table, string sheetName);
        void Import<T>(IEnumerable<T> enumerable);
        void Import<T>(IEnumerable<T> enumerable, string sheetName);
        IEnumerable<string> GetSheetNames();
        DataTable ReadSheet(string sheetName);
        DataTable ReadSheet(string sheetName, bool firstRowContainsColumnNames);
        DataSet ReadWorkbook();
        void Save();
        void SetActiveSheet(string sheetName);
    }
}