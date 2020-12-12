using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Syncfusion.XlsIO;

namespace Fred.Business.RepriseDonnees.Personnel.ExcelDataExtractor
{
    /// <summary>
    /// Service pour extraire les Personnels du fichier excel
    /// </summary>
    public class PersonnelExtractorService : IPersonnelExtractorService
    {
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;

        /// <summary>
        ///  Récupération des Personnels
        /// </summary>
        /// <param name="excelStream">stream representant le fichier excel</param>   
        /// <returns>resultat du parsage</returns>
        public ParsePersonnelResult ParseExcelFile(Stream excelStream)
        {
            ParsePersonnelResult result = new ParsePersonnelResult();
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];

                    IRange[] rows = sheet.Rows;

                    result.Personnels.AddRange(ParseRows(sheet, rows));
                }
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(BusinessResources.RepriseDonnees_Erreur_Extraction_Personnel, ex);
            }

            return result;
        }

        private IEnumerable<RepriseExcelPersonnel> ParseRows(IWorksheet sheet, IRange[] rows)
        {
            List<RepriseExcelPersonnel> result = new List<RepriseExcelPersonnel>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows.Length; i++)
            {
                IRange row = sheet.Rows[i];

                RepriseExcelPersonnel excelPersonnelModel = ParseRow(row);

                // Si au moins une valeur est renseigné on traite la ligne
                if (!excelPersonnelModel.CodeSociete.IsNullOrEmpty()
                    || !excelPersonnelModel.Matricule.IsNullOrEmpty()
                    || !excelPersonnelModel.TypePersonnel.IsNullOrEmpty()
                    || !excelPersonnelModel.Nom.IsNullOrEmpty()
                    || !excelPersonnelModel.Prenom.IsNullOrEmpty()
                    || !excelPersonnelModel.TypePointage.IsNullOrEmpty()
                    || !excelPersonnelModel.DateEntree.IsNullOrEmpty()
                    || !excelPersonnelModel.DateSortie.IsNullOrEmpty()
                    || !excelPersonnelModel.CodeRessource.IsNullOrEmpty()
                    || !excelPersonnelModel.Email.IsNullOrEmpty()
                    || !excelPersonnelModel.Adresse1.IsNullOrEmpty()
                    || !excelPersonnelModel.Adresse2.IsNullOrEmpty()
                    || !excelPersonnelModel.Adresse3.IsNullOrEmpty()
                    || !excelPersonnelModel.CodePostal.IsNullOrEmpty()
                    || !excelPersonnelModel.Ville.IsNullOrEmpty()
                    || !excelPersonnelModel.CodePays.IsNullOrEmpty()
                    || !excelPersonnelModel.Tel1.IsNullOrEmpty()
                    || !excelPersonnelModel.Tel2.IsNullOrEmpty())
                {
                    result.Add(excelPersonnelModel);
                }
            }
            return result;
        }

        /// <summary>
        /// Convertit une ligne excel en RepriseExcelPersonnel
        /// </summary>
        /// <param name="row">IRange representant le Personnel</param>        
        /// <returns>Un RepriseExcelPersonnel qui represente un Personnel sur le fichier excel</returns>
        private RepriseExcelPersonnel ParseRow(IRange row)
        {
            return new RepriseExcelPersonnel()
            {
                NumeroDeLigne = row.Cells[0].Value,
                CodeSociete = row.Cells[1].Value,
                Matricule = row.Cells[2].Value,
                TypePersonnel = row.Cells[3].Value,
                Nom = row.Cells[4].Value,
                Prenom = row.Cells[5].Value,
                TypePointage = row.Cells[6].Value,
                DateEntree = row.Cells[7].Value,
                DateSortie = row.Cells[8].Value,
                CodeRessource = row.Cells[9].Value,
                Email = row.Cells[10].Value,
                Adresse1 = row.Cells[11].Value,
                Adresse2 = row.Cells[12].Value,
                Adresse3 = row.Cells[13].Value,
                CodePostal = row.Cells[14].Value,
                Ville = row.Cells[15].Value,
                CodePays = row.Cells[16].Value,
                Tel1 = row.Cells[17].Value,
                Tel2 = row.Cells[18].Value
            };
        }
    }
}
