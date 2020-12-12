
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les primes.
    /// </summary>
    public interface IPrimeRepository : IRepository<PrimeEnt>
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
        ///   Retourne la liste des primes actives éligibles à être affectées par un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI pour lequel on recherche les primes.</param>
        /// <returns>La liste des primes actives affectables au CI passé en paramètre.</returns>
        IEnumerable<PrimeEnt> GetPrimesListForCi(int ciId);

        /// <summary>
        ///   Retourne l'union de la liste des primes publiques et de la liste des primes privées affectées au CI passé en
        ///   paramètre filtrée avec le paramètre texte passé en paramètre.
        /// </summary>
        /// <param name="text">Valeur de recherche permettant le filtrage des primes.</param>
        /// <param name="societeId">Identifiant du parent permettant de filtrer les primes</param>
        /// <param name="ciId">Identifiant du CI dont on veux afficher les primes privées affectées.</param>
        /// <returns>La liste des primes publiques accompagnées des primes privées affectées au CI passé en paramètre</returns>
        IEnumerable<PrimeEnt> SearchPrimesAllowedByCi(string text, int societeId, int ciId);

        /// <summary>
        ///   Retourne la prime portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="primeId">Identifiant de la prime à retrouver.</param>
        /// <returns>La prime retrouvée, sinon nulle.</returns>
        Task<PrimeEnt> GetPrimeByIdAsync(int primeId);

        /// <summary>
        ///   Retourne un CiPrime en fonction de l'identifiant d'une prime et l'identifiat d'un ci
        /// </summary>
        /// <param name="primeId">Identifiant du de la prime</param>
        /// <param name="ciId">Identifiant du ci</param>
        /// <returns>Un CIPrimes</returns>
        CIPrimeEnt GetCIPrimeByPrimeIdAndCiId(int primeId, int ciId);

        ///   Retourne la prime portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="code">Identifiant de la prime à retrouver.</param>
        /// <returns>La prime retrouvée, sinon nulle.</returns>
        Task<bool> IsPrimeExistAsync(string code, int? idCourant = null, int? societeId = null);

        /// <summary>
        ///   Ajoute une nouvelle prime.
        /// </summary>
        /// <param name="primeEnt">Prime à ajouter</param>
        /// <returns>L'identifiant de la prime ajoutée</returns>
        Task AddPrimeAsync(PrimeEnt primeEnt);

        /// <summary>
        ///   Sauvegarde les modifications apportée à une prime.
        /// </summary>
        /// <param name="primeEnt">Prime à modifier</param>
        Task UpdatePrimeAsync(PrimeEnt primeEnt);

        /// <summary>
        ///   Supprime une prime.
        /// </summary>
        /// <param name="primeId"> Lidentifiant de la prime à supprimer</param>
        Task DeletePrimeAsync(PrimeEnt prime);

        /// <summary>
        ///   Permet de récupérer la liste de toutes les primes en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="predicate">Prédicat de recherche de primes</param>
        /// <returns>Retourne la liste filtrée de toutes les primes</returns>
        Task<IEnumerable<PrimeEnt>> SearchPrimeAllWithSearchPrimeTextAsync(int societeId, string text, SearchPrimeEnt filters);

        Task<IEnumerable<PrimeEnt>> SearchPrimeAllWithFiltersAsync(int societeId, Expression<Func<PrimeEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer la liste des primes en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Prédicat de recherche de primes</param>
        /// <returns>Retourne la liste filtrée des primes</returns>
        IEnumerable<PrimeEnt> SearchPrimeWithFilters(Expression<Func<PrimeEnt, bool>> predicate);

        /// <summary>
        /// Permet de récupérer une liste de prime d'une société depuis une date
        /// </summary>
        /// <param name="societeId">L'identifant de la société</param>
        /// <param name="lastModification">La date de modification</param>
        /// <returns>Une liste de prime</returns>
        IEnumerable<PrimeEnt> GetPrimes(int? societeId, DateTime lastModification);

        #region Gestion primes avec CI

        /// <summary>
        ///   Retourne la liste complète de CIPrimes
        /// </summary>
        /// <returns>Une liste de CIPrimes</returns>
        IEnumerable<CIPrimeEnt> GetCiPrimeList();

        /// <summary>
        ///   Cherche une liste d'ID de code majoration en fonction d'un ID de CI
        /// </summary>
        /// <param name="ciId">ID du CI pour lequel on recherche les IDs de codes majoration correspndants</param>
        /// <returns>Une liste d'ID de CodeMajoration.</returns>
        List<int> GetPrimesIdsByCiId(int ciId);

        /// <summary>
        ///   Ajout un nouvelle association CI/Prime
        /// </summary>
        /// <param name="ciPrime"> CIPrime à associer</param>
        void AddCiPrimes(CIPrimeEnt ciPrime);

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
        /// <param name="dependanceParameter"></param>
        /// <returns>True = utilisée</returns>
        Task<bool> IsPrimeUsedAsync(int primeId, string dependanceParameter);

        /// <summary>
        /// Get primes by groupe id and Ci id
        /// </summary>
        /// <param name="groupeId">Groupe identifier</param>
        /// <param name="societeId">Societe identifier</param>
        /// <param name="isEtamIac">is affichage Etam/IAC ou ouvrier</param>
        /// <param name="text">Text chercher</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ciId">Ci identifier</param>
        /// <returns>List des codes primes</returns>
        IEnumerable<PrimeEnt> SearchByGroupeAndCiId(int groupeId, int societeId, bool? isEtamIac, string text, int page, int pageSize, int ciId, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null);

        IQueryable<PrimeEnt> SearchLightSEP(string text, SocieteEnt societe, int? ciId, Expression<Func<PrimeEnt, bool>> predicate);

        IQueryable<PrimeEnt> SearchLightBaseAsync(string text, int? societeId, int? ciId, Expression<Func<PrimeEnt, bool>> predicate);

        /// <summary>
        /// Get primes by groupe id
        /// </summary>
        /// <param name="groupeId">Groupe identifier</param>
        /// <param name="societeId">Societe identifier</param>
        /// <param name="isEtamIac">is affichage Etam/IAC ou ouvrier</param>
        /// <param name="text">Text chercher</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List des codes primes</returns>
        IEnumerable<PrimeEnt> SearchByGroupe(int groupeId, int societeId, bool? isEtamIac, string text, int page, int pageSize, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null);
    }
}
