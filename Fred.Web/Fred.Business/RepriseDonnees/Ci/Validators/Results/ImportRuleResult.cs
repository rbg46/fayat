using System.Diagnostics;

namespace Fred.Business.RepriseDonnees.Ci.Validators.Results
{
    /// <summary>
    /// Resultat d'une validation de regle
    /// </summary>
    [DebuggerDisplay("ImportRuleType = {ImportRuleType.ToString()} IsValid = {IsValid} ErrorMessage = {ErrorMessage}")]
    public class ImportRuleResult
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
        public ImportRuleType ImportRuleType { get; set; }
    }
}
