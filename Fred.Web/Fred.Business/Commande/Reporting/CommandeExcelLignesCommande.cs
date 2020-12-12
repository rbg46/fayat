using Fred.Business.Commande.Models;
using Fred.Framework.Exceptions;
using Fred.Framework.Reporting;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.App_LocalResources;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fred.Business.Commande.Reporting
{
    /// <summary>
    /// Class Commande Excel Lignes Commande
    /// </summary>
    public class CommandeExcelLignesCommande
    {
        private readonly string excelTemplate = "TemplateFichierExempleCommandeLignes.xlsx";
        private const int NumeroDeLigneDeDepartSurFichierExcel = 2;
        private const int NumColumDiminution = 9;

        /// <summary>
        /// Generer un Fichier Exemple de commande MLignes
        /// </summary>
        /// <param name="ressources">liste des ressources</param>
        /// <param name="taches">Listes des taches</param>
        /// <param name="unities">liste unité</param>
        /// <param name="isAvenant">Type Avenant</param>
        /// <returns>File en bytes</returns>
        public byte[] CreateExempleCommandeLignes(List<RessourceModel> ressources, List<TacheModel> taches, List<UniteModel> unities, bool isAvenant)
        {
            try
            {
                using (var excelFormat = new ExcelFormat())
                {
                    string pathName = AppDomain.CurrentDomain.BaseDirectory + @"Templates\Commande\" + excelTemplate;
                    IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
                    // Create Template Marker Processor
                    ITemplateMarkersProcessor marker = workbook.CreateTemplateMarkersProcessor();

                    if (!isAvenant)
                    {
                        //ne pas changer l'ordre
                        workbook.Worksheets[0].DeleteColumn(NumColumDiminution); //delete colonne diminution
                        workbook.Worksheets[0].DeleteColumn(2);//delete colone numéro commande
                    }

                    marker.AddVariable(typeof(RessourceModel).Name, ressources);
                    marker.AddVariable(typeof(TacheModel).Name, taches);
                    marker.AddVariable(typeof(UniteModel).Name, unities);
                    marker.ApplyMarkers();

                    // retourne le fichier sous forme de bytes
                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        var bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);
                        workbook.Close();
                        return bytes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredTechnicalException(ex.Message, ex);
            }
        }

        /// <summary>
        ///   Parse le fichier excel
        /// </summary>
        /// <param name="excelStream">stream represantant le fichier excel</param>
        /// <param name="isAvenant">Is Avenant</param>   
        /// <returns>Le resultat du parsage</returns>
        public List<ExcelLigneCommandeModel> ParseExcelFile(Stream excelStream, bool isAvenant)
        {
            var result = new List<ExcelLigneCommandeModel>();
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(excelStream);

                    IWorksheet sheet = workbook.Worksheets[0];
                    int lastrows = sheet.UsedRange.LastRow;
                    result.AddRange(ParseRows(sheet, lastrows, isAvenant));
                }
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(FeatureExportExcel.Invalid_FileFormat, ex);
            }

            return result;
        }

        private List<ExcelLigneCommandeModel> ParseRows(IWorksheet sheet, int rows, bool isAvenant)
        {
            var result = new List<ExcelLigneCommandeModel>();

            for (int i = NumeroDeLigneDeDepartSurFichierExcel; i < rows; i++)
            {
                var row = sheet.Rows[i];

                var excelModel = ParseRow(row, isAvenant);

                if (!IsLigneEmty(row))
                {
                    result.Add(excelModel);
                }
            }
            return result;
        }

        /// <summary>
        /// Convertie une ligne excel en RepriseExcelCi
        /// </summary>
        /// <param name="line">IRange represantant le ci</param>
        /// <param name="isAvenant">Is Avenant</param>        
        /// <returns>un RepriseExcelCi qui represente un ci sur le fichier excel</returns>
        private ExcelLigneCommandeModel ParseRow(IRange line, bool isAvenant)
        {
            int i = 0;
            return new ExcelLigneCommandeModel()
            {
                //Ps:DisplayText prend la valeur afficher dans la cellule par contre Value prend la formule dans un fichier excel
                //il faut mieux prendre DisplayText
                NumeroDeLigne = line.Cells[i++].DisplayText,
                NumeroComande = isAvenant ? line.Cells[i++].DisplayText : null,
                DesignationLigneCommande = line.Cells[i++].DisplayText,
                CodeRessource = line.Cells[i++].DisplayText,
                CodeTache = line.Cells[i++].DisplayText,
                Unite = line.Cells[i++].DisplayText,
                PuHt = line.Cells[i++].DisplayText,
                QuantiteCommande = line.Cells[i++].DisplayText,
                IsDiminution = isAvenant
                    ? line.Cells[i].DisplayText
                    : string.Empty
            };
        }

        private bool IsLigneEmty(IRange row)
        {
            for (int i = 1; i < row.Columns.Length - 1; i++)
            {
                if (row.Cells[i].DisplayText != string.Empty)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
