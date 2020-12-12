
using Fred.Entities.Budget;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Référentiel de données pour les bibliothèques des prix.
    /// </summary>
    public interface IBudgetBibliothequePrixRepository : IRepository<BudgetBibliothequePrixEnt>
    {
        BudgetBibliothequePrixEnt GetBibliothequePrixByOrganisationIdAndDeviseId(int OrganisationId, int DeviseId);
    }
}
