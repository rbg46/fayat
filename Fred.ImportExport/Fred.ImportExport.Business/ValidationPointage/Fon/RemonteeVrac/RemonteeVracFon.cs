using System;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fon.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac.Interfaces;
using Fred.Web.Shared.Models;
using Hangfire;

namespace Fred.ImportExport.Business.ValidationPointage
{
    /// <summary>
    ///   Gestion de la Remontée Vrac :
    ///     - Création et Exécution de la requête lancant le programme AS400 de Remontée Vrac
    ///     - Récupération des erreurs de Remontée Vrac
    /// </summary>
    public class RemonteeVracFon
    {
        private readonly IRemonteeVracProvider remonteeProvider;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IFluxManager fluxManager;
        private readonly IValidationPointageSettingsProvider validationPointageSettingsProvider;
        private readonly IRemonteeVracFonHangFireJob remonteeVracFonHangFireJob;
        private readonly ValidationPointageFonLogger logger;

        public RemonteeVracFon(IRemonteeVracProvider remonteeProvider,
            IRemonteeVracManager remonteeVracManager,
            IFluxManager fluxManager,
            IValidationPointageSettingsProvider validationPointageSettingsProvider,
            IRemonteeVracFonHangFireJob remonteeVracFonHangFireJob,
            ValidationPointageFonLogger logger)
        {
            this.remonteeProvider = remonteeProvider;
            this.remonteeVracManager = remonteeVracManager;
            this.fluxManager = fluxManager;
            this.validationPointageSettingsProvider = validationPointageSettingsProvider;
            this.remonteeVracFonHangFireJob = remonteeVracFonHangFireJob;
            this.logger = logger;
        }

        public RemonteeVracResult Execute(int utilisateurId, DateTime periode, PointageFiltre filtre)
        {
            RemonteeVracEnt remonteeVrac = remonteeProvider.CreateRemonteeVrac(utilisateurId, periode);
            var setting = validationPointageSettingsProvider.GetFactorySetting(utilisateurId);
            var flux = fluxManager.GetByCode(setting.FluxCode);
            if (flux == null)
            {
                var errorMsg = logger.ErrorInExecuteRemonteeVracFluxNotFound(setting.FluxCode);

                throw new FredBusinessException(errorMsg);
            }

            if (flux.IsActif)
            {
                BackgroundJob.Enqueue(() => remonteeVracFonHangFireJob.RemonteeVracJob(remonteeVrac.RemonteeVracId, periode, utilisateurId, filtre, null));
            }
            else
            {
                remonteeVrac.Statut = FluxStatus.Refused.ToIntValue();
                remonteeVrac.DateFin = DateTime.UtcNow;

                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac);

                var errorMsg = logger.ErrorInExecuteRemonteeVracFluxInactif(flux.Code);

                throw new FredBusinessException(errorMsg);
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
