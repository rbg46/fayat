using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Groupe;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.GroupSpecific.Infrastructure;

namespace Fred.Business.Societe
{
    /// <summary>
    ///  Interface des gestionnaires des sociétés
    /// </summary>
    public interface ISocieteManager : IManager<SocieteEnt>, IGroupAwareService
    {
        /// <summary>
        ///  Retourne la liste de toutes les sociétés.
        /// </summary>
        /// <returns>Liste de toutes les sociétés.</returns>
        IEnumerable<SocieteEnt> GetSocieteListAll();

        /// <summary>
        ///  Retourne la liste de toutes les sociétés pour la synchronisation mobile.
        /// </summary>
        /// <returns>Liste de toutes les sociétés.</returns>
        IEnumerable<SocieteEnt> GetSocieteListAllSync();

        /// <summary>
        ///  Retourne la liste des Sociétés.
        /// </summary>
        /// <returns>Renvoie la liste des sociétés.</returns>
        IEnumerable<SocieteEnt> GetSocieteList();

        /// <summary>
        ///  Permet de connaître l'existence d'une société depuis son code ou libelle.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="codeSociete">Le code de la société</param>
        /// <param name="libelle">libelle</param>
        /// <returns>Retourne un SocieteExistResult</returns>
        SocieteExistResult GetSocieteExistsWithSameCodeOrLibelle(int idCourant, string codeSociete, string libelle);

        /// <summary>
        ///  Permet l'initialisation d'une nouvelle instance de société.
        /// </summary>
        /// <returns>Nouvelle instance de société intialisée</returns>
        SocieteEnt GetNewSociete();

        /// <summary>
        ///  Retourne la société portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à retrouver.</param>
        /// <param name="includeGroupe">Si vrai, on charge le groupe de la société</param>        
        /// <returns>la société retrouvée, sinon nulle.</returns>
        SocieteEnt GetSocieteById(int societeId, bool includeGroupe = false);

        /// <summary>
        ///  Retourne la société portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à retrouver.</param>
        /// <returns>la société retrouvée, sinon nulle.</returns>
        SocieteEnt GetSocieteWithOrganisation(int societeId);

        /// <summary>
        ///  Retourne la société intérimaire (SOC_INTERIM) par défaut
        /// </summary>    
        /// <returns>la société intérimaire par défaut</returns>
        SocieteEnt GetDefaultSocieteInterim();

        /// <summary>
        /// Rechercher les unités par société
        /// </summary>
        /// <param name="recherche">text</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="type">Identifiant de la ressource courante dans la lookup</param>
        /// <returns>les unités par société</returns>
        IEnumerable<UniteEnt> SearchLightUniteBySocieteId(string recherche, int societeId, int page, int pageSize, string type);

        /// <summary>
        /// Retourne une société avec ses paramètres
        /// </summary>
        /// <param name="societeId">Id de la société à récupérer</param>
        /// <returns>L'entité de la société, null si aucune société n'existe pour cet id</returns>
        SocieteEnt GetSocieteByIdWithParameters(int societeId);

        /// <summary>
        ///  Retourne l'identifiant de la société portant le code société comptable indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>identifiant retrouvé, sinon nulle.</returns>
        int? GetSocieteIdByCodeSocieteComptable(string code);

        /// <summary>
        /// Retourne la liste des indentifiant des société pour un identifiant de groupe
        /// </summary>
        /// <param name="groupeId">identifiant de groupe</param>
        /// <returns> la liste des indentifiant des société
        IReadOnlyList<int> GetSocieteIdsByGroupeId(int groupeId);

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        SocieteEnt GetSocieteByCodeSocieteComptable(string code);

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        SocieteEnt GetSocieteByCodeSocietePaye(string code);

        /// <summary>
        ///  Ajout une nouvelle Société.
        /// </summary>
        /// <param name="societeEnt"> Société à ajouter.</param>
        /// <returns>Société ajoutée.</returns>    
        Task<SocieteEnt> AddSocieteAsync(SocieteEnt societeEnt);

        /// <summary>
        ///  Sauvegarde les modifications d'un Société.
        /// </summary>
        /// <param name="societeEnt">Société à modifier</param>
        /// <returns>Société mise à jour</returns>
        Task<SocieteEnt> UpdateSocieteAsync(SocieteEnt societeEnt);

        /// <summary>
        ///  Supprime un Société.
        /// </summary>
        /// <param name="societeId">La société à supprimer.</param>
        void DeleteSocieteById(int societeId);

        /// <summary>
        ///  Retourne l'identifiant de la société portant le code société paye indiqué.
        /// </summary>
        /// <param name="codeSociete">Code de la société de paye dont l'identifiant est à retrouver.</param>
        /// <returns>identifiant retrouvé, sinon nulle.</returns>
        int? GetSocieteIdByCodeSocietePaye(string codeSociete);

        /// <summary>
        ///  Retourne la liste des sociétés dont les factures doivent être importées
        /// </summary>
        /// <returns>La liste des sociétés dontles factures doivent être importées.</returns>
        IEnumerable<SocieteEnt> GetListSocieteToImportFacture();

        /// <summary>
        ///  Permet de récupérer la liste de toutes les sociétés en fonction des critères de recherche.
        /// </summary>    
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré de toutes les sociétés</returns>
        IEnumerable<SocieteEnt> SearchSocieteAllWithFilters(SearchSocieteEnt filters);

        /// <summary>
        ///  Retourne la société parent de l'organisation passé en paramètre
        /// </summary>
        /// <param name="organisationId">ID du de l'organisation</param>
        /// <returns>societe parent de l'organisation passé en paramètre</returns>
        SocieteEnt GetSocieteParentByOrgaId(int organisationId);

        /// <summary>
        /// Retourne la société parente de l'organisation passée en paramètre.
        /// </summary>
        /// <param name="organisationId">L'identifiant de l'organisation.</param>
        /// <param name="withIncludeTypeSociete">Indique si on veux charger le type de la société</param>
        /// <returns>La societé parente de l'organisation passée en paramètre ou null si aucune société trouvée.</returns>
        /// <remarks>La fonction GetSocieteParentByOrgaId semble ne pas fonctionner.</remarks>
        SocieteEnt GetSocieteByOrganisationIdEx(int organisationId, bool withIncludeTypeSociete);

        /// <summary>
        ///  Retourne la liste des devises de la societe
        /// </summary>
        /// <param name="societeId">La société</param>
        /// <returns>la liste des devises de la societe</returns>
        IEnumerable<DeviseEnt> GetListDeviseBySocieteId(int societeId);

        /// <summary>
        ///  Récupère la liste des devises à partir d'une societe
        /// </summary>
        /// <param name="societe">Société servant de reference pour la récupération des données</param>
        /// <returns>Retourne une enumeration de devise</returns>
        IEnumerable<SocieteDeviseEnt> GetListSocieteDevise(SocieteEnt societe);

        /// <summary>
        ///  Récupère la liste des devises de reference à partir d'une societe
        /// </summary>
        /// <param name="societe">Société servant de reference pour la récupération des données</param>
        /// <returns>Retourne une devise</returns>
        DeviseEnt GetListSocieteDeviseRef(SocieteEnt societe);

        /// <summary>
        ///  Récupère la liste des devises secondaire à partir d'une societe
        /// </summary>
        /// <param name="societeId">Société servant de reference pour la récupération des données</param>
        /// <returns>Retourne une enumeration de devise</returns>
        IEnumerable<DeviseEnt> GetListSocieteDeviseSec(int societeId);

        /// <summary>
        ///  Suppression d'un couple societe - devise
        /// </summary>
        /// <param name="societeDevise">Association societe - devise à supprimer</param>
        void DeleteDeviseBySocieteDevise(SocieteDeviseEnt societeDevise);

        /// <summary>
        ///  Suppression des association devise societe par id societe
        /// </summary>
        /// <param name="idSociete">Id de la societe</param>
        void DeleteDeviseBySocieteId(int idSociete);

        /// <summary>
        ///  Permet de récupérer les champs de recherche lié à une société.
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'une société</returns>
        SearchSocieteEnt GetDefaultFilter();

        /// <summary>
        ///  Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="typeSocieteCodes">Liste de codes type société</param>
        /// <param name="isExterne">Société Externe ou non</param>
        /// <returns>Retourne une liste des référentiels</returns>
        IEnumerable<SocieteEnt> SearchLight(string text, int page, int pageSize, List<string> typeSocieteCodes, bool? isExterne = null);

        /// <summary>
        /// Recherche les societes pour l'utilisateur courant.
        /// Si l'utilisateur est admin on retourne toutes les societes
        /// </summary>
        /// <param name="text">texte recherche</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="typeSocieteCodes">Liste de codes type de sociétés</param>
        /// <returns>Une liste de societe</returns>
        IEnumerable<SocieteEnt> SearchLightForRoles(string text, int page, int pageSize, List<string> typeSocieteCodes);

        /// <summary>
        ///  Permet de récupérer l'id de l'organisation liée à la société spécifiée.
        /// </summary>
        /// <param name="id">Identifiant de la société</param>
        /// <returns>Retourne l'identifiant de l'organisation</returns>
        int? GetOrganisationIdBySocieteId(int? id);

        /// <summary>
        ///  Récupère la liste des association SocieteDevise d'une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des associations SocieteDevise</returns>
        IEnumerable<SocieteDeviseEnt> GetSocieteDeviseList(int societeId);

        /// <summary>
        ///  Ajout / Mise à jour ou Suppression d'une relation SocieteDevise
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="societeDeviseList">Liste des relations SocieteDevise</param>
        /// <returns>Liste mise à jour</returns>
        IEnumerable<SocieteDeviseEnt> ManageSocieteDeviseList(int societeId, IEnumerable<SocieteDeviseEnt> societeDeviseList);

        /// <summary>
        ///  Ajout ou Suppression d'une relation SocieteUnite
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="uniteSocieteList">Liste des relations SocieteUnite</param>
        /// <returns>Liste mis à jour</returns>
        IEnumerable<UniteSocieteEnt> ManageSocieteUniteList(int societeId, IEnumerable<UniteSocieteEnt> uniteSocieteList);

        /// <summary>
        ///  Récupère la société en fonction de l'identifiant de son organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Société</returns>
        SocieteEnt GetSocieteByOrganisationId(int organisationId);

        /// <summary>
        /// Ajoute une relation société/unité
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="uniteId">Identifiant de l'unité</param>
        void AddSocieteUnite(int societeId, int uniteId);

        /// <summary>
        /// Supprime une relation société/unité
        /// </summary>
        /// <param name="uniteSocieteId">Identifiant de la relation société/unité</param>
        void DeleteSocieteUnite(int uniteSocieteId);

        /// <summary>
        /// Retourne la liste de société/unité
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste de société/unité</returns>
        List<UniteSocieteEnt> GetListSocieteUnite(int societeId);

        /// <summary>
        /// Retourne vrai si la societe appartient à un groupe
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <param name="codeGroupe">Code du groupe</param>
        /// <returns>Vrai si la societe appartient à un groupe</returns>
        bool IsSocieteInGroupe(int societeId, string codeGroupe);

        /// <summary>
        /// Retourne La société en fonction de son code et de son identifiant unique de groupe 
        /// </summary>
        /// <param name="code">Code de la societe</param>
        /// <param name="groupeId">Indentifiant unique du groupe</param>
        /// <returns>la société</returns>
        SocieteEnt GetSocieteByCodeAndGroupeId(string code, int groupeId);

        /// <summary>
        /// Récupération d'une société par son identifiant
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="includes">Propriété a inclure</param>
        /// <param name="asNoTracking">boolean pour récupéré la société avec la condition asNoTracking</param>
        /// <returns>Société retrouvée</returns>
        SocieteEnt GetSocieteById(int societeId, List<Expression<Func<SocieteEnt, object>>> includes, bool asNoTracking = false);

        /// <summary>
        /// Retourne vrai si la société appartient au groupe passé en paramètre
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="codeGroupe">Code du groupe</param>
        /// <returns>Vrai si la société appartient au groupe</returns>
        bool IsSocieteIdInGroupe(int societeId, string codeGroupe);

        Task<Dictionary<string, int>> GetCompanyIdsByPayrollCompanyCodesAsync();

        /// <summary>
        /// Récupération de la société intérim du groupe
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Société intérimaire</returns>
        SocieteEnt GetSocieteInterim(int groupeId);

        /// <summary>
        /// Recupertaion des societes avec une liste de societeIds
        /// </summary>
        /// <param name="societeIds">liste de societe Ids</param>
        /// <returns>liste de societe</returns>
        List<SocieteEnt> GetAllSocietesByIds(List<int> societeIds);

        /// <summary>
        ///  Cette Methode sert pour recuperer la list des ID de type de societe 
        /// </summary>
        /// <param name="isSEP">prend la valeur True si il y un filter pour les societes SEP </param>
        /// <returns>liste des Ids de type  societe ou bien liste contient l ID de type societe SEP</returns>
        List<int?> GetTypeSocieteId(bool isSEP);

        /// <summary>
        ///  Retourner la requête de récupération des societes
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        List<SocieteEnt> Search(List<Expression<Func<SocieteEnt, bool>>> filters,
                                                  Func<IQueryable<SocieteEnt>, IOrderedQueryable<SocieteEnt>> orderBy = null,
                                                  List<Expression<Func<SocieteEnt, object>>> includeProperties = null,
                                                  int? page = null,
                                                  int? pageSize = null,
                                                  bool asNoTracking = false);
        /// <summary>
        /// PErmet d'obtenir la liste des groupes et sociétés  en tenant compte des habilitations de l’utilisateur 
        /// </summary>
        /// <returns>liste des sociétés groupé par Groupe</returns>
        IEnumerable<GroupeEnt> GetSocietesGroupesByUserHabibilitation();

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        SocieteEnt GetSocieteByCodeSocieteComptables(string code);

        Task<int?> GetSocieteImageLogoIdByCodeAsync(int organisationId);

        List<SocieteEnt> GetSocieteBySirenAndGroupeCode(string siren, string groupeCode);

        /// <summary>
        /// Obtenir la liste des sociétés par organisation
        /// </summary>
        /// <param name="organisationIds">List des organisations ids</param>
        /// <returns>Liste des sociétés</returns>
        List<SocieteEnt> GetSocieteByOrganisationIds(List<int> organisationIds);

        /// <summary>
        /// Get societes list for remontee vrac fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="recherche">Recherche</param>
        /// <returns>List des societes</returns>
        Task<IEnumerable<SocieteEnt>> GetSocietesListForRemonteeVracFesAsync(int page, int pageSize, string recherche);
    }
}
