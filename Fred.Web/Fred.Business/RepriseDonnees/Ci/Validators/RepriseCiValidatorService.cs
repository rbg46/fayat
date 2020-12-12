using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Business.RepriseDonnees.Ci.Selector;
using Fred.Business.RepriseDonnees.Ci.Validators.Results;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Ci.Validators
{
    public class RepriseCiValidatorService : IRepriseCiValidatorService
    {
        private readonly IPersonnelSelectorService personnelSelectorService;
        private readonly IPaysSelectorService paysSelectorService;
        private readonly ICiSelectorService ciSelectorService;

        public RepriseCiValidatorService(
            IPersonnelSelectorService personnelSelectorService,
            IPaysSelectorService paysSelectorService,
            ICiSelectorService ciSelectorService)
        {
            this.personnelSelectorService = personnelSelectorService;
            this.paysSelectorService = paysSelectorService;
            this.ciSelectorService = ciSelectorService;
        }

        /// <summary>
        /// Verifie les regles d'import des Ci
        /// </summary>
        /// <param name="repriseExcelCis">les cis venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// 
        /// <returns>VerifyImportRulesResult</returns>
        public ImportRulesResult VerifyImportRules(List<RepriseExcelCi> repriseExcelCis, ContextForImportCi context)
        {
            var result = new ImportRulesResult();

            foreach (var repriseExcelCi in repriseExcelCis)
            {
                ImportRuleResult ciIsInDatabaseResult = VerifyCiIsInFredBatabaseRule(ImportRuleType.CiIsInDatabase, repriseExcelCi, context);

                ImportRuleResult codeSocieteResult = VerifyCodeSocieteRule(ImportRuleType.CiSocieteIsInGroupe, repriseExcelCi, context.SocietesOfGroupe);

                ImportRuleResult codeCiResult = VerifyCodeCiRule(ImportRuleType.CiCiIsInSociete, repriseExcelCi, context.OrganisationTree, context.SocietesOfGroupe);

                ImportRuleResult codePaysResult = VerifyCodePaysRule(ImportRuleType.CiCodePaysUnknow, context.PaysUsedInExcel, repriseExcelCi);

                ImportRuleResult codePaysFacturationResult = VerifyCodePaysRule(ImportRuleType.CiCodePaysFacturationUnknow, context.PaysUsedInExcel, repriseExcelCi);

                ImportRuleResult codePaysLivraisonResult = VerifyCodePaysRule(ImportRuleType.CiCodePaysLivraisonUnknow, context.PaysUsedInExcel, repriseExcelCi);

                ImportRuleResult responsableChantierResult = VerifyMatriculeResponsableChanierRule(ImportRuleType.CiResponsableChantierUnknow, repriseExcelCi, context);

                ImportRuleResult responsableAdministratifResult = VerifyMatriculeResponsableAdministratifRule(ImportRuleType.CiResponsableAdministratifUnknow, repriseExcelCi, context);

                ImportRuleResult zoneModifiableResult = VerifyZoneModifiableRule(ImportRuleType.CiZoneModifiableUnknow, repriseExcelCi);


                ImportRuleResult zoneDateOuvertureFormatResult = VerifyFormatDateOuvertureRule(ImportRuleType.CiDateOuvertureFormatIncorrect, repriseExcelCi);

                ImportRuleResult longitudeFormatFormatResult = VerifyLongitudeDecimalFormat(ImportRuleType.LongitudeFormatIncorrect, repriseExcelCi);

                ImportRuleResult latitudeFormatFormatResult = VerifyLatitudeDecimalFormat(ImportRuleType.LatitudeFormatIncorrect, repriseExcelCi);

                ImportRuleResult latitudeAndLongitudeResult = VerifyLongitudeOrLatitudeMissingRule(ImportRuleType.CiLongitudeOrLatitudeMissing, repriseExcelCi);


                result.ImportRuleResults.Add(ciIsInDatabaseResult);

                result.ImportRuleResults.Add(codeSocieteResult);

                result.ImportRuleResults.Add(codeCiResult);

                result.ImportRuleResults.Add(codePaysResult);

                result.ImportRuleResults.Add(codePaysFacturationResult);

                result.ImportRuleResults.Add(codePaysLivraisonResult);

                result.ImportRuleResults.Add(responsableChantierResult);

                result.ImportRuleResults.Add(responsableAdministratifResult);

                result.ImportRuleResults.Add(zoneModifiableResult);

                result.ImportRuleResults.Add(zoneDateOuvertureFormatResult);

                result.ImportRuleResults.Add(longitudeFormatFormatResult);

                result.ImportRuleResults.Add(latitudeFormatFormatResult);

                result.ImportRuleResults.Add(latitudeAndLongitudeResult);

            }

            return result;
        }

        private ImportRuleResult VerifyCiIsInFredBatabaseRule(ImportRuleType ciIsInDatabase, RepriseExcelCi repriseExcelCi, ContextForImportCi context)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciIsInDatabase;

            var ciInFred = ciSelectorService.GetCiOfDatabase(repriseExcelCi, context);

            result.IsValid = ciInFred != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageAucunCiAvecCeCodeExiste, repriseExcelCi.NumeroDeLigne, repriseExcelCi.CodeCi);

            return result;
        }

        private ImportRuleResult VerifyCodeSocieteRule(ImportRuleType ciSocieteIsInGroupe, RepriseExcelCi repriseExcelCi, List<OrganisationBase> societesOfGroupe)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciSocieteIsInGroupe;

            var codeSocieteIsValid = societesOfGroupe.Select(x => x.Code).ToList().Contains(repriseExcelCi.CodeSociete);

            result.IsValid = codeSocieteIsValid;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCodeSocieteInvalide, repriseExcelCi.NumeroDeLigne);

            return result;
        }


        private ImportRuleResult VerifyCodeCiRule(ImportRuleType ciCiIsInSociete, RepriseExcelCi repriseExcelCi, OrganisationTree organisationTree, List<OrganisationBase> societesOfGroupe)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciCiIsInSociete;

            var messageErreur = string.Format(BusinessResources.ImportCIErrorMessageCodeCiInvalide, repriseExcelCi.NumeroDeLigne);

            var societe = societesOfGroupe.FirstOrDefault(s => s.Code == repriseExcelCi.CodeSociete);
            if (societe == null)
            {
                result.IsValid = false;

                result.ErrorMessage = messageErreur;

                return result;
            }

            var cisOfSociete = organisationTree.GetAllCisOfSociete(societe.Id);

            var codeCiIsValid = cisOfSociete.Select(x => x.Code).ToList().Contains(repriseExcelCi.CodeCi);

            result.IsValid = codeCiIsValid;

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;
        }

        private ImportRuleResult VerifyCodePaysRule(ImportRuleType paysUnknow, List<PaysEnt> pays, RepriseExcelCi repriseExcelCi)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = paysUnknow;

            if (paysUnknow == ImportRuleType.CiCodePaysUnknow)
            {
                var codePays = paysSelectorService.GetPaysByCode(pays, repriseExcelCi.CodePays);

                result.IsValid = repriseExcelCi.CodePays.IsNullOrEmpty() || codePays != null;

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCodePaysInvalide, repriseExcelCi.NumeroDeLigne);
            }

            if (paysUnknow == ImportRuleType.CiCodePaysFacturationUnknow)
            {
                var codePays = paysSelectorService.GetPaysByCode(pays, repriseExcelCi.CodePaysFacturation);

                result.IsValid = repriseExcelCi.CodePaysFacturation.IsNullOrEmpty() || codePays != null;

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCodePaysFacturationInvalide, repriseExcelCi.NumeroDeLigne);
            }

            if (paysUnknow == ImportRuleType.CiCodePaysLivraisonUnknow)
            {
                var codePays = paysSelectorService.GetPaysByCode(pays, repriseExcelCi.CodePaysLivraison);

                result.IsValid = repriseExcelCi.CodePaysLivraison.IsNullOrEmpty() || codePays != null;

                result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCodePaysLivraisonInvalide, repriseExcelCi.NumeroDeLigne);
            }


            return result;
        }


        private ImportRuleResult VerifyMatriculeResponsableChanierRule(ImportRuleType ciResponsableChantierUnknow, RepriseExcelCi repriseExcelCi, ContextForImportCi context)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciResponsableChantierUnknow;

            if (repriseExcelCi.MatriculeResponsableChantier.IsNullOrEmpty())
            {
                result.IsValid = true; // pas de matricule => RG OK

                return result;
            }
            var responsableChantier = personnelSelectorService.GetPersonnel(repriseExcelCi.CodeSociete, repriseExcelCi.MatriculeResponsableChantier, context);

            result.IsValid = responsableChantier != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageAucunResponsableChantierConnu, repriseExcelCi.NumeroDeLigne);

            return result;
        }

        private ImportRuleResult VerifyMatriculeResponsableAdministratifRule(ImportRuleType ciResponsableAdministratifUnknow, RepriseExcelCi repriseExcelCi, ContextForImportCi context)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciResponsableAdministratifUnknow;

            if (repriseExcelCi.MatriculeResponsableAdministratif.IsNullOrEmpty())
            {
                result.IsValid = true; // pas de matricule => RG OK

                return result;
            }

            var responsableAdministratif = personnelSelectorService.GetPersonnel(repriseExcelCi.CodeSociete, repriseExcelCi.MatriculeResponsableAdministratif, context);

            result.IsValid = responsableAdministratif != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageAucunResponsableAdministratifConnu, repriseExcelCi.NumeroDeLigne);

            return result;
        }


        private ImportRuleResult VerifyZoneModifiableRule(ImportRuleType ciResponsableAdministratifUnknow, RepriseExcelCi repriseExcelCi)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciResponsableAdministratifUnknow;

            var zoneModifiable = repriseExcelCi.ZoneModifiable.Trim().ToUpper();

            result.IsValid = zoneModifiable == "O" || zoneModifiable == "N" || zoneModifiable.IsNullOrEmpty();

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageZoneModifiableInvalide, repriseExcelCi.NumeroDeLigne);

            return result;
        }

        private ImportRuleResult VerifyFormatDateOuvertureRule(ImportRuleType ciDateOuvertureFormatIncorrect, RepriseExcelCi repriseExcelCi)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciDateOuvertureFormatIncorrect;

            var validator = new CommonFieldsValidator();

            result.IsValid = validator.DateIsValid(repriseExcelCi.DateOuverture);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCiDateOuvertureFormatIncorrect, repriseExcelCi.NumeroDeLigne);

            return result;
        }

        private ImportRuleResult VerifyLongitudeDecimalFormat(ImportRuleType formatIncorrect, RepriseExcelCi repriseExcelCi)
        {
            var result = new ImportRuleResult();

            var validator = new CommonFieldsValidator();

            result.ImportRuleType = formatIncorrect;

            result.IsValid = validator.DecimalIsValid(repriseExcelCi.Longitude);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageLongitudeFormatinvalide, repriseExcelCi.NumeroDeLigne);

            return result;
        }
        private ImportRuleResult VerifyLatitudeDecimalFormat(ImportRuleType formatIncorrect, RepriseExcelCi repriseExcelCi)
        {
            var result = new ImportRuleResult();

            var validator = new CommonFieldsValidator();

            result.ImportRuleType = formatIncorrect;

            result.IsValid = validator.DecimalIsValid(repriseExcelCi.Latitude);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageLatitudeFormatinvalide, repriseExcelCi.NumeroDeLigne);

            return result;
        }

        private ImportRuleResult VerifyLongitudeOrLatitudeMissingRule(ImportRuleType ciLongitudeOrLatitudeMissingRule, RepriseExcelCi repriseExcelCi)
        {
            var result = new ImportRuleResult();

            result.ImportRuleType = ciLongitudeOrLatitudeMissingRule;

            var latitude = repriseExcelCi.Latitude?.Trim();
            var longitude = repriseExcelCi.Longitude?.Trim();

            var latitudeMissing = string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude);

            var longitudeMissing = !string.IsNullOrEmpty(latitude) && string.IsNullOrEmpty(longitude);

            string errorMessage = string.Empty;

            if (latitudeMissing)
            {
                errorMessage = string.Format(BusinessResources.ImportCIErrorMessageLatitudeManquante, repriseExcelCi.NumeroDeLigne);
            }

            if (longitudeMissing)
            {
                errorMessage = string.Format(BusinessResources.ImportCIErrorMessageLongitudeManquante, repriseExcelCi.NumeroDeLigne);
            }

            result.IsValid = !latitudeMissing && !longitudeMissing;

            result.ErrorMessage = result.IsValid ? string.Empty : errorMessage;

            return result;
        }

    }
}
