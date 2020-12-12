using System.Collections.Generic;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Models;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import des Indemnité de Déplacement
    /// </summary>
    public interface IIndemniteDeplacementValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles d'import des Indemnités de Déplacement
        /// </summary>
        /// <param name="listIndemniteDeplacement">Les Indemnités de Déplacement venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>PersonnelImportRulesResult</returns>
        IndemniteDeplacementImportRulesResult VerifyImportRules(List<RepriseExcelIndemniteDeplacement> listIndemniteDeplacement, ContextForImportIndemniteDeplacement context);
    }
}
