using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Materiel.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des Materiels a partir du fichier excel
    /// </summary>
    public class ParseMaterielResult
    {
        /// <summary>
        /// Liste des Materiels du fichier excel
        /// </summary>
        public List<RepriseExcelMateriel> Materiels { get; set; } = new List<RepriseExcelMateriel>();
    }
}
