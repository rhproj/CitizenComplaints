using Complaints_WPF.Models;
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
        public static void SaveComplaintsToExcel(IList<Complaint> collection)
        {
            SaveFileDialog sfD = new SaveFileDialog();
            sfD.DefaultExt = ".xlsx";
            sfD.Filter = "Файл Excel (.xlsx)|*.xlsx";
            sfD.FileName = $"ЖРЖ (на {DateTime.Now.ToString("dd.MM.yy")})";

            if (sfD.ShowDialog() == true)
            {
                var file = new FileInfo(sfD.FileName);
                if (file.Exists)
                    file.Delete();

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Обращения");
                    ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    ws.Cells[Address: "B2"].Value = "№";
                    ws.Cells[Address: "C2"].Value = "Дата";
                    ws.Cells[Address: "D2"].Value = "Содержание обращения";
                    ws.Cells[Address: "H2"].Value = "Примечание";
                    ws.Cells[Address: "I2"].Value = "ФИО Заявителя";
                    ws.Cells[Address: "J2"].Value = "Принял(а)";
                    ws.Cells[Address: "K2"].Value = "Руководитель";
                    ws.Cells[Address: "L2"].Value = "Результат";

                    ws.Column(col: 2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(col: 3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(row: 1).Height = 30;
                    ws.Row(row: 2).Height = 22;

                    ExcelRangeBase range = ws.Cells[Address: "A3"].LoadFromCollection(collection); //, PrintHeaders: true //LFC allows to pass INmbl<T> (in our case List<PM>),  PrintHeaders - will take prop names for headers  //"A2"-starting point

                    ws.Column(col: 4).Style.WrapText = true;
                    ws.Column(col: 8).Style.WrapText = true;

                    ws.Column(col: 1).Hidden = true;
                    ws.Column(col: 5).Hidden = true;
                    ws.Column(col: 6).Hidden = true;
                    ws.Column(col: 7).Hidden = true;

                    ws.Column(col: 2).Width = 5;
                    ws.Column(col: 3).Style.Numberformat.Format = "dd.mm.yy";
                    ws.Column(col: 3).Width = 8;
                    ws.Column(col: 4).Width = 60;
                    ws.Column(col: 8).Width = 30;
                    ws.Column(col: 9).Width = 30;
                    ws.Column(col: 10).Width = 15;
                    ws.Column(col: 11).Width = 14;
                    ws.Column(col: 12).Width = 12;

                    #region Header
                    ws.Cells[Address: "A1"].Value = $"Журнал регистрации обращений на {DateTime.Now.ToString("dd.MM.yyyy")}";
                    ws.Cells[Address: "A1:L1"].Merge = true;

                    ws.Row(row: 1).Style.Font.Name = "Times New Roman";
                    ws.Row(row: 1).Style.Font.Size = 16;
                    ws.Row(row: 2).Style.Font.Name = "Times New Roman";
                    ws.Row(row: 2).Style.Font.Size = 12;

                    ws.Cells[Address: "A2:L2"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Row(row: 1).Style.Font.Bold = true;
                    ws.Row(row: 2).Style.Font.Bold = true;
                    ws.Row(row: 1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(row: 2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    #endregion

                    package.Save();

                    //w/o using make sure to close excel: package.Dispose();
                }
            }

            //SaveFileDialog sfD = new SaveFileDialog();
            //sfD.DefaultExt = ".xlsx";
            //sfD.Filter = "Файл Excel (.xlsx)|*.xlsx";
            //sfD.FileName = $"ЖРЖ (на {DateTime.Now.ToString("dd.MM.yy")})";

            //if (sfD.ShowDialog() == true)
            //{
            //    var path = new FileInfo(sfD.FileName);

            //    CreateComplaintsSpreadSheet(collection, path);
            //}
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
