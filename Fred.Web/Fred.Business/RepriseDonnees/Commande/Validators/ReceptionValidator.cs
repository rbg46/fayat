using Fred.Business.RepriseDonnees.Commande.Validators.Results;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.Validators
{
    /// <summary>
    /// Verifie les rgs des receptions
    /// </summary>
    public class ReceptionValidator
    {
        /// <summary>
        /// A positionner avec la date du champ "Date Réception"
        /// Format non valide = rejet de la ligne  Rejet ligne n°# : Date Réception invalide.
        /// </summary>
        /// <param name="repriseExcelCommande">repriseExcelCommande</param>
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyDateReceptionRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.DateReceptionForReceptionInvalid;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.DateIsValid(repriseExcelCommande.DateReception);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeDateReceptionForReceptionInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

    }
}
