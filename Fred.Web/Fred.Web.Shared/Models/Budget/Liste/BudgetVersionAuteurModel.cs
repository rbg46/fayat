using System;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Budget;

namespace Fred.Web.Shared.Models.Budget.Liste
{
    /// <summary>
    /// Model du budget ne contannt que le numéro de version et le nom et le prénom de l'auteur
    /// </summary>
    public class BudgetVersionAuteurModel
    {

        public string Version { get; set; }

        public string PrenomAuteur { get; set; }

        public string NomAuteur { get; set; }

        public int BudgetId { get; set; }


        /// <summary>
        /// Selector permettant de constuire ce modèle a partir d'un chapitre.
        /// </summary>
        public static Expression<Func<BudgetEnt, BudgetVersionAuteurModel>> Selector
        {
            get
            {
                return budget => new BudgetVersionAuteurModel
                {
                    Version = budget.Version,
                    PrenomAuteur = budget.Workflows.OrderByDescending(w => w.Date).Select(w => w.Auteur.Personnel.Prenom).FirstOrDefault(),
                    NomAuteur = budget.Workflows.OrderByDescending(w => w.Date).Select(w => w.Auteur.Personnel.Nom).FirstOrDefault(),
                    BudgetId = budget.BudgetId
                };
            }
        }
    }
}
