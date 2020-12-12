using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fred.Business.RepriseDonnees.ValidationCommande.Validators.Results
{
    /// <summary>
    /// Container des resultats des validations
    /// </summary>
    [DebuggerDisplay("AllLignesAreValid = {AllLignesAreValid()} ImportRuleResults = {ImportRuleResults.Count}")]
    public class ValidationCommandeRulesResult
    {
        /// <summary>
        /// Liste des resultats des validations
        /// </summary>
        public List<ValidationCommandeRuleResult> ImportRuleResults { get; set; } = new List<ValidationCommandeRuleResult>();

        /// <summary>
        /// Permet de savoir si toutes les rgs de toutes les lignes sont respectées
        /// </summary>
        /// <returns>true si toutes les rgs sont valide</returns>
        public bool AllLignesAreValid()
        {
            return this.ImportRuleResults.All(x => x.IsValid);
        }
    }
}
