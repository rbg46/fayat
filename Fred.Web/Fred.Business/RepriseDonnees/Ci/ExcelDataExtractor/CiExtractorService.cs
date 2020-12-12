using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.Ci.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les ci du fichier excel
    /// </summary>
    public class CiExtractorService : ICiExtractorService
    {

        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///   Récupération des personnels d'AS400 
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>   
        /// <returns>Liste des personnels</returns>
        public ParseCisResult ParseExcelFile(Stream excelStream)
        {
            var result = new ParseCisResult();
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
                if(ex is IndexOutOfRangeException)
                {
                    string errorMessage = BusinessResources.RepriseDonnees_Erreur_StructureFichierIncorrecte;
                    throw new FredBusinessException(errorMessage, ex);
                }

                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Cis, ex);
            }

            return result;
        }

        private List<RepriseExcelCi> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            var result = new List<RepriseExcelCi>();

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
        private RepriseExcelCi ParseRow(IRange line)
        {
            return new RepriseExcelCi()
            {
                NumeroDeLigne = line.Cells[0].Value,
                CodeSociete = line.Cells[1].Value,
                CodeCi = line.Cells[2].Value,
                Adresse = line.Cells[3].Value,
                Adresse2 = line.Cells[4].Value,
                Adresse3 = line.Cells[5].Value,
                Ville = line.Cells[6].Value,
                CodePostal = line.Cells[7].Value,
                CodePays = line.Cells[8].Text,
                EnteteLivraison = line.Cells[9].Value,
                AdresseLivraison = line.Cells[10].Value,
                CodePostalLivraison = line.Cells[11].Value,
                VilleLivraison = line.Cells[12].Value,
                CodePaysLivraison = line.Cells[13].Value,
                AdresseFacturation = line.Cells[14].Value,
                CodePostalFacturation = line.Cells[15].Value,
                VilleFacturation = line.Cells[16].Value,
                CodePaysFacturation = line.Cells[17].Value,
                MatriculeResponsableChantier = line.Cells[18].Value,
                MatriculeResponsableAdministratif = line.Cells[19].Value,
                ZoneModifiable = line.Cells[20].Value,
                DateOuverture = line.Cells[21].Value,
                FacturationEtablissement = line.Cells[22].Value,
                Longitude = line.Cells[23].Value,
                Latitude = line.Cells[24].Value
            };
        }

    }
}
