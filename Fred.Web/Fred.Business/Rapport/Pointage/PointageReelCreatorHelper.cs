using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    /// Helper pour créer un pointage light
    /// </summary>
    public static class PointageReelCreatorHelper
    {
        private const int NbrMaxPrimes = 4;

        /// <summary>
        /// Créer un pointage light
        /// </summary>
        /// <returns>Un pointage light</returns>
        public static RapportLigneEnt GetNewPointageReelLight()
        {
            return new RapportLigneEnt
            {
                PointageId = 0,
                RapportId = 0,
                NbMaxPrimes = NbrMaxPrimes,
                ListRapportLignePrimes = new List<RapportLignePrimeEnt>(),
                ListRapportLigneTaches = new List<RapportLigneTacheEnt>()
            };
        }
    }
}
