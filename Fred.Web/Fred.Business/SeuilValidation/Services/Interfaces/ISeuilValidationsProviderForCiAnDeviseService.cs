using System.Collections.Generic;
using Fred.Business.SeuilValidation.Models;

namespace Fred.Business.SeuilValidation.Services.Interfaces
{
    /// <summary>
    /// Service qui Recupere toutes les seuils de validation pour un ci et une devise donnée
    /// </summary>
    public interface ISeuilValidationsProviderForCiAnDeviseService : IService
    {
        /// <summary>
        /// Recupere toutes les seuil de validation pour un ci et une devise donnée
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="deviseId">deviseId</param>
        /// <returns>liste de SeuilValidationForUserResult </returns>
        List<SeuilValidationForUserResult> GetUsersWithSeuilValidationsOnCi(int ciId, int deviseId);
    }
}
