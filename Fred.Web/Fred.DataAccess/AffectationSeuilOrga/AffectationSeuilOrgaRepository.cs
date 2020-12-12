using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation;
using Fred.EntityFramework;

namespace Fred.DataAccess.AffectationSeuilOrga
{
    /// <summary>
    /// Repo des AffectationSeuilOrgaEnt
    /// </summary>
    public class AffectationSeuilOrgaRepository : FredRepository<AffectationSeuilOrgaEnt>, IAffectationSeuilOrgaRepository
    {
        public AffectationSeuilOrgaRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne la liste des AffectationSeuilOrgaEnt qui ont la meme 'deviseId'
        /// et dont l'organisation est contenue dans la liste 'organisationIds'
        /// et dont le role est contenue dans la liste 'rolesIds'
        /// </summary>
        /// <param name="deviseId">deviseId</param>
        /// <param name="organisationIds">organisationIds</param>
        /// <param name="rolesIds">rolesIds</param>
        /// <returns>liste des AffectationSeuilOrgaEnt</returns>
        public List<AffectationSeuilOrgaEnt> Get(int deviseId, List<int> organisationIds, List<int> rolesIds)
        {
            return Context.SeuilRoleOrgas.Where(x => x.DeviseId == deviseId && organisationIds.Contains(x.OrganisationId.Value) && rolesIds.Contains(x.RoleId.Value)).ToList();

        }
    }
}
