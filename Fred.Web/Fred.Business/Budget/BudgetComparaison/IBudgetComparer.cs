using Fred.Business.Budget.BudgetComparaison.Dto.Comparaison;

namespace Fred.Business.Budget.BudgetComparaison
{
    /// <summary>
    /// Interface de la comparaison de budget.
    /// </summary>
    public interface IBudgetComparer
    {
        /// <summary>
        /// Compare les budgets.
        /// </summary>
        /// <param name="request">La requête de comparaison.</param>
        /// <returns>Le résultat de la comparaison.</returns>
        ResultDto Compare(RequestDto request);
    }
}
