using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Rapport.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des rapports a partir du fichier excel
    /// </summary>
    public class ParseRapportsResult
    {
        /// <summary>
        /// Liste des rapports du fichier excel
        /// </summary>
        public List<RepriseExcelRapport> Rapports{ get; set; } = new List<RepriseExcelRapport>();

    }
}
