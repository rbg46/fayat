using System;
using System.Threading.Tasks;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.ExternalService.Materiel
{
    /// <summary>
    /// Définit un manager de moyen externe
    /// </summary>
    public interface IMoyenManagerExterne : IManagerExterne
    {
        /// <summary>
        /// Export du pointage des moyens
        /// </summary>
        /// <param name="startDate">Date de début d'export</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Retour de l'export</returns>
        Task<EnvoiPointageMoyenResultModel> ExportPointageMoyenAsync(DateTime startDate, DateTime endDate);
    }
}
