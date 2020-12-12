using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Materiel.Models;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Materiel.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en Materiels
    /// </summary>
    public interface IMaterielDataMapper : IService
    {
        /// <summary>
        /// Creer des Materiels à partir d'une liste de RepriseExcelMateriel
        /// </summary>
        /// <param name="context">Contexte contenant les data nécessaires à l'import</param>
        /// <param name="listRepriseExcelMateriel">Materiel sous la forme excel</param>
        /// <returns>Liste de Materiels</returns>
        List<MaterielEnt> Transform(ContextForImportMateriel context, List<RepriseExcelMateriel> listRepriseExcelMateriel);
    }
}
