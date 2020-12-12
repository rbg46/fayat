using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.RepriseDonnees.Commande.Selector;
using Fred.Business.RepriseDonnees.Commande.Validators.Results;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.Validators
{
    /// <summary>
    /// Verifie les rgs des commandes lignes
    /// </summary>
    public class CommandeLignesValidator
    {
        /// <summary>
        /// Si la valeur "Code Tâche" est non vide : rechercher cette valeur dans la table FRED_TACHE, parmi les tâches de niveau 3 associées au CI identifié pour la commande.
        /// Si non reconnu = rejet de la ligne.
        /// Si la valeur "Code Tâche" est vide, rechercher la tâche par défaut du CI identifié pour la commande.
        /// Si non trouvé => rejet de la ligne.
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <param name="context">contient toutes les données pour faire la validation</param>
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyCodeTacheForCommandeLigneRule(RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.CodeTacheForCommandeLigneInvalid;

            var selector = new CommonFieldSelector();

            var ci = context.OrganisationTree.GetCi(context.GroupeId, repriseExcelCommande.CodeSociete, repriseExcelCommande.CodeCi);

            var tache = selector.GetTache(ci?.Id, repriseExcelCommande.CodeTache.ToLower(), context.TachesUsedInExcel);

            result.IsValid = tache != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeCodeTacheForCommandeLigneInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Ressource" dans la table FRED_RESSOURCE, parmi les ressources associées au Groupe choisi par l'utilisateur.
        /// Si non reconnu = rejet de la ligne.Rejet ligne n°# : Code Ressource invalide.
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <param name="context">contient toutes les données pour faire la validation</param>
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyRessourceRule(RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.RessourceForCommandeLigneInvalid;

            result.IsValid = context.RessourcesUsedInExcel.Any(x => string.Equals(x.Code, repriseExcelCommande.CodeRessource, System.StringComparison.OrdinalIgnoreCase));

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRessourceForCommandeLigneInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Positionner à la valeur "Qté commandée" – "Qté facturée rapprochée" si "Qté commandée" – "Qté facturée rapprochée" > 0
        /// Sinon, positionner à la valeur "Qté réceptionnée" – "Qté facturée rapprochée" si "Qté réceptionnée" – "Qté facturée rapprochée" > 0
        /// Sinon rejet de la ligne.Rejet ligne n°# : Qté commandée invalide
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>    
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyQuantiteReceptionneeFormatRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.QuantiteReceptionneeForCommandeLigneInvalid;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.DecimalIsValid(repriseExcelCommande.QuantiteReceptionnee);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeQuantiteReceptionneeForCommandeLigneFormatInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Positionner à la valeur "Qté commandée" – "Qté facturée rapprochée" si "Qté commandée" – "Qté facturée rapprochée" > 0
        /// Sinon, positionner à la valeur "Qté réceptionnée" – "Qté facturée rapprochée" si "Qté réceptionnée" – "Qté facturée rapprochée" > 0
        /// Sinon rejet de la ligne.Rejet ligne n°# : Qté commandée invalide
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>       
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyQuantiteFactureeRapprocheeFormatRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.QuantiteFactureeRapprocheeForCommandeLigneInvalid;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.DecimalIsValid(repriseExcelCommande.QuantiteFactureeRapprochee);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeQuantiteFactureeRapprocheeForCommandeLigneFormatInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Positionner à la valeur "Qté commandée" – "Qté facturée rapprochée" si "Qté commandée" – "Qté facturée rapprochée" > 0
        /// Sinon, positionner à la valeur "Qté réceptionnée" – "Qté facturée rapprochée" si "Qté réceptionnée" – "Qté facturée rapprochée" > 0
        /// Sinon rejet de la ligne.Rejet ligne n°# : Qté commandée invalide 
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>      
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyQuantiteCommandeeFormatRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.QuantiteCommandeeForCommandeLigneInvalid;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.DecimalIsValid(repriseExcelCommande.QuantiteCommandee);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeQuantiteCommandeeForCommandeLigneFormatInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Positionner à la valeur "Qté commandée" – "Qté facturée rapprochée" si "Qté commandée" – "Qté facturée rapprochée" > 0
        /// Sinon, positionner à la valeur "Qté réceptionnée" – "Qté facturée rapprochée" si "Qté réceptionnée" – "Qté facturée rapprochée" > 0
        /// Sinon rejet de la ligne.Rejet ligne n°# : Qté commandée invalide 
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>       
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyQuantiteRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.QuantiteForCommandeLigneInvalid;

            var validator = new CommonFieldsValidator();

            var quantiteCommandeeIsValid = validator.DecimalIsValid(repriseExcelCommande.QuantiteCommandee);
            var quantiteFactureeRapprocheeIsValid = validator.DecimalIsValid(repriseExcelCommande.QuantiteFactureeRapprochee);
            var quantiteReceptionneeeIsValid = validator.DecimalIsValid(repriseExcelCommande.QuantiteReceptionnee);

            if (!quantiteCommandeeIsValid || !quantiteFactureeRapprocheeIsValid || !quantiteReceptionneeeIsValid)
            {
                result.IsValid = false;
                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeQuantiteAnyFormatErrorForCommandeLigneInvalid, repriseExcelCommande.NumeroDeLigne);
                return result;
            }

            var selector = new QuantiteSelector();

            result.IsValid = selector.GetQuantiteCommandeLigne(repriseExcelCommande) != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeQuantiteForCommandeLigneInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }



        /// <summary>
        /// Positionner à la valeur du champ "PU HT"
        /// Si format non valide => rejet de la ligne.Rejet ligne n°# : PU HT invalide.
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>    
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyPuhtFormatRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.PuhtForCommandeLigneInvalid;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.DecimalIsValid(repriseExcelCommande.PuHt);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandePuhtForCommandeLigneInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }


        /// <summary>
        /// Rechercher la valeur "Code Unité" dans la table FRED_UNITE.
        /// Si non reconnu = rejet de la ligne.Rejet ligne n°# : Code Unité invalide.
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <param name="context">contient toutes les données pour faire la validation</param>
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyUniteRule(RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.UniteForCommandeLigneInvalid;

            result.IsValid = context.UnitesUsedInExcel.Any(x => string.Equals(x.Code, repriseExcelCommande.Unite, System.StringComparison.OrdinalIgnoreCase));

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeUniteForCommandeLigneInvalid, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }
    }
}
