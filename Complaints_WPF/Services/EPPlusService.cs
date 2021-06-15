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
        /// <summary>Writes given collection of complaints to Excel file</summary>
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

                CreateComplaintsSpreadSheet(file, collection);
            }
        }

        private static void CreateComplaintsSpreadSheet(FileInfo file, IList<Complaint> collection)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Обращения");
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells[Address: "B2"].Value = "№";
                ws.Cells[Address: "C2"].Value = "Дата";
                ws.Cells[Address: "D2"].Value = "ФИО Заявителя";
                ws.Cells[Address: "E2"].Value = "Содержание обращения";
                ws.Cells[Address: "I2"].Value = "Примечание";
                ws.Cells[Address: "J2"].Value = "Принял(а)";
                ws.Cells[Address: "K2"].Value = "Результат";
                ws.Cells[Address: "L2"].Value = "Руководитель";

                ws.Column(col: 2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(row: 1).Height = 30;
                ws.Row(row: 2).Height = 22;

                ExcelRangeBase range = ws.Cells[Address: "A3"].LoadFromCollection(collection);

                //using (ExcelRange excelRange = ws.Cells[3, 2, collection.Count-1,12])  //get ascending Num for collection
                //{
                //    excelRange.Sort();
                //}

                ws.Column(col: 5).Style.WrapText = true;
                ws.Column(col: 9).Style.WrapText = true;

                ws.Column(col: 1).Hidden = true;
                ws.Column(col: 6).Hidden = true;
                ws.Column(col: 7).Hidden = true;
                ws.Column(col: 8).Hidden = true;

                ws.Column(col: 2).Width = 5;
                ws.Column(col: 3).Style.Numberformat.Format = "dd.mm.yy hh:mm";
                ws.Column(col: 3).Width = 13;

                ws.Column(col: 4).Width = 30;
                ws.Column(col: 5).Width = 60;
                ws.Column(col: 9).Width = 40;
                ws.Column(col: 10).Width = 15;
                ws.Column(col: 11).Width = 12;
                ws.Column(col: 12).Width = 14;

                #region Header style
                ws.Cells[Address: "A1"].Value = $"Журнал регистрации жалоб на {DateTime.Now.ToString("dd.MM.yyyy")}";
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
            }
        }
    }
}
