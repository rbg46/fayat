using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac;
using Fred.Web.Shared.Models;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp
{
    public class ValidationPointageFluxManagerFtp : AbstractFluxManager, IValidationPointageFluxManagerFtp
    {
        private readonly ControleVracFtp controleVrac;
        private readonly RemonteeVracFtp remonteeVrac;

        public ValidationPointageFluxManagerFtp(IFluxManager fluxManager, ControleVracFtp controleVrac, RemonteeVracFtp remonteeVrac)
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
