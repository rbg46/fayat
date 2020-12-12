using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.FeatureFlipping;
using Fred.Business.Images;
using Fred.Business.Organisation;
using Fred.Business.Personnel;
using Fred.Entities.Rapport;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.EtatPaie;
using Syncfusion.XlsIO;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Manager de l'édition Controle des pontages
    /// </summary>
    public class ListeHeuresSpecifiquesManager : IListeHeuresSpecifiquesManager
    {
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IOrganisationManager organisationManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IImageManager imageManager;

        /// <summary>
        /// Permet de gérer les états paie 
        /// </summary>
        /// <param name="featureFlippingManager">Manager de featureFlipping</param>
        /// <param name="imageManager">Manager d'image</param>
        /// <param name="organisationManager">Manager d'organisation</param>
        /// <param name="personnelManager">Manager de personnel</param>
        public ListeHeuresSpecifiquesManager(IFeatureFlippingManager featureFlippingManager, IOrganisationManager organisationManager, IPersonnelManager personnelManager, IImageManager imageManager)
        {
            this.featureFlippingManager = featureFlippingManager;
            this.organisationManager = organisationManager;
            this.personnelManager = personnelManager;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listePointageMensuel">Liste des models de pointage mensuel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateMemoryStreamListeHeuresSpecifiques(EtatPaieExportModel etatPaieExportModel, List<EtatPaieListeCodeMajorationModel> listePointageMensuel, int userId, string templateFolderPath)
        {
            string pathName = Path.Combine(templateFolderPath, "TemplateEtatPaieHeuresSpecifiques.xlsx");

            ExcelFormat excelFormat = new ExcelFormat();

            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("EtatPaieListeCodeMajorationModel", listePointageMensuel);
            excelFormat.ApplyVariables();

            // On cache le champs IsHeureNuit nécessair euniquement au grisage des lignes
            IWorksheet filledSheet = workbook.Worksheets[0];
            filledSheet.ShowColumn(7, false);

            // On grise les lignes de type Heure Nuit
            GriserHeureNuitLigneCodeMajoration(excelFormat, listePointageMensuel, workbook);

            string affaireLib = "TOUTES";

            if (etatPaieExportModel.OrganisationId != 0)
            {
                var orga = organisationManager.GetOrganisationById(etatPaieExportModel.OrganisationId);
                if (orga != null)
                {
                    affaireLib = string.Concat(orga.Code, " - ", orga.Libelle);
                }
            }

            // RG_5172_001-5
            if (etatPaieExportModel.Filtre == TypeFiltreEtatPaie.Population && featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
            {
                affaireLib += " (uniquement mes rapports verrouillés)";
            }

            // Gestion de l'entête
            var editeur = personnelManager.GetPersonnel(userId);
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path;
            string dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Interimaire_Export__Espace + DateTime.Now.ToShortTimeString();
            string editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + editeur.CodeNomPrenom;
            string periode = BusinessResources.Pointage_Interimaire_Export_Periode + new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).ToString("MM/yyyy");
            var buildHeaderModel = new BuildHeaderExcelModel(FeatureRapport.EtatPaie_HeuresSpecifiques_Titre, affaireLib, dateEdition, editePar, periode, pathLogo, new IndexHeaderExcelModel(2, 4, 6, 6));
            excelFormat.BuildHeader(workbook.Worksheets[0], buildHeaderModel);

            MemoryStream stream = excelFormat.GeneratePdfOrExcel(etatPaieExportModel.Pdf, workbook);
            excelFormat.Dispose();
            return stream;
        }

        /// <summary>
        /// Permet de griser les lignes de l'édition récapitulative des heures spécifiques (Code Majorations) qui sont de type Heure Nuit
        /// </summary>
        /// <param name="excelFormat">Classe de génération de document excel</param>
        /// <param name="listePointageMensuel">La liste des pointages mensuels des codes majorations</param>
        /// <param name="workbook">Excel Workbook</param>
        private void GriserHeureNuitLigneCodeMajoration(ExcelFormat excelFormat, List<EtatPaieListeCodeMajorationModel> listePointageMensuel, IWorkbook workbook)
        {
            int indexRow = 8; // 1ere ligne de donnée (8)
            int indexColOrigin = 1; // 1ere colonne de donnée
            int indexColEnd = 6; // derniere colonne de donnée (H)

            foreach (EtatPaieListeCodeMajorationModel pointage in listePointageMensuel)
            {
                string cellBegin = excelFormat.GetCellAdress(workbook, indexRow, indexColOrigin);
                string cellEnd = excelFormat.GetCellAdress(workbook, indexRow, indexColEnd);
                string range = cellBegin + ":" + cellEnd;

                // Grise la ligne si elle est de type Heure de Nuit
                if (pointage.IsHeureNuit)
                {
                    excelFormat.ChangeColor(workbook, range, Color.LightGray);
                }

                // Ligne suivante
                indexRow++;
            }
        }
    }
}
