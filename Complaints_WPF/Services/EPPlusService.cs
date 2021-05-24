using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Services
{
    internal static class EPPlusService
    {
        public static void SaveCollectionToExcel(IEnumerable<object> collection)
        {
            SaveFileDialog sfD = new SaveFileDialog();
            sfD.DefaultExt = ".xlsx";
            sfD.Filter = "Файл Excel (.xlsx)|*.xlsx";
            sfD.FileName = $"ЖРЖ (на {DateTime.Now.ToString("dd.MM.yy")})";

            if (sfD.ShowDialog() == true)
            {
                var path = new FileInfo(sfD.FileName);

                CreateComplaintsSpreadSheet(collection, path);
            }
        }

        //public static void SaveComplaintsToExcel(IEnumerable<object> collection)
        //{
        //    var file = SaveDialog(collection, "ЖРЖ");

        //    if (file.Exists)
        //        file.Delete();

        //    CreateComplaintsSpreadSheet(collection, file);
        //}

        //private static FileInfo SaveDialog(IEnumerable<object> collection, string header)
        //{
        //    SaveFileDialog saveDialog = new SaveFileDialog();

        //    if (saveDialog.ShowDialog() != true)
        //    {
        //        return null;
        //    }

        //    saveDialog.Filter = "Файл Excel (.xlsx)|*.xlsx";
        //    saveDialog.DefaultExt = ".xlsx";
        //    saveDialog.FileName = header; //$"ЖРЖ (на {DateTime.Now.ToString("yyyy.MM.dd")}";

        //    return new FileInfo(saveDialog.FileName);
        //}

        private static void CreateComplaintsSpreadSheet(IEnumerable<object> collection, FileInfo file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Обращения");

                #region Header
                ws.Cells[Address: "A1"].Value = $"Журнал регистрации обращений на {DateTime.Now.ToString("dd.MM.yyyy")}";
                ws.Cells[Address: "A1:H1"].Merge = true;
                ws.Row(row: 1).Style.Font.Size = 18;
                //ws.Row(row: 1).Style.Font.Color.SetColor(Color.DarkGray);
                ws.Row(row: 2).Style.Font.Bold = true;
                #endregion

                ExcelRangeBase range = ws.Cells[Address: "A2"].LoadFromCollection(collection); //, PrintHeaders: true //LFC allows to pass INmbl<T> (in our case List<PM>),  PrintHeaders - will take prop names for headers  //"A2"-starting point
                range.AutoFitColumns(); //so we see everything
                ws.Column(col: 3).Width = 30;

                package.Save(); //.SaveAsync();

                //w/o using make sure to close excel: package.Dispose();
            }
        }

        //public static async Task WriteReportExcelFile(IEnumerable<object> collection, FileInfo file, string header)
        //{

        //}
    }
}
