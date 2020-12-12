using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.CI;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Referential;
namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données des cis.
    /// </summary>
    public interface ICIRepository : IFredRepository<CIEnt>
    {
        /// <summary>
        /// Recherche un ci selon les filtres definis
        /// </summary>
        /// <param name="filters">les filtres</param>
        /// <param name="orderBy">les orderby</param>
        /// <param name="includeProperties">les includes</param>
        /// <param name="page">la page corrante</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Liste de ci</returns>
        List<CIEnt> Search(List<Expression<Func<CIEnt, bool>>> filters,
                                                      Func<IQueryable<CIEnt>, IOrderedQueryable<CIEnt>> orderBy = null,
                                                      List<Expression<Func<CIEnt, object>>> includeProperties = null,
                                                      int? page = null,
                                                      int? pageSize = null,
                                                      bool asNoTracking = true);
        /// <summary>
        ///   Retourne la liste des cis.
        /// </summary>
        /// <param name="onlyChantierFred">Indique si seuls les chantiers gérés par FRED sont retournés</param>
        /// <param name="groupeId">L'identifiant du groupe concerné ou null pour tous les groupes.</param>
        /// <returns>Liste des cis.</returns>
        IEnumerable<CIEnt> Get(bool onlyChantierFred = false, int? groupeId = null);

        /// <summary>
        ///   Retourne le ci par rapport à son identifiant unique 
        /// </summary>
        /// <param name="id">Identifiant unique</param>
        /// <param name="withSocieteInclude">Indique si on inclut la société</param>
        /// <returns>Renvoie le ci.</returns>
        CIEnt GetCiById(int id, bool withSocieteInclude);

        /// <summary>
        ///   Retourne l' ci portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ciID">Identifiant de l'ci à retrouver.</param>
        /// <returns>L' ci retrouvée, sinon nulle.</returns>
        CIEnt Get(int ciID);

        /// <summary>
        ///   Ajout une nouvelle ci
        /// </summary>
        /// <param name="ciEnt">CI à ajouter</param>
        /// <returns>CI ajouté</returns>
        CIEnt AddCI(CIEnt ciEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'une ci
        /// </summary>
        /// <param name="ciEnt">CI à modifier</param>
        /// <returns>CI mis à jour</returns>
        CIEnt UpdateCI(CIEnt ciEnt);

        /// <summary>
        ///   Ajoute ou met à jour des CI selon la liste en paramètre
        /// </summary>
        /// <param name="cis">Liste des CI</param>
        /// <param name="updateOrganisation">Mets aussi a jour l'organisation quand on met a jour le ci</param>    
        /// <returns>Les CI ajoutés</returns>
        IEnumerable<CIEnt> AddOrUpdateCIList(IEnumerable<CIEnt> cis, bool updateOrganisation = false);

        /// <summary>
        ///   Supprime une ci
        /// </summary>
        /// <param name="id">L'identifiant de l'ci à supprimer</param>
        void DeleteCIById(int id);

        /// <summary>
        /// Retourne l'organisationId pour un ci
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <returns>L organisationId</returns>
        int? GetOrganisationIdByCiId(int ciId);

        /// <summary>
        ///     Permet de récupérer tous les identifiants des organisations de chaque CI dans 'ciIds'
        /// </summary>
        /// <param name="ciIds">Liste d'identifiants de CI</param>
        /// <returns>Dictionnaire contenant (CiId, OrganisationId)</returns>
        Dictionary<int, int?> GetOrganisationIdByCiId(IEnumerable<int> ciIds);

        /// <summary>
        /// Retourne la date d'ouverture du CI 
        /// </summary>
        /// <param name="ciId">Id du CI dont on veut la date d'ouverture</param>
        /// <returns>une datetime si la date d'ouverture est renseignée, null sinon</returns>
        DateTime? GetDateOuvertureCi(int ciId);

        /// <summary>
        /// Permet de récupérer la liste des types de CI.
        /// </summary>
        /// <returns>Liste des types de CI</returns>
        IEnumerable<CITypeEnt> GetCITypes();

        /// <summary>
        /// Get Ci by identifiant unique de société
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        List<CIEnt> GetCIListBySocieteId(int societeId);

        /// <summary>
        /// Get liste ciid par identifiant unique de société
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        List<int> GetCiIdListBySocieteId(int societeId);

        /// <summary>
        /// Get liste ciid par identifiant unique de société pour traitement des exports reception intérimaire
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        List<int> GetCiIdListBySocieteIdForInterimaire(int societeId);

        /// <summary>
        /// Renvoie la liste des Ci par liste des codes
        /// </summary>
        /// <param name="codeList">Liste des codes des Cis à renvoyer</param>
        /// <returns>Liste des Cis qui corresponds aux codes demandés</returns>
        IEnumerable<CIEnt> GetCiByCodeList(IEnumerable<string> codeList);

        #region Gestion des CI

        /// <summary>
        ///   Récupère une liste de CI selon l'identifiant de l'organisation du CI
        /// </summary>
        /// <param name="organisationIds">Liste d'identifiants d'organisation</param>
        /// <param name="loadNestedObjects">Permet de charger les objets liés (par défaut à true)</param>
        /// <returns>Liste de CI</returns>
        IEnumerable<CIEnt> GetByOrganisationId(IEnumerable<int> organisationIds, bool loadNestedObjects = true);

        /// <summary>
        ///   Récupération de la liste des CI dont l'organisationId est compris dans la liste en paramètre
        /// </summary>
        /// <param name="organisationIds">Liste d'identifiants d'organisation</param>
        /// <returns>Liste des identifiants CI</returns>
        [Obsolete("Prefer to use " + nameof(GetCIIdListByOrganisationIdAsync) + " instead")]
        IEnumerable<int> GetCIIdListByOrganisationId(IEnumerable<int> organisationIds);

        /// <summary>
        ///   Récupération de la liste des CI dont l'organisationId est compris dans la liste en paramètre de manière async
        /// </summary>
        /// <param name="organisationIds">Liste d'identifiants d'organisation</param>
        /// <returns>Liste des identifiants CI</returns>
        Task<IEnumerable<int>> GetCIIdListByOrganisationIdAsync(IEnumerable<int> organisationIds);

        /// <summary>
        /// Retourne les cis appartenant à un établissement comptable pour picklist
        /// </summary>
        /// <param name="organisationId">identifiant unique de l'organisation de l'établissemet comptable</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">taille de la page</param>
        /// <returns>Liste de Ci appartenant à une société</returns>
        IEnumerable<CIEnt> SearchLightOrganisationCiByOrganisationPereId(int organisationId, int page, int pageSize);

        /// <summary>
        /// Renvoie la liste des identifiants d'organisation des cis cloturés
        /// </summary>
        /// <returns>Liste des identifiants d'organisation des cis cloturés</returns>
        List<int> GetOrganisationIdCiClose();

        /// <summary>
        /// Permet d'obtenir la liste des cis generique absence
        /// </summary>
        /// <returns>Liste des CIEnt</returns>
        List<CIEnt> GetCisAbsenceGenerique();

        /// <summary>
        /// PErmet d'avoir le ci à partir de l'établissementComptaId
        /// </summary>
        /// <param name="etablissementComptaId">Identifiant etab compta</param>
        /// <returns>retourne une ci</returns>
        CIEnt GetCiAbsenceGeneriqueByEtabId(int etablissementComptaId);

        #endregion

        #region Gestion des Devises
        /// <summary>
        ///   Retourne la liste des devise d'un CI.
        /// </summary>
        /// <returns>Liste des devises d'un CI</returns>
        /// <param name="ciId">identifiant du ci à mettre a jour</param>
        IEnumerable<CIDeviseEnt> GetCIDevise(int ciId);

        /// <summary>
        /// Récupère les devises pour la comparaison de budget.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI du budget.</param>
        /// <returns>Les devises.</returns>
        List<DeviseDao> GetDevisesPourBudgetComparaison(int ciId);

        /// <summary>
        ///   Retourne la devise de référence d'un CI.
        /// </summary>
        /// <returns>Devise de référence d'un CI</returns>
        /// <param name="ciId">identifiant du ci à mettre a jour</param>
        DeviseEnt GetDeviseRef(int ciId);

        /// <summary>
        ///   Retourne la liste de devise secondaire du CI passé en paramètre
        /// </summary>
        /// <param name="idCI"> Identifiant du CI </param>
        /// <returns> Liste de toutes les sociétés/Devises </returns>
        IEnumerable<DeviseEnt> GetCIDeviseSecList(int idCI);

        /// <summary>
        ///   Evalue si le Ci possèdent plusieurs Devises
        /// </summary>
        /// <param name="idCI"> Identifiant du CI </param>
        /// <returns> Vrai si le Ci possède plusieurs Devises, faux sinon </returns>
        bool IsCiHaveManyDevises(int idCI);

        /// <summary>
        /// Mise à jour d'un CIDevise 
        /// </summary>
        /// <param name="ciDeviseEnt">CIDEvise à mettre à jour</param>
        /// <returns>CIDevise mis à jour</returns>
        CIDeviseEnt UpdateCIDevise(CIDeviseEnt ciDeviseEnt);

        /// <summary>
        ///   Insertion de masse des CIDevise
        /// </summary>
        /// <param name="ciDeviseList">Liste de CIDevise</param>
        void BulkAddCIDevise(IEnumerable<CIDeviseEnt> ciDeviseList);

        /// <summary>
        ///   Sauvegarde les modifications d'une ci.
        /// </summary>
        /// <param name="ciDeviseEnt">relation Ci Devise</param>
        /// <returns>CIDevise crée</returns>
        CIDeviseEnt AddCIDevise(CIDeviseEnt ciDeviseEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'une ci.
        /// </summary>
        /// <param name="listCIDevise">liste des relation Ci Devise</param>    
        /// <returns>Liste des CIDevise mise à jour</returns>
        IEnumerable<CIDeviseEnt> AddOrUpdateCIDevises(IEnumerable<CIDeviseEnt> listCIDevise);

        /// <summary>
        ///   Suppression d'une relation CIDevise en fonction de son identifiant
        /// </summary>
        /// <param name="ciDeviseId">Identifiant de la relation CIDevise</param>
        void DeleteCIDevise(int ciDeviseId);
        #endregion

        #region Gestion des Ressources

        /// <summary>
        ///   Ajoute une relation CIRessource
        /// </summary>
        /// <param name="ciRessource">CIRessource à ajouter</param>
        /// <returns>CIRessource ajouté</returns>
        CIRessourceEnt AddCIRessource(CIRessourceEnt ciRessource);

        /// <summary>
        ///   Mise à jour d'un CIRessource
        /// </summary>
        /// <param name="ciRessource">CIRessource à mettre à jour</param>
        /// <returns>CIRessource mis à jour</returns>
        CIRessourceEnt UpdateCIRessource(CIRessourceEnt ciRessource);

        /// <summary>
        ///   Supprime un CIRessource en fonction de son identifiant
        /// </summary>
        /// <param name="ciRessourceId">Identifiant du CIRessource</param>
        void DeleteCIRessource(int ciRessourceId);

        #endregion

        /// <summary>
        ///   Récupère le ci de passage d'un ci sep ainsi que sa société
        /// </summary>
        /// <param name="ciId">Identifiant unique du ci sep </param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        CIEnt GetCICompteInterneByCiid(int ciId);

        /// <summary>
        /// Récupére une liste de CI en fonction d'une liste d'identifiant (version light sans societe)
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant</param>
        /// <returns>Liste de CI</returns>
        IEnumerable<CIEnt> GetCIsByIdsLight(List<int> ciIds);

        /// <summary>
        /// Récupére une liste de CI en fonction d'une liste d'identifiant
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant</param>
        /// <returns>Liste de CI</returns>
        IEnumerable<CIEnt> GetCIsByIds(List<int> ciIds);

        IReadOnlyList<int> GetCiIdsBySocieteIds(List<int> societeIds);

        /// <summary>
        /// Recupere les ci pour la vue 'liste des cis'
        /// </summary>
        /// <param name="filters">filtre de la vue</param>
        /// <param name="includeInCiList">liste de ci dans laquelle la recherche doit etre faites</param>
        /// <param name="includeInTypeSocietes">liste de type de societe dans laquelle la recherche doit etre faites</param>
        /// <param name="page">la page</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <returns>Liste de ci en fonction de tous les filtre</returns>
        List<CIEnt> GetForCiListView(SearchCIEnt filters, List<int> includeInCiList, List<int?> includeInTypeSocietes, int page, int pageSize);

        /// <summary>
        /// SearchLight pour Lookup des CI Sep
        /// CI visibles par l’utilisateur ET rattachés à une société de type SEP.
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchedText">Texte a rechercher</param>
        /// <param name="ciIdList">Liste des Cis Utilisateur</param>
        /// <returns>Liste de CI</returns>
        List<CIEnt> SearchLightCiSep(int page, int pageSize, string searchedText, List<int> ciIdList);

        /// <summary>
        /// recuperer la liste de CI afféctés à un personnel
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="personnelId">personnel id</param>
        /// <returns>retourne liste des CI d'un personnel donné</returns>
        IEnumerable<CIEnt> SearchLightByPersonnelIdStandard(string text, int page, int pageSize, int personnelId);

        /// <summary>
        /// Get ci for affectation by ci id
        /// </summary>
        /// <param name="ciId">Ci Identifier</param>
        /// <returns>Ci ent</returns>
        CIEnt GetCiForAffectationByCiId(int ciId);

        /// <summary>
        /// Récupère le ci par code et l'identifiant de la société 
        /// </summary>
        /// <param name="code">Code CI</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        CIEnt GetCIByCodeAndSocieteId(string code, int societeId);
        List<CiDateOuvertureDateFermeture> GetDateOuvertureFermeturesCis(List<int> ciIds);
    }
}
