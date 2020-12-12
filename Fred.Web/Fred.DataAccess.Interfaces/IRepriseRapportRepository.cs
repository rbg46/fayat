using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using System.Collections.Generic;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public interface IRepriseRapportRepository : IMultipleRepository
    {
        /// <summary>
        /// UnitOfWork
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation des rapports, rapportLignes
        /// </summary>
        /// <param name="rapports">Les rapports</param>
        /// <param name="rapportLignes">Les rapport lignes</param>
        void SaveEntities(List<RapportEnt> rapports, List<RapportLigneEnt> rapportLignes);

        /// <summary>
        /// Recherche dans la base les taches a partir de Requette :
        /// Si la requette n'a pas de code alors : 
        /// - rechercher la tâche par défaut du CI identifié
        /// Si la valeur "Code" de la requette est non vide :
        /// - rechercher cette valeur parmi les tâches de niveau 3 associées au CI identifié
        /// </summary>
        /// <param name="requests">Liste de requettes </param>
        /// <returns>Liste de reponse : si la "Tache" de la reponse est vide, c'est qu'aucune tache n'a été trouvée.</returns>
        List<GetT3ByCodesOrDefaultResponse> GetT3ByCodesOrDefault(List<GetT3ByCodesOrDefaultRequest> requests);
        /// <summary>
        /// Recupere la liste des code deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="societeIds">societeId list</param>
        /// <param name="codeDeplacementCodes">codes des codeDeplacements rechercher</param>
        /// <returns>les codes deplacements</returns>
        List<CodeDeplacementEnt> GetCodeDeplacementListByCodes(List<int> societeIds, List<string> codeDeplacementCodes);

        /// <summary>
        /// Recupere la liste des code zone deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="societeIds">societeId list</param>
        /// <param name="codeZoneDeplacementCodes">codes des codeZoneDeplacements rechercher</param>
        /// <returns>les code zone deplacements</returns>
        List<CodeZoneDeplacementEnt> GetCodeZoneDeplacementListByCodes(List<int> societeIds, List<string> codeZoneDeplacementCodes);

        /// <summary>
        ///  Recupere les personnels dont le matricule est contenu dans la liste matricules et pour plusieurs societes
        ///  ATTENTION !!!! : il se peut que 2 personnels aient le meme matricule pour 2 societes differentes.
        /// </summary>
        /// <param name="societeIds">liste des societes dans lequel les personnels existent</param>
        /// <param name="matricules">liste des matricules recherchés</param>
        /// <returns>les personnels pour plusieurs societes</returns>
        List<PersonnelEnt> GetPersonnelListBySocieteIdsAndMatricules(List<int> societeIds, List<string> matricules);

        /// <summary>
        ///  Recupere statuts de rapport
        /// </summary>
        /// <returns>statuts de rapport</returns>
        List<RapportStatutEnt> GetRapportStatutList();

    }
}
