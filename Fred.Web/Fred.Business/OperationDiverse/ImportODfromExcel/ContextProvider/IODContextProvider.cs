using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.OperationDiverse.Excel;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des Opérations Diverses
    /// </summary>
    public interface IODContextProvider: IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des rapports
        /// </summary>
        /// <param name="dateComptable">date comptable recupéré de l'écran Rapprochement Comta/Gestion</param>
        /// <param name="excelODs">ExcelOdModel</param>
        /// <returns>les données necessaires a l'import des operations diverses</returns>
        ContextForImportOD GetContextForImportOD(DateTime dateComptable, List<ExcelOdModel> excelODs);
    }
}
