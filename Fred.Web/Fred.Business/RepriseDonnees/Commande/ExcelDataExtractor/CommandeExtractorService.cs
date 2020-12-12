using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.Commande.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les Commandes du fichier excel
    /// </summary>
    public class CommandeExtractorService : ICommandeExtractorService
    {
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///   Parse le fichier excel
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Le resultat du parsage</returns>
        public ParseCommandesResult ParseExcelFile(Stream excelStream)
        {
            var result = new ParseCommandesResult();
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

        private List<RepriseExcelCommande> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            var result = new List<RepriseExcelCommande>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                var row = sheet.Rows[i];

                var excelCiModel = ParseRow(row);

                if (!excelCiModel.CodeCi.IsNullOrEmpty() && !excelCiModel.CodeSociete.IsNullOrEmpty() && !excelCiModel.NumeroCommandeExterne.IsNullOrEmpty())
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
        private RepriseExcelCommande ParseRow(IRange line)
        {
            return new RepriseExcelCommande()
            {
                NumeroDeLigne = line.Cells[0].Value,
                NumeroCommandeExterne = line.Cells[1].Value,
                CodeSociete = line.Cells[2].Value,
                CodeCi = line.Cells[3].Value,
                CodeFournisseur = line.Cells[4].Value,
                TypeCommande = line.Cells[5].Value,
                LibelleEnteteCommande = line.Cells[6].Value,
                CodeDevise = line.Cells[7].Value,
                DateCommande = line.Cells[8].Value,
                DesignationLigneCommande = line.Cells[9].Value,
                CodeRessource = line.Cells[10].Value,
                CodeTache = line.Cells[11].Value,
                Unite = line.Cells[12].Value,
                PuHt = line.Cells[13].Value,
                QuantiteCommandee = line.Cells[14].Value,
                QuantiteReceptionnee = line.Cells[15].Value,
                QuantiteFactureeRapprochee = line.Cells[16].Value,
                DateReception = line.Cells[17].Value,
                Far = line.Cells[18].Value,
            };
        }
    }
}
