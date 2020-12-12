using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fred.Business.RepriseDonnees.Rapport.Validators.Results
{
    /// <summary>
    /// Container des resultats des validations
    /// </summary>
    [DebuggerDisplay("AllLignesAreValid = {AllLignesAreValid()} ImportRuleResults = {ImportRuleResults.Count}")]
    public class ImportRapportRulesResult
    {
        /// <summary>
        /// Liste des resultats des validations
        /// </summary>
        public List<ImportRapportRuleResult> ImportRuleResults { get; set; } = new List<ImportRapportRuleResult>();


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
