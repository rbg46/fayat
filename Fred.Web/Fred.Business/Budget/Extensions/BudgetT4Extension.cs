using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Entities.Budget;

namespace Fred.Business.Budget.Extensions
{
    /// <summary>
    /// Classe d'extension offrant des méthodes manipulant des listes de T4 ou un T4 unitaire
    /// </summary>
    public static class BudgetT4Extension
    {

#pragma warning disable S3776
        /// <summary>
        /// Vérifie les t4 donnés.
        /// </summary>
        /// <param name="budgetT4s">Une liste de budget T4</param>
        /// <param name="isTypeAvancementDynamiqueSurSociete">True si la société contenant le CI contenant le Budget contenant les T4 testés, est paramétrée pour supporter le type d'avancement dynamique</param>
        /// <returns>Le message d'erreur</returns>
        public static string CheckBudgetT4OfBudget(this IEnumerable<BudgetT4Ent> budgetT4s, bool isTypeAvancementDynamiqueSurSociete)
        {
            string erreur = string.Empty;
            if (budgetT4s == null || (budgetT4s != null && !budgetT4s.Any()))
            {
                erreur = string.Concat(erreur, "Un budget doit au moins contenir un T4 saisi." + Environment.NewLine);
            }
            else
            {
                foreach (var t4 in budgetT4s)
                {
                    //Une tache T4 (BudgetT4Ent) révisée contient une Tache (TacheEnt) spécifique qui sera liée au budget
                    //Dont on a fait la révision. On ne vérifie PAS la conformité de ces tâches étant données qu'elles sont générées
                    //par le code et pas par l'utilisateur.
                    if (t4.T4.BudgetId != null)
                    {
                        continue;
                    }


                    if (!t4.MontantT4.HasValue || t4.MontantT4.Value < 0)
                    {
                        erreur = string.Concat(erreur, t4.T4.Code, " : Le montant du T4 doit être supérieur ou égal à zéro." + Environment.NewLine);
                    }
                    if (!t4.PU.HasValue || t4.PU.Value < 0)
                    {
                        erreur = string.Concat(erreur, t4.T4.Code, " : Le prix unitaire doit être supérieur ou égal à zéro." + Environment.NewLine);
                    }
                    if (!t4.QuantiteARealiser.HasValue || t4.QuantiteARealiser.Value < 0)
                    {
                        erreur = string.Concat(erreur, t4.T4.Code, " : La quantité à réaliser doit être supérieur ou égal à zéro." + Environment.NewLine);
                    }
                    if ((!t4.TypeAvancement.HasValue || t4.TypeAvancement.Value == (int)TypeAvancementBudget.Aucun) && !isTypeAvancementDynamiqueSurSociete)
                    {
                        //Si le type de l'avancement est dynamique sur le CI alors il ne faut pas le tester car sa valeur pourra changer lors de la saisie de l'avancement
                        //cela n'a donc rien a voir avec la validité du BudgetT4
                        erreur = string.Concat(erreur, t4.T4.Code, " : Le type d'avancement est obligatoire." + Environment.NewLine);
                    }
                    if (!t4.UniteId.HasValue)
                    {
                        erreur = string.Concat(erreur, t4.T4.Code, " : L'unité est obligatoire." + Environment.NewLine);
                    }
                    if (t4.BudgetSousDetails.Count == 0)
                    {
                        erreur = string.Concat(erreur, t4.T4.Code, " : Une tâche de niveau 4 doit être constitué d'au moins d'une ressource." + Environment.NewLine);
                    }
                    foreach (var sd in t4.BudgetSousDetails)
                    {
                        if (!sd.Montant.HasValue || sd.Montant < 0)
                        {
                            erreur = string.Concat(erreur, "   ", sd.Ressource.Code, " : Le montant d'une ressource doit être supérieur ou égal à zéro." + Environment.NewLine);
                        }
                        if (sd.UniteId == null)
                        {
                            erreur = string.Concat(erreur, "   ", sd.Ressource.Code, " : L'unité est obligatoire." + Environment.NewLine);
                        }
                    }
                }
            }
            
            return erreur;
        }
#pragma warning restore S3776
    }
}
