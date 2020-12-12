using System.Collections.Generic;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Entities.OperationDiverse;
using Fred.Entities.OperationDiverse.Excel;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Mapper
{
    /// <summary>
    /// permet de transformer un model excel vers une operation diverse entite
    /// </summary>
    public interface IOperationDiverseDataMapper : IService
    {
        /// <summary>
        /// returne la liste des rapports à créer 
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="excelOperationDiverses">les operations diverses sous la forme excel</param>
        /// <returns>Liste des operations diverses à créer</returns>
        List<OperationDiverseEnt> Transform(ContextForImportOD context, List<ExcelOdModel> excelOperationDiverses);
    }
}
