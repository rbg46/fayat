using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Models;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import des Indemnités de Déplacement
    /// </summary>
    public class IndemniteDeplacementValidatorService : IIndemniteDeplacementValidatorService
    {
        /// <summary>
        /// Verifie les regles d'import des Indemnités de Déplacement
        /// </summary>
        /// <param name="listIndemniteDeplacement">Les Indemnités de Déplacement venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>PersonnelImportRulesResult</returns>
        public IndemniteDeplacementImportRulesResult VerifyImportRules(List<RepriseExcelIndemniteDeplacement> listIndemniteDeplacement, ContextForImportIndemniteDeplacement context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            IndemniteDeplacementImportRulesResult result = new IndemniteDeplacementImportRulesResult();

            foreach (RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement in listIndemniteDeplacement)
            {
                IndemniteDeplacementImportRuleResult requiredFieldsResult = VerifyRequiredFieldsRule(repriseExcelIndemniteDeplacement);

                IndemniteDeplacementImportRuleResult iVDResult = VerifyIVDRule(repriseExcelIndemniteDeplacement);

                IndemniteDeplacementImportRuleResult saisieManuelleResult = VerifySaisieManuelleRule(repriseExcelIndemniteDeplacement);

                // Matricule Valide
                IndemniteDeplacementImportRuleResult matriculePersonnelResult = VerifyMatriculePersonnelRule(repriseExcelIndemniteDeplacement, context);

                // CI Valide
                IndemniteDeplacementImportRuleResult codeCIResult = VerifyCodeCIRule(repriseExcelIndemniteDeplacement, context);

                //Check Unicité du couple PersonnelId + CiId (donc Matricule et Societe du CI)
                IndemniteDeplacementImportRuleResult personnelAndCIUniqueResult = VerifyPersonnelAndCIUniqueRule(repriseExcelIndemniteDeplacement, context, listIndemniteDeplacement);

                if (!repriseExcelIndemniteDeplacement.DateDernierCalcul.IsNullOrEmpty())
                {
                    IndemniteDeplacementImportRuleResult dateDernierCalculResult = VerifyDateDernierCalculRule(repriseExcelIndemniteDeplacement);

                    result.ImportRuleResults.Add(dateDernierCalculResult);
                }

                if (!repriseExcelIndemniteDeplacement.CodeDeplacement.IsNullOrEmpty())
                {
                    IndemniteDeplacementImportRuleResult codeDeplacementResult = VerifyCodeDeplacementRule(repriseExcelIndemniteDeplacement, context);

                    result.ImportRuleResults.Add(codeDeplacementResult);
                }

                if (!repriseExcelIndemniteDeplacement.CodeZoneDeplacement.IsNullOrEmpty())
                {
                    IndemniteDeplacementImportRuleResult codeZoneDeplacementResult = VerifyCodeZoneDeplacementRule(repriseExcelIndemniteDeplacement, context);

                    result.ImportRuleResults.Add(codeZoneDeplacementResult);
                }

                result.ImportRuleResults.Add(requiredFieldsResult);

                result.ImportRuleResults.Add(iVDResult);

                result.ImportRuleResults.Add(saisieManuelleResult);

                result.ImportRuleResults.Add(matriculePersonnelResult);

                result.ImportRuleResults.Add(codeCIResult);

                result.ImportRuleResults.Add(personnelAndCIUniqueResult);
            }

            return result;
        }

        /// <summary>
        /// RG_6456_004 : Vérification de l'unicité du couple (Personnel, CI) dans la table des indemnités
        /// Vérifier que :
        /// - L'ensemble des couples ([PersonnelId], [CiId]) du fichier d'entrée n'existent pas déjà dans la table [FRED_INDEMNITE_DEPLACEMENT] (en excluant les
        /// lignes supprimée logiquement, c'est à dire avec [DateSuppression] non vide)
        /// - Les mêmes couples ([PersonnelId], [CiId]) ne sont pas présents plus d'une fois à l'intérieur du fichier d'entrée
        /// Si un ou plusieurs lignes(s) est(sont) en anomalie, ne pas effectuer le chargement des données, et retourner la liste complète des anomalries trouvées,
        /// avec les messages suivants :
        /// Rejet ligne n°# : le personnel a déjà une indémnité pour ce CI.
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <param name="context">Contexte</param>
        /// <param name="listIndemniteDeplacement">Liste des lignes Excel</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyPersonnelAndCIUniqueRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement, ContextForImportIndemniteDeplacement context, List<RepriseExcelIndemniteDeplacement> listIndemniteDeplacement)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            result.ImportRuleType = IndemniteDeplacementImportRuleType.PersonnelAndCINonUnique;

            //1- Check si le couple Personnel + Ci existe déjà en BDD (FRED_INDEMNITE_DEPLACEMENT) en excluant les lignes supprimées logiquement 
            // (avec une DateSuppression différente de NULL)
            result.IsValid = !context.IndemniteDeplacementUsedInExcel.Any(x => x.Personnel.Matricule == repriseExcelIndemniteDeplacement.MatriculePersonnel
                                                                                && x.CI.Code == repriseExcelIndemniteDeplacement.CodeCI
                                                                                && !x.DateSuppression.HasValue);

            if (result.IsValid)
            {
                //2- S'il n'existe pas déjà en BDD, on check qu'il existe pas déjà dans notre fichier excel
                result.IsValid = listIndemniteDeplacement.Count(x => x.MatriculePersonnel == repriseExcelIndemniteDeplacement.MatriculePersonnel
                                                                    && x.CodeCI == repriseExcelIndemniteDeplacement.CodeCI)
                                                                    <= 1;
            }

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessagePersonnelAndCINonUnique, repriseExcelIndemniteDeplacement.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher d'abord la valeur "Société du CI" dans la table FRED_SOCIETE pour le Groupe spécifié par l'utilisateur pour trouver le SocieteId, puis rechercher ensuite
        /// la valeur "Code CI" dans le champs [Code] de la table FRED_CI pour le SocieteId du CI trouvé précédemment.
        /// Non trouvé (société ou CI) => rejet de la ligne
        /// Rejet ligne n°# : Société du CI invalide.
        /// Rejet ligne n°# : Code CI invalide.
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <param name="context">Contexte</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyCodeCIRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement, ContextForImportIndemniteDeplacement context)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            CommonFieldsValidator validator = new CommonFieldsValidator();

            // On teste d'abord la societé du CI
            result.IsValid = validator.CodeSocieteIsValid(context.GroupeId, repriseExcelIndemniteDeplacement.SocieteCI, context.OrganisationTree);

            if (result.IsValid)
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.CodeCIInvalide;

                // Si pas d'erreur de Societe du CI, on check le Code CI
                result.IsValid = context.CIsUsedInExcel.Any(x => x.Code == repriseExcelIndemniteDeplacement.CodeCI
                                                            && x.Societe.Code == repriseExcelIndemniteDeplacement.SocieteCI);

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageCodeCIInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }
            else
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.SocieteDuCIInvalide;

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageSocieteDuCIInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher d'abord la valeur "Société du Personnel" dans la table FRED_SOCIETE pour le Groupe spécifié par l'utilisateur pour trouver le SocieteId, 
        /// puis rechercher ensuite dans la table FRED_PERSONNEL la valeur "Matricule Personnel" dans le champ [Matricule] pour le [SocieteId] du 
        /// Personnel trouvé précédemment.
        /// Non trouvé(société ou personnel) => rejet de la ligne
        /// Rejet ligne n°# : Société du Personnel invalide.
        /// Rejet ligne n°# : Matricule Personnel invalide.
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <param name="context">Contexte</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyMatriculePersonnelRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement, ContextForImportIndemniteDeplacement context)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            CommonFieldsValidator validator = new CommonFieldsValidator();

            // On teste d'abord la societé du Personnel
            result.IsValid = validator.CodeSocieteIsValid(context.GroupeId, repriseExcelIndemniteDeplacement.SocietePersonnel, context.OrganisationTree);

            if (result.IsValid)
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.MatriculeDuPersonnelInvalide;

                // Si pas d'erreur de Societe du Personnel, on check le Matricule
                result.IsValid = context.PersonnelsUsedInExcel.Any(x => x.Matricule == repriseExcelIndemniteDeplacement.MatriculePersonnel);

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageMatriculePersonnelInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }
            else
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.SocieteDuPersonnelInvalide;

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageSocieteDuPersonnelInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher d'abord la valeur "Société des Codes Dep" dans la table FRED_SOCIETE pour le Groupe spécifié par l'utilisateur pour trouver le 
        /// SocieteId, puis rechercher ensuite la valeur "Code Zone Déplacement" dans le champ [Code] de la table FRED_CODE_ZONE_DEPLACEMENT pour le
        /// SocieteId des Codes Dep trouvé précédemment.
        /// Non trouvé(société ou code zone déplacement) => rejet de la ligne
        /// Rejet ligne n°# : Société des Codes Dep invalide.
        /// Rejet ligne n°# : Code Zone Déplacement invalide.
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <param name="context">Contexte</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyCodeZoneDeplacementRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement, ContextForImportIndemniteDeplacement context)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            CommonFieldsValidator validator = new CommonFieldsValidator();

            // On teste d'abord la societé des Codes Dep
            result.IsValid = validator.CodeSocieteIsValid(context.GroupeId, repriseExcelIndemniteDeplacement.SocieteCodeDeplacement, context.OrganisationTree);

            if (result.IsValid)
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.CodeZoneDeplacementInvalide;

                // Si pas d'erreur de Societe Code Dep on check le Code Zone Déplacement
                result.IsValid = context.CodesZoneDeplacementUsedInExcel.Any(x => x.Code == repriseExcelIndemniteDeplacement.CodeZoneDeplacement);

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageCodeZoneDeplacementInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }
            else
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.SocieteDesCodesDepInvalide;

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageSocieteDesCodeDeplacementInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher d'abord la valeur "Société des Codes Dep" dans la table FRED_SOCIETE pour le Groupe spécifié par l'utilisateur pour trouver le SocieteId
        /// puis rechercher ensuite la valeur "Code Déplacement" dans le champs [Code] de la table FRED_CODE_DEPLACEMENT pour le SocieteId des Codes Dep trouvé
        /// précédement.
        /// Non trouvé (société ou code déplacement) => rejet de la ligne
        /// Rejet ligne n°# : Société des Codes Dep invalide.
        /// Rejet ligne n°# : Code Déplacement invalide.
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <param name="context">Contexte</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyCodeDeplacementRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement, ContextForImportIndemniteDeplacement context)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            // Si le champs CodeDeplacement est renseigné alors on traite
            CommonFieldsValidator validator = new CommonFieldsValidator();

            // On teste d'abord la societé des Codes Dep
            result.IsValid = validator.CodeSocieteIsValid(context.GroupeId, repriseExcelIndemniteDeplacement.SocieteCodeDeplacement, context.OrganisationTree);
            if (result.IsValid)
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.CodeDeplacementInvalide;

                // Si pas d'erreur de Societe Code Dep on check le Code Déplacement
                result.IsValid = context.CodesDeplacementUsedInExcel.Any(x => x.Code == repriseExcelIndemniteDeplacement.CodeDeplacement);

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageCodeDeplacementInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }
            else
            {
                result.ImportRuleType = IndemniteDeplacementImportRuleType.SocieteDesCodesDepInvalide;

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageSocieteDesCodeDeplacementInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);
            }

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rejet si valeur non reconnue : "Saisie Manuelle (O/N)" vrai si "O", faux si "N"
        /// Rejet ligne n°# : Saisie Manuelle invalide
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifySaisieManuelleRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            result.ImportRuleType = IndemniteDeplacementImportRuleType.SaisieManuelleInvalide;

            string saisieManuelle = repriseExcelIndemniteDeplacement.SaisieManuelle.Trim().ToLower();

            result.IsValid = saisieManuelle == "o" || saisieManuelle == "n";

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageSaisieManuelleInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rejet si valeur non reconnue : "IVD (O/N)" vrai si "O", faux si "N"
        /// Rejet ligne n°# : IVD invalide
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyIVDRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            result.ImportRuleType = IndemniteDeplacementImportRuleType.IVDInvalide;

            string iVD = repriseExcelIndemniteDeplacement.IVD.Trim().ToLower();

            result.IsValid = iVD == "o" || iVD == "n";

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageIVDInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Si le format de la date est invalide on rejette la ligne
        /// Rejet ligne n°# : Date Dernier Calcul invalide
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne Excel</param>
        /// <returns>Résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyDateDernierCalculRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            result.ImportRuleType = IndemniteDeplacementImportRuleType.DateDernierCalculInvalide;

            CommonFieldsValidator validator = new CommonFieldsValidator();

            result.IsValid = validator.DateIsValid(repriseExcelIndemniteDeplacement.DateDernierCalcul);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportIndemniteDeplacementErrorMessageDateDernierCalculInvalide, repriseExcelIndemniteDeplacement.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Verification de la RG RG_5415_003
        /// <para>
        /// RG_5415_003 : Vérification des champs obligatoires
        ///        Les informations suivantes sont obligatoires :
        /// •    Société du CI
        /// •    Code CI
        /// •    Société du Personnel
        /// •    Matricule Personnel
        /// •    Société des Codes Dep
        /// •    IVD (O/N)
        /// •    Saisie Manuelle (O/N)
        /// </para>
        /// <para>
        /// => Pour chaque ligne du fichier pour lesquelles au moins une valeur est remplie, vérifier que tous ces champs sont non vides.
        /// </para>
        /// <para>
        /// Si un ou plusieurs ligne(s) est(sont) en anomalie, ne pas effectuer le chargmenet des données et retourner la liste complète des anomalies trouvées, avec les messages suivants :
        /// « Rejet ligne n°# : champ(s) obligatoire(s) non renseigné(s). »
        /// </para>
        /// </summary>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne excel</param>
        /// <returns>Le résultat de la validation</returns>
        private IndemniteDeplacementImportRuleResult VerifyRequiredFieldsRule(RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement)
        {
            IndemniteDeplacementImportRuleResult result = new IndemniteDeplacementImportRuleResult();

            result.ImportRuleType = IndemniteDeplacementImportRuleType.RequiredField;

            bool societeCIIsValid = !repriseExcelIndemniteDeplacement.SocieteCI.IsNullOrEmpty();
            bool codeCIIsValid = !repriseExcelIndemniteDeplacement.CodeCI.IsNullOrEmpty();
            bool societePersonnelIsValid = !repriseExcelIndemniteDeplacement.SocietePersonnel.IsNullOrEmpty();
            bool matriculePersonnelIsValid = !repriseExcelIndemniteDeplacement.MatriculePersonnel.IsNullOrEmpty();
            bool societeCodeDeplacementIsValid = !repriseExcelIndemniteDeplacement.SocieteCodeDeplacement.IsNullOrEmpty();
            bool iVDIsValid = !repriseExcelIndemniteDeplacement.IVD.IsNullOrEmpty();
            bool saisieManuelleIsValid = !repriseExcelIndemniteDeplacement.SaisieManuelle.IsNullOrEmpty();

            result.IsValid = societeCIIsValid
                && codeCIIsValid
                && societePersonnelIsValid
                && matriculePersonnelIsValid
                && societeCodeDeplacementIsValid
                && iVDIsValid
                && saisieManuelleIsValid;

            // BusinessResources.ImportCommandeRequiredFieldError affiche le meme message que nous pour les Indemnités de Déplacement.
            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRequiredFieldError, repriseExcelIndemniteDeplacement.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelIndemniteDeplacement.NumeroDeLigne);

            return result;
        }
    }
}
