using System;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;
using Fred.Web.Shared.Models;
using Hangfire;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac
{
    public class ControleVracFes
    {
        private readonly IControlePointageProvider controlePointageProvider;
        private readonly IControlePointageManager controlePointageManager;
        private readonly IValidationPointageSettingsProvider validationPointageSettingsProvider;
        private readonly ValidationPointageFesLogger logger;
        private readonly IControleVracFesHangFireJob controleVracFesHangFireJob;
        private readonly IFluxManager fluxManager;

        public ControleVracFes(IControlePointageProvider controlePointageProvider,
            IControlePointageManager controlePointageManager,
            IValidationPointageSettingsProvider validationPointageSettingsProvider,
            ValidationPointageFesLogger logger,
            IControleVracFesHangFireJob controleVracFesHangFireJob,
            IFluxManager fluxManager)
        {
            this.controlePointageProvider = controlePointageProvider;
            this.controlePointageManager = controlePointageManager;
            this.validationPointageSettingsProvider = validationPointageSettingsProvider;
            this.logger = logger;
            this.controleVracFesHangFireJob = controleVracFesHangFireJob;
            this.fluxManager = fluxManager;
        }

        public ControlePointageResult Execute(int utilisateurId, int lotPointageId, PointageFiltre filtre)
        {
            /******** 1 - Création du contrôle pointage (Récupération du lot de pointage, des lignes de pointages) ********/
            ControlePointageEnt ctrlPointage = controlePointageProvider.CreateControlePointage(utilisateurId, lotPointageId, TypeControlePointage.ControleVrac.ToIntValue());
            var setting = validationPointageSettingsProvider.GetFactorySetting(utilisateurId);
            var flux = fluxManager.GetByCode(setting.FluxCode);

            if (flux == null)
            {
                var errorMsg = logger.ErrorInExecuteJobFluxNotFound(setting.FluxCode);

                throw new FredBusinessException(errorMsg);
            }

            if (flux.IsActif)
            {
                BackgroundJob.Enqueue(() => controleVracFesHangFireJob.ControleVracJob(ctrlPointage.ControlePointageId, lotPointageId, utilisateurId, filtre, null));
            }
            else
            {
                ctrlPointage.Statut = FluxStatus.Refused.ToIntValue();

                controlePointageManager.UpdateControlePointage(ctrlPointage);

                var errorMsg = logger.ErrorInExecuteJobFluxInactif(flux.Code);

                throw new FredBusinessException(errorMsg);
            }

            return new ControlePointageResult
            {
                DateDebut = DateTime.SpecifyKind(ctrlPointage.DateDebut, DateTimeKind.Utc),
                DateFin = (ctrlPointage.DateFin.HasValue) ? DateTime.SpecifyKind(ctrlPointage.DateFin.Value, DateTimeKind.Utc) : default(DateTime?),
                Statut = ctrlPointage.Statut,
                TypeControle = ctrlPointage.TypeControle,
                LotPointageId = ctrlPointage.LotPointageId,
                AuteurCreationPrenomNom = ctrlPointage.AuteurCreation.PrenomNom
            };
        }
    }
}
