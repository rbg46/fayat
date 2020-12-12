using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.ValidationCommande.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les validation de Commandes du fichier excel
    /// </summary>
    public class ValidationCommandeExtractorService : IValidationCommandeExtractorService
    {
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///   Parse le fichier excel
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Le resultat du parsage</returns>
        public ParseValidationCommandesResult ParseExcelFile(Stream excelStream)
        {
            var result = new ParseValidationCommandesResult();
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    var rows = sheet.Rows;

                    result.Commandes.AddRange(ParseRows(sheet, rows));
                }
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_CommandeReceptions, ex);
            }

            return result;
        }

        private List<RepriseExcelValidationCommande> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            var result = new List<RepriseExcelValidationCommande>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                var row = sheet.Rows[i];

                var excelModel = ParseRow(row);

                if (!excelModel.NumeroCommandeExterne.IsNullOrEmpty())
                {
                    result.Add(excelModel);
                }
            }
            return result;
        }

        /// <summary>
        /// Convertie une ligne excel en RepriseExcelValidationCommande
        /// </summary>
        /// <param name="line">IRange represantant le ci</param>        
        /// <returns>un RepriseExcelCi qui represente un ci sur le fichier excel</returns>
        private RepriseExcelValidationCommande ParseRow(IRange line)
        {
            return new RepriseExcelValidationCommande()
            {
                NumeroDeLigne = line.Cells[0].Value,
                NumeroCommandeExterne = line.Cells[1].Value
            };
        }
    }
}
