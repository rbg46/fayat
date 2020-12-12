using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.Validation
{
    /// <summary>
    /// Conteneur des données pour la validation pointage
    /// </summary>
    public class GlobalDataForValidationPointage
    {
        /// <summary>
        /// RapportsLignesOnAllRapports
        /// </summary>
        public IEnumerable<RapportLigneEnt> RapportsLignesOnAllRapports { get; set; } = new List<RapportLigneEnt>();

        /// <summary>
        /// RapportsLignesWithPrimes
        /// </summary>
        public IEnumerable<RapportLigneEnt> RapportsLignesWithPrimes { get; set; } = new List<RapportLigneEnt>();

        /// <summary>
        /// CiIdsOfPointagesWithRolePaieForCurrentUser
        /// </summary>
        public List<int> CiIdsOfPointagesWithRolePaieForCurrentUser { get; set; } = new List<int>();
    }
}
