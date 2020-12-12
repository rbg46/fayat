using System;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac.Interfaces;
using Fred.Web.Shared.Models;
using Hangfire;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac
{
    public class RemonteeVracFtp
    {
        private readonly IRemonteeVracProvider remonteeProvider;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IFluxManager fluxManager;
        private readonly IValidationPointageSettingsProvider validationPointageSettingsProvider;
        private readonly IRemonteeVracFtpHangFireJob remonteeVracFtpHangFireJob;

        public RemonteeVracFtp(IRemonteeVracProvider remonteeProvider,
            IRemonteeVracManager remonteeVracManager,
            IFluxManager fluxManager,
            IValidationPointageSettingsProvider validationPointageSettingsProvider,
            IRemonteeVracFtpHangFireJob remonteeVracFtpHangFireJob)
        {
            this.remonteeProvider = remonteeProvider;
            this.remonteeVracManager = remonteeVracManager;
            this.fluxManager = fluxManager;
            this.validationPointageSettingsProvider = validationPointageSettingsProvider;
            this.remonteeVracFtpHangFireJob = remonteeVracFtpHangFireJob;
        }

        public RemonteeVracResult Execute(int utilisateurId, DateTime periode, PointageFiltre filtre)
        {
            RemonteeVracEnt remonteeVrac = remonteeProvider.CreateRemonteeVrac(utilisateurId, periode);
            var setting = validationPointageSettingsProvider.GetFactorySetting(utilisateurId);
            var flux = fluxManager.GetByCode(setting.FluxCode);

            if (flux == null)
            {
                var exception = new FredBusinessException("Ce flux : " + setting.FluxCode + " n'a pas été trouvé en base de données.");
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "Flux inexistant.");
                throw exception;
            }

            if (flux.IsActif)
            {
                BackgroundJob.Enqueue(() => remonteeVracFtpHangFireJob.RemonteeVracJob(remonteeVrac.RemonteeVracId, periode, utilisateurId, filtre, null));
            }
            else
            {
                remonteeVrac.Statut = FluxStatus.Refused.ToIntValue();
                remonteeVrac.DateFin = DateTime.UtcNow;
                this.remonteeVracManager.UpdateRemonteeVrac(remonteeVrac);

                var exception = new FredBusinessException("Ce flux : " + setting.FluxCode + " n'est pas activé.");
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "Flux inactif.");
                throw exception;
            }

            return new RemonteeVracResult
            {
                DateDebut = DateTime.SpecifyKind(remonteeVrac.DateDebut, DateTimeKind.Utc),
                DateFin = (remonteeVrac.DateFin.HasValue) ? DateTime.SpecifyKind(remonteeVrac.DateFin.Value, DateTimeKind.Utc) : default(DateTime?),
                Periode = DateTime.SpecifyKind(remonteeVrac.Periode, DateTimeKind.Utc),
                Statut = remonteeVrac.Statut,
                RemonteeVracId = remonteeVrac.RemonteeVracId,
                AuteurCreationPrenomNom = remonteeVrac.AuteurCreation.PrenomNom
            };
        }
    }
}
