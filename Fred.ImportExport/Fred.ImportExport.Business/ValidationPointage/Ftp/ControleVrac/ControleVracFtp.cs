using System;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Utilisateur;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces;
using Fred.Web.Shared.Models;
using Hangfire;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac
{
    public class ControleVracFtp
    {
        private readonly IControlePointageProvider controleProvider;
        private readonly IFluxManager fluxManager;
        private readonly IValidationPointageSettingsProvider validationPointageSettingsProvider;
        private readonly IControleVracFtpHangFireJob controleVracFtpHangFireJob;
        private readonly IControlePointageManager controlePointageManager;

        public ControleVracFtp(IControlePointageProvider controleProvider,
            IFluxManager fluxManager,
            IValidationPointageSettingsProvider validationPointageSettingsProvider,
            IControleVracFtpHangFireJob controleVracFtpHangFireJob,
            IControlePointageManager controlePointageManager)
        {
            this.controleProvider = controleProvider;
            this.fluxManager = fluxManager;
            this.validationPointageSettingsProvider = validationPointageSettingsProvider;
            this.controleVracFtpHangFireJob = controleVracFtpHangFireJob;
            this.controlePointageManager = controlePointageManager;
        }

        public ControlePointageResult Execute(int utilisateurId, int lotPointageId, PointageFiltre filtre)
        {
            /******** 1 - Création du contrôle pointage (Récupération du lot de pointage, des lignes de pointages) ********/
            ControlePointageEnt controlePointageEnt = controleProvider.CreateControlePointage(utilisateurId, lotPointageId, TypeControlePointage.ControleVrac.ToIntValue());
            UtilisateurEnt auteurCreation = controlePointageEnt.AuteurCreation;
            controlePointageEnt.AuteurCreation = null;

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
                BackgroundJob.Enqueue(() => controleVracFtpHangFireJob.ControleVracJob(controlePointageEnt, utilisateurId, filtre, null));
            }
            else
            {
                controlePointageEnt.Statut = FluxStatus.Refused.ToIntValue();
                controlePointageManager.UpdateControlePointage(controlePointageEnt);

                var exception = new FredBusinessException("Ce flux : " + setting.FluxCode + " n'est pas activé.");
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "Flux inactif.");
                throw exception;
            }

            return new ControlePointageResult
            {
                DateDebut = DateTime.SpecifyKind(controlePointageEnt.DateDebut, DateTimeKind.Utc),
                DateFin = (controlePointageEnt.DateFin.HasValue) ? DateTime.SpecifyKind(controlePointageEnt.DateFin.Value, DateTimeKind.Utc) : default(DateTime?),
                Statut = controlePointageEnt.Statut,
                TypeControle = controlePointageEnt.TypeControle,
                LotPointageId = controlePointageEnt.LotPointageId,
                AuteurCreationPrenomNom = auteurCreation.PrenomNom
            };
        }
    }
}
