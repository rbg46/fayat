using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.EntityFramework;

namespace Fred.DataAccess.AffectationMoyen
{
    /// <summary>
    /// Affectation moyen type repository
    /// </summary>
    public class AffectationMoyenTypeRepository : FredRepository<AffectationMoyenTypeEnt>, IAffectationMoyenTypeRepository
    {
        public AffectationMoyenTypeRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Get affectation moyen type
        /// </summary>
        /// <returns>IEnumerable of AffectationMoyenTypeEnt</returns>
        public IEnumerable<AffectationMoyenTypeEnt> GetAffectationMoyenType()
        {
            return Query().Include(x => x.AffectationMoyenFamille).Get().ToList();
        }

        /// <summary>
        /// Récupére la liste des codes ci pour les restitutions et les maintenances
        /// </summary>
        /// <param name="codeList">La liste des codes</param>
        /// <returns>List des codes</returns>
        public IEnumerable<string> GetRestitutionAndMaintenanceCiCodes(IEnumerable<string> codeList)
        {
            return Context.AffectationMoyenTypes.Where(x => codeList.Contains(x.Code)).Select(x => x.CiCode).Distinct().ToList();
        }
    }
}
