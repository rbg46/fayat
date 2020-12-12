using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.ContextProviders
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des Commandes et receptions
    /// </summary>
    public interface ICommandeContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des Commandes et receptions
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelCommandes">repriseExcelCommandes</param>
        /// <returns>les données necessaires a l'import des Commandes et receptions</returns>
        ContextForImportCommande GetContextForImportCommandes(int groupeId, List<RepriseExcelCommande> repriseExcelCommandes);
    }
}
