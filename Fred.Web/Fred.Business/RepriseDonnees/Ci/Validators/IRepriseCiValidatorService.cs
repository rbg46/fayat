using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Business.RepriseDonnees.Ci.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.Validators
{
    /// <summary>
    /// Permet de valider les RGs d'import des cis
    /// </summary>
    public interface IRepriseCiValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles d'import des Ci
        /// </summary>
        /// <param name="repriseExcelCis">les ci venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// 
        /// <returns>VerifyImportRulesResult</returns>
        ImportRulesResult VerifyImportRules(List<RepriseExcelCi> repriseExcelCis, ContextForImportCi context);
    }
}
