using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Business.RepriseDonnees.Materiel.Models;
using Fred.Business.RepriseDonnees.Materiel.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Materiel.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import des Personnels
    /// </summary>
    public class MaterielValidatorService : IMaterielValidatorService
    {
        /// <summary>
        /// Verifie les regles d'import des Matériels
        /// </summary>
        /// <param name="listRepriseExcelMateriel">Les Matériels venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>MaterielImportRulesResult</returns>
        public MaterielImportRulesResult VerifyImportRules(List<RepriseExcelMateriel> listRepriseExcelMateriel, ContextForImportMateriel context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            MaterielImportRulesResult result = new MaterielImportRulesResult();

            foreach (RepriseExcelMateriel repriseExcelMateriel in listRepriseExcelMateriel)
            {
                MaterielImportRuleResult requiredFieldsResult = VerifyRequiredFieldsRule(repriseExcelMateriel);

                MaterielImportRuleResult codeSocieteResult = VerifyCodeSocieteRule(repriseExcelMateriel, context);

                MaterielImportRuleResult codeRessourceResult = VerifyCodeRessourceRule(repriseExcelMateriel, context);

                MaterielImportRuleResult codeMaterielResult = VerifyCodeMaterielRule(repriseExcelMateriel, context, listRepriseExcelMateriel);

                result.ImportRuleResults.Add(requiredFieldsResult);

                result.ImportRuleResults.Add(codeSocieteResult);

                result.ImportRuleResults.Add(codeRessourceResult);

                result.ImportRuleResults.Add(codeMaterielResult);
            }

            return result;
        }

        /// <summary>
        /// RG_5416_004 : Vérification de l'unicité du code Matériel par société
        /// Vérifier que :
        /// - l'ensemble des couples (société, code) du fichier d'entrée n'existent pas déjà dans la base FRED
        /// - les mêmes couples (société, code) ne sont pas présents plus d'une fois à l'intérieur du fichier
        /// NB - Récupérer la société à partir du "Code Societé Rattachement" et du Groupe choisi par l'utilisateur
        /// Rejet ligne n°# : le matricule existe déjà pour cette société.
        /// </summary>
        /// <param name="repriseExcelMateriel">Ligne Excel</param>
        /// <param name="context">Le contexte</param>
        /// <param name="listRepriseExcelMateriel">Liste des lignes du Excel</param>
        /// <returns>Résultat de la validation</returns>
        private MaterielImportRuleResult VerifyCodeMaterielRule(RepriseExcelMateriel repriseExcelMateriel, ContextForImportMateriel context, List<RepriseExcelMateriel> listRepriseExcelMateriel)
        {
            MaterielImportRuleResult result = new MaterielImportRuleResult();

            result.ImportRuleType = MaterielImportRuleType.CodeMaterielInvalide;

            //1- Check si le couple CodeMateriel + CodeSociete existe déjà en BDD
            result.IsValid = !context.MaterielsUsedInExcel.Any(x => x.Code == repriseExcelMateriel.CodeMateriel
                                                                        && x.Societe.Code == repriseExcelMateriel.CodeSociete);

            if (result.IsValid)
            {
                //2- S'il n'existe pas déjà en BDD, on check qu'il existe pas déjà dans notre fichier excel
                result.IsValid = listRepriseExcelMateriel.Count(x => x.CodeMateriel == repriseExcelMateriel.CodeMateriel
                                                                        && x.CodeSociete == repriseExcelMateriel.CodeSociete)
                                                                        <= 1;
            }

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportMaterielErrorMessageCodeMaterielNonUnique, repriseExcelMateriel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelMateriel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Ressource" dans la table FRED_RESSOURCE, parmi les ressources associées au Groupe choisi par l'utilisateur.
        /// Si non reconnu => Rejet ligne n°# : Code Ressource invalide.
        /// </summary>
        /// <param name="repriseExcelMateriel">Ligne Excel</param>
        /// <param name="context">Le contexte</param>
        /// <returns>Résultat de la validation</returns>
        private MaterielImportRuleResult VerifyCodeRessourceRule(RepriseExcelMateriel repriseExcelMateriel, ContextForImportMateriel context)
        {
            MaterielImportRuleResult result = new MaterielImportRuleResult();

            result.ImportRuleType = MaterielImportRuleType.CodeRessourceInvalide;

            result.IsValid = context.RessourcesUsedInExcel.Any(x => string.Equals(x.Code, repriseExcelMateriel.CodeRessource, System.StringComparison.OrdinalIgnoreCase));

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRessourceForCommandeLigneInvalid, repriseExcelMateriel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelMateriel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Société" dans la table FRED_SOCIETE pour le Groupe choisi par l'utilisateur
        /// Rejet ligne n°# : Code Société invalide
        /// </summary>
        /// <param name="repriseExcelMateriel">Ligne Excel</param>
        /// <param name="context">Contexte</param>
        /// <returns>Résultat de la validation</returns>
        private MaterielImportRuleResult VerifyCodeSocieteRule(RepriseExcelMateriel repriseExcelMateriel, ContextForImportMateriel context)
        {
            MaterielImportRuleResult result = new MaterielImportRuleResult();

            result.ImportRuleType = MaterielImportRuleType.SocieteNotInGroupe;

            CommonFieldsValidator validator = new CommonFieldsValidator();

            result.IsValid = validator.CodeSocieteIsValid(context.GroupeId, repriseExcelMateriel.CodeSociete, context.OrganisationTree);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCodeSocieteInvalide, repriseExcelMateriel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelMateriel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Verification de la RG RG_5416_003
        /// <para>
        /// RG_5416_003 : Vérification des champs obligatoires
        ///        Les informations suivantes sont obligatoires dans le fichier d'entrée :
        /// •    Code Société Rattachement
        /// •    Code Matériel
        /// •    Libellé Matériel
        /// •    Code Ressource        
        /// </para>
        /// <para>
        /// => Pour chaque ligne du fichier pour lesquelles au moins une valeur est remplie, vérifier que tous ces champs sont non vides.
        /// </para>
        /// <para>
        /// Si un ou plusieurs ligne(s) est(sont) en anomalie, ne pas effectuer le chargmenet des données et retourner la liste complète des anomalies trouvées, avec les messages suivants :
        /// « Rejet ligne n°# : champ(s) obligatoire(s) non renseigné(s). »
        /// </para>
        /// </summary>
        /// <param name="repriseExcelMateriel">Ligne excel</param>
        /// <returns>Le résultat de la validation</returns>
        private MaterielImportRuleResult VerifyRequiredFieldsRule(RepriseExcelMateriel repriseExcelMateriel)
        {
            MaterielImportRuleResult result = new MaterielImportRuleResult();

            result.ImportRuleType = MaterielImportRuleType.RequiredField;

            bool codeSocieteIsValid = !repriseExcelMateriel.CodeSociete.IsNullOrEmpty();
            bool codeMaterielIsValid = !repriseExcelMateriel.CodeMateriel.IsNullOrEmpty();
            bool libelleMaterielIsValid = !repriseExcelMateriel.LibelleMateriel.IsNullOrEmpty();
            bool codeRessourceIsValid = !repriseExcelMateriel.CodeRessource.IsNullOrEmpty();

            result.IsValid = codeSocieteIsValid
                && codeMaterielIsValid
                && libelleMaterielIsValid
                && codeRessourceIsValid;

            // BusinessResources.ImportCommandeRequiredFieldError affiche le meme message que nous pour les Materiels.
            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRequiredFieldError, repriseExcelMateriel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelMateriel.NumeroDeLigne);

            return result;
        }
    }
}
