using System.Collections.Generic;
using Fred.Business.Budget.BudgetComparaison.Dto.Comparaison.Result;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget.BudgetComparaison.Dto.Comparaison
{
    /// <summary>
    /// Représente le résultat de la comparaison de budget.
    /// </summary>
    public class ResultDto : ErreurResultModel
    {
        /// <summary>
        /// Information pour l'utilisateur.
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// L'arbre de comparaison.
        /// </summary>
        public TreeDto Tree { get; set; } = new TreeDto();

        /// <summary>
        /// La devise des budgets à comparer.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// La liste des unités utilisées.
        /// </summary>
        public List<UniteDto> Unites { get; set; }

        /// <summary>
        /// La liste des devises disponibles.
        /// </summary>
        public List<DeviseDto> Devises { get; set; }

        /// <summary>
        /// L'écart total entre les budgets.
        /// </summary>
        public GroupDto EcartTotal { get; set; } = new GroupDto();
    }
}
