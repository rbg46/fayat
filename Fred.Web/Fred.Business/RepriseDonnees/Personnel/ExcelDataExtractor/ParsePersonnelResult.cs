using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Personnel.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des Personnels a partir du fichier excel
    /// </summary>
    public class ParsePersonnelResult
    {
        /// <summary>
        /// Liste des Personnels du fichier excel
        /// </summary>
        public List<RepriseExcelPersonnel> Personnels { get; set; } = new List<RepriseExcelPersonnel>();
    }
}
