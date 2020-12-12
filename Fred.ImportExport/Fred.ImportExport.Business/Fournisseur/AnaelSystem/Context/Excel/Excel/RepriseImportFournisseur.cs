using System.Diagnostics;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Models
{
    /// <summary>
    /// Repersente un ci sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeFournisseur = {CodeFournisseur} TypeSequence = {TypeSequence}")]
    public class RepriseImportFournisseur
    {
        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }
        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeFournisseur { get; set; }
        /// <summary>
        /// Le type de sequence
        /// </summary>
        public string TypeSequence { get; set; }
    }
}
