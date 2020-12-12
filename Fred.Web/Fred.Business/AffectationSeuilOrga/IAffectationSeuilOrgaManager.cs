using System.Collections.Generic;
using Fred.Entities.Organisation;

namespace Fred.Business.AffectationSeuilOrga
{
    /// <summary>
    /// Manager des AffectationSeuilOrgaEnt
    /// </summary>
    public interface IAffectationSeuilOrgaManager : IManager<AffectationSeuilOrgaEnt>
    {
        /// <summary>
        /// Retourne la liste des AffectationSeuilOrgaEnt qui ont la meme 'deviseId'
        /// et dont l'organisation est contenue dans la liste 'organisationIds'
        /// et dont le role est contenue dans la liste 'rolesIds'
        /// </summary>
        /// <param name="deviseId">deviseId</param>
        /// <param name="organisationIds">organisationIds</param>
        /// <param name="rolesIds">rolesIds</param>
        /// <returns>liste des AffectationSeuilOrgaEnt</returns>
        List<AffectationSeuilOrgaEnt> Get(int deviseId, List<int> organisationIds, List<int> rolesIds);
    }
}
