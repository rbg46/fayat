using System.Collections.Generic;
using Fred.ImportExport.Business.CI.AnaelSystem.Models;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Excel
{
    /// <summary>
    /// Resultat du parsage du fichier excel
    /// </summary>
    public class ParseImportCisResult
    {
        /// <summary>
        /// Liste des ci du fichier excel
        /// </summary>
        public List<RepriseImportCi> Cis { get; set; } = new List<RepriseImportCi>();
    }
}
