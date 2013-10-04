using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E1Blurbs.Data;
using E1Blurbs.DataAccess;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace E1Blurbs.Domain.ImportExport
{
    public class BlurbImporter
    {
        private readonly BlurbDbUnitofWork _uow;

        public BlurbImporter(BlurbDbUnitofWork uow)
        {
            _uow = uow;


        }


        public string ProcessImportedBlurbs(Stream data, int ParentCategoryId)
        {
            int processedBlurbs = 0;
            int newTranslations = 0;

            int productId;

            if (ParentCategoryId > 0)
            {
                productId = _uow.AreaRepository.Find(a => a.CategoryId == ParentCategoryId).FirstOrDefault().ProductId;
            }
            else
            {
                throw new Exception("Parent area is required");
            }

            ExcelPackage pck = new ExcelPackage();

            pck.Load(data);

            var workBook = pck.Workbook;

            List<ExcelRowModel> rows = new List<ExcelRowModel>();

            if (pck.Workbook != null)
            {
                if (workBook.Worksheets.Count > 0)
                {
                    // Get the first worksheet
                    ExcelWorksheet currentWorksheet = workBook.Worksheets.First();

                    rows = ConvertSheetToModelList(currentWorksheet);

                    rows.ForEach(row => newTranslations += SaveCurrentRow(row, ParentCategoryId, productId));

                    processedBlurbs += _uow.SaveChanges();
                }
            }

            return string.Format("{0} rows processed; {1} rows updated; {2} new translations.", rows.Count, processedBlurbs, newTranslations);
        }

        private static List<ExcelRowModel> ConvertSheetToModelList(ExcelWorksheet currentWorksheet)
        {
            List<ExcelRowModel> rows = new List<ExcelRowModel>();

            object col1Header = currentWorksheet.Cells[1, 1].Value;

            if (col1Header == null || col1Header.ToString() != "AreaName")
            {
                throw new Exception("Imported excel template format not correct. You must keep the format of the exported template");
            }


            // read each row from the start of the data (start row + 1 header row) to the end of the spreadsheet.
            for (int rowNumber = 2; rowNumber <= currentWorksheet.Dimension.End.Row; rowNumber++)
            {
                if (currentWorksheet.Cells[rowNumber, 4].Value == null) continue; // skip rows with no English text

                var rowModel = new ExcelRowModel { English = currentWorksheet.Cells[rowNumber, 4].Value.ToString() };

                if (currentWorksheet.Cells[rowNumber, 1].Value != null)
                    rowModel.AreaName = currentWorksheet.Cells[rowNumber, 1].Value.ToString();

                if (currentWorksheet.Cells[rowNumber, 2].Value != null)
                    rowModel.AreaId = Convert.ToInt32(currentWorksheet.Cells[rowNumber, 2].Value);

                if (currentWorksheet.Cells[rowNumber, 3].Value != null)
                    rowModel.BlurbId = Convert.ToInt32(currentWorksheet.Cells[rowNumber, 3].Value);

                if (currentWorksheet.Cells[rowNumber, 5].Value != null)
                    rowModel.Indonesian = currentWorksheet.Cells[rowNumber, 5].Value.ToString();

                if (currentWorksheet.Cells[rowNumber, 6].Value != null)
                    rowModel.Chinese = currentWorksheet.Cells[rowNumber, 6].Value.ToString();

                if (currentWorksheet.Cells[rowNumber, 7].Value != null)
                    rowModel.Russian = currentWorksheet.Cells[rowNumber, 7].Value.ToString();

                if (currentWorksheet.Cells[rowNumber, 8].Value != null)
                    rowModel.Spanish = currentWorksheet.Cells[rowNumber, 8].Value.ToString();

                rows.Add(rowModel);
            }

            return rows;
        }


        private int SaveCurrentRow(ExcelRowModel row, int ParentCategoryId, int productId)
        {
            int translationCount = 0;

            // if areaid is 0, add new area
            if (row.AreaId == 0)
            {
                //row.AreaId = _uow.GetMaxId(typeof(Category));

                _uow.AreaRepository.Add(row.AreaId, row.AreaName, ParentCategoryId, productId);
            }
            else // othwerwise update area
            {
                var area = _uow.AreaRepository.First(a => a.CategoryId == row.AreaId);
                area.CategoryName = row.AreaName;
            }

            // if blurb id is 0, add new blurb

            if (row.BlurbId == 0)
            {
                _uow.BlurbRepository.Add(row.BlurbId, row.AreaId, row.English);
            }
            else // otherwise update blurb text
            {
                var Blurb = _uow.BlurbRepository.First(a => a.BlurbId == row.BlurbId);
                Blurb.Description = row.English;
            }

            // if translation is provided, add translation
            translationCount += AddOrUpdateTranslation(row, productId, row.Indonesian, Constants.CultureCodes.Indonesian);
            translationCount += AddOrUpdateTranslation(row, productId, row.Russian, Constants.CultureCodes.Russian);
            translationCount += AddOrUpdateTranslation(row, productId, row.Chinese, Constants.CultureCodes.Chinese);
            translationCount += AddOrUpdateTranslation(row, productId, row.Spanish, Constants.CultureCodes.Spanish);

            return translationCount;
        }

        private int AddOrUpdateTranslation(ExcelRowModel row, int productId, string translatedText, string cultureCode)
        {
            if (!string.IsNullOrWhiteSpace(translatedText))
            {
                Translation translation = _uow.TranslationRepository.Find(
                    t => t.BlurbId == row.BlurbId && t.LanguageCode == cultureCode)
                                              .FirstOrDefault();

                if (translation == null)
                {
                    int cultureId = _uow.Cultures.First(c => c.CultureCode == cultureCode).CultureId;

                    _uow.TranslationRepository.Add(_uow.GetMaxId(typeof(Translation)), row.BlurbId,
                                                   cultureCode, productId, translatedText, cultureId);
                    return 1;
                }
                translation.Text = translatedText;
            }
            return 0;
        }
    }
}

