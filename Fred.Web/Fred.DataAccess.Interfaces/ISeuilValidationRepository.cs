using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repo des SeuilValidationEnt
    /// </summary>
    public interface ISeuilValidationRepository : IFredRepository<SeuilValidationEnt>
    {

        /// <summary>
        /// Retourne la liste SeuilValidationEnt eyant la devise 'deviseId' et qui dont le role est contenue dans la liste 'roles'
        /// </summary>
        /// <param name="deviseId">deviseId</param>
        /// <param name="roles">roles</param>
        /// <returns>iste SeuilValidationEnt </returns>
        List<SeuilValidationEnt> Get(int deviseId, List<int> roles);
    }
}
