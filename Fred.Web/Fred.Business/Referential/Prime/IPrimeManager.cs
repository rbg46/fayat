using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des primes
    /// </summary>
    public interface IPrimeManager : IManager<PrimeEnt>
    {
        /// <summary>
        ///   Retourne la liste des primes.
        /// </summary>
        /// <returns>La liste des primes.</returns>
        IEnumerable<PrimeEnt> GetPrimesList();

        /// <summary>
        ///   Retourne la liste des primes pour la synchronisation mobile.
        /// </summary>
        /// <returns>La liste des primes.</returns>
        IEnumerable<PrimeEnt> GetPrimesListSync();

        /// <summary>
        ///   Retourne la liste des primes actives.
        /// </summary>
        /// <returns>La liste des primes actives.</returns>
        IEnumerable<PrimeEnt> GetActivesPrimesList();

        /// <summary>
        ///   Retourne la liste des primes actives éligibles à être utilisées par un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI pour lequel on recherche les primes.</param>
        /// <returns>La liste des primes actives.</returns>
        IEnumerable<PrimeEnt> GetPrimesListForCI(int ciId);

        /// <summary>
        ///   Retourne un CiPrime en fonction de l'identifiant d'une prime et l'identifiat d'un ci
        /// </summary>
        /// <param name="primeId">Identifiant du de la prime</param>
        /// <param name="ciId">Identifiant du ci</param>
        /// <returns>Un CIPrimes</returns>
        CIPrimeEnt GetCIPrimeByPrimeIdAndCiId(int primeId, int ciId);

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de prime.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Nouvelle instance de prime intialisée</returns>
        PrimeEnt GetNewPrime(int societeId);

        /// <summary>
        ///   Retourne la prime portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="primeId">Identifiant de la prime à retrouver.</param>
        /// <returns>La prime retrouvée, sinon nulle.</returns>
        Task<PrimeEnt> GetPrimeByIdAsync(int primeId);

        /// <summary>
        ///   Retourne l'établissement de paie portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="idCourant">Identifiant de la prime à retrouver.</param>
        /// <param name="code">Code de la prime à retrouver.</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Si oui ou non une prime existe avec ce code</returns>
        Task<bool> IsPrimeExistsByCodeAsync(int idCourant, string code, int societeId);

        /// <summary>
        ///   Ajoute une nouvelle prime.
        /// </summary>
        /// <param name="primeEnt">Prime à ajouter</param>
        /// <returns>L'identifiant de la prime ajoutée</returns>
        Task<PrimeEnt> AddPrimeAsync(PrimeEnt primeEnt);

        /// <summary>
        ///   Sauvegarde les modifications apportée à une prime.
        /// </summary>
        /// <param name="primeEnt">Prime à modifier</param>
        Task UpdatePrimeAsync(PrimeModel primeEnt);

        /// <summary>
        ///   Supprime une prime.
        /// </summary>
        /// <param name="primeEnt">La prime à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        Task DeletePrimeAsync(int primeId);

        /// <summary>
        ///   Permet de récupérer la liste de toutes les primes en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="text">Texte recherché dans toutes les primes</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré de toutes les primes</returns>
        Task<IEnumerable<PrimeEnt>> SearchPrimeAllWithSearchPrimeTextAsync(int societeId, string text, SearchPrimeEnt filters);

        /// <summary>
        ///   Permet de récupérer la liste des primes en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les primes</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des primes</returns>
        IEnumerable<PrimeEnt> SearchPrimeWithFilters(string text, SearchPrimeEnt filters);

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à une prime
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'une prime</returns>
        SearchPrimeEnt GetDefaultFilter();

        /// <summary>
        ///   Recherche de pays dans le référentiel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="societeId">identifiant de la société</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="isRapportPrime">Si on travaille sur un RapportPrime / Mensuel (TRUE) ou sur un Rapport Journalier (FALSE)</param>
        /// <returns>Une liste de primes</returns>
        Task<IEnumerable<PrimeEnt>> SearchLightAsync(string text, int page, int pageSize, int? societeId, int? ciId, bool isRapportPrime);

        /// <summary>
        /// Permet de récupérer les primes à synchroniser.
        /// </summary>
        /// <param name="lastModification">La date de modification</param>
        /// <returns>Une liste de prime</returns>
        IEnumerable<PrimeEnt> GetSyncPrimes(DateTime lastModification = default(DateTime));

        #region Gestion primes avec CI

        /// <summary>
        ///   Retourne la liste complète de CIPrimes
        /// </summary>
        /// <returns>Une liste de CIPrimes</returns>
        IEnumerable<CIPrimeEnt> GetCIPrimeList();

        /// <summary>
        ///   Création / Suppression des relations CIPrime
        /// </summary>
        /// <param name="ciId"> CI à associer</param>
        /// <param name="ciPrimeList"> Primes à associer</param>
        void ManageCIPrime(int ciId, IEnumerable<CIPrimeEnt> ciPrimeList);

        /// <summary>
        ///   Cherche une liste d'ID de code majoration en fonction d'un ID de CI
        /// </summary>
        /// <param name="ciId">ID du CI pour lequel on recherche les IDs de codes majoration correspndants</param>
        /// <returns>Une liste d'ID de CodeMajoration.</returns>
        List<int> GetPrimesIdsByCiId(int ciId);

        /// <summary>
        ///   Ajout un nouvelle association CI/Code majoration
        /// </summary>
        /// <param name="primeId"> Codes majoration à associer</param>
        /// <param name="ciId"> CI à associer</param>
        void AddCIPrimes(int primeId, int ciId);

        /// <summary>
        ///   Supprime un CIPrime à partir des ses IDs Prime et CI
        /// </summary>
        /// <param name="primeId">ID de la prime référencée</param>
        /// <param name="ciId">ID du CI référence</param>
        void DeleteCIPrimeById(int primeId, int ciId);

        #endregion

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="primeId">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        Task<bool> IsPrimeUsedAsync(int primeId);

        /// <summary>
        /// Get primes codes by groupe id
        /// </summary>
        /// <param name="groupeId">Groupe identifier</param>
        /// <param name="societeId">Societe identifier</param>
        /// <param name="isEtamIac">is affichage Etam/IAC ou ouvrier</param>
        /// <param name="text">Text chercher</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ciId">Ci identifier</param>
        /// <returns>List des codes primes</returns>
        IEnumerable<PrimeEnt> SearchByGroupe(int groupeId, int societeId, bool? isEtamIac, string text, int page, int pageSize, int? ciId, bool? isOuvrier = null,
                                                              bool? isEtam = null,
                                                              bool? isCadre = null);
    }
}
