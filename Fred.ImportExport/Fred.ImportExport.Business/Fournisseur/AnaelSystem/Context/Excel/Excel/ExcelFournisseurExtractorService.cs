using System;
using System.Collections.Generic;
using System.IO;
using Fred.Business;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Models;
using Syncfusion.XlsIO;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel
{
    /// <summary>
    /// Parse le fichier excel pour l'import des ci par excel
    /// </summary>
    public class ExcelFournisseurExtractorService : IExcelFournisseurExtractorService
    {

        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///  Parse le fichier excel
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des personnels</returns>
        public ParseImportFournisseursResult ParseExcelFile(Stream excelStream)
        {
            var result = new ParseImportFournisseursResult();
            try
            {

                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    var rows = sheet.Rows;

                    result.Fournisseurs.AddRange(ParseRows(sheet, rows));
                }

            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Import_Fournisseurs, ex);
            }

            return result;
        }

        private List<RepriseImportFournisseur> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            var result = new List<RepriseImportFournisseur>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                var row = sheet.Rows[i];

                var excelFournisseurModel = ParseRow(row);

                if (!excelFournisseurModel.CodeFournisseur.IsNullOrEmpty() && !excelFournisseurModel.TypeSequence.IsNullOrEmpty())
                {
                    result.Add(excelFournisseurModel);
                }

            }
            return result;
        }

        /// <summary>
        /// Convertie une ligne excel en RepriseExcelCi
        /// </summary>
        /// <param name="line">IRange represantant le ci</param>        
        /// <returns>un RepriseExcelCi qui represente un ci sur le fichier excel</returns>
        private RepriseImportFournisseur ParseRow(IRange line)
        {
            return new RepriseImportFournisseur()
            {
                NumeroDeLigne = line.Cells[0].Value,
                CodeFournisseur = line.Cells[1].Value,
                TypeSequence = line.Cells[2].Value,
            };
        }

    }
}
