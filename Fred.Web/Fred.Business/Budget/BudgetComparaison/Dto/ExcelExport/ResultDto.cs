using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget.BudgetComparaison.Dto.ExcelExport
{
    /// <summary>
    /// Représente le résultat de l'export Excel.
    /// </summary>
    public class ResultDto : ErreurResultModel
    {
        /// <summary>
        /// Les données représentant le fichier Excel.
        /// </summary>
        public byte[] Data { get; set; }
    }
}
