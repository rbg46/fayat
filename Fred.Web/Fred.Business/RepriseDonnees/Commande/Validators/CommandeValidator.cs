using System.Linq;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.RepriseDonnees.Commande.Selector;
using Fred.Business.RepriseDonnees.Commande.Validators.Results;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Commande.Validators
{
    /// <summary>
    /// Verifie les rgs des commandes
    /// </summary>
    public class CommandeValidator
    {
        /// <summary>
        /// Verification de la RG RG_5634_005
        /// <para>
        /// RG_5634_005 : 1er outil « Chargement des commandes et réceptions » - Vérification des champs obligatoires (= contrôle n°3)
        ///        Les informations suivantes sont obligatoires :
        /// •    Code Société
        /// •    Code CI
        /// •    Code Fournisseur
        /// •    N° Commande Externe
        /// •    Type Commande
        /// •    Libellé En-tête Commande
        /// •    Code Devise
        /// •    Date Commande
        /// •    Désignation ligne Commande
        /// •    Code Ressource
        /// •    Unité
        /// •    PU
        /// •    Qté commandée
        /// •    Date Réception
        /// </para>
        /// <para>
        /// => Pour chaque ligne du fichier pour lesquelles au moins une valeur est remplie, vérifier que tous ces champs sont non vides.
        /// </para>
        /// <para>
        /// Si un ou plusieurs ligne(s) est(sont) en anomalie, remonter la liste de ces lignes en erreur, avec les messages suivants :
        /// « Rejet ligne n°# : champ(s) obligatoire(s) non renseigné(s). »
        /// </para>
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyRequiredFieldsRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.RequiredField;

            var codeSocieteIsValid = !repriseExcelCommande.CodeSociete.IsNullOrEmpty();
            var codeCiIsValid = !repriseExcelCommande.CodeCi.IsNullOrEmpty();
            var codeFournisseurIsValid = !repriseExcelCommande.CodeFournisseur.IsNullOrEmpty();
            var numeroCommandeExterneIsValid = !repriseExcelCommande.NumeroCommandeExterne.IsNullOrEmpty();
            var typeCommandeIsValid = !repriseExcelCommande.TypeCommande.IsNullOrEmpty();
            var libelleEnteteCommandeIsValid = !repriseExcelCommande.LibelleEnteteCommande.IsNullOrEmpty();
            var codeDeviseIsValid = !repriseExcelCommande.CodeDevise.IsNullOrEmpty();
            var dateCommandeIsValid = !repriseExcelCommande.DateCommande.IsNullOrEmpty();
            var designationLigneCommandeIsValid = !repriseExcelCommande.DesignationLigneCommande.IsNullOrEmpty();
            var codeRessourceIsValid = !repriseExcelCommande.CodeRessource.IsNullOrEmpty();
            var uniteIsValid = !repriseExcelCommande.Unite.IsNullOrEmpty();
            var puHtIsValid = !repriseExcelCommande.PuHt.IsNullOrEmpty();
            var quantiteCommandeeIsValid = !repriseExcelCommande.QuantiteCommandee.IsNullOrEmpty();
            var dateReceptionIsValid = !repriseExcelCommande.DateReception.IsNullOrEmpty();

            result.IsValid = codeSocieteIsValid &&
                codeCiIsValid &&
                codeFournisseurIsValid &&
                numeroCommandeExterneIsValid &&
                typeCommandeIsValid &&
                libelleEnteteCommandeIsValid &&
                codeDeviseIsValid &&
                dateCommandeIsValid &&
                designationLigneCommandeIsValid &&
                codeRessourceIsValid &&
                uniteIsValid &&
                puHtIsValid &&
                quantiteCommandeeIsValid &&
                dateReceptionIsValid;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRequiredFieldError, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Transco à appliquer sur la valeur "Type Commande" :
        /// - "Fourniture" => 1
        /// - "Location" => 2
        /// - "Prestation" => 3
        /// - "Intérimaire" => 4
        /// Non reconnu => rejet de la ligne Rejet ligne n°# : Type Commande invalide.
        /// 
        /// </summary>
        /// <param name="repriseExcelCommande">repriseExcelCommande</param>
        /// <param name="context">context</param>
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyTypeIdRule(RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.TypeCommandeInvalide;

            var selector = new TypeIdSelectorHelper();

            result.IsValid = selector.GetTypeId(repriseExcelCommande.TypeCommande, context) != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeTypeCommandeInvalideError, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Fournisseur" dans la table FRED_FOURNISSEUR
        /// Non reconnu => rejet de la ligne Rejet ligne n°# : Code Fournisseur invalide.
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <param name="context">contient toutes les données pour faire la validation</param>
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyCodeFournisseurRule(RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.CodeFournisseurInvalide;

            result.IsValid = context.FournisseurUsedInExcel.Any(f => f.Code == repriseExcelCommande.CodeFournisseur);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeCodeFournisseurInvalideError, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher d'abord la valeur "Code Société" dans la table FRED_SOCIETE pour le Groupe spécifié par l'utilisateur pour trouver le SocieteId,
        /// puis rechercher ensuite la valeur "Code CI" dans la table FRED_CI pour le SocieteId trouvé précédemment.
        /// Non reconnu = rejet de la ligne Rejet ligne n°# : Code CI invalide.
        /// </summary>
        /// <param name="groupeId">groupeId</param>  
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <param name="organisationTree">organisationTree</param>      
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyCodeCiRule(int groupeId, RepriseExcelCommande repriseExcelCommande, OrganisationTree organisationTree)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.CiNotInSociete;

            var validator = new CommonFieldsValidator();

            var messageErreur = string.Format(BusinessResources.ImportCIErrorMessageCodeCiInvalide, repriseExcelCommande.NumeroDeLigne);

            result.IsValid = validator.CodeCiIsValid(groupeId, repriseExcelCommande.CodeSociete, repriseExcelCommande.CodeCi, organisationTree);

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher d'abord la valeur "Code Société" dans la table FRED_SOCIETE pour le Groupe spécifié par l'utilisateur pour trouver le SocieteId,
        /// puis rechercher ensuite la valeur "Code CI" dans la table FRED_CI pour le SocieteId trouvé précédemment.
        /// Non reconnu = rejet de la ligne ReRejet ligne n°{0} : Code Société invalide
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <param name="context">contient toutes les données pour faire la validation</param>
        /// <returns>Le resultat de la validation</returns>       
        public CommandeImportRuleResult VerifyCodeSocieteRule(RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.SocieteNotInGroupe;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.CodeSocieteIsValid(context.GroupeId, repriseExcelCommande.CodeSociete, context.OrganisationTree);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCodeSocieteInvalide, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// A positionner avec la date du champ "Date Commande"
        /// Format non valide = rejet de la ligne  Rejet ligne n°# : Date Commande invalide.
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>       
        /// <returns>Le resultat de la validation</returns>
        public CommandeImportRuleResult VerifyFormatDateCommandeRule(RepriseExcelCommande repriseExcelCommande)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.DateCommandeInvalid;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.DateIsValid(repriseExcelCommande.DateCommande);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageDateCommandeIncorrect, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Devise" dans la table FRED_DEVISE
        /// Non reconnu = rejet de la ligne Rejet ligne n°# : Code Devise invalide.
        /// </summary>
        /// <param name="repriseExcelCommande">ligne excel</param>
        /// <param name="context">contient toutes les données pour faire la validation</param>
        /// <returns>Le resultat de la validation</returns>    
        public CommandeImportRuleResult VerifyCommandeDeviseRule(RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.DeviseInvalide;

            result.IsValid = context.DevisesUsedInExcel.Any(x => x.IsoCode == repriseExcelCommande.CodeDevise);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeDeviseInvalideError, repriseExcelCommande.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelCommande.NumeroDeLigne);

            return result;
        }
    }
}
