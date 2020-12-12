using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.Pointage.PointageEtl.Parameter;
using Fred.ImportExport.Business.Pointage.PointageEtl.Settings;
using Fred.ImportExport.Framework.Etl.Input;


namespace Fred.ImportExport.Business.Pointage.PointageEtl.Input
{
    /// <summary>
    /// Cet input permet de recuperer les pointages dans fred
    /// </summary>
    public class BaseFredPointageInput : IEtlInput<RapportLigneEnt>
    {
        private readonly EtlPointageParameter parameter;
        private readonly EtlExecutionHelper etlExecutionHelper;
        private readonly string logLocation = "[FRED INPUT]";

        public BaseFredPointageInput(EtlPointageParameter parameter, EtlExecutionHelper etlExecutionHelper)
        {
            this.parameter = parameter;
            this.etlExecutionHelper = etlExecutionHelper;
        }

        public IList<RapportLigneEnt> Items { get; set; }

        public void Execute()
        {
            etlExecutionHelper.Log($"{parameter.LogPrefix} : {logLocation} : INFO : Execution de la requette d'extraction du rapport sur la base fred ({parameter.RapportId})");

            var pointagesForSap = parameter.EtlDependencies.FredIePointageFluxService.GetAllPointagesForPersonnelSap(this.parameter.RapportId);

            var applicationSapParameter = this.parameter.EtlDependencies.ApplicationsSapManager.GetParametersForSociete(parameter.SocieteId);

            if (applicationSapParameter.IsFound)
            {
                var uri = new Uri(applicationSapParameter.Url);
                var mandant = HttpUtility.ParseQueryString(uri.Query).Get("SAP-CLIENT");
                var flux = "CAT2";
                var logicielSap = this.parameter.EtlDependencies.LogicielTiersManager.GetOrCreateLogicielTiers("SAP", uri.Host, mandant);

                var lignesAEnvoyer = FiltreLignesRapportAEnvoyer(pointagesForSap.Where(x => x.PersonnelId != null), logicielSap.FredLogicielTiersId, flux);
                var lignesASupprimer = GetLignesRapportASupprimer(pointagesForSap, logicielSap.FredLogicielTiersId, flux);
                Items = lignesASupprimer.Concat(lignesAEnvoyer).ToList();

                var personnelIds = Items.Where(x => x.PersonnelId != null).Select(x => x.PersonnelId.Value).ToList();


                var matriculeExternes = parameter.EtlDependencies.MatriculeExterneManager.GetMatriculeExterneByPersonnelIds(personnelIds);

                foreach (var item in Items)
                {
                    if (item.PersonnelId.HasValue)
                    {
                        item.Personnel.MatriculeExterne = matriculeExternes.Where(x => x.PersonnelId == item.PersonnelId.Value).ToList();
                    }
                }
            }
        }

        /// <summary>
        /// On n'envoi au logiciel que les lignes de rapport ayant des changements 
        /// On retire de la liste des lignes à envoyer toutes les lignes ayant été supprimé avant du premier envoi du rapport a SAP
        /// Si l'id du logiciel passé en paramètre est null, alors la fonction retourne les lignes passées en paramètre
        /// </summary>
        /// <param name="lignes">lignes contenu dans le rapport</param>
        /// <param name="logicielTiersId">logiciel tiers vers lequel on doit exporter ces lignes</param>
        /// <param name="flux">Nom du flux vers lequel on envoie des données</param>
        /// <returns>Une liste de ligne de rapport, potentiellement vide, jamais null</returns>
        private IEnumerable<RapportLigneEnt> FiltreLignesRapportAEnvoyer(IEnumerable<RapportLigneEnt> lignes, int? logicielTiersId, string flux)
        {
            if (!logicielTiersId.HasValue)
            {
                return lignes;
            }

            var lignesDejaEnvoyees = this.parameter.EtlDependencies.WorkflowPointageManager.GetRapportLigneIdDejaEnvoye(logicielTiersId.Value, flux);

            var filtreLignesRapportAEnvoyer = lignes.Where(l =>
                l.DateSuppression == null
                ||
                (l.DateSuppression != null && lignesDejaEnvoyees.Contains(l.RapportLigneId)));
            return filtreLignesRapportAEnvoyer.ToList();
        }

        /// <summary>
        /// Pour chaque ligne de rapport, recherche de la dernière ligne envoyée à SAP stocké dans WorkflowPointage, 
        /// si le personnel a changé, créée une ligne à supprimer avec l'ancien personnel et l'ajoute à la collection de lignes à supprimer
        /// </summary>
        /// <param name="lignes">lignes contenu dans le rapport</param>
        /// <param name="logicielTiersId">logiciel tiers vers lequel on doit exporter ces lignes</param>
        /// <param name="flux">Nom du flux vers lequel on envoie des données</param>
        /// <returns>Une liste de ligne de rapport, potentiellement vide, jamais null</returns>
        private List<RapportLigneEnt> GetLignesRapportASupprimer(IEnumerable<RapportLigneEnt> lignes, int? logicielTiersId, string flux)
        {
            var listeLigneSuppression = new List<RapportLigneEnt>();

            // recherche de l'historique des workflow pointages du rapport
            var rapportWorkflowPointages = parameter.EtlDependencies.WorkflowPointageManager.GetRapportLigneWorkflowPointages(lignes.Select(x => x.RapportLigneId).ToList(), logicielTiersId.Value, flux);
            if (!rapportWorkflowPointages.Any())
            {
                return listeLigneSuppression;
            }

            foreach (var ligne in lignes.Where(x => !x.DateSuppression.HasValue))
            {
                // recherche le dernier workflowpointage envoyé pour le rapporLigneId
                var dernierWorkflowPointageEnvoye = rapportWorkflowPointages
                    .Where(x => x.RapportLigneId == ligne.RapportLigneId)
                        .OrderByDescending(x => x.WorkflowId)
                    .FirstOrDefault();
                // si le personnel a changé depuis le dernier envoi (et qu'il n'est pas supprimé), création d'une ligne de suppression avec l'ancien personnel
                if (dernierWorkflowPointageEnvoye != null
                    && dernierWorkflowPointageEnvoye.PersonnelId != ligne.PersonnelId
                    && !dernierWorkflowPointageEnvoye.DateEnvoiSuppression.HasValue)
                {
                    var ligneASupprimer = ligne.Duplicate();
                    ligneASupprimer.RapportLigneId = ligne.RapportLigneId;
                    ligneASupprimer.DatePointage = ligne.DatePointage;
                    ligneASupprimer.AuteurCreationId = ligne.AuteurCreationId;
                    ligneASupprimer.AuteurCreation = ligne.AuteurCreation;
                    ligneASupprimer.DateCreation = ligne.DateCreation;
                    ligneASupprimer.AuteurModificationId = ligne.AuteurModificationId;
                    ligneASupprimer.AuteurModification = ligne.AuteurModification;
                    ligneASupprimer.DateModification = ligne.DateModification;
                    // Informations de suppression
                    ligneASupprimer.AuteurSuppressionId = ligne.AuteurModificationId ?? ligne.AuteurCreationId;
                    ligneASupprimer.AuteurSuppression = ligne.AuteurModification ?? ligne.AuteurCreation;
                    ligneASupprimer.DateSuppression = ligne.DateModification ?? DateTime.Now;
                    // Informations de personnel à supprimer
                    ligneASupprimer.PersonnelId = dernierWorkflowPointageEnvoye.PersonnelId;
                    ligneASupprimer.Personnel = parameter.EtlDependencies.PersonnelManager.GetPersonnelById(dernierWorkflowPointageEnvoye.PersonnelId.Value);
                    // pas de suppression si pas de personnel trouvé
                    if (ligneASupprimer.Personnel != null)
                    {
                        listeLigneSuppression.Add(ligneASupprimer);
                    }
                }
            }

            return listeLigneSuppression;
        }
    }
}
