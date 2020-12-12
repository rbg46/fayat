using System;
using System.Threading.Tasks;
using Fred.Entities.RapportPrime;
using Fred.Entities.ValidationPointage;
using Fred.Web.Shared.Models;

namespace Fred.DataAccess.ExternalService.FredImportExport.ValidationPointage
{
    public interface IValidationPointageRepositoryExterne : IExternalRepository
    {
        Task<ControlePointageResult> ExecuteControleVracAsync(int userId, int lotPointageId, PointageFiltre filtre);

        Task<RemonteeVracResult> ExecuteRemonteeVracAsync(int userId, DateTime periode, PointageFiltre filtre);

        Task<RapportPrimeLignePrimeEnt> ExecuteRemonteePrimeAsync(int utilisateurId, DateTime periode);
    }
}