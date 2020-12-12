using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire des Indemnités de Déplacement du fichier excel
    /// </summary>
    public class IndemniteDeplacementExtractorService : IIndemniteDeplacementExtractorService
    {
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///  Récupération des Indemnités de Déplacement
        /// </summary>
        /// <param name="excelStream">Stream representant le fichier excel</param>   
        /// <returns>Resultat du parsage</returns>
        public ParseIndemniteDeplacementResult ParseExcelFile(Stream excelStream)
        {
            ParseIndemniteDeplacementResult result = new ParseIndemniteDeplacementResult();
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    IRange[] rows = sheet.Rows;

                    result.IndemniteDeplacements.AddRange(ParseRows(sheet, rows));
                }
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Personnel, ex);
            }

            return result;
        }

        private IEnumerable<RepriseExcelIndemniteDeplacement> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            List<RepriseExcelIndemniteDeplacement> result = new List<RepriseExcelIndemniteDeplacement>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                IRange row = sheet.Rows[i];

                RepriseExcelIndemniteDeplacement excelIndemniteDeplacementModel = ParseRow(row);

                // Si au moins une valeur est renseigné on traite la ligne
                if (!excelIndemniteDeplacementModel.SocieteCI.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.CodeCI.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.SocietePersonnel.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.MatriculePersonnel.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.NbKlm.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.NbKlmVODomicileRattachement.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.NbKlmVODomicileChantier.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.NbKlmVOChantierRattachement.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.SocieteCodeDeplacement.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.CodeDeplacement.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.CodeZoneDeplacement.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.IVD.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.DateDernierCalcul.IsNullOrEmpty()
                    || !excelIndemniteDeplacementModel.SaisieManuelle.IsNullOrEmpty())
                {
                    result.Add(excelIndemniteDeplacementModel);
                }
            }
            return result;
        }

        /// <summary>
        /// Convertit une ligne excel en RepriseExcelIndemniteDeplacement
        /// </summary>
        /// <param name="row">IRange representant l'Indemnité de Déplacement</param>        
        /// <returns>Un RepriseExcelIndemniteDeplacement qui represente une Indemnité de Déplacement sur le fichier excel</returns>
        private RepriseExcelIndemniteDeplacement ParseRow(IRange row)
        {
            return new RepriseExcelIndemniteDeplacement()
            {
                NumeroDeLigne = row.Cells[0].Value,
                SocieteCI = row.Cells[1].Value,
                CodeCI = row.Cells[2].Value,
                SocietePersonnel = row.Cells[3].Value,
                MatriculePersonnel = row.Cells[4].Value,
                NbKlm = row.Cells[5].Value,
                NbKlmVODomicileRattachement = row.Cells[6].Value,
                NbKlmVODomicileChantier = row.Cells[7].Value,
                NbKlmVOChantierRattachement = row.Cells[8].Value,
                SocieteCodeDeplacement = row.Cells[9].Value,
                CodeDeplacement = row.Cells[10].Value,
                CodeZoneDeplacement = row.Cells[11].Value,
                IVD = row.Cells[12].Value,
                DateDernierCalcul = row.Cells[13].Value,
                SaisieManuelle = row.Cells[14].Value
            };
        }
    }
}
