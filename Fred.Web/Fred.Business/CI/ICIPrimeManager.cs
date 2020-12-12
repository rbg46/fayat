using Fred.Entities.CI;
using System;
using System.Collections.Generic;

namespace Fred.Business.CI
{
  /// <summary>
  ///   Interface des gestionnaires des CIPrimes
  /// </summary>
  public interface ICIPrimeManager : IManager<CIPrimeEnt>
  {
    /// <summary>
    /// Permet de récupérer les lien entre les primes privées et les cis à synchroniser.
    /// </summary>
    /// <param name="lastModification">La date de modification</param>
    /// <returns>Une liste de prime</returns>
    IEnumerable<CIPrimeEnt> GetSyncCIPrimes(DateTime lastModification);
   
    /// <summary>
    /// Permet de récupérer toutes les primes
    /// </summary>
    /// <returns>Une liste de prime</returns>
    IEnumerable<CIPrimeEnt> GetPrimes();

    /// <summary>
    /// Permet de récupérer toutes les primes pour la synchronisation mobile.
    /// </summary>
    /// <returns>Une liste de prime</returns>
    IEnumerable<CIPrimeEnt> GetPrimesSync();
  }
}
