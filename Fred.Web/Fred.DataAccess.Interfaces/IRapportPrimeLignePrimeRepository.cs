using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.RapportPrime;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Représente un référentiel de données des primes d'une ligne de rapports de primes.
    /// </summary>
    public interface IRapportPrimeLignePrimeRepository : IRepository<RapportPrimeLignePrimeEnt>
    {
        /// <summary>
        /// Permet l'insertion d'une liste de ligne de prime
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        Task InsertRangeAsync(List<RapportPrimeLignePrimeEnt> list);

        /// <summary>
        /// Supprime une liste de ligne de prime
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        Task DeleteRangeAsync(List<RapportPrimeLignePrimeEnt> list);

        /// <summary>
        /// Mise à jour par liste de ligne de prime
        /// </summary>
        /// <param name="list">Lite à mettre à jour</param>
        Task UpdateRangeAsync(List<RapportPrimeLignePrimeEnt> list);

        /// <summary>
        /// Retourne la liste des ligne de prime pour un rapports de prime en fonction d'un date
        /// </summary>
        /// <param name="periode">Periode</param>
        /// <returns>liste de prime associée a un rapport de ligne de prime</returns>
        List<RapportPrimeLignePrimeEnt> GetRapportPrimeLignePrime(DateTime periode);

        Task<int> GetRapportPrimeLignePrimeIdAsync(int primeId);
    }
}
