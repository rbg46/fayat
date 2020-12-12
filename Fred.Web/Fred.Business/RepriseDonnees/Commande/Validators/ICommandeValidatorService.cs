using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.RepriseDonnees.Commande.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.Validators
{
    /// <summary>
    /// Permet de valider les RGs d'import des cis
    /// </summary>
    public interface ICommandeValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles d'import des Commandes
        /// </summary>
        /// <param name="repriseExcelCommandes">les commandes venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// 
        /// <returns>VerifyImportRulesResult</returns>
        CommandeImportRulesResult VerifyImportRules(List<RepriseExcelCommande> repriseExcelCommandes, ContextForImportCommande context);
    }
}
