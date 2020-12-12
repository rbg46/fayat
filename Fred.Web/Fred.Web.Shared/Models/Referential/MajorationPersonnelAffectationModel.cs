using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Referential
{
    /// <summary>
    /// personnel prime
    /// </summary>
    public class MajorationPersonnelAffectationModel
    {
        /// <summary>
        /// Personnel identifier
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// List prime affected
        /// </summary>
        public List<MajorationAffectationModel> MajorationList { get; set; }
    }
}
