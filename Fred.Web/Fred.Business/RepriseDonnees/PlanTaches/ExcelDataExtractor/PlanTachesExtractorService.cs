using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.PlanTaches.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les Plans de tâches du fichier excel
    /// </summary>
    public class PlanTachesExtractorService : IPlanTachesExtractorService
    {
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///  Récupération des Plans de tâches
        /// </summary>
        /// <param name="excelStream">stream representant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        public ParsePlanTachesResult ParseExcelFile(Stream excelStream)
        {
            ParsePlanTachesResult result = new ParsePlanTachesResult();
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    IRange[] rows = sheet.Rows;

                    result.PlanTaches.AddRange(ParseRows(sheet, rows));
                }
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_PlanTaches, ex);
            }

            return result;
        }

        private IEnumerable<RepriseExcelPlanTaches> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            List<RepriseExcelPlanTaches> result = new List<RepriseExcelPlanTaches>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                IRange row = sheet.Rows[i];

                RepriseExcelPlanTaches excelPlanTachesModel = ParseRow(row);

                // Si au moins une valeur est renseignée on traite la ligne
                if (!excelPlanTachesModel.CodeCi.IsNullOrEmpty()
                    || !excelPlanTachesModel.CodeSociete.IsNullOrEmpty()
                    || !excelPlanTachesModel.NiveauTache.IsNullOrEmpty()
                    || !excelPlanTachesModel.CodeTache.IsNullOrEmpty()
                    || !excelPlanTachesModel.LibelleTache.IsNullOrEmpty()
                    || !excelPlanTachesModel.CodeTacheParent.IsNullOrEmpty()
                    || !excelPlanTachesModel.T3ParDefaut.IsNullOrEmpty())
                {
                    result.Add(excelPlanTachesModel);
                }
            }
            return result;
        }

        /// <summary>
        /// Convertit une ligne excel en RepriseExcelPlanTaches
        /// </summary>
        /// <param name="row">IRange representant la tache</param>   
        /// <returns>Un RepriseExcelPlanTaches qui represente une tache sur le fichier excel</returns>
        private RepriseExcelPlanTaches ParseRow(IRange row)
        {
            return new RepriseExcelPlanTaches()
            {
                NumeroDeLigne = row.Cells[0].Value,
                CodeSociete = row.Cells[1].Value,
                CodeCi = row.Cells[2].Value,
                NiveauTache = row.Cells[3].Value,
                CodeTache = row.Cells[4].Value,
                LibelleTache = row.Cells[5].Value,
                CodeTacheParent = row.Cells[6].Value,
                T3ParDefaut = row.Cells[7].Value
            };
        }
    }
}
