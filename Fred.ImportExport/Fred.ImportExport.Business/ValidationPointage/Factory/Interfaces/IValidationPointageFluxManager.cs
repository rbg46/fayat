using System;
using Fred.Entities.ValidationPointage;
using Fred.Web.Shared.Models;

namespace Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces
{
    public interface IValidationPointageFluxManager
    {
        ControlePointageResult ExecuteControleVrac(int utilisateurId, int lotPointageId, PointageFiltre filtre);

        RemonteeVracResult ExecuteRemonteeVrac(int utilisateurId, DateTime periode, PointageFiltre filtre);

        void ExecuteRemonteePrimes(DateTime periode, int utilisateurId);
    }
}
