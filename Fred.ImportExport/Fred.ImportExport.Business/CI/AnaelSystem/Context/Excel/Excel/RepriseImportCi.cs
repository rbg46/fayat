using System.Diagnostics;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Models
{
    /// <summary>
    /// Repersente un ci sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeSociete = {CodeSociete} CodeCi = {CodeCi}")]
    public class RepriseImportCi
    {
        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }
        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSociete { get; set; }
        /// <summary>
        /// Le code du ci
        /// </summary>
        public string CodeCi { get; set; }
    }
}
