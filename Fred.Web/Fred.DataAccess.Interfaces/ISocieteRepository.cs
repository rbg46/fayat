using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les sociétés.
    /// </summary>
    public interface ISocieteRepository : IFredRepository<SocieteEnt>
    {
        /// <summary>
        ///   Retourne la liste de toutes les sociétés.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        IEnumerable<SocieteEnt> GetSocieteListAll();

        /// <summary>
        ///   Retourne la liste de toutes les sociétés pour la synchronisation mobile.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        IEnumerable<SocieteEnt> GetSocieteListAllSync();

        /// <summary>
        ///   Vérifie si un groupe ne possède pas déjà une société intérimaire
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Vrai si le groupe possède une société intérimaire, sinon faux</returns>
        bool FindSocieteInterimaire(int groupeId);

        /// <summary>
        ///   Retourne la liste des sociétés.
        /// </summary>
        /// <returns>La liste des sociétés.</returns>
        IEnumerable<SocieteEnt> GetSocieteList();

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code et libelle.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="codeSociete"> code societe</param>
        /// <param name="libelle">libelle</param>
        /// <returns>Retourne la SocieteEnt qui est identique</returns>
        SocieteEnt GetSocieteWithSameCodeOrLibelle(int idCourant, string codeSociete, string libelle);

        /// <summary>
        ///   Retourne la société dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à retrouver.</param>
        /// <param name="includeGroupe">Si vrai, on charge le groupe de la société</param>
        /// <returns>La société retrouvée, sinon nulle.</returns>
        SocieteEnt GetSocieteById(int societeId, bool includeGroupe);

        /// <summary>
        ///   Retourne la société dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à retrouver.</param>
        /// <returns>La société retrouvée, sinon nulle.</returns>
        SocieteEnt GetSocieteWithOrganisation(int societeId);

        /// <summary>
        ///   Retourne l'identifiant de la société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>identifiant retrouvé, sinon nulle.</returns>
        int? GetSocieteIdByCodeSocieteComptable(string code);

        /// <summary>
        ///   Ajout une nouvelle société
        /// </summary>
        /// <param name="societe"> société à ajouter</param>
        /// <returns>Société ajoutée</returns>
        SocieteEnt AddSociete(SocieteEnt societe);

        /// <summary>
        ///   Sauvegarde des modifications d'un société
        /// </summary>
        /// <param name="societe">société à modifier</param>
        /// <returns>Société mise à jour</returns>
        SocieteEnt UpdateSociete(SocieteEnt societe);

        /// <summary>
        ///   Supprime un société
        /// </summary>
        /// <param name="id">L'identifiant de la société à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool DeleteSocieteById(int id);

        /// <summary>
        ///   Vérifie qu'une societe peut être supprimée
        /// </summary>
        /// <param name="societeId">L'identifiant de la société à vérifier</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(int societeId);

        /// <summary>
        ///   Retourne l'identifiant de la société portant le code indiqué.
        /// </summary>
        /// <param name="id">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>identifiant retrouvé, sinon nulle.</returns>
        string GetCodeSocietePayeById(int id);

        /// <summary>
        ///   Retourne l'identifiant de la société portant le code indiqué.
        /// </summary>
        /// <param name="codeSociete">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>identifiant retrouvé, sinon nulle.</returns>
        int? GetSocieteIdByCodeSocietePaye(string codeSociete);

        /// <summary>
        ///   Permet de récupérer la liste de toutes les sociétés en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur toutes les sociétés</param>
        /// <returns>Retourne la liste filtrée de toutes les sociétés</returns>
        IEnumerable<SocieteEnt> SearchSocieteAllWithFilters(Expression<Func<SocieteEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer l'id de l'organisation liée à la société spécifiée.
        /// </summary>
        /// <param name="id">Identifiant de la société</param>
        /// <returns>Retourne l'identifiant de l'organisation</returns>
        int? GetOrganisationIdBySocieteId(int? id);

        /// <summary>
        ///   Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        IEnumerable<SocieteEnt> SearchLight(string text);

        /// <summary>
        ///   Retourne la société parent de l'organisation passé en paramètre
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
        SocieteEnt GetSocieteParentByOrgaIdEx(int organisationId, bool withIncludeTypeSociete);

        /// <summary>
        ///   Retourne la liste des devises de la societe
        /// </summary>
        /// <param name="societeId">La société</param>
        /// <returns>la liste des devises de la societe</returns>
        IEnumerable<DeviseEnt> GetListDeviseBySocieteId(int societeId);

        /// <summary>
        ///   Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        SocieteEnt GetSocieteByCodeSocieteComptable(string code);

        /// <summary>
        ///   Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        SocieteEnt GetSocieteByCodeSocietePaye(string code);

        /// <summary>
        ///   Retourne la liste des sociétés dont les factures doivent être importées
        /// </summary>
        /// <returns>La liste des sociétés dontles factures doivent être importées.</returns>
        IEnumerable<SocieteEnt> GetListSocieteToImportFacture();

        /// <summary>
        ///   Retourne la devise de référence de la societe
        /// </summary>
        /// <param name="societeId">La société</param>
        /// <returns>la devise de référence de la société</returns>
        DeviseEnt GetDeviseRefBySocieteId(int societeId);

        /// <summary>
        ///   Récupère la société en fonction de l'identifiant de son organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Société</returns>
        SocieteEnt GetSocieteByOrganisationId(int organisationId);

        /// <summary>
        /// Retourne La société en fonction de son code et de son identifiant unique de groupe 
        /// </summary>
        /// <param name="code">Code de la societe</param>
        /// <param name="groupeId">Indentifiant unique du groupe</param>
        /// <returns>la société</returns>
        SocieteEnt GetSocieteByCodeAndGroupeId(string code, int groupeId);

        /// <summary>
        ///   Retourner la requête de récupération sociétés
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
        /// Retourne vrai si la société appartient au groupe passé en paramètre
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="codeGroupe">Code du groupe</param>
        /// <returns>Vrai si la société appartient au groupe</returns>
        bool IsSocieteIdInGroupe(int societeId, string codeGroupe);

        Task<Dictionary<string, int>> GetCompanyIdsByPayrollCompanyCodesAsync();

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        SocieteEnt GetSocieteByCodeSocieteComptables(string code);

        /// <summary>
        /// Retourne la liste des indentifiant des société pour un identifiant de groupe
        /// </summary>
        /// <param name="groupeId">identifiant de groupe</param>
        /// <returns> la liste des indentifiant des société</returns>
        IReadOnlyList<int> GetSocieteIdsByGroupeId(int groupeId);

        Task<int?> GetSocieteImageLogoIdByOrganisationIdAsync(int organisationId);

        List<SocieteEnt> GetSocieteBySirenAndGroupeCode(string siren, string groupeCode);

        /// <summary>
        /// Obtenir la liste des sociétés par organisation
        /// </summary>
        /// <param name="organisationIds">List des organisations ids</param>
        /// <returns>Liste des sociétés</returns>
        IEnumerable<SocieteEnt> GetSocieteListByOrganisationIds(List<int> organisationIds);

        Task<string> GetCodeSocieteComptableByCodeAsync(string code);
        Task<string> GetCodeSocieteComptableByIdAsync(int id);

        int GetSepGerantSocieteId(int ciId);
    }
}
