using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des Indemnités de Déplacement a partir du fichier excel
    /// </summary>
    public class ParseIndemniteDeplacementResult
    {
        /// <summary>
        /// Liste des Indemnités de Déplacement du fichier excel
        /// </summary>
        public List<RepriseExcelIndemniteDeplacement> IndemniteDeplacements { get; set; } = new List<RepriseExcelIndemniteDeplacement>();
    }
}
