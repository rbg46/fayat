using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation;

namespace Fred.Business.AffectationSeuilOrga
{
    /// <summary>
    /// Manager des AffectationSeuilOrgaEnt
    /// </summary>
    public class AffectationSeuilOrgaManager : Manager<AffectationSeuilOrgaEnt, IAffectationSeuilOrgaRepository>, IAffectationSeuilOrgaManager
    {
        public AffectationSeuilOrgaManager(IUnitOfWork uow, IAffectationSeuilOrgaRepository affectationSeuilOrgaRepository)
            : base(uow, affectationSeuilOrgaRepository)
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
            return Repository.Get(deviseId, organisationIds, rolesIds);
        }
    }
}
