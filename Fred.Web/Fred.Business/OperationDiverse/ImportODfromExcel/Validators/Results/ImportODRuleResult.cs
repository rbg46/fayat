using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Validators.Results
{
    /// <summary>
    /// Resultat d'une validation de regle
    /// </summary>
    [DebuggerDisplay("ImportRapportRuleType = {ImportRuleType.ToString()} IsValid = {IsValid} ErrorMessage = {ErrorMessage}")]
    public class ImportODRuleResult
    {
        /// <summary>
        /// Permet de savoir si la regle est respecter
        /// </summary>
        public bool IsValid { get; internal set; }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Type de regle verifiée
        /// </summary>
        public ImportODRuleType ImportRuleType { get; set; }
    }
}
