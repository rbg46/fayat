using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.SeuilValidation
{
    /// <summary>
    /// Repo des SeuilValidationEnt
    /// </summary>
    public class SeuilValidationRepository : FredRepository<SeuilValidationEnt>, ISeuilValidationRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="SeuilValidationRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow"> Unit Of Work</param>
        public SeuilValidationRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne la liste SeuilValidationEnt eyant la devise 'deviseId' et qui dont le role est contenue dans la liste 'roles'
        /// </summary>
        /// <param name="deviseId">deviseId</param>
        /// <param name="roles">roles</param>
        /// <returns>iste SeuilValidationEnt </returns>
        public List<SeuilValidationEnt> Get(int deviseId, List<int> roles)
        {
            return this.Context.SeuilValidations.Where(x => x.DeviseId == deviseId && roles.Contains(x.RoleId.Value)).ToList();
        }
    }
}
