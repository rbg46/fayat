using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Business.RepriseDonnees.Rapport.Selector;
using Fred.Business.RepriseDonnees.Rapport.Validators.Results;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Rapport.Validators
{
    /// <summary>
    /// Permet de valider les RGs d'import des cis
    /// </summary>
    public class RepriseRapportValidatorService : IRepriseRapportValidatorService
    {
        /// <summary>
        /// Verifie les regles d'import des rapports
        /// </summary>
        /// <param name="repriseExcelRapports">les rapports venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// 
        /// <returns>VerifyImportRapportRulesResult</returns>
        public ImportRapportRulesResult VerifyImportRules(List<RepriseExcelRapport> repriseExcelRapports, ContextForImportRapport context)
        {
            var result = new ImportRapportRulesResult();

            foreach (var repriseExcelRapport in repriseExcelRapports)
            {
                // RG_5635_002 : Vérification des champs obligatoires
                ImportRapportRuleResult requiredFields = VerifyRequiredFiledsRule(repriseExcelRapport);

                // RG_5635_003 : Verification des RG pour la creation des rapports 
                ImportRapportRuleResult ciUnknowResult = VerifyCiRule(ImportRapportRuleType.CiUnknow, repriseExcelRapport, context.OrganisationTree, context.SocietesOfGroupe);
                ImportRapportRuleResult dateChantierFormatResult = VerifyFormatDateChantierRule(ImportRapportRuleType.DateChantierFormatIncorrect, repriseExcelRapport);

                result.ImportRuleResults.Add(requiredFields);
                result.ImportRuleResults.Add(ciUnknowResult);
                result.ImportRuleResults.Add(dateChantierFormatResult);

                // RG_5635_004 : Verification des RG pour la creation des rapports lignes
                ImportRapportRuleResult personnelUnknowResult = VerifyPersonnelUnknowRule(ImportRapportRuleType.PersonnelUnknow, context, repriseExcelRapport);
                ImportRapportRuleResult codeDeplacementResult = VerifyCodeDeplacementRule(ImportRapportRuleType.CodeDeplacementUnknow, context, repriseExcelRapport);
                ImportRapportRuleResult codeZoneDeplacementResult = VerifyCodeZoneDeplacementRule(ImportRapportRuleType.CodeZoneDeplacementUnknow, context, repriseExcelRapport);
                ImportRapportRuleResult ivdResult = VerifyIVDRule(ImportRapportRuleType.IVDUnknow, repriseExcelRapport);
                ImportRapportRuleResult totalHeuresFormatResult = VerifyFormatHeuresTotalRule(ImportRapportRuleType.HeuresTotalFormatIncorrect, repriseExcelRapport);

                result.ImportRuleResults.Add(personnelUnknowResult);
                result.ImportRuleResults.Add(codeDeplacementResult);
                result.ImportRuleResults.Add(codeZoneDeplacementResult);
                result.ImportRuleResults.Add(ivdResult);
                result.ImportRuleResults.Add(totalHeuresFormatResult);

                // RG_5635_005 : Verification des RG pour la creation des rapports lignes taches
                ImportRapportRuleResult tacheNotFound = VerifyTacheRule(ImportRapportRuleType.TacheNotFound, repriseExcelRapport, context);
                result.ImportRuleResults.Add(tacheNotFound);
            }
            return result;
        }

        /// <summary>
        /// RG_5635_002 : Vérification des champs obligatoires
        /// <para>
        /// RG_5635_002 : 3 éme outil « Chargement des rapports » - Vérification des champs obligatoires 
        ///        Les informations suivantes sont obligatoires :
        /// •    Code Société CI
        /// •    Code CI
        /// •    Date Rapport
        /// •    Code Société Personnel
        /// •    Matricule Personnel
        /// •    IVD(O/N)
        /// •    Heures Totales
        /// </para>
        /// <para>
        /// => Pour chaque ligne du fichier pour lesquelles au moins une valeur est remplie, vérifier que tous ces champs sont non vides.
        /// </para>
        /// <para>
        /// Si un ou plusieurs ligne(s) est(sont) en anomalie, remonter la liste de ces lignes en erreur, avec les messages suivants :
        /// « Rejet ligne n°# : champ(s) obligatoire(s) non renseigné(s). »
        /// </para>
        /// </summary>
        /// <param name="repriseExcelRapport">ligne excel</param>
        /// <returns>ImportRapportRuleResult</returns>
        private ImportRapportRuleResult VerifyRequiredFiledsRule(RepriseExcelRapport repriseExcelRapport)
        {
            var result = new ImportRapportRuleResult();

            result.ImportRuleType = ImportRapportRuleType.RequiredField;

            var codeSocieteIsValid = !repriseExcelRapport.CodeSocieteCi.IsNullOrEmpty();
            var codeCiIsValid = !repriseExcelRapport.CodeCi.IsNullOrEmpty();
            var dateRapportIsValid = !repriseExcelRapport.DateRapport.IsNullOrEmpty();
            var codeSocietePersonnelIsValid = !repriseExcelRapport.CodeSocietePersonnel.IsNullOrEmpty();
            var matriculePersonnelIsValid = !repriseExcelRapport.MatriculePersonnel.IsNullOrEmpty();
            var ivdIsValid = !repriseExcelRapport.IVD.IsNullOrEmpty();
            var heuresTotalIsValid = !repriseExcelRapport.HeuresTotal.IsNullOrEmpty();

            result.IsValid =
                codeSocieteIsValid &&
                codeCiIsValid &&
                dateRapportIsValid &&
                codeSocietePersonnelIsValid &&
                matriculePersonnelIsValid &&
                ivdIsValid &&
                heuresTotalIsValid;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportRequiredFieldError, repriseExcelRapport.NumeroDeLigne);

            return result;
        }

        private ImportRapportRuleResult VerifyCiRule(ImportRapportRuleType ciUnknow, RepriseExcelRapport repriseExcelRapport, OrganisationTree organisationTree, List<OrganisationBase> societesOfGroupe)
        {
            var result = new ImportRapportRuleResult();

            result.ImportRuleType = ciUnknow;

            var societe = societesOfGroupe.FirstOrDefault(s => s.Code == repriseExcelRapport.CodeSocieteCi);
            if (societe == null)
            {
                result.IsValid = false;

                result.ErrorMessage = string.Format(BusinessResources.ImportCIErrorMessageCodeSocieteInvalide, repriseExcelRapport.NumeroDeLigne);

                return result;
            }

            var cisOfSociete = organisationTree.GetAllCisOfSociete(societe.Id);

            var codeCiIsValid = cisOfSociete.Select(x => x.Code).ToList().Contains(repriseExcelRapport.CodeCi);

            result.IsValid = codeCiIsValid;

            var messageErreur = string.Format(BusinessResources.ImportCIErrorMessageCodeCiInvalide, repriseExcelRapport.NumeroDeLigne);

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;
        }

        private ImportRapportRuleResult VerifyPersonnelUnknowRule(ImportRapportRuleType personnelUnknow, ContextForImportRapport context, RepriseExcelRapport repriseExcelRapport)
        {
            ImportRapportRuleResult result = new ImportRapportRuleResult();
            PersonnelSelectorService personnelSelectorService = new PersonnelSelectorService();
            result.ImportRuleType = personnelUnknow;

            PersonnelEnt personnel = personnelSelectorService.GetPersonnel(repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.MatriculePersonnel, context);

            result.IsValid = personnel != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorPersonnelNonConnu, repriseExcelRapport.NumeroDeLigne);

            return result;
        }

        private ImportRapportRuleResult VerifyCodeDeplacementRule(ImportRapportRuleType codeDeplacamentUnknow, ContextForImportRapport context, RepriseExcelRapport repriseExcelRapport)
        {
            var result = new ImportRapportRuleResult();
            result.ImportRuleType = codeDeplacamentUnknow;

            // le champ CodeDeplacement n'est pas obligatoire
            bool isEmpty = (repriseExcelRapport.CodeDeplacement?.Trim()).IsNullOrEmpty();
            if (isEmpty)
            {
                result.IsValid = true;
                result.ErrorMessage = string.Empty;
                return result;
            }
            CodeDeplacementSelectorService codeDeplacementSelectorService = new CodeDeplacementSelectorService();

            var codeDeplacement = codeDeplacementSelectorService.GetCodeDeplacement(repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.CodeDeplacement, context);
            result.IsValid = codeDeplacement != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorCodeDepartementNonConnu, repriseExcelRapport.NumeroDeLigne);

            return result;
        }
        private ImportRapportRuleResult VerifyCodeZoneDeplacementRule(ImportRapportRuleType codeZoneDeplacamentUnknow, ContextForImportRapport context, RepriseExcelRapport repriseExcelRapport)
        {
            ImportRapportRuleResult result = new ImportRapportRuleResult();
            result.ImportRuleType = codeZoneDeplacamentUnknow;

            // le champ CodeZoneDeplacement n'est pas obligatoire
            bool isEmpty = (repriseExcelRapport.CodeZoneDeplacement?.Trim()).IsNullOrEmpty();
            if (isEmpty)
            {
                result.IsValid = true;
                result.ErrorMessage = string.Empty;
                return result;
            }

            CodeZoneDeplacSelectorService codeZoneDeplacementSelectorService = new CodeZoneDeplacSelectorService();

            var codeDeplacement = codeZoneDeplacementSelectorService.GetCodeZoneDeplacement(repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.CodeZoneDeplacement, context);
            result.IsValid = codeDeplacement != null;
            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorCodeZoneDepartementNonConnu, repriseExcelRapport.NumeroDeLigne);
            return result;
        }
        private ImportRapportRuleResult VerifyFormatHeuresTotalRule(ImportRapportRuleType heuresTotalFormatIncorrect, RepriseExcelRapport repriseExcelRapport)
        {
            var result = new ImportRapportRuleResult();

            result.ImportRuleType = heuresTotalFormatIncorrect;
            double heuresTotalOutput;
            var heuresTotal = double.TryParse(repriseExcelRapport.HeuresTotal, out heuresTotalOutput);

            result.IsValid = heuresTotal;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorHeureTotalFormatInvalid, repriseExcelRapport.NumeroDeLigne);

            return result;
        }
        private ImportRapportRuleResult VerifyIVDRule(ImportRapportRuleType ivdUnkow, RepriseExcelRapport repriseExcelRapport)
        {
            var result = new ImportRapportRuleResult();

            result.ImportRuleType = ivdUnkow;

            var ivd = repriseExcelRapport.IVD.Trim().ToUpper();

            result.IsValid = ivd == "O" || ivd == "N" || ivd.IsNullOrEmpty();

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorIVDNonConnu, repriseExcelRapport.NumeroDeLigne);

            return result;
        }

        private ImportRapportRuleResult VerifyFormatDateChantierRule(ImportRapportRuleType dateChantierFormatIncorrect, RepriseExcelRapport repriseExcelRapport)
        {
            var commonValidator = new CommonFieldsValidator();
            var result = new ImportRapportRuleResult();

            result.ImportRuleType = dateChantierFormatIncorrect;

            result.IsValid = commonValidator.DateIsValid(repriseExcelRapport.DateRapport);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorDateFormatInvalid, repriseExcelRapport.NumeroDeLigne);

            return result;
        }
        private ImportRapportRuleResult VerifyTacheRule(ImportRapportRuleType tacheNotFound, RepriseExcelRapport repriseExcelRapport, ContextForImportRapport context)
        {
            var selector = new CommonFieldSelector();
            var result = new ImportRapportRuleResult();

            result.ImportRuleType = tacheNotFound;

            var ci = context.OrganisationTree.GetCi(context.GroupeId, repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.CodeCi);

            TacheEnt tache = selector.GetTache(ci?.Id, repriseExcelRapport.CodeTache, context.TachesUsedInExcel);

            result.IsValid = tache != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorTacheNonConnu, repriseExcelRapport.NumeroDeLigne);

            return result;
        }

    }
}
