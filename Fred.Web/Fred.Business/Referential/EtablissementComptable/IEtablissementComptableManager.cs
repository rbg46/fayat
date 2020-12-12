using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Web.Models.Referential;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des établissements comptables.
    /// </summary>
    public interface IEtablissementComptableManager : IManager<EtablissementComptableEnt>
    {
        /// <summary>
        ///   Retourne la liste des établissements comptables
        /// </summary>
        /// <returns>Liste des établissements comptables.</returns>
        IEnumerable<EtablissementComptableEnt> GetEtablissementComptableList();

        /// <summary>
        ///   Retourne l'établissement comptable portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementComptableId">Identifiant de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        EtablissementComptableEnt GetEtablissementComptableById(int etablissementComptableId);

        /// <summary>
        ///   Retourne l'établissement comptable portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="code">Identifiant de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        int? GetEtablissementComptableIdByCode(string code);

        /// <summary>
        ///   Vérifie la validité et enregistre un établissement comptable importé depuis ANAËL Finances
        /// </summary>
        /// <param name="etablissements">Liste des entités dont il faut vérifier la validité</param>
        /// <returns>Retourne vrai si l'établissement comptable peut etre importé</returns>
        bool ManageImportedEtablissementComptables(IEnumerable<EtablissementComptableEnt> etablissements);

        /// <summary>
        ///   Ajoute un nouvel établissement comptable.
        /// </summary>
        /// <param name="etablissementComptableEnt">Etablissement comptable à ajouter</param>
        /// <returns>L'identifiant de l'établissement comptable ajouté</returns>
        int AddEtablissementComptable(EtablissementComptableEnt etablissementComptableEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement comptable.
        /// </summary>
        /// <param name="etablissementComptableEnt">Etablissement comptable à modifier</param>
        void UpdateEtablissementComptable(EtablissementComptableEnt etablissementComptableEnt);

        /// <summary>
        ///   Upload CGA Files
        /// </summary>
        /// <param name="file">File and FileName of CGA</param>
        void UploadCGA(string cgaFournitureFileDoc, string cgaLocationFileDoc, string cgaPrestationFileDoc, string cgaFournitureFileName, string cgaLocationFileName, string cgaPrestationFileName);

        /// <summary>
        ///   Is CGA File Exist
        /// </summary>
        /// <param name="file">FileName of CGA</param>
        bool IsCGAFileExist(string cgaFileName);

        /// <summary>
        ///   Liste des etablissement Comptables avec CGA
        /// </summary>
        /// <param name="ecList">Liste des etablissement Comptables</param>
        /// <returns>Renvoie une liste de EtablissementComptable</returns>
        IEnumerable<EtablissementComptableEnt> GetListWithCGA(IEnumerable<EtablissementComptableEnt> ecList);

        /// <summary>
        ///   Etablissement Comptable avec CGA
        /// </summary>
        /// <param name="ecList">Etablissement Comptable</param>
        /// <returns>Renvoie une Etablissement Comptable</returns>
        EtablissementComptableModel GetEtablissementComptableWithCGA(EtablissementComptableModel etablissementComptableModel);

        /// <summary>
        ///   Supprime un établissement comptable.
        /// </summary>
        /// <param name="etablissementComptableEnt">L'établissement comptable à supprimer</param>
        void DeleteEtablissementComptableById(EtablissementComptableEnt etablissementComptableEnt);

        /// <summary>
        ///   Etablissement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie une liste de EtablissementCompteble</returns>
        IEnumerable<EtablissementComptableEnt> GetListBySocieteId(int societeId);

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements compltables en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les établissements compltables</param>
        /// <returns>Retourne la liste filtrée de tous les établissements compltables</returns>
        IEnumerable<EtablissementComptableEnt> SearchListAllWithFilters(Expression<Func<EtablissementComptableEnt, bool>> predicate);

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance d'établissement comptable.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Nouvelle instance d'établissement de paie intialisée</returns>
        EtablissementComptableEnt GetNewEtablissementComptable(int societeId);

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements comptables en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="filters">Filtres de recherche sur tous les établissements comptables</param>
        /// <param name="societeId">Id de la societe</param>
        /// <param name="text">text de recherche</param>
        /// <returns>Retourne la liste filtrée de tous les établissements comptables</returns>
        IEnumerable<EtablissementComptableEnt> SearchEtablissementComptableAllBySocieteIdWithFilters(SearchEtablissementComptableEnt filters, int societeId, string text);

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à un établissement comptable
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'un établissement comptable</returns>
        SearchEtablissementComptableEnt GetDefaultFilter();

        /// <summary>
        ///   Permet de recupérer l'organisation de l'établissement comptable
        /// </summary>
        /// <param name="etablissementId">L'iD de etablissementId</param>
        /// <returns>l'organisation de l'établissement comptable</returns>
        OrganisationEnt GetOrganisationByEtablissementId(int etablissementId);

        /// <summary>
        ///   Permet de connaître l'existence d'une établissement comptable depuis son code.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="codeEtablissementComptable"> code établissement comptable</param>
        /// <param name="societeId">Id de la société</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsEtablissementComptableExistsByCode(int idCourant, string codeEtablissementComptable, int societeId);

        /// <summary>
        ///   Recherche light des etablissements comptables
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Texte recherché</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="showAllOrganisations">désactivation du filtre sur les organisations de l'utilisateur connecté</param>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Liste d'établissements comptable</returns>
        IEnumerable<EtablissementComptableEnt> SearchLight(int page = 1, int pageSize = 20, string recherche = "", int? societeId = null, bool showAllOrganisations = false, int? organisationId = null);

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
        List<EtablissementComptableEnt> GetEtablissementComptableGFESList();

        /// <summary>
        /// Ge current User etab comptable without paranet tree
        /// </summary>
        /// <param name="organisationPereId">L'identifiant de l'organisation pere</param>
        /// <returns>liste des étblissements comptable</returns>
        Task<IEnumerable<EtablissementComptableEnt>> GeCurrentUserEtabComptableWithOrganisationPartentId(int page, int pageSize, string text, int? organisationPereId);

        /// <summary>
        /// Permet d'obtenir la liste des établissements par organisation id
        /// </summary>
        /// <param name="organisationIds">Liste des identifiants</param>
        /// <returns>liste des étblissements</returns>
        IEnumerable<EtablissementComptableEnt> GetEtablissementComptableByOrganisationIds(List<int> organisationIds);
    }
}
