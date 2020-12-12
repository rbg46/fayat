using Fred.Entities.CI;
using Fred.Entities.DatesClotureComptable;

namespace Fred.Entities.CloturesPeriodes
{
    /// <summary>
    /// CiDateClotureComptableNavigableEnt
    /// </summary>
    public class CiDateClotureComptableNavigableEnt
    {
        /// <summary>
        /// Ci
        /// </summary>
        public CIEnt Ci { get; set; }

        /// <summary>
        /// DatesClotureComptable
        /// </summary>
        public DatesClotureComptableEnt DatesClotureComptable { get; set; }
    }
}
