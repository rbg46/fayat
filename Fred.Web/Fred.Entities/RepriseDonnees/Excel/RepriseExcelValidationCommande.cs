using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente un ligne sur un fichier excel d'import pour la validation d'une commande
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} NumeroCommandeExterne = {NumeroCommandeExterne}")]
    public class RepriseExcelValidationCommande
    {

        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// NumeroCommandeExterne
        /// </summary>
        public string NumeroCommandeExterne { get; set; }

    }
}
