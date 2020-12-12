using Fred.Business.Budget.BudgetComparaison.Dto.ExcelExport;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Interface de l'export en Excel d'une comparaison de budget.
    /// </summary>
    public interface IBudgetComparaisonExcelExporter
    {
        /// <summary>
        /// Exporte la comparaison de budget.
        /// </summary>
        /// <param name="request">La requête de l'export Excel.</param>
        /// <returns>Le résultat de l'export.</returns>
        ResultDto Export(RequestDto request);
    }
}
