using System;
using System.Linq;
using Fred.Business.Budget.Helpers;
using Fred.Entities.Budget;
using Fred.Framework.Security;
using CommonServiceLocator;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget.Extensions
{
    /// <summary>
    /// Classe d'extensions pour la manipulation d'un budget
    /// </summary>
    public static class BudgetEntExtension
    {
        /// <summary>
        /// Calcul la somme des T4 du budget donné
        /// </summary>
        /// <param name="budget">le budget dont on calcule la somme</param>
        /// <returns>Decimal contenant la somme des budgets T4</returns>
        public static decimal CalculMontantTotalT4(this BudgetEnt budget)
        {
            return budget.BudgetT4s.Where(b => b.MontantT4.HasValue).Sum(b => b.MontantT4.Value);
        }

        /// <summary>
        /// Renvoi le prenom et le nom du valideur du budget si le budget a été validé
        /// </summary>
        /// <param name="budget">budget dont on doit récupérer le valideur</param>
        /// <returns>Le Prenom et le nom du valideur, null si le budget n'a pas été validé</returns>
        public static string GetValideurBudget(this BudgetEnt budget)
        {
            //On regarde dans l'historique du budet si il a été validé
            var workflowValidation = budget.Workflows.FirstOrDefault(bw => bw.EtatCible.Code == EtatBudget.EnApplication);
            if (workflowValidation != null)
            {
                return workflowValidation.Auteur.PrenomNom;
            }
            return "--";
        }

        /// <summary>
        /// Renvoi la date de validation du budget si le budget a été validé
        /// </summary>
        /// <param name="budget">budget dont on doit récupérer la date de validation</param>
        /// <returns>La date de validation, null si le budget n'a pas été validé</returns>
        public static DateTime? GetDateValidation(this BudgetEnt budget)
        {
            //On regarde dans l'historique du budet si il a été validé
            var workflowValidation = budget.Workflows.FirstOrDefault(bw => bw.EtatCible.Code == EtatBudget.EnApplication);
            return workflowValidation?.Date;

        }


        /// <summary>
        /// Retourne la date de la dernière modification apportée sur ce budget
        /// </summary>
        /// <param name="budget">budget à analyser</param>
        /// <returns>Date time de la dernière modification</returns>
        public static DateTime? GetDateDerniereModification(this BudgetEnt budget)
        {
            //On regarde dans l'historique du budet si il a été validé
            var dernierWorkflow = budget.Workflows.OrderByDescending(w => w.Date).First();
            return dernierWorkflow.Date;
        }

        /// <summary>
        /// Retourne le prenom et le nom de l'auteur ayant apporté la dernière modification sur le budget
        /// </summary>
        /// <param name="budget">budget à analyser</param>
        /// <returns>Chaine de caractère au format Prenom Nom</returns>
        public static string GetAuteurDerniereModification(this BudgetEnt budget)
        {
            //On regarde dans l'historique du budet si il a été validé
            var dernierWorkflow = budget.Workflows.OrderByDescending(w => w.Date).First();
            return dernierWorkflow.Auteur?.PrenomNom;
        }

        /// <summary>
        /// Retourne la date de création du budget
        /// </summary>
        /// <param name="budget">Budget à analyser</param>
        /// <returns>DateTime contenant la date de création</returns>
        public static DateTime? GetDateCreation(this BudgetEnt budget)
        {
            //On regarde dans l'historique du budet si il a été validé
            var workflowValidation = budget.Workflows.OrderBy(w => w.Date).First();
            return workflowValidation.Date;
        }

        /// <summary>
        /// Retourne le prenom et nom du créateur du budget
        /// </summary>
        /// <param name="budget">budget à analyser</param>
        /// <returns>une chaine de caractère toujours valide</returns>
        public static string GetCreateur(this BudgetEnt budget)
        {
            //On regarde dans l'historique du budet si il a été validé
            var premierWorkflow = budget.Workflows.OrderBy(w => w.Date).FirstOrDefault();
            return premierWorkflow != null ? premierWorkflow.Auteur?.PrenomNom : string.Empty;
        }

        /// <summary>
        /// Retourne le dernier commentaire saisie sur le budget
        /// </summary>
        /// <param name="budget">budget dont on doit récupérer le commentaire</param>
        /// <returns>une chaine de caractère pouvant être null si aucun commentaire n'est saisie</returns>
        public static string GetDernierCommentaire(this BudgetEnt budget)
        {
            var dernierWorkflow = budget.Workflows.OrderByDescending(w => w.Date).FirstOrDefault();
            return dernierWorkflow != null ? dernierWorkflow.Commentaire : string.Empty;
        }

        /// <summary>
        /// Retourne la période du budget au format Mois (litéral) et année e.g Juin 2018
        /// </summary>
        /// <param name="budget">budget à analyser</param>
        /// <param name="periode">periode à analyser</param>
        /// <returns>chaine de caractère pouvant être vide si le budget n'a pas été validé</returns>
        public static string FormatPeriode(this BudgetEnt budget, int? periode)
        {
            return periode.HasValue
                ? PeriodeHelper.FormatCulture(periode.Value)
                : "--";
        }

        /// <summary>
        /// Retourne la somme des recettes de ce budget
        /// </summary>
        /// <param name="budget">budget à analyser</param>
        /// <returns>0 si il n'y a aucune recette, la somme sinon</returns>
        public static decimal GetSommeRecettes(this BudgetEnt budget)
        {
            if (budget.Recette == null)
            {
                return 0m;
            }

            var montantMarche = budget.Recette.MontantMarche ?? 0m;
            var montantAvenant = budget.Recette.MontantAvenants ?? 0m;
            var sommeAValoir = budget.Recette.SommeAValoir ?? 0m;
            var travauxSupplementaires = budget.Recette.TravauxSupplementaires ?? 0m;
            var revision = budget.Recette.Revision ?? 0m;
            var autreRecette = budget.Recette.AutresRecettes ?? 0m;
            var penaliteEtRetenues = budget.Recette.PenalitesEtRetenues ?? 0m;


            return montantMarche + montantAvenant + sommeAValoir + travauxSupplementaires + revision + autreRecette + penaliteEtRetenues;
        }

        /// <summary>
        /// Contrôle la recette de ce budget
        /// </summary>
        /// <param name="budget">budget à controler</param>
        /// <returns>Le message d'erreur, vide si pas d'erreur</returns>
        public static string CheckBudgetRecette(this BudgetEnt budget)
        {
            var erreur = string.Empty;
            if ((budget.Recette?.MontantMarche ?? 0) == 0)
            {
                erreur = string.Concat("La saisie du montant du marché dans la recette est obligatoire", Environment.NewLine);
            }
            return erreur;
        }

        /// <summary>
        /// La fonction renvoi true si l'utilisateur passé en paramètre a le droit de modifier l'état de partage du budget
        /// de partager ce budget
        /// </summary>
        /// <param name="budget">this, le budget dont on observe la partageabilité (je sais c'est pas un mot)</param>
        /// <returns>renvoi true si l'utilisateur a le droit de partager ce budget false sinon</returns>
        public static bool IsBudgetPartageableParUtilisateurConnecte(this BudgetEnt budget)
        {
            var premierWorkflow = budget.Workflows.OrderBy(w => w.Date).FirstOrDefault();

            var securityManager = ServiceLocator.Current.GetInstance<ISecurityManager>();

            if (securityManager == null)
            {
                return false;
            }

            var utilisateurId = securityManager.GetUtilisateurId();
            var utilisateurIsCreateur = premierWorkflow.AuteurId == utilisateurId;
            var budgetIsBrouillon = budget.BudgetEtat.Code == EtatBudget.Brouillon;
            return utilisateurIsCreateur && budgetIsBrouillon;
        }
    }
}
