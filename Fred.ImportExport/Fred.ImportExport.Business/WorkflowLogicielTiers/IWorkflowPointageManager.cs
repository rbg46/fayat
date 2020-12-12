using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.Business.WorkflowLogicielTiers
{
    public interface IWorkflowPointageManager
    {
        /// <summary>
        /// Sauvegarde dans la table des workflow l'envoi des informations de la ligne du rapport vers le logiciel donné
        /// </summary>
        /// <param name="rapportLignes">Lignes du rapport qui ont été envoyées</param>
        /// <param name="logicielTiersId">Id du logiciel vers lequel on a envoyé la ligne</param>
        /// <param name="auteurId">Id de l'auteur a l'origine de l'envoi</param>
        /// <param name="hangFireJobId">Identifiant Job Hangfire</param>
        /// <param name="flux">Nom du flux</param>
        void SaveWorkflowPointage(IEnumerable<RapportLigneEnt> rapportLignes, int logicielTiersId, int auteurId, string hangFireJobId, string flux);


        /// <summary>
        /// Retourne la liste des workflow pointages du rapport
        /// </summary>
        /// <param name="rapportLigneIdList">rapport Id</param>
        /// <param name="logicielTiersId">logiciel tiers Id</param>
        /// <param name="fluxName">nom du flux</param>
        /// <returns>Liste de WorkFlowPointage</returns>
        List<WorkflowPointageEnt> GetRapportLigneWorkflowPointages(List<int> rapportLigneIdList, int logicielTiersId, string fluxName);

        /// <summary>
        /// Retourne tous les ide
        /// </summary>
        /// <param name="logicielTiersId">Id du logiciel tiers de destination</param>
        /// <param name="fluxName">Nom du flux</param>
        /// <returns>Une liste de int, potentiellement vide, jamais null</returns>
        IEnumerable<int> GetRapportLigneIdDejaEnvoye(int logicielTiersId, string fluxName);

    }
}
