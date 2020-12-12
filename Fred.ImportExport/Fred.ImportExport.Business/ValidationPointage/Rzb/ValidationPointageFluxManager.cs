using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.Web.Shared.Models;

namespace Fred.ImportExport.Business.ValidationPointage.Rzb
{
    public class ValidationPointageFluxManager : AbstractFluxManager, IValidationPointageFluxManagerRzb
    {
        private readonly ControleVrac controleVrac;
        private readonly RemonteeVrac remonteeVrac;
        private readonly RemonteePrimes remonteePrimes;

        public ValidationPointageFluxManager(IFluxManager fluxManager, ControleVrac controleVrac, RemonteeVrac remonteeVrac, RemonteePrimes remonteePrimes)
            : base(fluxManager)
        {
            this.controleVrac = controleVrac;
            this.remonteeVrac = remonteeVrac;
            this.remonteePrimes = remonteePrimes;
        }

        public ControlePointageResult ExecuteControleVrac(int utilisateurId, int lotPointageId, PointageFiltre filtre)
        {
            return controleVrac.Execute(utilisateurId, lotPointageId, filtre);
        }

        public RemonteeVracResult ExecuteRemonteeVrac(int utilisateurId, DateTime periode, PointageFiltre filtre)
        {
            return remonteeVrac.Execute(utilisateurId, periode, filtre);
        }

        public void ExecuteRemonteePrimes(DateTime periode, int utilisateurId)
        {
            remonteePrimes.Execute(periode, utilisateurId);
        }
    }
}
