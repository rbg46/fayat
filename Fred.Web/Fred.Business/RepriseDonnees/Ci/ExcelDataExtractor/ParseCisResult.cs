using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des ci a partir du fichier excel
    /// </summary>
    public class ParseCisResult
    {
        /// <summary>
        /// Liste des ci du fichier excel
        /// </summary>
        public List<RepriseExcelCi> Cis { get; set; } = new List<RepriseExcelCi>();

    }
}
