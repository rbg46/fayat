using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Materiel.Models;
using Fred.Business.RepriseDonnees.Materiel.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Materiel.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import des Matériels
    /// </summary>
    public interface IMaterielValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles d'import des Matériels
        /// </summary>
        /// <param name="listRepriseExcelMateriel">Les Matériels venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>MaterielImportRulesResult</returns>
        MaterielImportRulesResult VerifyImportRules(List<RepriseExcelMateriel> listRepriseExcelMateriel, ContextForImportMateriel context);
    }
}
