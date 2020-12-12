using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.ValidationCommande.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des validation de commandes a partir du fichier excel
    /// </summary>
    public class ParseValidationCommandesResult
    {
        /// <summary>
        /// Liste des validation de commandes du fichier excel
        /// </summary>
        public List<RepriseExcelValidationCommande> Commandes { get; set; } = new List<RepriseExcelValidationCommande>();

    }
}
