using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Personnel.Models;
using Fred.Business.RepriseDonnees.Personnel.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Personnel.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import des Personnels
    /// </summary>
    public interface IPersonnelValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles d'import des Personnels
        /// </summary>
        /// <param name="listRepriseExcelPersonnel">les Personnels venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>PersonnelImportRulesResult</returns>
        PersonnelImportRulesResult VerifyImportRules(List<RepriseExcelPersonnel> listRepriseExcelPersonnel, ContextForImportPersonnel context);
    }
}
