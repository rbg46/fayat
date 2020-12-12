using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Referential;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Permet de recuperer un pays en fonction de son code
    /// </summary>
    public class PaysSelectorService : IPaysSelectorService
    {
        /// <summary>
        /// Permet de recuperer un pays en fonction de son code
        /// </summary>
        /// <param name="pays">les pays fred</param>
        /// <param name="codePays">le code du pays</param>
        /// <returns>le pays correspondant au code</returns>
        public PaysEnt GetPaysByCode(List<PaysEnt> pays, string codePays)
        {
            return pays.FirstOrDefault(s => s.Code == codePays);
        }
    }
}
