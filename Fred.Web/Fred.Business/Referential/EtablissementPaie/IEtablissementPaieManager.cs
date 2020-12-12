using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Referential;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des établissements de paie.
    /// </summary>
    public interface IEtablissementPaieManager : IManager<EtablissementPaieEnt>
    {
        /// <summary>
        ///   Retourne la liste des établissements de paie
        /// </summary>
        /// <returns>Liste des établissements de paie.</returns>
        IEnumerable<EtablissementPaieEnt> GetEtablissementPaieList();

        /// <summary>
        ///   Méthode GET de récupération de tous les établissements de paie éligibles à être une agence de rattachement
        /// </summary>
        /// <param name="currentEtabPaieId">ID de l'établissement de paie à exclure de la recherche</param>
        /// <returns>Retourne une nouvelle instance d'établissement de paie intialisée</returns>
        IEnumerable<EtablissementPaieEnt> AgencesDeRattachement(int currentEtabPaieId);

        /// <summary>
        ///   Retourne l'établissement de paie portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementPaieId">Identifiant de l'établissement de paie à retrouver.</param>
        /// <returns>L'établissement de paie retrouvé, sinon nulle.</returns>
        EtablissementPaieEnt GetEtablissementPaieById(int etablissementPaieId);

        /// <summary>
        ///   Permet de connaître l'existence d'un établissement de paie depuis son code.
        /// </summary>
        /// <param name="idCourant">L'IdCourant de l'établissement</param>
        /// <param name="code">Le code</param>
        /// <param name="libelle">Le libellé</param>
        /// <returns>Retourne vrai si le code + le libellés passés en paramètres existent déjà, faux sinon</returns>
        bool IsEtablissementPaieExistsByCodeLibelle(int idCourant, string code, string libelle);

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance d'établissement de paie.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <returns>Nouvelle instance d'établissement de paie intialisée</returns>
        EtablissementPaieEnt GetNewEtablissementPaie(int societeId);

        /// <summary>
        ///   Ajoute un nouvel établissement de paie.
        /// </summary>
        /// <param name="etablissementPaieEnt">Etablissement de paie à ajouter</param>
        /// <returns>L'identifiant de l'établissement de paie ajouté</returns>
        int AddEtablissementPaie(EtablissementPaieEnt etablissementPaieEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement de paie.
        /// </summary>
        /// <param name="etablissementPaieEnt">Etablissement de paie à modifier</param>
        void UpdateEtablissementPaie(EtablissementPaieEnt etablissementPaieEnt);

        /// <summary>
        ///   Supprime un établissement de paie.
        /// </summary>
        /// <param name="etablissementPaieEnt">L'établissement de paie à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        bool DeleteEtablissementPaieById(EtablissementPaieEnt etablissementPaieEnt);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un  établissement comptable
        /// </summary>
        /// <param name="etablissementPaieEnt">L'établissement de paie à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(EtablissementPaieEnt etablissementPaieEnt);

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="agenceId">Agence de rattachement</param>
        /// <param name="isHorsRegion">Est hors région</param>
        /// <param name="isAgenceRattachement">Est agence de rattachement</param>    
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<EtablissementPaieEnt> SearchLight(string text, int page, int pageSize, int? societeId = null, int? agenceId = null, bool? isHorsRegion = null, bool? isAgenceRattachement = null);

        /// <summary>
        ///   Retourne la liste des établissements de paie par societe ID.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <returns>Liste des établissements de paie.</returns>
        IEnumerable<EtablissementPaieEnt> GetEtablissementPaieBySocieteId(int societeId);

        /// <summary>
        ///   Retourne la liste des établissements de paie par societe ID.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Liste des établissements de paie.</returns>
        IEnumerable<EtablissementPaieEnt> GetEtablissementPaieByOrganisationId(int organisationId);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <param name="text"> Texte recherché dans tous les codes déplacement </param>
        /// <param name="filters"> Filtres de recherche </param>
        /// <returns> Retourne la liste filtré de tous les codes déplacement </returns>
        IEnumerable<EtablissementPaieEnt> SearchEtablissementPaieAllWithFilters(int societeId, string text, SearchEtablissementPaieEnt filters);

        /// <summary>
        ///   Permet de connaître l'existence d'un etab depuis son code.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="code">Le code de déplacement</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsCodeEtablissementPaieExistsByCode(int idCourant, string code, int societeId);

        /// <summary>
        /// Get etab paie list for validation pointage vrac Fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="societeId">Societe Id</param>
        /// <returns>Etab Paie List</returns>
        Task<IEnumerable<EtablissementPaieEnt>> GetEtabPaieListForValidationPointageVracFesAsync(int page, int pageSize, string recherche, int? societeId);
    }
}