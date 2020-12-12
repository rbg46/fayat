using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.RepriseDonnees.Commande.Selector;
using Fred.Business.RepriseDonnees.Commande.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.Validators
{
    /// <summary>
    /// Permet de valider les RGs d'import des cis
    /// </summary>
    public class CommandeValidatorService : ICommandeValidatorService
    {
        /// <summary>
        /// Verifie les regles d'import des commandes
        /// </summary>
        /// <param name="repriseExcelCommandes">les cis venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>   
        /// <returns>VerifyImportRulesResult</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
        public CommandeImportRulesResult VerifyImportRules(List<RepriseExcelCommande> repriseExcelCommandes, ContextForImportCommande context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            var result = new CommandeImportRulesResult();
            var validatorGeneral = new CommandeGeneralValidator();

            var commmandesGroupedByNumeroExterne = repriseExcelCommandes.GroupBy(x => x.NumeroCommandeExterne);

            // verification generale coherence / unicite / pattern numerocommande externe
            foreach (var groupedCommande in commmandesGroupedByNumeroExterne)
            {
                CommandeImportRuleResult verifyCoherenceInformationEnteteRule = validatorGeneral.VerifyCoherenceInformationEnteteRule(groupedCommande.Key, groupedCommande.ToList());
                CommandeImportRuleResult verifyUniciteNumeroExterneRule = validatorGeneral.VerifyUniciteNumeroExterneRule(groupedCommande.Key, context);
                CommandeImportRuleResult verifyPatternNumeroCommandeExterneRule = validatorGeneral.VerifyPatternNumeroCommandeExterneRule(groupedCommande.Key);
                result.ImportRuleResults.Add(verifyCoherenceInformationEnteteRule);
                result.ImportRuleResults.Add(verifyUniciteNumeroExterneRule);
                result.ImportRuleResults.Add(verifyPatternNumeroCommandeExterneRule);
            }


            var commadeValidator = new CommandeValidator();
            // RG POUR LES COMMANDES
            foreach (var repriseExcelCommande in repriseExcelCommandes)
            {
                CommandeImportRuleResult requiredFiledsResult = commadeValidator.VerifyRequiredFieldsRule(repriseExcelCommande);
                CommandeImportRuleResult typeIdResult = commadeValidator.VerifyTypeIdRule(repriseExcelCommande, context);
                CommandeImportRuleResult codeFournisseur = commadeValidator.VerifyCodeFournisseurRule(repriseExcelCommande, context);
                CommandeImportRuleResult codeSocieteResult = commadeValidator.VerifyCodeSocieteRule(repriseExcelCommande, context);
                CommandeImportRuleResult codeCiResult = commadeValidator.VerifyCodeCiRule(context.GroupeId, repriseExcelCommande, context.OrganisationTree);
                CommandeImportRuleResult dateCommandeResult = commadeValidator.VerifyFormatDateCommandeRule(repriseExcelCommande);
                CommandeImportRuleResult deviseResult = commadeValidator.VerifyCommandeDeviseRule(repriseExcelCommande, context);

                result.ImportRuleResults.Add(requiredFiledsResult);
                result.ImportRuleResults.Add(typeIdResult);
                result.ImportRuleResults.Add(codeFournisseur);
                result.ImportRuleResults.Add(codeSocieteResult);
                result.ImportRuleResults.Add(codeCiResult);
                result.ImportRuleResults.Add(dateCommandeResult);
                result.ImportRuleResults.Add(deviseResult);

            }

            var commandeLignesValidator = new CommandeLignesValidator();

            // RG POUR LES LIGNES DE COMMANDES
            foreach (var repriseExcelCommande in repriseExcelCommandes)
            {
                CommandeImportRuleResult verifyCodeTacheForCommandeLigne = commandeLignesValidator.VerifyCodeTacheForCommandeLigneRule(repriseExcelCommande, context);
                CommandeImportRuleResult verifyRessourceRule = commandeLignesValidator.VerifyRessourceRule(repriseExcelCommande, context);
                CommandeImportRuleResult verifyQuantiteCommandeeRule = commandeLignesValidator.VerifyQuantiteCommandeeFormatRule(repriseExcelCommande);
                CommandeImportRuleResult verifyQuantiteFactureeRapprocheeRule = commandeLignesValidator.VerifyQuantiteFactureeRapprocheeFormatRule(repriseExcelCommande);
                CommandeImportRuleResult verifyQuantiteReceptionneeRapprocheeRule = commandeLignesValidator.VerifyQuantiteReceptionneeFormatRule(repriseExcelCommande);
                CommandeImportRuleResult verifyQuantiteRule = commandeLignesValidator.VerifyQuantiteRule(repriseExcelCommande);
                CommandeImportRuleResult verifyPuhtFormatRule = commandeLignesValidator.VerifyPuhtFormatRule(repriseExcelCommande);
                CommandeImportRuleResult verifyUniteRule = commandeLignesValidator.VerifyUniteRule(repriseExcelCommande, context);
                result.ImportRuleResults.Add(verifyCodeTacheForCommandeLigne);
                result.ImportRuleResults.Add(verifyRessourceRule);
                result.ImportRuleResults.Add(verifyQuantiteCommandeeRule);
                result.ImportRuleResults.Add(verifyQuantiteFactureeRapprocheeRule);
                result.ImportRuleResults.Add(verifyQuantiteReceptionneeRapprocheeRule);
                result.ImportRuleResults.Add(verifyQuantiteRule);
                result.ImportRuleResults.Add(verifyPuhtFormatRule);
                result.ImportRuleResults.Add(verifyUniteRule);
            }

            var quantiteSelector = new QuantiteSelector();
            var receptionsValidator = new ReceptionValidator();
            // RG POUR LES Receptions
            foreach (var repriseExcelCommande in repriseExcelCommandes)
            {
                var canCreateReception = quantiteSelector.CanCreateReception(repriseExcelCommande);
                if (canCreateReception)
                {

                    CommandeImportRuleResult verifyDateReceptionRule = receptionsValidator.VerifyDateReceptionRule(repriseExcelCommande);
                    result.ImportRuleResults.Add(verifyDateReceptionRule);
                }
            }

            return result;
        }


    }
}
