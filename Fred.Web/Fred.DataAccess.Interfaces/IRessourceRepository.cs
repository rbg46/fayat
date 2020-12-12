
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.Carburant;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les ressources.
    /// </summary>
    public interface IRessourceRepository : IRepository<RessourceEnt>
    {
        /// <summary>
        ///   Retourne le ressource avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ressourceId">Identifiant du ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        RessourceEnt GetById(int ressourceId);

        /// <summary>
        ///   Retourne la liste des ressources.
        /// </summary>
        /// <returns>Liste des ressources.</returns>
        IEnumerable<RessourceEnt> GetList();

        /// <summary>
        /// Retourne une liste en lecture seule de <see cref="RessourceEnt"/> actif
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressources</param>
        /// <returns>Une liste en lecture seul de <see cref="RessourceEnt"/></returns>
        IReadOnlyList<RessourceEnt> GetList(List<string> ressourceCodes);

        /// <summary>
        /// Retourne une liste en lecture seule de <see cref="RessourceEnt" /> actif
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressources</param>
        /// <param name="societeIds">Liste d'identifiant de societe</param>
        /// <returns>Liste de <see cref="RessourceEnt" /></returns>
        IEnumerable<RessourceEnt> GetList(List<string> ressourceCodes, List<int> societeIds);

        /// <summary>
        ///   Obtient la collection des ressources (suppprimées inclus)
        /// </summary>
        /// <returns>La collection des ressources</returns>
        IEnumerable<RessourceEnt> GetAllList();

        /// <summary>
        ///   Obtient la collection des ressources appartenant à un sous-chapitre spécifié
        /// </summary>
        /// <param name="sousChapitreId">Identifiant du sous-chapitre.</param>
        /// <returns>La collection des ressources</returns>
        IEnumerable<RessourceEnt> GetListBySousChapitreId(int sousChapitreId);

        /// <summary>
        /// Retourne le type de ressource en fonction de son code
        /// </summary>
        /// <param name="code">Code de la ressource</param>
        /// <returns>Le type de ressource</returns>
        TypeRessourceEnt GetTypeRessourceByCode(string code);

        /// <summary>
        ///   Obtient la collection des types de ressources
        /// </summary>
        /// <returns>La collection des types de ressource</returns>
        IEnumerable<CarburantEnt> GetCarburantList();

        /// <summary>
        ///   Renvoi la liste des ressources actives qui ont été rattachées à une nature
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>retourne la liste des ressources actives qui ont été rattachées à une nature</returns>
        IEnumerable<RessourceEnt> GetListRessourcesBySocieteId(int societeId);

        /// <summary>
        ///   Indique si le code existe déjà pour les ressources d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        bool IsCodeRessourceExist(string code, int groupeId);

        /// <summary>
        ///   Cherche une liste de Ressource.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Ressources.</param>
        /// <returns>Une liste de Ressource.</returns>
        IEnumerable<RessourceEnt> SearchRessources(string text);

        /// <summary>
        ///   Cherche une liste des ressources.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des ressources.</param>
        /// <param name="societeId">Identifiant de la societe.</param>
        /// <returns>Une liste de ressources.</returns>
        IEnumerable<RessourceEnt> SearchRessources(string text, int societeId);

        /// <summary>
        ///   Renvoi la liste des ressources
        /// </summary>
        /// <param name="groupId">Identifiant du groupe</param>
        /// <returns>retourne la liste des ressources appartenant à un groupe</returns>
        IEnumerable<RessourceEnt> GetListByGroupeId(int groupId);

        /// <summary>
        ///   Renvoi la liste des ressources en fonction du code et du groupe.
        /// </summary>
        /// <param name="code">Le code de la ressource.</param>
        /// <param name="groupId">L'identifiant du groupe.</param>
        /// <returns>La liste des ressources concernées.</returns>
        IEnumerable<RessourceEnt> Get(string code, int groupId);

        /// <summary>
        ///   Détermine si une ressource peut être supprimée ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="resource">Elément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        bool IsDeletable(RessourceEnt resource);

        /// <summary>
        /// Vérifie que le code de la ressource n'est pas déjà utilisé
        /// </summary>
        /// <param name="code">Code de la ressource à comparer</param>
        /// <returns>Vrai si le code de la ressource existe, faux sinon</returns>
        bool IsExistByCode(string code);

        /// <summary>
        ///   Retourne l'identifiant du Ressource ajouté
        /// </summary>
        /// <param name="ressourceEnt">Ressource à ajouter.</param>
        /// <returns>l'identifiant du Ressource ajouté</returns>
        RessourceEnt AddRessource(RessourceEnt ressourceEnt);

        /// <summary>
        ///   Mise à jour d'une ressource
        /// </summary>
        /// <param name="ressourceEnt">Ressource à mettre à jour.</param>
        /// <returns>l'identifiant du Ressource à mettre à jour</returns>
        RessourceEnt UpdateRessource(RessourceEnt ressourceEnt);

        /// <summary>
        /// Retourne toutes les ressources du referentiel étendu de la société ou étant une ressource spécifique du CI donné.
        /// La liste sera chargée avec le sous chapitre et le chapitre associé à la ressource
        /// Cette fonction inclue les sous chapitre et les chapitres pour chaque ressource
        /// </summary>
        /// <param name="societeId">Id de la société</param>
        /// <param name="ciId">Id du CI</param>
        /// <returns>Une liste potentiellement vide jamais null</returns>
        IEnumerable<RessourceEnt> GetListBySocieteIdWithSousChapitreEtChapitre(int societeId, int? ciId = null);

        /// <summary>
        /// Récupère l'identifiant correspondant à la nature d'une ressource
        /// </summary>
        /// <param name="ressourceIdNatureFilter">L'identifiant de la ressource dont on veut retrouver la nature</param>
        /// <param name="societeId">La société de la ressource</param>
        /// <returns>Un nullable correspondant</returns>
        int? GetNatureIdRessource(int? ressourceIdNatureFilter, int? societeId);

        /// <summary>
        /// Récupère les ressources pour la comparaison de budget.
        /// </summary>
        /// <param name="ressourceIds">Les identifiants des ressources concernées.</param>
        /// <returns>Les ressources.</returns>
        List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> ressourceIds);

        Task<List<RessourceEnt>> SearchRessourcesForAchatAsync(SearchRessourcesRequestModel searchRessourcesRequestModel);

        Dictionary<string, int> GetOperationDiverseDefaultsByGroupe(List<string> codesSousChapitre, string codeGroupe);

        Task<IEnumerable<TypeRessourceEnt>> SearchRessourceTypesByCodeOrLabelAsync(string text);

        RessourceEnt GetRessourceById(int ressourceId);
    }
}
