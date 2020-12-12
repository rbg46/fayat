using System.Collections.Generic;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Models.Groupe;
using Hangfire.Storage;

namespace Fred.ImportExport.Web.Models
{
    public class ExportViewModel
    {
        // KLM
        public FluxEnt KlmFlux { get; set; }

        public RecurringJobDto KlmRecurringJob { get; set; }

        // Réception Intérimaire
        public FluxEnt ReceptionInterimaireFlux { get; set; }

        public RecurringJobDto ReceptionInterimaireRecurringJob { get; set; }

        // STAIR PERSONNEL
        public FluxEnt StairPersonnelFlux { get; set; }

        public RecurringJobDto StairPersonnelRecurringJob { get; set; }

        // STAIR CI
        public FluxEnt StairCIFlux { get; set; }
        public RecurringJobDto StairCIRecurringJob { get; set; }

        //Réception Matériel Externe
        public FluxEnt ReceptionMaterielExterneFlux { get; set; }

        public RecurringJobDto ReceptionMaterielExterneRecurringJob { get; set; }

        //Personnel FES FIGGO
        public FluxEnt PersonnelFesFlux { get; set; }

        public RecurringJobDto PersonnelFesRecurringJob { get; set; }

        // Not Flux : GroupeInterimaire
        public List<GroupeInterimaireModel> GroupeInterimaire { get; set; }

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
