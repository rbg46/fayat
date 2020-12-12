using System.Collections.Generic;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.DataAccess.WorkflowLogicielTiers
{
    public interface IWorkflowPointageRepository
    {
        /// <summary>
        /// Sauvegarde dans la table des workflow l'envoi des informations de la ligne du rapport vers le logiciel donné
        /// </summary>
        /// <param name="workflowPointageList">Lignes du rapport qui ont été envoyées</param>
        void SaveWorkflowPointage(List<WorkflowPointageEnt> workflowPointageList);

        /// <summary>
        /// Retourne la liste des identifiants de pointage déjà présent dans la table de workflow
        /// </summary>
        /// <param name="logicielTiersId">Identifant du logiciel tiers</param>
        /// <param name="fluxName">Libellé du flux</param>
        /// <returns>La liste des identifiants de pointage déjà présent dans la table de workflow</returns>
        IEnumerable<int> GetRapportLigneIdDejaEnvoyeALogiciel(int logicielTiersId, string fluxName);

        /// <summary>
        /// Retourne la liste des workflow pointages du rapport
        /// </summary>
        /// <param name="rapportLigneIdList">Liste de rapportLigneId</param>
        /// <param name="logicielTiersId">logiciel tiers Id</param>
        /// <param name="fluxName">nom du flux</param>
        /// <returns>Liste de WorkFlowPointage</returns>
        List<WorkflowPointageEnt> GetRapportLigneWorkflowPointages(List<int> rapportLigneIdList, int logicielTiersId, string fluxName);
    }
}
