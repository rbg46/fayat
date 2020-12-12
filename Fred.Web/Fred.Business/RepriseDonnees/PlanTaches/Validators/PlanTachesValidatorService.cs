using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Business.RepriseDonnees.PlanTaches.Models;
using Fred.Business.RepriseDonnees.PlanTaches.Validators.Results;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.PlanTaches.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import d'un plan de taches
    /// </summary>
    public class PlanTachesValidatorService : IPlanTachesValidatorService
    {
        /// <summary>
        /// Verifie les regles d'import d'un plan de taches
        /// </summary>
        /// <param name="listRepriseExcelPlanTaches">les taches venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>PlanTachesImportRulesResult</returns>
        public PlanTachesImportRulesResult VerifyImportRules(List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches, ContextForImportPlanTaches context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            var result = new PlanTachesImportRulesResult();

            foreach (RepriseExcelPlanTaches repriseExcelPlanTaches in listRepriseExcelPlanTaches)
            {
                PlanTachesImportRuleResult requiredFieldsResult = VerifyRequiredFieldsRule(repriseExcelPlanTaches);

                PlanTachesImportRuleResult niveauTacheResult = VerifyNiveauTacheRule(repriseExcelPlanTaches);

                PlanTachesImportRuleResult codeTacheResult = VerifyCodeTacheRule(repriseExcelPlanTaches, context, listRepriseExcelPlanTaches);

                PlanTachesImportRuleResult codeCiResult = VerifyCodeCiRule(repriseExcelPlanTaches, context);

                PlanTachesImportRuleResult codeTacheParentResult = VerifyCodeTacheParentRule(repriseExcelPlanTaches, context, listRepriseExcelPlanTaches);

                result.ImportRuleResults.Add(requiredFieldsResult);

                result.ImportRuleResults.Add(niveauTacheResult);

                result.ImportRuleResults.Add(codeTacheResult);

                result.ImportRuleResults.Add(codeCiResult);

                result.ImportRuleResults.Add(codeTacheParentResult);
            }

            return result;
        }

        #region Règles de validations        

        /// <summary>
        /// Verification de la RG RG_5635_002
        /// <para>
        /// RG_5635_002 : 1er outil « Chargement des commandes et réceptions » - Vérification des champs obligatoires (= contrôle n°3)
        ///        Les informations suivantes sont obligatoires :
        /// •    Code Société
        /// •    Code CI
        /// •    Niveau T1/T2/T3
        /// •    Code Tâche T1/T2/T3
        /// •    Libellé Tâche T1/T2/T3        
        /// </para>
        /// <para>
        /// => Pour chaque ligne du fichier pour lesquelles au moins une valeur est remplie, vérifier que tous ces champs sont non vides.
        /// </para>
        /// <para>
        /// Si un ou plusieurs ligne(s) est(sont) en anomalie, ne pas effectuer le chargmenet des données et retourner la liste complète des anomalies trouvées, avec les messages suivants :
        /// « Rejet ligne n°# : champ(s) obligatoire(s) non renseigné(s). »
        /// </para>
        /// </summary>
        /// <param name="repriseExcelPlanTaches">ligne excel</param>
        /// <returns>Le resultat de la validation</returns>
        private PlanTachesImportRuleResult VerifyRequiredFieldsRule(RepriseExcelPlanTaches repriseExcelPlanTaches)
        {
            PlanTachesImportRuleResult result = new PlanTachesImportRuleResult();

            result.ImportRuleType = PlanTachesImportRuleType.RequiredField;

            bool codeSocieteIsValid = !repriseExcelPlanTaches.CodeSociete.IsNullOrEmpty();
            bool codeCiIsValid = !repriseExcelPlanTaches.CodeCi.IsNullOrEmpty();
            bool niveauTacheIsValid = !repriseExcelPlanTaches.NiveauTache.IsNullOrEmpty();
            bool codeTacheIsValid = !repriseExcelPlanTaches.CodeTache.IsNullOrEmpty();
            bool libelleTacheIsValid = !repriseExcelPlanTaches.LibelleTache.IsNullOrEmpty();

            result.IsValid = codeSocieteIsValid
                && codeCiIsValid
                && niveauTacheIsValid
                && codeTacheIsValid
                && libelleTacheIsValid;

            // BusinessResources.ImportCommandeRequiredFieldError affiche le meme message que nous pour le Plan de Tâches.
            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRequiredFieldError, repriseExcelPlanTaches.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPlanTaches.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Si autre valeur que T1, T2 ou T3 => rejet de ligne : "Rejet ligne n°# : Niveau T1/T2/T3 invalide."
        /// </summary>
        /// <param name="repriseExcelPlanTaches">ligne excel</param>
        /// <returns>Le resultat de la validation</returns>
        private PlanTachesImportRuleResult VerifyNiveauTacheRule(RepriseExcelPlanTaches repriseExcelPlanTaches)
        {
            PlanTachesImportRuleResult result = new PlanTachesImportRuleResult();

            result.ImportRuleType = PlanTachesImportRuleType.NiveauTacheInvalide;

            string niveauTache = repriseExcelPlanTaches.NiveauTache.Trim().ToUpper();

            result.IsValid = niveauTache == "T1" || niveauTache == "T2" || niveauTache == "T3";

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportPlanTachesErrorMessageNiveauTacheInvalide, repriseExcelPlanTaches.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Si "Code Tâche Parent" non vide alors que le niveau de la tâche en cours est "T1" => rejet de la ligne
        /// Si "Code Tâche Parent" vide alors que le niveau de la tâche en cours est "T2" ou "T3" => rejet de la ligne
        /// Rechercher la valeur "Code Tâche Parent" dans la table FRED_TACHE pour le même CiId de la ligne(cf.détermination du CiId plus haut) :
        /// Si tâche parent ID non trouvé pour le CI => rejet de la ligne
        /// Si trouvé, mais le niveau de la tâche parent n'est pas inférieur de 1 par rapport au niveau de la tâche en cours => rejet de la ligne
        /// : "Rejet ligne n°# : Code Tâche Parent invalide."
        /// </summary>
        /// <param name="repriseExcelPlanTaches">ligne excel</param>
        /// <param name="context">Contexte</param>
        /// <param name="listRepriseExcelPlanTaches">liste des lignes du excel</param>
        /// <returns>Le resultat de la validation</returns>
        private PlanTachesImportRuleResult VerifyCodeTacheParentRule(RepriseExcelPlanTaches repriseExcelPlanTaches, ContextForImportPlanTaches context, List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches)
        {
            PlanTachesImportRuleResult result = new PlanTachesImportRuleResult();

            result.ImportRuleType = PlanTachesImportRuleType.CodeTacheParentInvalide;

            string messageErreur = string.Format(BusinessResources.ImportPlanTachesErrorMessageCodeTacheParentInvalide, repriseExcelPlanTaches.NumeroDeLigne);

            result.IsValid = true;

            //1- Si "Code Tâche Parent" non vide alors que le niveau de la tâche en cours est "T1" => rejet de la ligne
            if (!repriseExcelPlanTaches.CodeTacheParent.IsNullOrEmpty() && repriseExcelPlanTaches.NiveauTache == "T1")
            {
                result.IsValid = false;
            }
            //2- Si "Code Tâche Parent" vide alors que le niveau de la tâche en cours est "T2" ou "T3" => rejet de la ligne
            else if (repriseExcelPlanTaches.CodeTacheParent.IsNullOrEmpty() && (repriseExcelPlanTaches.NiveauTache == "T2" || repriseExcelPlanTaches.NiveauTache == "T3"))
            {
                result.IsValid = false;
            }
            //3- Rechercher la valeur "Code Tâche Parent" dans la table FRED_TACHE pour le même CiId de la ligne (cf. détermination du CiId plus haut) :
            else if (!repriseExcelPlanTaches.CodeTacheParent.IsNullOrEmpty())
            {
                CheckCodeTacheParentValidInExcelAndDB(repriseExcelPlanTaches, context, listRepriseExcelPlanTaches, result);
            }

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            result.SetNumeroDeLigne(repriseExcelPlanTaches.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher d'abord la valeur "Code Société" dans la table FRED_SOCIETE pour le Groupe spécifié par l'utilisateur pour trouver le SocieteId, 
        /// puis rechercher ensuite la valeur "Code CI" dans la table FRED_CI pour le SocieteId trouvé précédemment.
        /// Non reconnu => rejet de la ligne
        /// : "Rejet ligne n°# : Code Ci invalide."
        /// </summary>
        /// <param name="repriseExcelPlanTaches">ligne excel</param>
        /// <param name="context">Contexte</param>
        /// <returns>Le resultat de la validation</returns>
        private PlanTachesImportRuleResult VerifyCodeCiRule(RepriseExcelPlanTaches repriseExcelPlanTaches, ContextForImportPlanTaches context)
        {
            PlanTachesImportRuleResult result = new PlanTachesImportRuleResult();

            result.ImportRuleType = PlanTachesImportRuleType.CodeCiInvalide;

            CommonFieldsValidator validator = new CommonFieldsValidator();

            // BusinessResources.ImportCIErrorMessageCodeCiInvalide affiche le meme message que nous pour le Plan de Tâches.
            string messageErreur = string.Format(BusinessResources.ImportCIErrorMessageCodeCiInvalide, repriseExcelPlanTaches.NumeroDeLigne);

            result.IsValid = validator.CodeCiIsValid(context.GroupeId, repriseExcelPlanTaches.CodeSociete, repriseExcelPlanTaches.CodeCi, context.OrganisationTree);

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            result.SetNumeroDeLigne(repriseExcelPlanTaches.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Tâche T1/T2/T3" dans la table FRED_TACHE pour le même CiId de la ligne (cf. détermination du CiId plus bas)
        /// Si non trouvée, copier à l'identique le champ "Code Tâche T1/T2/T3".
        /// Si trouvée => rejet de la ligne(chaque code tâche doit être unique à l'intérieur du CI).
        /// : "Rejet ligne n°# : Code Tâche T1/T2/T3 invalide."
        /// </summary>
        /// <param name="repriseExcelPlanTaches">ligne excel</param>
        /// <param name="context">Contexte</param>
        /// <param name="listRepriseExcelPlanTaches">Liste des lignes du Excel</param>
        /// <returns>Le resultat de la validation</returns>
        private PlanTachesImportRuleResult VerifyCodeTacheRule(RepriseExcelPlanTaches repriseExcelPlanTaches, ContextForImportPlanTaches context, List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches)
        {
            PlanTachesImportRuleResult result = new PlanTachesImportRuleResult();

            result.ImportRuleType = PlanTachesImportRuleType.CodeTacheInvalide;

            string messageErreur = string.Format(BusinessResources.ImportPlanTachesErrorMessageCodeTacheInvalide, repriseExcelPlanTaches.NumeroDeLigne);

            CheckCodeTacheValidInExcelAndDB(repriseExcelPlanTaches, context, listRepriseExcelPlanTaches, result);

            result.ErrorMessage = result.IsValid ? string.Empty : messageErreur;

            result.SetNumeroDeLigne(repriseExcelPlanTaches.NumeroDeLigne);

            return result;
        }
        #endregion

        #region Méthodes utilitaires

        private int GetCiIdByCodeCi(int groupeId, string codeSociete, string codeCi, OrganisationTree organisationTree)
        {
            int result = 0;

            List<OrganisationBase> societesOfGroupe = organisationTree.GetAllSocietesForGroupe(groupeId);

            OrganisationBase societe = societesOfGroupe.Find(s => s.Code == codeSociete);

            if (societe != null)
            {
                List<OrganisationBase> cisOfSociete = organisationTree.GetAllCisOfSociete(societe.Id);

                result = cisOfSociete.Where(x => x.Code == codeCi).Select(x => x.Id).FirstOrDefault();
            }

            return result;
        }

        private void CheckCodeTacheValidInExcelAndDB(RepriseExcelPlanTaches repriseExcelPlanTaches, ContextForImportPlanTaches context, List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches, PlanTachesImportRuleResult result)
        {
            // 1- On recherche la tache dans le fichier Excel
            if (listRepriseExcelPlanTaches.Count(x => x.CodeCi == repriseExcelPlanTaches.CodeCi
                                                    && x.CodeSociete == repriseExcelPlanTaches.CodeSociete
                                                    && x.CodeTache == repriseExcelPlanTaches.CodeTache)
                                                    > 1)
            {
                //  2- Si trouvé dans Excel, rejet de la ligne
                result.IsValid = false;
            }
            else
            {
                CheckCodeTacheValidInDB(repriseExcelPlanTaches, context, result);
            }
        }

        private void CheckCodeTacheValidInDB(RepriseExcelPlanTaches repriseExcelPlanTaches, ContextForImportPlanTaches context, PlanTachesImportRuleResult result)
        {
            // 1- Si non trouvé dans Excel, on le recherche en BDD
            int ciId = GetCiIdByCodeCi(context.GroupeId, repriseExcelPlanTaches.CodeSociete, repriseExcelPlanTaches.CodeCi, context.OrganisationTree);

            result.IsValid = !context.TachesUsedInExcel.Any(x => x.Code == repriseExcelPlanTaches.CodeTache
                                            && x.CiId == ciId);
        }

        private void CheckCodeTacheParentValidInExcelAndDB(RepriseExcelPlanTaches repriseExcelPlanTaches, ContextForImportPlanTaches context, List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches, PlanTachesImportRuleResult result)
        {
            int niveauTacheEnfant = 0;
            if (int.TryParse(repriseExcelPlanTaches.NiveauTache.Trim('T'), out niveauTacheEnfant))
            {
                // 3-I- On recherche le parent dans le fichier Excel
                RepriseExcelPlanTaches tacheParentExcel = listRepriseExcelPlanTaches.Find(x => x.CodeCi == repriseExcelPlanTaches.CodeCi
                                                            && x.CodeSociete == repriseExcelPlanTaches.CodeSociete
                                                            && x.CodeTache == repriseExcelPlanTaches.CodeTacheParent);
                if (tacheParentExcel != null)
                {
                    CheckCodeTacheParentValidInExcel(result, niveauTacheEnfant, tacheParentExcel);
                }
                else
                {
                    CheckCodeTacheParentValidInDB(repriseExcelPlanTaches, context, result, niveauTacheEnfant);
                }
            }
            else
            {
                result.IsValid = false;
            }
        }

        private void CheckCodeTacheParentValidInDB(RepriseExcelPlanTaches repriseExcelPlanTaches, ContextForImportPlanTaches context, PlanTachesImportRuleResult result, int niveauTacheEnfant)
        {
            // 3-I- Si non trouvé dans Excel, on recherche le parent en BDD
            int ciId = GetCiIdByCodeCi(context.GroupeId, repriseExcelPlanTaches.CodeSociete, repriseExcelPlanTaches.CodeCi, context.OrganisationTree);

            TacheEnt codeTacheParentFromBDD = context.TachesParentsUsedInExcel.Find(x => x.Code == repriseExcelPlanTaches.CodeTacheParent
                                            && x.CiId == ciId);

            //  b- Si trouvé, mais le niveau de la tâche parent n'est pas inférieur de 1 par rapport au niveau de la tâche en cours => rejet de la ligne
            if (codeTacheParentFromBDD != null)
            {
                if (codeTacheParentFromBDD.Niveau.HasValue
                    && (codeTacheParentFromBDD.Niveau.Value >= niveauTacheEnfant
                        || codeTacheParentFromBDD.Niveau.Value != (niveauTacheEnfant - 1)))
                {
                    result.IsValid = false;
                }
            }
            else
            {
                // Si non trouvé en BDD non plus on rejette la ligne
                result.IsValid = false;
            }
        }

        private void CheckCodeTacheParentValidInExcel(PlanTachesImportRuleResult result, int niveauTacheEnfant, RepriseExcelPlanTaches tacheParentExcel)
        {
            int niveauTacheParent = 0;

            //  b- Si trouvé, mais le niveau de la tâche parent n'est pas inférieur de 1 par rapport au niveau de la tâche en cours => rejet de la ligne
            if (int.TryParse(tacheParentExcel.NiveauTache.Trim('T'), out niveauTacheParent)
                && (niveauTacheParent >= niveauTacheEnfant
                    || niveauTacheParent != (niveauTacheEnfant - 1)))
            {
                result.IsValid = false;
            }
        }
        #endregion
    }
}
