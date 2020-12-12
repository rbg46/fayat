using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.ExcelDataExtractor
{
    /// <summary>
    /// Resultat du parsage des ci a partir du fichier excel
    /// </summary>
    public class ParseCommandesResult
    {
        /// <summary>
        /// Liste des ci du fichier excel
        /// </summary>
        public List<RepriseExcelCommande> Commandes { get; set; } = new List<RepriseExcelCommande>();

    }
}
