using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Business.RepriseDonnees.Rapport.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Rapport.Validators
{
    /// <summary>
    /// Permet de valider les RGs d'import des cis
    /// </summary>
    public interface IRepriseRapportValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles d'import des rapports
        /// </summary>
        /// <param name="repriseExcelRapports">les rapports venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// 
        /// <returns>VerifyImportRulesResult</returns>
        ImportRapportRulesResult VerifyImportRules(List<RepriseExcelRapport> repriseExcelRapports, ContextForImportRapport context);
    }
}
