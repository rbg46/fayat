using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    /// personnel prime
    /// </summary>
    public class MajorationPersonnelAffectationEnt
    {
        /// <summary>
        /// Personnel identifier
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// List prime affected
        /// </summary>
        public List<MajorationAffectationEnt> MajorationList { get; set; }
    }
}
