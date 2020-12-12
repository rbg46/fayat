
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.CI;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données des CIPrime
    /// </summary>
    public interface ICIPrimeRepository : IRepository<CIPrimeEnt>
    {
        /// <summary>
        /// Permet de récupérer une liste de CI/Prime privée pour une société depuis une date
        /// </summary>
        /// <param name="societeId">L'identifant de la société</param>
        /// <param name="lastModification">La date de modification</param>
        /// <returns>Une liste de prime</returns>
        IEnumerable<CIPrimeEnt> GetCIPrimePrivated(int? societeId, DateTime lastModification);

        /// <summary>
        /// Permet de récupérer les CI primes pour la synchronisation mobile.
        /// </summary>
        /// <returns>Les CI primes</returns>
        IEnumerable<CIPrimeEnt> GetCIPrimeSync();

        /// <summary>
        /// Permet de récupérer l'id d'un CI Prime anticipé lié au à la prime spécifiée.
        /// </summary>
        /// <param name="primeId">Identifiant du code de la prime</param>
        /// <returns>Retourne l'identifiant du 1er CI prime</returns>
        Task<int> GetCIPrimeIdAsync(int primeId);
    }
}
