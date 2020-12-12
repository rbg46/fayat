using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Validators.Results
{
    /// <summary>
    /// Container des resultats des validations
    /// </summary>
    [DebuggerDisplay("AllLignesAreValid = {AllLignesAreValid()} ImportRuleResults = {ImportRuleResults.Count}")]
    public class ImportODRulesResult
    {
        /// <summary>
        /// Liste des resultats des validations
        /// </summary>
        public List<ImportODRuleResult> ImportRuleResults { get; set; } = new List<ImportODRuleResult>();

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
