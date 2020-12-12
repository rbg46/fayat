using Fred.ImportExport.Entities.ImportExport;
using Hangfire.Storage;

namespace Fred.ImportExport.Web.Models
{
    public class ImportViewModel
    {
        #region CI
        public FluxEnt CiFlux { get; set; }

        public RecurringJobDto CiRecurringJob { get; set; }

        // CI Générique
        public FluxEnt CIFluxGRZB { get; set; }

        public RecurringJobDto CIRecurringJobGRZB { get; set; }

        #endregion

        #region Fournisseur
        public FluxEnt FournisseurFlux { get; set; }

        public RecurringJobDto FournisseurRecurringJob { get; set; }

        #endregion

        #region Personnel
        public FluxEnt PersonnelFlux { get; set; }

        public RecurringJobDto PersonnelRecurringJob { get; set; }

        public FluxEnt PersonnelFluxGRZB { get; set; }

        public RecurringJobDto PersonnelRecurringJobGRZB { get; set; }

        public FluxEnt PersonnelFluxGFTP { get; set; }

        public RecurringJobDto PersonnelRecurringJobGFTP { get; set; }

        public FluxEnt PersonnelFluxFES { get; internal set; }

        public RecurringJobDto PersonnelRecurringJobFES { get; internal set; }

        public FluxEnt PersonnelFluxFON { get; internal set; }

        public RecurringJobDto PersonnelRecurringJobFON { get; internal set; }

        #endregion

        #region Etablissement Comptable
        public FluxEnt EtablissementComptableFlux { get; set; }

        public RecurringJobDto EtablissementComptableRecurringJob { get; set; }

        #endregion

        #region Ecriture comptable
        public RecurringJobDto EcritureComptableRecurringJobRzb { get; set; }

        public FluxEnt EcritureComptableFluxRzb { get; set; }

        public FluxEnt EcritureComptableFluxFTP { get; set; }

        public System.DateTime EcritureComptableDateDebutComptableRzb { get; set; }

        public System.DateTime EcritureComptableDateFinComptableRzb { get; set; }

        public string CodeEtablissementRzb { get; set; }

        public RecurringJobDto EcritureComptableRecurringJobMoulins { get; set; }

        public FluxEnt EcritureComptableFluxMoulins { get; set; }

        public System.DateTime EcritureComptableDateDebutComptableMoulins { get; set; }

        public System.DateTime EcritureComptableDateFinComptableMoulins { get; set; }

        public string CodeEtablissementMoulins { get; set; }
        #endregion

        #region Matériel
        public FluxEnt MaterielFlux { get; set; }

        public FluxEnt MaterielFayatTpFlux { get; set; }

        public RecurringJobDto MaterielRecurringJob { get; set; }

        public RecurringJobDto MaterielFayatTpRecurringJob { get; set; }
        #endregion

        #region Stair
        public FluxEnt StairFlux { get; set; }

        public RecurringJobDto StairRecurringJob { get; set; }

        public FluxEnt SphinxFlux { get; set; }

        public RecurringJobDto SphinxRecurringJob { get; set; }
        #endregion

        #region Journaux comptables
        public FluxEnt JournauxComptableFluxRzb { get; set; }

        public RecurringJobDto JournauxComptableRecurringJobRzb { get; set; }

        public FluxEnt JournauxComptableFluxMoulins { get; set; }

        public RecurringJobDto JournauxComptableRecurringJobMoulins { get; set; }
        #endregion

        #region Contrats interimaires

        public FluxEnt ContratInteriamireFlux { get; set; }

        public RecurringJobDto ContratInteriamireRecurringJob { get; set; }

        #endregion

        #region Nettoyage des rôles et logins des utilisateurs sortis 

        public FluxEnt CleaningOutgoingUsers { get; set; }

        public RecurringJobDto CleaningOutgoingUsersJob { get; set; }

        #endregion



        /// <summary>
        /// Retourne la chaîne à afficher pour la prochaine exécution.
        /// Ceci parce que Hangfire ne calcule pas l'instant de la prochaine exécution immédiatement après l'ajout du job.
        /// </summary>
        /// <param name="recurringJob">Le job concerné</param>
        /// <returns>La chaîne à afficher pour la prochaine exécution</returns>
        public string GetNextExecutionToString(RecurringJobDto recurringJob)
        {
            if (!recurringJob.NextExecution.HasValue)
            {
                return "Pas encore disponible, rafraichir ultérieurement";
            }
            else
            {
                return recurringJob.NextExecution.Value.ToLocalTime().ToString();
            }
        }
    }
}
