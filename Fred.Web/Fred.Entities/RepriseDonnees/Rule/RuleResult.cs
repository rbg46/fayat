using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Rule
{
    /// <summary>
    /// Resultat d'une regle d'import
    /// </summary>
    [DebuggerDisplay("IsValid = {IsValid} ErrorMessage = {ErrorMessage}")]
    public class RuleResult
    {
        /// <summary>
        /// Permet de savoir si l'import est valide
        /// </summary>
        public bool IsValid { get; set; }


        /// <summary>
        /// Messages d'erreur
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
