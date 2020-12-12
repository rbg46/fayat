using System.Diagnostics;

namespace Fred.Business.RepriseDonnees.ValidationCommande.Validators.Results
{
    /// <summary>
    /// Resultat d'une validation de regle
    /// </summary>
    [DebuggerDisplay("IsValid = {IsValid} ImportRuleType = {ImportRuleType.ToString()} ErrorMessage = {ErrorMessage}")]
    public class ValidationCommandeRuleResult
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
        public ValidationCommandeRuleType ImportRuleType { get; set; }

    }
}
