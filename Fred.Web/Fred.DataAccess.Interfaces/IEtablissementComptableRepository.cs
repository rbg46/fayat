
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les établissements comptables.
    /// </summary>
    public interface IEtablissementComptableRepository : IRepository<EtablissementComptableEnt>
    {
        /// <summary>
        ///   Retourne la liste des établissements comptables.
        /// </summary>
        /// <returns>La liste des établissements comptables.</returns>
        IEnumerable<EtablissementComptableEnt> GetEtablissementComptableList();

        /// <summary>
        ///   Retourne l'établissement comptable portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementComptableId">Identifiant de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        EtablissementComptableEnt GetEtablissementComptableById(int etablissementComptableId);

        /// <summary>
        ///   Retourne l'établissement comptable portant le code indiqué.
        /// </summary>
        /// <param name="etablissementComptableCode"> Code de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        EtablissementComptableEnt GetEtablissementComptableByCode(string etablissementComptableCode);

        /// <summary>
        ///   Retourne l'établissement comptable portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="code">Identifiant de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        int? GetEtablissementComptableIdByCode(string code);

        /// <summary>
        ///   Ajoute un nouvel établissement comptable.
        /// </summary>
        /// <param name="etablissementComptableEnt">Etablissement comptable à ajouter</param>
        /// <returns>L'identifiant de l'établissement comptable ajouté</returns>
        int AddEtablissementComptable(EtablissementComptableEnt etablissementComptableEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement comptable.
        /// </summary>
        /// <param name="etablissementComptable">Etablissement comptable à modifier</param>
        void UpdateEtablissementComptable(EtablissementComptableEnt etablissementComptable);

        /// <summary>
        ///   Supprime un établissement comptable.
        /// </summary>
        /// <param name="etablissementComptableId">L'identifiant du établissement comptable à supprimer</param>
        void DeleteEtablissementComptableById(int etablissementComptableId);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un établissement comptable
        /// </summary>
        /// <param name="etablissementComptable">L'établissement comptable à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(EtablissementComptableEnt etablissementComptable);

        /// <summary>
        ///   Permet de récupérer l'id de l'organisation liée à la établissement spécifié.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement</param>
        /// <returns>Retourne l'identifiant de l'organisation</returns>
        int? GetOrganisationIdByEtablissementId(int? id);

        /// <summary>
        ///   Etablissement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie une liste de EtablissementCompteble</returns>
        IEnumerable<EtablissementComptableEnt> GetListBySocieteId(int societeId);

        ///// <summary>
        ///// Permet de récupérer la liste de tous les établissements compltables en fonction des critères de recherche par société.
        ///// </summary>
        ///// <param name="predicate">Filtres de recherche sur tous les établissements compltables</param>
        ///// <param name="societeId">Id de la societe</param>
        ///// <returns>Retourne la liste filtrée de tous les établissements compltables</returns>

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements compltables en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les établissements compltables</param>
        /// <returns>Retourne la liste filtrée de tous les établissements compltables</returns>
        IEnumerable<EtablissementComptableEnt> SearchListAllWithFilters(Expression<Func<EtablissementComptableEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements compltables en fonction des critères de recherche par
        ///   société.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les établissements compltables</param>
        /// <param name="societeId">Id de la societe</param>
        /// <returns>Retourne la liste filtrée de tous les établissements compltables</returns>
        IEnumerable<EtablissementComptableEnt> SearchEtablissementComptableAllBySocieteIdWithFilters(Expression<Func<EtablissementComptableEnt, bool>> predicate, int societeId);

        /// <summary>
        ///   Permet de récupérer l'organisation liée à la établissement spécifié.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement</param>
        /// <returns>Retourne l'organisation</returns>
        OrganisationEnt GetOrganisationByEtablissementId(int? id);

        /// <summary>
        ///   Permet de connaître l'existence d'une établissement comptable depuis son code.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="codeEtablissementComptable"> code établissement comptable</param>
        /// <param name="societeId"> Id de la société</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsEtablissementComptableExistsByCode(int idCourant, string codeEtablissementComptable, int societeId);

        /// <summary>
        /// Retourne l'établissement parent de l'organisation passée en paramètre.
        /// </summary>
        /// <param name="organisationId">L'identifiant de l'organisation.</param>
        /// <returns>L'établissement parent de l'organisation passée en paramètre ou null.</returns>
        EtablissementComptableEnt GetEtablissementComptableByOrganisationIdEx(int organisationId);

        /// <summary>
        /// Permet d'obtenir la liste des établissements de GFES
        /// </summary>
        /// <returns>liste des étblissements GFES</returns>
        IEnumerable<EtablissementComptableEnt> GetEtablissementComptableGFESList();

        /// <summary>
        /// Get etablissement comptable list by organisation list id
        /// </summary>
        /// <param name="organisationIdList">Organisations ids list</param>
        /// <returns>List des etablissements comptables</returns>
        Task<List<EtablissementComptableEnt>> GetEtabComptableListByOrganisationListId(IEnumerable<int> organisationIdList);

        /// <summary>
        /// Get etablissement comptable list by organisation pere id
        /// </summary>
        /// <param name="organisationPereId">Organisation pere id</param>
        /// <returns>List des etablissements comptables</returns>
        Task<List<EtablissementComptableEnt>> GetEtabComptableListByOrganisationPereIdList(IEnumerable<int> organisationPereId);

        /// <summary>
        /// Permet d'obtenir la liste des établissements par organisation id
        /// </summary>
        /// <param name="organisationIds">Liste des identifiants</param>
        /// <returns>liste des étblissements</returns>
        IEnumerable<EtablissementComptableEnt> GetEtablissementComptableByOrganisationIds(List<int> organisationIds);
    }
}