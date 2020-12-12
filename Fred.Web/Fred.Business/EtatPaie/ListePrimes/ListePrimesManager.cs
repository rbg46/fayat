using System;
using System.Collections.Generic;
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
    public class ListePrimesManager : IListePrimesManager
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
        public ListePrimesManager(IFeatureFlippingManager featureFlippingManager, IOrganisationManager organisationManager, IPersonnelManager personnelManager, IImageManager imageManager)
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
        public MemoryStream GenerateMemoryStreamListePrimes(EtatPaieExportModel etatPaieExportModel, List<EtatPaieListePrimesModel> listePointageMensuel, int userId, string templateFolderPath)
        {
            string pathName;
            string etatPaiePrimesTitre;
            ExcelFormat excelFormat = new ExcelFormat();

            if (etatPaieExportModel.FiltresPrimesMensuelles)
            {
                etatPaiePrimesTitre = FeatureRapport.EtatPaie_Primes_Titre_Mensuelles;
                pathName = Path.Combine(templateFolderPath, "TemplateEtatPaieListePrimeMensuelles.xlsx");
            }
            else
            {
                etatPaiePrimesTitre = FeatureRapport.EtatPaie_Primes_Titre_Journalier;
                pathName = Path.Combine(templateFolderPath, "TemplateEtatPaieListePrime.xlsx");
            }

            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("EtatPaieListePrimesModel", listePointageMensuel);
            excelFormat.ApplyVariables();

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
            var buildHeaderModel = new BuildHeaderExcelModel(etatPaiePrimesTitre, affaireLib, dateEdition, editePar, periode, pathLogo, new IndexHeaderExcelModel(2, 4, 6, 8));
            excelFormat.BuildHeader(workbook.Worksheets[0], buildHeaderModel, true, true);

            excelFormat.AutoFitRows(workbook.ActiveSheet, 8, 9 + listePointageMensuel.Count);


            MemoryStream stream = excelFormat.GeneratePdfOrExcel(etatPaieExportModel.Pdf, workbook);
            excelFormat.Dispose();
            return stream;
        }
    }
}
