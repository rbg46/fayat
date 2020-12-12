using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac;
using Fred.Web.Shared.Models;

namespace Fred.ImportExport.Business.ValidationPointage.Fes
{
    public class ValidationPointageFluxManagerFes : AbstractFluxManager, IValidationPointageFluxManagerFes
    {
        private readonly ControleVracFes controleVrac;
        private readonly RemonteeVracFes remonteeVrac;

        public ValidationPointageFluxManagerFes(IFluxManager fluxManager, ControleVracFes controleVrac, RemonteeVracFes remonteeVrac)
            : base(fluxManager)
        {
            this.controleVrac = controleVrac;
            this.remonteeVrac = remonteeVrac;
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
            throw new NotImplementedException();
        }
    }
}
