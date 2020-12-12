using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.Rapport.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les rapports du fichier excel
    /// </summary>
    public class RapportExtractorService : IRapportExtractorService
    {

        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///   Récupération des rapports
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des personnels</returns>
        public ParseRapportsResult ParseExcelFile(Stream excelStream)
        {
            var result = new ParseRapportsResult();
            try
            {

                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    var rows = sheet.Rows;

                    result.Rapports.AddRange(ParseRows(sheet, rows));
                }

            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Cis, ex);
            }

            return result;
        }

        private List<RepriseExcelRapport> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            var result = new List<RepriseExcelRapport>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                var row = sheet.Rows[i];

                var excelRapportModel = ParseRow(row);

                if (!excelRapportModel.CodeCi.IsNullOrEmpty() && !excelRapportModel.CodeSocieteCi.IsNullOrEmpty())
                {
                    result.Add(excelRapportModel);
                }

            }
            return result;
        }

        /// <summary>
        /// Convertie une ligne excel en RepriseExcelRapport
        /// </summary>
        /// <param name="line">IRange represantant le rapport</param>        
        /// <returns>un RepriseExcelRapport qui represente un ci sur le fichier excel</returns>
        private RepriseExcelRapport ParseRow(IRange line)
        {
            return new RepriseExcelRapport()
            {
                NumeroDeLigne = line.Cells[0].Value,
                CodeSocieteCi = line.Cells[1].Value,
                CodeCi = line.Cells[2].Value,
                DateRapport = line.Cells[3].Value,
                CodeSocietePersonnel = line.Cells[4].Value,
                MatriculePersonnel = line.Cells[5].Value,
                CodeDeplacement = line.Cells[6].Value,
                CodeZoneDeplacement = line.Cells[7].Value,
                IVD = line.Cells[8].Value,
                HeuresTotal = line.Cells[9].Value,
                CodeTache = line.Cells[10].Value
            };
        }

    }
}
