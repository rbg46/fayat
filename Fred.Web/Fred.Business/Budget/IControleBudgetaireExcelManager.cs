using Fred.Business.Budget.ControleBudgetaire.Models;
using Fred.Entities.Budget;
using System.Threading.Tasks;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Manager centré vers l'export excel du controle budgétaire
    /// </summary>
    public interface IControleBudgetaireExcelManager : IManager<ControleBudgetaireEnt>
    {
        /// <summary>
        /// Genère un model de données représenté par un tableau d'octets utilisable pour réaliser l'export excel 
        /// </summary>
        /// <param name="excelLoadModel">Données du controle budgétaire à exporter</param>
        /// <returns>Un tableau d'octets contenant les données</returns>
        Task<byte[]> GetExportExcelAsync(ControleBudgetaireExcelLoadModel excelLoadModel);

    }
}
