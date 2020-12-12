using System;
using System.Collections.Generic;
using System.IO;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Entities.Utilisateur;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.EtatPaie;
using Syncfusion.XlsIO;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Manager de l'édition Controle des pontages
    /// </summary>
    public class ListeAbsencesMensuellesManager : IListeAbsencesMensuellesManager
    {
        private readonly IPersonnelManager personnelManager;
        private readonly IImageManager imageManager;

        /// <summary>
        /// Permet de gérer les états paie 
        /// </summary>
        public ListeAbsencesMensuellesManager(IPersonnelManager personnelManager, IImageManager imageManager)
        {
            this.personnelManager = personnelManager;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listeAbsencesMensuels">Liste des models de pointage mensuel</param>
        /// <param name="user">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateMemoryStreamListeAbsencesMensuelles(EtatPaieExportModel etatPaieExportModel, List<AbsenceLigne> listeAbsencesMensuels, UtilisateurEnt user, string templateFolderPath)
        {
            string pathName = Path.Combine(templateFolderPath, "TemplateAbsencesListV2.xlsx");
            ExcelFormat excelFormat = new ExcelFormat();

            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("listeAbsencesMensuelsModel", listeAbsencesMensuels);
            excelFormat.ApplyVariables();

            workbook.Worksheets[0].View = SheetView.Normal;
            var startRow = 3;
            var lastRow = listeAbsencesMensuels.Count + startRow;
            workbook.Worksheets[0].Range["B2:H" + lastRow].AutofitColumns();

            var editeur = personnelManager.GetPersonnel(user.UtilisateurId);
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path;
            string dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Interimaire_Export__Espace + DateTime.Now.ToShortTimeString();
            string editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + editeur.CodeNomPrenom;
            string periode = BusinessResources.Pointage_Interimaire_Export_Periode + new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).ToString("MM/yyyy");
            var buildHeaderModel = new BuildHeaderExcelModel(FeatureRapport.Rapport_Absences_Titre, null, dateEdition, editePar, periode, pathLogo, new IndexHeaderExcelModel(2, 5, 7, 8));
            excelFormat.BuildHeader(workbook.Worksheets[0], buildHeaderModel);
            foreach (var sheet in workbook.Worksheets)
            {
                foreach (var row in sheet.Rows)
                {
                    row.WrapText = true;
                }
            }

            MemoryStream stream = excelFormat.GeneratePdfOrExcel(etatPaieExportModel.Pdf, workbook);
            excelFormat.Dispose();
            return stream;
        }
    }
}
