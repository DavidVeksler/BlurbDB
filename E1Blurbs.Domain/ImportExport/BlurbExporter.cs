using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E1Blurbs.Data;
using E1Blurbs.DataAccess;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace E1Blurbs.Domain.ImportExport
{
    public class BlurbExporter
    {
        private readonly BlurbDbUnitofWork _uow;

        public BlurbExporter(BlurbDbUnitofWork uow)
        {
            _uow = uow;


        }

        public ExcelPackage GenerateExcel(int areaId, out string areaName)
        {
            var area = _uow.AreaRepository.First(a => a.CategoryId == areaId);
            areaName = area.CategoryName;

            var blurbs = _uow.BlurbRepository.Find(b => b.CategoryId == areaId || b.CategoryId == area.ParentCategoryId).Take(5000);

            var rowList = new List<ExcelRowModel>();

            blurbs.ToList().ForEach(blurb =>
                {
                    var indonesian = blurb.Translations.FirstOrDefault(f => f.LanguageCode == Constants.CultureCodes.Indonesian);
                    var chinese = blurb.Translations.FirstOrDefault(f => f.LanguageCode == Constants.CultureCodes.Chinese);
                    var russian = blurb.Translations.FirstOrDefault(f => f.LanguageCode == Constants.CultureCodes.Russian);
                    var spanish = blurb.Translations.FirstOrDefault(f => f.LanguageCode == Constants.CultureCodes.Spanish);

                    rowList.Add(new ExcelRowModel()
                            {
                                English = blurb.Description,

                                AreaName = (blurb.Category != null) ? blurb.Category.CategoryName : null,
                                AreaId = blurb.CategoryId,

                                BlurbId = blurb.BlurbId,

                                Indonesian = indonesian != null ? indonesian.Text : "",
                                Chinese = chinese != null ? chinese.Text : "",
                                Russian = russian != null ? russian.Text : "",
                                Spanish = spanish != null ? spanish.Text : "",

                            });
                });

            return DumpExcel(rowList);

            // generate excel export



        }

        public ExcelPackage DumpExcel(IEnumerable<ExcelRowModel> tbl)
        {
            ExcelPackage pck = new ExcelPackage();

            //Create the worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Blurbs");

            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
            ws.Cells["A1"].LoadFromCollection(tbl, true);
            //ws.Cells["A1"].LoadFromDataTable(tbl, true);

            //Format the header for column 1-3
            using (ExcelRange rng = ws.Cells["A1:H1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                rng.Style.Font.Color.SetColor(Color.White);
            }

            //ws.Column(0).Hidden = true;

            using (ExcelRange rng = ws.Cells["D1:H1"])
            {
                rng.AutoFitColumns(40,100);
            }

            //Example how to Format Column 1 as numeric 
            using (ExcelRange col = ws.Cells[2, 1, 2 + tbl.ToList().Count(), 1])
            {
                col.Style.Numberformat.Format = "#,##0.00";
                col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }

            //using (ExcelRange col = ws.Cells[2, 3, 2 + tbl.ToList().Count(), 1])
            //{
            //    col.Style.Numberformat.Format = "#,##0.00";
            //    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //}

            return pck;
        }
    }
}

