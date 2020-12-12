using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.Materiel.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire des Materiels du fichier excel
    /// </summary>
    public class MaterielExtractorService : IMaterielExtractorService
    {
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///  Récupération des Materiels
        /// </summary>
        /// <param name="excelStream">stream representant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        public ParseMaterielResult ParseExcelFile(Stream excelStream)
        {
            ParseMaterielResult result = new ParseMaterielResult();
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    IRange[] rows = sheet.Rows;

                    result.Materiels.AddRange(ParseRows(sheet, rows));
                }
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Materiel, ex);
            }

            return result;
        }

        private IEnumerable<RepriseExcelMateriel> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            List<RepriseExcelMateriel> result = new List<RepriseExcelMateriel>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                IRange row = sheet.Rows[i];

                RepriseExcelMateriel excelMaterielModel = ParseRow(row);

                // Si au moins une valeur est renseigné on traite la ligne
                if (!excelMaterielModel.CodeSociete.IsNullOrEmpty()
                    || !excelMaterielModel.CodeMateriel.IsNullOrEmpty()
                    || !excelMaterielModel.LibelleMateriel.IsNullOrEmpty()
                    || !excelMaterielModel.CodeRessource.IsNullOrEmpty())
                {
                    result.Add(excelMaterielModel);
                }
            }
            return result;
        }

        /// <summary>
        /// Convertit une ligne excel en RepriseExcelMateriel
        /// </summary>
        /// <param name="row">IRange representant le Materiel</param>        
        /// <returns>Un RepriseExcelMateriel qui represente un Materiel sur le fichier excel</returns>
        private RepriseExcelMateriel ParseRow(IRange row)
        {
            return new RepriseExcelMateriel()
            {
                NumeroDeLigne = row.Cells[0].Value,
                CodeSociete = row.Cells[1].Value,
                CodeMateriel = row.Cells[2].Value,
                LibelleMateriel = row.Cells[3].Value,
                CodeRessource = row.Cells[4].Value
            };
        }
    }
}
