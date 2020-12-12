using System.Collections.Generic;
using Fred.Entities.Moyen;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Affectation moyen type repository
    /// </summary>
    public interface IAffectationMoyenTypeRepository : IFredRepository<AffectationMoyenTypeEnt>
    {
        /// <summary>
        /// Get affectation moyen type
        /// </summary>
        /// <returns>IEnumerable of AffectationMoyenTypeEnt</returns>
        IEnumerable<AffectationMoyenTypeEnt> GetAffectationMoyenType();

        /// <summary>
        /// Récupére la liste des codes ci pour les restitutions et les maintenances
        /// </summary>
        /// <param name="codeList">La liste des codes</param>
        /// <returns>List des codes</returns>
        IEnumerable<string> GetRestitutionAndMaintenanceCiCodes(IEnumerable<string> codeList);
    }
}
