using System.Collections.Generic;

namespace Fred.ImportExport.Models.EcritureComptable
{
    /// <summary>
    /// Model de retour pour l'import des écritures comptable vers SAP
    /// </summary>
    public class EcritureComptableFtpSapModel
    {
        /// <summary>
        /// Numéro de la pièce comptable
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Message de l'importation de l'écriture 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Liste des erreurs
        /// </summary>
        public List<string> Erreurs { get; set; }
    }
}
