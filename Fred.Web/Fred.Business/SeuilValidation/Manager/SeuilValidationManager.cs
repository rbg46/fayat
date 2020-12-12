using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;

namespace Fred.Business.SeuilValidation.Manager
{
    /// <summary>
    /// Manager des SeuilValidationEnt
    /// </summary>
    public class SeuilValidationManager : Manager<SeuilValidationEnt, ISeuilValidationRepository>, ISeuilValidationManager
    {
        public SeuilValidationManager(IUnitOfWork uow, ISeuilValidationRepository seuilValidationRepository)
            : base(uow, seuilValidationRepository) { }

        /// <summary>
        /// Retourne la liste SeuilValidationEnt eaynt la devise 'deviseId' et qui dont le role est contenue dans la liste 'roles'
        /// </summary>
        /// <param name="deviseId">deviseId</param>
        /// <param name="roles">roles</param>
        /// <returns>iste SeuilValidationEnt </returns>
        public List<SeuilValidationEnt> Get(int deviseId, List<int> roles)
        {
            return this.Repository.Get(deviseId, roles);
        }
    }
}
