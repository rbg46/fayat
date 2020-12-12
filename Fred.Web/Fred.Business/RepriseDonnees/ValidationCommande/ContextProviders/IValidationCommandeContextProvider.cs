using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.ValidationCommande.ContextProviders
{
    /// <summary>
    /// Service qui fournit les données necessaires a la validation des Commandes 
    /// </summary>
    public interface IValidationCommandeContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires la validation en masse des commandes
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelValidationCommandes">repriseExcelValidationCommandes</param>
        /// <returns>les données necessaires la validation en masse des commandes</returns>
        ContextForValidationCommande GetContextForValidationCommandes(int groupeId, List<RepriseExcelValidationCommande> repriseExcelValidationCommandes);
    }
}
