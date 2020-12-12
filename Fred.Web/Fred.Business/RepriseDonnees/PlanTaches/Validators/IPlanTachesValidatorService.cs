using System.Collections.Generic;
using Fred.Business.RepriseDonnees.PlanTaches.Models;
using Fred.Business.RepriseDonnees.PlanTaches.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.PlanTaches.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import d'un plan de taches
    /// </summary>
    public interface IPlanTachesValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles d'import d'un plan de taches
        /// </summary>
        /// <param name="listRepriseExcelPlanTaches">les taches venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>PlanTachesImportRulesResult</returns>
        PlanTachesImportRulesResult VerifyImportRules(List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches, ContextForImportPlanTaches context);
    }
}
