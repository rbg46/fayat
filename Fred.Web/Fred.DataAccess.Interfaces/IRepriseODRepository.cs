using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using System.Collections.Generic;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public interface IRepriseODRepository : IMultipleRepository
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
        /// Recupere la liste des unités dont le Code est contenu dans la liste 'uniteCodes'
        /// </summary>
        /// <param name="uniteCodes">codes des unités à rechercher</param>
        /// <returns>les unités</returns>
        List<UniteEnt> GetUniteListByCodes(List<string> uniteCodes);

        /// <summary>
        /// Recupere la liste des devises dont le Code est contenu dans la liste 'deviseCodes'
        /// </summary>
        /// <param name="deviseCodes">codes des devises à rechercher</param>
        /// <returns>les devises</returns>
        List<DeviseEnt> GetDeviseListByCodes(List<string> deviseCodes);

        /// <summary>
        ///  Recupere les famille Oparation diverses dont le code est contenu dans la liste des codes famille
        /// </summary>
        /// <param name="societesIds">societeId list</param>
        /// <param name="codesFamille">liste des codes famille recherchés</param>
        /// <returns>les FamilleOperationDiverseEnt</returns>
        List<FamilleOperationDiverseEnt> GetFamilleODListByCodes(List<int> societesIds, List<string> codesFamille);

        /// <summary>
        ///  Recupere les famille Oparation diverses dont le code est contenu dans la liste des codes famille
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="codesRessource">liste des codes famille recherchés</param>
        /// <returns>les RessourceEnt</returns>
        List<RessourceEnt> GetRessourceListByCodes(int groupeId, List<string> codesRessource);

        /// <summary>
        /// Recupere la liste des code deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="deviseCode">code devise rechercher</param>
        /// <returns>les codes deplacements</returns>
        DeviseEnt GetDefaultDeviseCode(string deviseCode);

        /// <summary>
        /// Recupere la liste des code deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="uniteCode">codes des codeDeplacements rechercher</param>
        /// <returns>les codes deplacements</returns>
        UniteEnt GetDefaultUniteCode(string uniteCode);

    }
}
