using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Common.Validation;
using Fred.Business.RepriseDonnees.Personnel.Models;
using Fred.Business.RepriseDonnees.Personnel.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Personnel.Validators
{
    /// <summary>
    /// Permet de valider les RG de l'import des Personnels
    /// </summary>
    public class PersonnelValidatorService : IPersonnelValidatorService
    {
        /// <summary>
        /// Verifie les regles d'import des Personnels
        /// </summary>
        /// <param name="listRepriseExcelPersonnel">les Personnels venu du fichier excel</param>
        /// <param name="context">Les données necessaires a la verification</param>
        /// <returns>PersonnelImportRulesResult</returns>
        public PersonnelImportRulesResult VerifyImportRules(List<RepriseExcelPersonnel> listRepriseExcelPersonnel, ContextForImportPersonnel context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            PersonnelImportRulesResult result = new PersonnelImportRulesResult();

            foreach (RepriseExcelPersonnel repriseExcelPersonnel in listRepriseExcelPersonnel)
            {
                PersonnelImportRuleResult requiredFieldsResult = VerifyRequiredFieldsRule(repriseExcelPersonnel);

                PersonnelImportRuleResult codeSocieteResult = VerifyCodeSocieteRule(repriseExcelPersonnel, context);

                PersonnelImportRuleResult typePersonnelResult = VerifyTypePersonnelRule(repriseExcelPersonnel);

                PersonnelImportRuleResult emailResult = VerifyEmailRule(repriseExcelPersonnel, context, listRepriseExcelPersonnel);

                PersonnelImportRuleResult dateEntreeResult = VerifyDateEntreeRule(repriseExcelPersonnel);

                PersonnelImportRuleResult codeRessourceResult = VerifyCodeRessourceRule(repriseExcelPersonnel, context);

                PersonnelImportRuleResult matriculeResult = VerifyMatriculeRule(repriseExcelPersonnel, context, listRepriseExcelPersonnel);

                if (!repriseExcelPersonnel.DateSortie.IsNullOrEmpty())
                {
                    PersonnelImportRuleResult dateSortieResult = VerifyDateSortieRule(repriseExcelPersonnel);

                    result.ImportRuleResults.Add(dateSortieResult);
                }

                if (!repriseExcelPersonnel.CodePays.IsNullOrEmpty())
                {
                    PersonnelImportRuleResult codePaysResult = VerifyCodePaysRule(repriseExcelPersonnel, context);

                    result.ImportRuleResults.Add(codePaysResult);
                }

                result.ImportRuleResults.Add(requiredFieldsResult);

                result.ImportRuleResults.Add(codeSocieteResult);

                result.ImportRuleResults.Add(typePersonnelResult);

                result.ImportRuleResults.Add(emailResult);

                result.ImportRuleResults.Add(dateEntreeResult);

                result.ImportRuleResults.Add(codeRessourceResult);

                result.ImportRuleResults.Add(matriculeResult);
            }

            return result;
        }

        /// <summary>
        /// Pas de RG dans la SFD mais Carine demande à ce que l'on check l'Email pour voir s'il existe déjà en BDD, si oui on rejette la ligne
        /// Rejet ligne n°# : l'Email existe déjà pour cette société.
        /// </summary>
        /// <param name="repriseExcelPersonnel">Liste des lignes du</param>
        /// <param name="context">Le contexte</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyEmailRule(RepriseExcelPersonnel repriseExcelPersonnel, ContextForImportPersonnel context, List<RepriseExcelPersonnel> listRepriseExcelPersonnel)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.EmailInvalide;

            result.IsValid = true;

            if (!repriseExcelPersonnel.Email.IsNullOrEmpty())
            {
                // Lève une erreur si il y a un doublons soit en BDD soit dans le fichier Excel
                result.IsValid = !context.MailUsedInExcel.Any(x => string.Equals(x, repriseExcelPersonnel.Email, StringComparison.OrdinalIgnoreCase))
                            && (listRepriseExcelPersonnel.Count(x => string.Equals(x.Email, repriseExcelPersonnel.Email, StringComparison.OrdinalIgnoreCase)) == 1);
            }

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportPersonnelErrorMessageEmailInvalide, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// RG_5415_003 : Vérification de l'unicité du matricule par société
        /// Vérifier que :
        /// - l'ensemble des couples (sociétés, matricule) du fichier d'entrée n'existent pas déjà dans la base FRED
        /// - les mêmes couples (sociétés, matricule) ne sont pas présents plus d'une fois à l'intérieur du fichier
        /// NB - Récupérer la société à partir du "Code Societé Rattachement" et du Groupe choisi par l'utilisateur
        /// Rejet ligne n°# : le matricule existe déjà pour cette société.
        /// </summary>
        /// <param name="repriseExcelPersonnel">Ligne Excel</param>
        /// <param name="context">Le contexte</param>
        /// <param name="listRepriseExcelPersonnel">Liste des lignes du Excel</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyMatriculeRule(RepriseExcelPersonnel repriseExcelPersonnel, ContextForImportPersonnel context, List<RepriseExcelPersonnel> listRepriseExcelPersonnel)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.MatriculeInvalide;

            //1- Check si le couple Matricule + CodeSociete existe déjà en BDD
            result.IsValid = !context.PersonnelsUsedInExcel.Any(x => x.Matricule == repriseExcelPersonnel.Matricule
                                                                        && x.Societe.Code == repriseExcelPersonnel.CodeSociete);

            if (result.IsValid)
            {
                //2- S'il n'existe pas déjà en BDD, on check qu'il existe pas déjà dans notre fichier excel
                result.IsValid = listRepriseExcelPersonnel.Count(x => x.Matricule == repriseExcelPersonnel.Matricule
                                                                        && x.CodeSociete == repriseExcelPersonnel.CodeSociete)
                                                                        <= 1;
            }

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportPersonnelErrorMessageMatriculeNonUnique, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Ressource" dans la table FRED_RESSOURCE, parmi les ressources associées au Groupe choisi par l'utilisateur.
        /// Si non reconnu => Rejet ligne n°# : Code Ressource invalide.
        /// </summary>
        /// <param name="repriseExcelPersonnel">Ligne Excel</param>
        /// <param name="context">Le contexte</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyCodeRessourceRule(RepriseExcelPersonnel repriseExcelPersonnel, ContextForImportPersonnel context)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.CodeRessourceInvalide;

            result.IsValid = context.RessourcesUsedInExcel.Any(x => string.Equals(x.Code, repriseExcelPersonnel.CodeRessource, StringComparison.OrdinalIgnoreCase));

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRessourceForCommandeLigneInvalid, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Recherche le "Code Pays" dans la table FRED_PAYS
        /// Si non trouvé => Rejet ligne n°# : Code Pays invalide.
        /// Rejet ligne n°# : Code Pays invalide
        /// </summary>
        /// <param name="repriseExcelPersonnel">Ligne Excel</param>
        /// <param name="context">Le contexte</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyCodePaysRule(RepriseExcelPersonnel repriseExcelPersonnel, ContextForImportPersonnel context)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.CodePaysInvalide;

            result.IsValid = context.PaysUsedInExcel.Any(x => x.Code == repriseExcelPersonnel.CodePays);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportPersonnelErrorMessageCodePaysInvalide, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Si le format de la date est invalide on rejette la ligne
        /// Rejet ligne n°# : Date Sortie invalide
        /// </summary>
        /// <param name="repriseExcelPersonnel">Ligne Excel</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyDateSortieRule(RepriseExcelPersonnel repriseExcelPersonnel)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.DateSortieInvalide;

            CommonFieldsValidator validator = new CommonFieldsValidator();

            result.IsValid = validator.DateIsValid(repriseExcelPersonnel.DateSortie);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportPersonnelErrorMessageDateSortieInvalide, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Rechercher la valeur "Code Société" dans la table FRED_SOCIETE pour le Groupe choisi par l'utilisateur
        /// Rejet ligne n°# : Code Société invalide
        /// </summary>
        /// <param name="repriseExcelPersonnel">Ligne Excel</param>
        /// <param name="context">Contexte</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyCodeSocieteRule(RepriseExcelPersonnel repriseExcelPersonnel, ContextForImportPersonnel context)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.SocieteNotInGroupe;

            CommonFieldsValidator validator = new CommonFieldsValidator();

            result.IsValid = validator.CodeSocieteIsValid(context.GroupeId, repriseExcelPersonnel.CodeSociete, context.OrganisationTree);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCIErrorMessageCodeSocieteInvalide, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Si le format de la date est invalide on rejette la ligne
        /// Rejet ligne n°# : Date Entrée invalide
        /// </summary>
        /// <param name="repriseExcelPersonnel">Ligne Excel</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyDateEntreeRule(RepriseExcelPersonnel repriseExcelPersonnel)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.DateEntreeInvalide;

            CommonFieldsValidator validator = new CommonFieldsValidator();

            result.IsValid = validator.DateIsValid(repriseExcelPersonnel.DateEntree);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportPersonnelErrorMessageDateEntreeInvalide, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Si Type Personnel différent de Interne, Externe ou Intérimaire on rejette la ligne
        /// Rejet ligne n°# : Type Personnel invalide.
        /// </summary>
        /// <param name="repriseExcelPersonnel">Ligne excel</param>
        /// <returns>Résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyTypePersonnelRule(RepriseExcelPersonnel repriseExcelPersonnel)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.TypePersonnelInvalide;

            string typePersonnel = repriseExcelPersonnel.TypePersonnel.Trim().ToLower();

            result.IsValid = typePersonnel == "externe" || typePersonnel == "interne" || typePersonnel == "intérimaire" || typePersonnel == "interimaire";

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportPersonnelErrorMessageTypePersonnelInvalide, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }

        /// <summary>
        /// Verification de la RG RG_5415_003
        /// <para>
        /// RG_5415_003 : Vérification des champs obligatoires
        ///        Les informations suivantes sont obligatoires :
        /// •    Code Société Rattachement
        /// •    Matricule
        /// •    Type Personnel
        /// •    Nom
        /// •    Prénom
        /// •    Date Entrée
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
        /// <param name="repriseExcelPersonnel">Ligne excel</param>
        /// <returns>Le résultat de la validation</returns>
        private PersonnelImportRuleResult VerifyRequiredFieldsRule(RepriseExcelPersonnel repriseExcelPersonnel)
        {
            PersonnelImportRuleResult result = new PersonnelImportRuleResult();

            result.ImportRuleType = PersonnelImportRuleType.RequiredField;

            bool codeSocieteIsValid = !repriseExcelPersonnel.CodeSociete.IsNullOrEmpty();
            bool matriculeIsValid = !repriseExcelPersonnel.Matricule.IsNullOrEmpty();
            bool typePersonnelIsValid = !repriseExcelPersonnel.TypePersonnel.IsNullOrEmpty();
            bool nomIsValid = !repriseExcelPersonnel.Nom.IsNullOrEmpty();
            bool prenomIsValid = !repriseExcelPersonnel.Prenom.IsNullOrEmpty();
            bool dateEntreeIsValid = !repriseExcelPersonnel.DateEntree.IsNullOrEmpty();
            bool codeRessourceIsValid = !repriseExcelPersonnel.CodeRessource.IsNullOrEmpty();

            result.IsValid = codeSocieteIsValid
                && matriculeIsValid
                && typePersonnelIsValid
                && nomIsValid
                && prenomIsValid
                && dateEntreeIsValid
                && codeRessourceIsValid;

            // BusinessResources.ImportCommandeRequiredFieldError affiche le meme message que nous pour les Personnels.
            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeRequiredFieldError, repriseExcelPersonnel.NumeroDeLigne);

            result.SetNumeroDeLigne(repriseExcelPersonnel.NumeroDeLigne);

            return result;
        }
    }
}
