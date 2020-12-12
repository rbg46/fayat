using System.Collections.Generic;

namespace Fred.Business.Budget.BudgetComparaison
{
    /// <summary>
    /// Interface du gestionnaire de la comparaison de budget.
    /// </summary>
    public interface IBudgetComparaisonManager : IManager
    {
        /// <summary>
        /// Recherche des révisions de budget d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI concerné.</param>
        /// <param name="page">L'index de la page.</param>
        /// <param name="pageSize">La taille d'un page.</param>
        /// <param name="recherche">Le texte recherché.</param>
        /// <returns>Les révisions de budget concernées.</returns>
        List<Dto.BudgetRevisionDto> SearchBudgetRevisions(int ciId, int page, int pageSize, string recherche);

        /// <summary>
        /// Compare des budgets.
        /// </summary>
        /// <param name="request">La requête de comparaison.</param>
        /// <returns>Le résultat de la comparaison.</returns>
        Dto.Comparaison.ResultDto Compare(Dto.Comparaison.RequestDto request);

        /// <summary>
        /// Exporte la comparaison au format Excel.
        /// </summary>
        /// <param name="request">La requête de l'export Excel.</param>
        /// <returns>Le résultat de l'export.</returns>
        Dto.ExcelExport.ResultDto ExcelExport(Dto.ExcelExport.RequestDto request);
    }
}
