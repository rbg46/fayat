using System;
using System.Threading.Tasks;
using Fred.Entities.RapportPrime;
using Fred.Entities.ValidationPointage;
using Fred.Web.Shared.Models;

namespace Fred.Business.ExternalService.ValidationPointage
{
    public interface IValidationPointageManagerExterne : IManagerExterne
    {
        Task<ControlePointageResult> ExecuteControleVracAsync(int lotPointageId, PointageFiltre filtre);

        Task<RemonteeVracResult> ExecuteRemonteeVracAsync(DateTime periode, PointageFiltre filtre);

        Task<RapportPrimeLignePrimeEnt> ExecuteRemonteePrimeAsync(DateTime periode);
    }
}
