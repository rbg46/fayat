using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Entities.Rapport;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Rapport.Mapper
{
    /// <summary>
    /// Mappe les données du ci excel vers le ciEnt
    /// </summary>
    public interface IRapportDataMapper : IService
    {
        /// <summary>
        /// returne la liste des rapports à créer 
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="repriseExcelRapports">les rapports sous la forme excel</param>
        /// <returns>Liste des rapports à créer</returns>
        RapportTransformResult Transform(ContextForImportRapport context, List<RepriseExcelRapport> repriseExcelRapports);
    }
}
