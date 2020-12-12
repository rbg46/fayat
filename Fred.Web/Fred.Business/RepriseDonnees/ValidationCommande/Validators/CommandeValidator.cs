using System.Linq;
using Fred.Business.RepriseDonnees.ValidationCommande.ContextProviders;
using Fred.Business.RepriseDonnees.ValidationCommande.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.ValidationCommande.Validators
{
    /// <summary>
    /// verifie les rg globales
    /// </summary>
    public class CommandeValidator
    {
        /// <summary>        
        /// RG_5634_011 : 2ème outil « Validation en masse des commandes » - Reconnaissance des numéros de commande(= contrôle n°1)
        /// Pour chacun des numéros de commande présent dans le fichier d’entrée :
        /// -   Faire une recherche d’abord sur le champ[NumeroCommandeExterne] de la table[FRED_COMMANDE] pour retrouver la commande à valider.
        /// -   Si non trouvé, faire une recherche sur le champ[Numero] de la table[FRED_COMMANDE].
        /// -   Si toujours aucune correspondance trouvée => rejet de la ligne.
        /// 
        /// Si au moins une commande est non reconnue, interrompre le chargement et retourner la liste complète des rejets trouvés :
        /// « Rejet ligne n°# : commande non reconnue. »
        /// « Rejet ligne n°# : commande non reconnue. »
        /// « Rejet ligne n°# : commande non reconnue. »
        /// </summary>
        /// <param name="repriseExcelValidationCommande">repriseExcelValidationCommande</param>
        /// <param name="context">context de l'import</param>       
        /// <returns>CommandeImportRuleResult</returns>
        public ValidationCommandeRuleResult ReconnaissanceDesNumerosDecommande(RepriseExcelValidationCommande repriseExcelValidationCommande, ContextForValidationCommande context)
        {
            var result = new ValidationCommandeRuleResult();

            result.ImportRuleType = ValidationCommandeRuleType.ReconnaissanceDesNumerosDeCommande;

            result.IsValid = context.CommandesUsedInExcel.Any(x => x.Numero == repriseExcelValidationCommande.NumeroCommandeExterne || x.NumeroCommandeExterne == repriseExcelValidationCommande.NumeroCommandeExterne);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ValidationCommandeReconnaissanceDesNumerosDeCommandeError, repriseExcelValidationCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Pour chaque commande du fichier d’entrée : vérifier que la commande n’a pas déjà été envoyée à SAP, c’est-à-dire que le champ [HangfireJobId] de la table [FRED_COMMANDE] est bien à NULL.
        /// Si le champ[HangfireJobId] de la table[FRED_COMMANDE] est non NULL = rejet de la ligne.
        /// 
        /// Si au moins une commande est déjà envoyée à SAP, interrompre le chargement et retourner la liste complète des rejets trouvés :
        /// « Rejet ligne n°# : commande déjà envoyée au Module Rapprochement. »
        /// « Rejet ligne n°# : commande déjà envoyée au Module Rapprochement. »
        /// « Rejet ligne n°# : commande déjà envoyée au Module Rapprochement. »
        /// </summary>
        /// <param name="repriseExcelValidationCommande">repriseExcelValidationCommande</param>
        /// <param name="context">context de l'import</param>
        /// <returns>ValidationCommandeRuleResult</returns>
        public ValidationCommandeRuleResult VerificationDesCommandesDejaEnvoyeesASap(RepriseExcelValidationCommande repriseExcelValidationCommande, ContextForValidationCommande context)
        {
            var result = new ValidationCommandeRuleResult();

            result.ImportRuleType = ValidationCommandeRuleType.VerificationDesCommandesDejaEnvoyeesASap;

            var commande = context.CommandesUsedInExcel.FirstOrDefault(x => x.Numero == repriseExcelValidationCommande.NumeroCommandeExterne ||
            x.NumeroCommandeExterne == repriseExcelValidationCommande.NumeroCommandeExterne);

            result.IsValid = commande != null && commande.HangfireJobId == null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.VerificationDesCommandesDejaEnvoyeesASapError, repriseExcelValidationCommande.NumeroDeLigne);

            return result;
        }
    }
}
