using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.CI;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Business.OperationDiverse.ImportODfromExcel.Selectors;
using Fred.Business.OperationDiverse.ImportODfromExcel.Validators.Results;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Business.Utilisateur;
using Fred.Entities.CI;
using Fred.Entities.OperationDiverse;
using Fred.Entities.OperationDiverse.Excel;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Extensions;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Validators
{
    public class ImportODValidatorService : IImportODValidatorService
    {
        private readonly ICIManager ciManager;
        private readonly IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager;
        private readonly IUtilisateurManager utilisateurManager;

        public ImportODValidatorService(
            ICIManager ciManager,
            IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager,
            IUtilisateurManager utilisateurManager)
        {
            this.ciManager = ciManager;
            this.affectationSeuilUtilisateurManager = affectationSeuilUtilisateurManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Verifie les regles d'import des rapports
        /// </summary>
        /// <param name="excelODs">les operations diverses venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>VerifyImportRapportRulesResult</returns>
        public ImportODRulesResult VerifyImportRules(List<ExcelOdModel> excelODs, ContextForImportOD context)
        {
            CommonFieldSelector commonFieldSelector = new CommonFieldSelector();
            ImportODRulesResult result = new ImportODRulesResult();

            foreach (ExcelOdModel ligneExcelOD in excelODs)
            {
                CIEnt ci = commonFieldSelector.GetCiOfDatabase(context.OrganisationTree, context.GroupeId, ligneExcelOD.CodeSociete, ligneExcelOD.CodeCi, context.CisUsedInExcel);
                DateTime dateComptable;
                DateTime.TryParse(ligneExcelOD.DateComptable, out dateComptable);

                if (dateComptable.Equals(DateTime.MinValue))
                {
                    dateComptable = context.DateComtableFromUI;
                }
                // RG_5996_007 : Vérification des champs obligatoires
                ImportODRuleResult requiredFields = VerifyRequiredFiledsRule(ligneExcelOD);

                // RG_5996_008 : Vérification des périodes
                ImportODRuleResult isDateComptableClosed = VerifyDateComptableClosedRule(ligneExcelOD, ci, dateComptable);

                // RG_5996 : Vérification des autorisations de l’utilisateur sur les CI
                ImportODRuleResult ciUnknowResult = VerifyCiRule(ligneExcelOD, context);
                ImportODRuleResult ciAuthorisationResult = VerifyCiAuthorisationRule(ligneExcelOD, context, ci);

                ImportODRuleResult dateComtableFormatResult = VerifyFormatDateComptableRule(ligneExcelOD);
                ImportODRuleResult isQuantiteFormatInvalidResult = VerifyQuantiteFormatRule(ligneExcelOD, ligneExcelOD.Quantite);
                ImportODRuleResult isPuHUFormatInvalidResult = VerifyPuHTFormatRule(ligneExcelOD, ligneExcelOD.PuHT);
                ImportODRuleResult isTacheNotFoundResult = VerifyTacheRule(ligneExcelOD, context);
                ImportODRuleResult isUniteNotFoundResult = VerifyUniteUnknowRule(context, ligneExcelOD);
                ImportODRuleResult isDeviseNotFoundResult = VerifyDeviseUnknowRule(context, ligneExcelOD);
                ImportODRuleResult isFamilleNotFoundResult = VerifyFamilleODUnknowRule(context, ligneExcelOD);
                ImportODRuleResult isRessourceNotFoundResult = VerifyRessourceUnknowRule(context, ligneExcelOD);

                result.ImportRuleResults.Add(requiredFields);
                result.ImportRuleResults.Add(isDateComptableClosed);
                result.ImportRuleResults.Add(ciAuthorisationResult);
                result.ImportRuleResults.Add(ciUnknowResult);
                result.ImportRuleResults.Add(dateComtableFormatResult);
                result.ImportRuleResults.Add(isQuantiteFormatInvalidResult);
                result.ImportRuleResults.Add(isPuHUFormatInvalidResult);
                result.ImportRuleResults.Add(isTacheNotFoundResult);
                result.ImportRuleResults.Add(isUniteNotFoundResult);
                result.ImportRuleResults.Add(isDeviseNotFoundResult);
                result.ImportRuleResults.Add(isFamilleNotFoundResult);
                result.ImportRuleResults.Add(isRessourceNotFoundResult);

            }
            return result;
        }

        /// <summary>
        /// RG_5996_007 : Vérification des champs obligatoires
        /// <para>
        /// RG_5996_007 : « Chargement des operations diverses » - Vérification des champs obligatoires 
        ///        Les informations suivantes sont obligatoires :
        /// •    Code Société CI
        /// •    Code CI
        /// •    Quantité
        /// •    PU HT
        /// •    Code Famille
        /// •    Code Ressource
        /// </para>
        /// <para>
        /// => Pour chaque ligne du fichier pour lesquelles au moins une valeur est remplie, vérifier que tous ces champs sont non vides.
        /// </para>
        /// <para>
        /// Si un ou plusieurs ligne(s) est(sont) en anomalie, remonter la liste de ces lignes en erreur, avec les messages suivants :
        /// « Rejet ligne n°# : champ(s) obligatoire(s) non renseigné(s). »
        /// </para>
        /// </summary>
        /// <param name="ligneExcelOD">ligne excel</param>
        /// <returns>ImportODRuleResult</returns>
        private ImportODRuleResult VerifyRequiredFiledsRule(ExcelOdModel ligneExcelOD)
        {
            var result = new ImportODRuleResult();
            result.ImportRuleType = ImportODRuleType.RequiredField;

            var codeSocieteIsValid = !ligneExcelOD.CodeSociete.IsNullOrEmpty();
            var codeCiIsValid = !ligneExcelOD.CodeCi.IsNullOrEmpty();
            var quantiteIsValid = !ligneExcelOD.Quantite.IsNullOrEmpty();
            var puHTIsValid = !ligneExcelOD.PuHT.IsNullOrEmpty();
            var codeFamilleIsValid = !ligneExcelOD.CodeFamille.IsNullOrEmpty();
            var codeRessourceIsValid = !ligneExcelOD.CodeRessource.IsNullOrEmpty();
            result.IsValid = codeSocieteIsValid && codeCiIsValid && quantiteIsValid && puHTIsValid && codeFamilleIsValid && codeRessourceIsValid;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportRequiredFieldError, ligneExcelOD.NumeroDeLigne);
            return result;
        }

        private ImportODRuleResult VerifyCiRule(ExcelOdModel excelOdModel, ContextForImportOD context)
        {
            var result = new ImportODRuleResult();

            result.ImportRuleType = ImportODRuleType.CiUnknow;

            var societe = context.SocietesOfGroupe.Find(s => s.Code == excelOdModel.CodeSociete);
            if (societe == null)
            {
                result.IsValid = false;

                result.ErrorMessage = string.Format(BusinessResources.ImportCIErrorMessageCodeSocieteInvalide, excelOdModel.NumeroDeLigne);
                return result;
            }

            var cisOfSociete = context.OrganisationTree.GetAllCisOfSociete(societe.Id);

            result.IsValid = cisOfSociete.Select(x => x.Code).ToList().Contains(excelOdModel.CodeCi);

            var messageErreur = string.Format(BusinessResources.ImportRapportMessageErrorCodeCiInvalid, excelOdModel.NumeroDeLigne);
            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            return result;
        }

        private ImportODRuleResult VerifyCiAuthorisationRule(ExcelOdModel excelOdModel, ContextForImportOD context, CIEnt ci)
        {
            var result = new ImportODRuleResult();

            result.ImportRuleType = ImportODRuleType.CiUnknow;

            List<OrganisationEnt> userOrganisations = affectationSeuilUtilisateurManager.GetListByUtilisateurId(context.User.UtilisateurId).Select(x => x.Organisation).ToList();
            List<int> authorisedCi = new List<int>();

            foreach (OrganisationEnt organisation in userOrganisations)
            {
                authorisedCi.AddRange(utilisateurManager.GetAllCIIdbyOrganisation(organisation.OrganisationId));
            }

            result.IsValid = ci?.Organisation != null && authorisedCi.Any(x => x == ci.CiId);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportODIsCiAuthorizedErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyUniteUnknowRule(ContextForImportOD context, ExcelOdModel excelOdModel)
        {
            ImportODRuleResult result = new ImportODRuleResult();
            var operationDiverseSelectors = new OperationDiverseSelectors();
            result.ImportRuleType = ImportODRuleType.UniteInvalid;

            UniteEnt unite = operationDiverseSelectors.GetUnite(excelOdModel.CodeUnite, context);

            result.IsValid = unite != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportOdUniteUnkowErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyDeviseUnknowRule(ContextForImportOD context, ExcelOdModel excelOdModel)
        {
            ImportODRuleResult result = new ImportODRuleResult();
            var operationDiverseSelectors = new OperationDiverseSelectors();
            result.ImportRuleType = ImportODRuleType.DeviseInvalid;

            DeviseEnt devise = operationDiverseSelectors.GetDevise(excelOdModel.CodeDevise, context);

            result.IsValid = devise != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportOdDeviseUnkowErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyFamilleODUnknowRule(ContextForImportOD context, ExcelOdModel excelOdModel)
        {
            ImportODRuleResult result = new ImportODRuleResult();
            var operationDiverseSelectors = new OperationDiverseSelectors();
            result.ImportRuleType = ImportODRuleType.CodeFamilleInvalid;

            FamilleOperationDiverseEnt famille = operationDiverseSelectors.GetFamilleOD(excelOdModel.CodeSociete, excelOdModel.CodeFamille, context);

            result.IsValid = famille != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportOdFamilleUnkowErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyRessourceUnknowRule(ContextForImportOD context, ExcelOdModel excelOdModel)
        {
            ImportODRuleResult result = new ImportODRuleResult();
            var operationDiverseSelectors = new OperationDiverseSelectors();
            result.ImportRuleType = ImportODRuleType.CodeRessourceInvalid;

            RessourceEnt ressource = operationDiverseSelectors.GetRessource(excelOdModel.CodeRessource, context);

            result.IsValid = ressource != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportOdRessourceUnkowErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyQuantiteFormatRule(ExcelOdModel excelOdModel, string decimalValue)
        {
            var result = new ImportODRuleResult();

            result.ImportRuleType = ImportODRuleType.QuantiteFormatIncorrect;
            double output;
            var parseResult = double.TryParse(decimalValue, out output);

            result.IsValid = parseResult;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportOdQuantiteFormatErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyPuHTFormatRule(ExcelOdModel excelOdModel, string decimalValue)
        {
            var result = new ImportODRuleResult();

            result.ImportRuleType = ImportODRuleType.QuantiteFormatIncorrect;
            double output;
            var parseResult = double.TryParse(decimalValue, out output);

            result.IsValid = parseResult;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportOdPuHTFormatErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyFormatDateComptableRule(ExcelOdModel excelOdModel)
        {
            var commonValidator = new CommonFieldsValidator();
            var result = new ImportODRuleResult();

            result.ImportRuleType = ImportODRuleType.DateComptableFormatIncorrect;

            result.IsValid = (string.IsNullOrEmpty(excelOdModel.DateComptable)) || commonValidator.DateIsValid(excelOdModel.DateComptable);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportOdDateComptableFormatErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyTacheRule(ExcelOdModel excelOdModel, ContextForImportOD context)
        {
            var selector = new CommonFieldSelector();
            var result = new ImportODRuleResult();

            result.ImportRuleType = ImportODRuleType.TacheNotFound;

            var ci = context.OrganisationTree.GetCi(context.GroupeId, excelOdModel.CodeSociete, excelOdModel.CodeCi);

            TacheEnt tache = selector.GetTache(ci?.Id, excelOdModel.CodeTache, context.TachesUsedInExcel);

            result.IsValid = tache != null;

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportRapportMessageErrorTacheNonConnu, excelOdModel.NumeroDeLigne);

            return result;
        }

        private ImportODRuleResult VerifyDateComptableClosedRule(ExcelOdModel excelOdModel, CIEnt ci, DateTime dateComptable)
        {
            var result = new ImportODRuleResult();

            result.ImportRuleType = ImportODRuleType.PeriodClosed;

            result.IsValid = (ci != null) && !ciManager.IsCIClosed(ci.CiId, dateComptable);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportODPeriodClosedErrorMessage, excelOdModel.NumeroDeLigne);

            return result;
        }
    }
}
