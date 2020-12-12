using Fred.Web.Shared.Models.Budget.Details;

namespace Fred.Business.Budget.Details
{
    /// <summary>
    /// Interface détaillant la feature de l'export excel
    /// </summary>
    public interface IBudgetDetailsExportExcelFeature
    {
        /// <summary>
        /// Renvoi un export excel en utilisant la classe BudgetDetailExportExcelFeature
        /// </summary>
        /// <param name="model">BudgetDetailsExportExcelLoadModel</param>
        /// <returns>les données de l'excel sous forme de tableau d'octets</returns>
        byte[] GetBudgetDetailExportExcel(BudgetDetailsExportExcelLoadModel model);
    }
}
