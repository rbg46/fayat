using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Personnel.Models;
using Fred.Entities.Personnel;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Personnel.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en Personnels
    /// </summary>
    public interface IPersonnelDataMapper : IService
    {
        /// <summary>
        /// Creer des Personnels à partir d'une liste de RepriseExcelPlanTaches
        /// </summary>
        /// <param name="context">Contexte contenant les data nécessaires à l'import</param>
        /// <param name="listRepriseExcelPersonnel">Personnel sous la forme excel</param>
        /// <returns>Liste de Personnels</returns>
        List<PersonnelEnt> Transform(ContextForImportPersonnel context, List<RepriseExcelPersonnel> listRepriseExcelPersonnel);
    }
}
