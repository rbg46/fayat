using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fred.Business.RepriseDonnees;
using Fred.Business.RepriseDonnees.Ci;
using Fred.Business.RepriseDonnees.Commande;
using Fred.Business.RepriseDonnees.IndemniteDeplacement;
using Fred.Business.RepriseDonnees.Materiel;
using Fred.Business.RepriseDonnees.Personnel;
using Fred.Business.RepriseDonnees.PlanTaches;
using Fred.Business.RepriseDonnees.Rapport;
using Fred.Business.RepriseDonnees.ValidationCommande;
using Fred.Entities.Groupe;
using Fred.Entities.RepriseDonnees;
using Fred.ImportExport.Business.CI.AnaelSystem;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;
using Hangfire;

namespace Fred.ImportExport.Business.RepriseDonnees
{
    /// <summary>
    /// Manager des vues RepriseDonnees
    /// </summary>
    public class RepriseDonneesViewManager : IRepriseDonneesViewManager
    {
        private readonly IRepriseDonneeManager repriseDonneeManager;
        private readonly IRepriseDonneesCiManager repriseDonneeCiManager;
        private readonly IRepriseDonneesPlanTachesManager repriseDonneePlanTachesManager;
        private readonly IRepriseDonneesCommandeManager repriseDonneesCommandeAndReceptionManager;
        private readonly IRepriseDonneesValidationCommandeManager repriseDonneesValidationCommandeManager;
        private readonly CommandeFluxManager commandeFluxManager;
        private readonly IRepriseDonneesRapportManager repriseDonneeRapportManager;
        private readonly IRepriseDonneesPersonnelManager repriseDonneesPersonnelManager;
        private readonly IRepriseDonneesMaterielManager repriseDonneesMaterielManager;
        private readonly IRepriseDonneesIndemniteDeplacementManager repriseDonneesIndemniteDeplacementManager;
        private readonly IImportCiAnaelSystemManager importCiAnaelSystemManager;
        private readonly IImportFournisseurAnaelSystemManager importFournisseurAnaelSystemManager;

        public RepriseDonneesViewManager(
            IRepriseDonneeManager repriseDonneeManager,
            IRepriseDonneesCiManager repriseDonneeCiManager,
            IRepriseDonneesPlanTachesManager repriseDonneePlanTachesManager,
            IRepriseDonneesCommandeManager repriseDonneesCommandeAndReceptionManager,
            IRepriseDonneesValidationCommandeManager repriseDonneesValidationCommandeManager,
            CommandeFluxManager commandeFluxManager,
            IRepriseDonneesRapportManager repriseDonneeRapportManager,
            IRepriseDonneesPersonnelManager repriseDonneesPersonnelManager,
            IRepriseDonneesMaterielManager repriseDonneesMaterielManager,
            IRepriseDonneesIndemniteDeplacementManager repriseDonneesIndemniteDeplacementManager,
            IImportCiAnaelSystemManager importCiAnaelSystemManager,
            IImportFournisseurAnaelSystemManager importFournisseurAnaelSystemManager)
        {
            this.repriseDonneeManager = repriseDonneeManager;
            this.repriseDonneeCiManager = repriseDonneeCiManager;
            this.repriseDonneePlanTachesManager = repriseDonneePlanTachesManager;
            this.repriseDonneesCommandeAndReceptionManager = repriseDonneesCommandeAndReceptionManager;
            this.repriseDonneesValidationCommandeManager = repriseDonneesValidationCommandeManager;
            this.commandeFluxManager = commandeFluxManager;
            this.repriseDonneeRapportManager = repriseDonneeRapportManager;
            this.repriseDonneesPersonnelManager = repriseDonneesPersonnelManager;
            this.repriseDonneesMaterielManager = repriseDonneesMaterielManager;
            this.repriseDonneesIndemniteDeplacementManager = repriseDonneesIndemniteDeplacementManager;
            this.importCiAnaelSystemManager = importCiAnaelSystemManager;
            this.importFournisseurAnaelSystemManager = importFournisseurAnaelSystemManager;
        }

        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        public List<GroupeEnt> GetAllGroupes()
        {
            return repriseDonneeManager.GetAllGroupes();
        }

        /// <summary>
        /// Importation des Cis 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportCiResult ImportCis(int groupeId, Stream stream)
        {
            return repriseDonneeCiManager.ImportCis(groupeId, stream);
        }

        /// <summary>
        /// Creation des commandes et des recepetions
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportCommandeResult CreateCommandeAndReceptions(int groupeId, Stream stream)
        {
            return repriseDonneesCommandeAndReceptionManager.CreateCommandeAndReceptions(groupeId, stream);
        }

        /// <summary>
        /// Creation des rapports et rapport lignes 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportRapportResult ImportRapports(int groupeId, Stream stream)
        {
            return repriseDonneeRapportManager.ImportRapports(groupeId, stream);
        }


        /// <summary>
        /// Validation des commandes
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportValidationCommandeResult ValidationCommandes(int groupeId, Stream stream)
        {
            // ici je passe l'action en parametre car Fred.Business n'a pas acces au classe de Hanfire.
            Func<int, string> backgroundJobFunc = (commandeId) => BackgroundJob.Enqueue(() => commandeFluxManager.ExportCommandeToSap(commandeId));

            var result = repriseDonneesValidationCommandeManager.ValidateCommandes(groupeId, stream, backgroundJobFunc);

            return result;
        }

        /// <summary>
        /// Creation du Plan de taches
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportPlanTachesResult CreatePlanTaches(int groupeId, Stream stream)
        {
            return repriseDonneePlanTachesManager.CreatePlanTaches(groupeId, stream);
        }

        /// <summary>
        /// Creation des Personnels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportPersonnelResult CreatePersonnel(int groupeId, Stream stream)
        {
            return repriseDonneesPersonnelManager.CreatePersonnel(groupeId, stream);
        }

        /// <summary>
        /// Creation des Materiels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le résultat de l'import</returns>
        public ImportMaterielResult CreateMateriel(int groupeId, Stream stream)
        {
            return repriseDonneesMaterielManager.CreateMateriel(groupeId, stream);
        }

        /// <summary>
        /// Creation des Indemnites de Deplacement (et suppression logique si déjà existantes)
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le résultat de l'import</returns>
        public ImportIndemniteDeplacementResult CreateIndemniteDeplacement(int groupeId, Stream stream)
        {
            return repriseDonneesIndemniteDeplacementManager.CreateIndemniteDeplacement(groupeId, stream);
        }

        /// <summary>
        /// Import des ci depuis Anael et export vers Sap
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public async Task<ImportResult> ImportCiFromAnaelAndSendToSapAsync(int groupeId, Stream stream)
        {
            return await importCiAnaelSystemManager.ImportCiByExcelAsync(new ImportCisByExcelInputs()
            {
                ExcelStream = stream,
                GroupeId = groupeId
            });
        }

        /// <summary>
        /// Import des fournisseurs depuis Anael et export vers Sap
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public async Task<Common.ImportResult> ImportFournisseursFromAnaelAndSendToSapAsync(int groupeId, Stream stream)
        {
            string regleGestions = "'F'";
            var societeCodeAnael = new List<string> { "1000" }; // Par défaut : société Razel-Bec
            var societeModel = "1";

            return await importFournisseurAnaelSystemManager.ImportFournisseurByExcelAsync(new ImportFournisseursByExcelInputs()
            {
                ExcelStream = stream,
                CodeSocietes = societeCodeAnael,
                RegleGestion = regleGestions,
                ModeleSociete = societeModel
            });
        }
    }
}
