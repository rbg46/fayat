using System.Collections.Generic;
using Fred.Business;

namespace Fred.ImportExport.Business.WorkflowLogicielTiers
{
    public interface IWorkflowLogicielTiersManager
    {
        /// <summary>
        /// Sauvegarde dans la table des workflow l'envoi des informations de la ligne du rapport vers le logiciel donné
        /// </summary>
        /// <param name="rapportLignesId">Lignes du rapport qui ont été envoyées</param>
        /// <param name="logicielTiersId">Id du logiciel vers lequel on a envoyé la ligne</param>
        /// <param name="auteurId">Id de l'auteur a l'origine de l'envoi</param>
        /// <param name="flux">Nom du flux</param>
        void SaveWorkflowLogicielTiers(IEnumerable<int> rapportLignesId, int logicielTiersId, int auteurId, string flux);

        /// <summary>
        /// Retourne une liste contenant les id des lignes de rapports déjà envoyé au logiciel dont l'ID est passé en paramètre
        /// et pour le flux donné
        /// </summary>
        /// <param name="logicielTiersId">id du logiciel tiers</param>
        /// <param name="flux">Nom du flux vers lequel on veut récupérer les envois</param>
        /// <returns>Une liste de int, potentiellement vide, jamais null</returns>
        IEnumerable<int> GetRapportLigneIdDejaEnvoye(int logicielTiersId, string flux);

    }
}
