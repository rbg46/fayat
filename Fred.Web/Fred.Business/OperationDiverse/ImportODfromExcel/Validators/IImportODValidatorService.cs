using System.Collections.Generic;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Business.OperationDiverse.ImportODfromExcel.Validators.Results;
using Fred.Entities.OperationDiverse.Excel;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Validators
{
    /// <summary>
    /// Permet de valider les RGs d'import des operations diverses
    /// </summary>
    public interface IImportODValidatorService : IService
    {
        /// <summary>
        /// Verifie les regles de gestion d'import des operations diverses
        /// </summary>
        /// <param name="excelODs">les operations diverses venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>VerifyImportRapportRulesResult</returns>
        ImportODRulesResult VerifyImportRules(List<ExcelOdModel> excelODs, ContextForImportOD context);
    }
}
