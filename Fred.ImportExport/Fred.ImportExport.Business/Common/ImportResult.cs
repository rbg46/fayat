using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.ImportExport.Business.Common
{
    [DebuggerDisplay("IsValid = {IsValid} ErrorMessages = {ErrorMessages.Count}")]
    public class ImportResult
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
