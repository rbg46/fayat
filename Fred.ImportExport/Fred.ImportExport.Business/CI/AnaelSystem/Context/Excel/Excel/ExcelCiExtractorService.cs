using System;
using System.Collections.Generic;
using System.IO;
using Fred.Business;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.CI.AnaelSystem.Models;
using Syncfusion.XlsIO;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Excel
{
    /// <summary>
    /// Parse le fichier excel pour l'import des ci par excel
    /// </summary>
    public class ExcelCiExtractorService : IExcelCiExtractorService
    {

        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///  Parse le fichier excel
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des personnels</returns>
        public ParseImportCisResult ParseExcelFile(Stream excelStream)
        {
            var result = new ParseImportCisResult();
            try
            {

                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    var rows = sheet.Rows;

                    result.Cis.AddRange(ParseRows(sheet, rows));
                }

            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Import_Cis, ex);
            }

            return result;
        }

        private List<RepriseImportCi> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            var result = new List<RepriseImportCi>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                var row = sheet.Rows[i];

                var excelCiModel = ParseRow(row);

                if (!excelCiModel.CodeCi.IsNullOrEmpty() && !excelCiModel.CodeSociete.IsNullOrEmpty())
                {
                    result.Add(excelCiModel);
                }

            }
            return result;
        }

        /// <summary>
        /// Convertie une ligne excel en RepriseExcelCi
        /// </summary>
        /// <param name="line">IRange represantant le ci</param>        
        /// <returns>un RepriseExcelCi qui represente un ci sur le fichier excel</returns>
        private RepriseImportCi ParseRow(IRange line)
        {
            return new RepriseImportCi()
            {
                NumeroDeLigne = line.Cells[0].Value,
                CodeSociete = line.Cells[1].Value,
                CodeCi = line.Cells[2].Value,
            };
        }

    }
}
