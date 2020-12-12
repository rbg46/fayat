using System.Collections.Generic;

namespace Fred.Entities.RepriseDonnees
{
    /// <summary>
    /// Resultat d'un import Rapport
    /// </summary>
    public class ImportRapportResult
    {
        /// <summary>
        /// Permet de savoir si l'import est valide
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Liste des messages d'erreurs
        /// </summary>
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
