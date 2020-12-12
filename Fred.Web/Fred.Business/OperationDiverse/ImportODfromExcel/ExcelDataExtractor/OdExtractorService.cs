using System;
using System.IO;
using Fred.Entities.OperationDiverse.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les operations diverses du fichier excel
    /// </summary>
    public class OdExtractorService : IOdExtractorService
    {
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;
        private const int ExcelFileColumnCount = 13;

        /// <summary>
        ///   Récupération des OperationsDiverses
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des OperationsDiverses</returns>
        public ParseODsResult ParseExcelFile(Stream excelStream)
        {
            ParseODsResult result = new ParseODsResult();
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    if (sheet.Columns.Length == ExcelFileColumnCount)
                    {
                        IRange[] rows = sheet.Rows;
                        result = ParseRows(sheet, rows);
                    }
                    else
                    {
                        result.ErrorMessages.Add(BusinessResources.ImportOdFileErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Cis, ex);
            }

            return result;
        }

        private ParseODsResult ParseRows(IWorksheet sheet, IRange[] rows)
        {
            ParseODsResult result = new ParseODsResult();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                IRange row = sheet.Rows[i];

                bool isEmptyRow = row.IsBlank || IsEmptyRow(row);

                if (!isEmptyRow)
                {
                    ExcelOdModel excelRapportModel = ParseRow(row);

                    if (!excelRapportModel.CodeCi.IsNullOrEmpty() && !excelRapportModel.CodeSociete.IsNullOrEmpty())
                    {
                        result.OperationsDiverses.Add(excelRapportModel);
                    }
                    else
                    {
                        result.ErrorMessages.Add(string.Format(BusinessResources.ImportCIAndSocieteRequiredErrorMessage, i.ToString()));
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Convertie une ligne excel en ExcelOdModel (operation diverse)
        /// </summary>
        /// <param name="line">IRange represantant l'opération diverse</param>        
        /// <returns>un ExcelOdModel qui represente une od sur le fichier excel</returns>
        private ExcelOdModel ParseRow(IRange line)
        {
            return new ExcelOdModel()
            {
                NumeroDeLigne = line.Cells[0].Value,
                CodeSociete = line.Cells[1].Value,
                CodeCi = line.Cells[2].Value,
                Libelle = line.Cells[3].Value,
                Quantite = line.Cells[4].Value,
                PuHT = line.Cells[5].Value,
                CodeUnite = line.Cells[6].Value,
                CodeDevise = line.Cells[7].Value,
                DateComptable = line.Cells[8].Value,
                CodeFamille = line.Cells[9].Value,
                CodeRessource = line.Cells[10].Value,
                CodeTache = line.Cells[11].Value,
                Commentaire = line.Cells[12].Value
            };
        }

        private static bool IsEmptyRow(IRange row)
        {
            for (int i = 1; i < row.Columns.Length; i++)
            {
                if (!string.IsNullOrEmpty(row.Cells[i].Value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
