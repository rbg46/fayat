using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Business.SeuilValidation.Manager
{
    /// <summary>
    /// Manager des SeuilValidationEnt
    /// </summary>
    public interface ISeuilValidationManager : IManager<SeuilValidationEnt>
    {
        /// <summary>
        /// Retourne la liste SeuilValidationEnt eaynt la devise 'deviseId' et qui dont le role est contenue dans la liste 'roles'
        /// </summary>
        /// <param name="deviseId">deviseId</param>
        /// <param name="roles">roles</param>
        /// <returns>iste SeuilValidationEnt </returns>
        List<SeuilValidationEnt> Get(int deviseId, List<int> roles);
    }
}
