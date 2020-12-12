using System.Collections.Generic;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Excel
{
    /// <summary>
    /// Resultat du parsage du fichier excel
    /// </summary>
    public class ParseImportFournisseursResult
    {
        /// <summary>
        /// Liste des ci du fichier excel
        /// </summary>
        public List<RepriseImportFournisseur> Fournisseurs { get; set; } = new List<RepriseImportFournisseur>();
    }
}
