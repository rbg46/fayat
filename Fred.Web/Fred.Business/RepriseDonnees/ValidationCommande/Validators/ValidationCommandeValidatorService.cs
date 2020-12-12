using System.Collections.Generic;
using Fred.Business.RepriseDonnees.ValidationCommande.ContextProviders;
using Fred.Business.RepriseDonnees.ValidationCommande.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.ValidationCommande.Validators
{
    /// <summary>
    /// Permet de valider les RGs la validation en masse des commandes
    /// </summary>
    public class ValidationCommandeValidatorService : IValidationCommandeValidatorService
    {
        /// <summary>
        /// Verifie les regles de la validation en masse des commandes
        /// </summary>
        /// <param name="repriseExcelCommandes">les commandes venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>   
        /// <returns>VerifyImportRulesResult</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
        public ValidationCommandeRulesResult VerifyImportRules(List<RepriseExcelValidationCommande> repriseExcelCommandes, ContextForValidationCommande context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            var result = new ValidationCommandeRulesResult();

            var validator = new CommandeValidator();

            foreach (var repriseExcelCommande in repriseExcelCommandes)
            {
                ValidationCommandeRuleResult reconnaissanceDesNumerosDecommandeResult = validator.ReconnaissanceDesNumerosDecommande(repriseExcelCommande, context);
                ValidationCommandeRuleResult verificationDesCommandesDejaEnvoyeesASapResult = validator.VerificationDesCommandesDejaEnvoyeesASap(repriseExcelCommande, context);

                result.ImportRuleResults.Add(reconnaissanceDesNumerosDecommandeResult);
                result.ImportRuleResults.Add(verificationDesCommandesDejaEnvoyeesASapResult);
            }

            return result;
        }


    }
}
