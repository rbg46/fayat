using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.Societe;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.CI.AnaelSystem;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Societe.Input;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Entities.ImportExport;
using NLog;
using ImportResult = Fred.ImportExport.Business.CI.AnaelSystem.Context.Common.ImportResult;

namespace Fred.ImportExport.Business.CI
{
    public class CiFluxAnaelSystemService : ICiFluxAnaelSystemService
    {
        private readonly IImportCiAnaelSystemManager importCiAnaelSystemManager;
        private readonly ISocieteManager societeManager;
        private readonly IFluxManager fluxManager;

        public CiFluxAnaelSystemService(
            IImportCiAnaelSystemManager importCiAnaelSystemManager,
            ISocieteManager societeManager,
            IFluxManager fluxManager)
        {
            this.importCiAnaelSystemManager = importCiAnaelSystemManager;
            this.societeManager = societeManager;
            this.fluxManager = fluxManager;
        }

        public async Task ImportationByCodeFluxAsync(string codeFlux, bool bypassDate)
        {
            FluxEnt flux = fluxManager.GetByCode(codeFlux);

            EnsureJobCanBeExecuted(flux, codeFlux);

            string errorMessage = await ImportationBySocietesAsync(flux, bypassDate);
            if (!errorMessage.IsNullOrEmpty())
            {
                var exception = new FredBusinessException(errorMessage);
                LogManager.GetCurrentClassLogger().Error(exception, errorMessage);
                throw exception;
            }

            UpdateDateExecution(flux);
        }

        private void EnsureJobCanBeExecuted(FluxEnt flux, string codeFlux)
        {
            if (flux == null)
            {
                throw new FredBusinessException(string.Format(IEBusiness.FluxInconnu, codeFlux));
            }
            if (!flux.IsActif)
            {
                throw new FredBusinessException(string.Format(IEBusiness.FluxInactif, codeFlux));
            }
        }

        private async Task<string> ImportationBySocietesAsync(FluxEnt flux, bool bypassDate)
        {
            var errorMessage = new StringBuilder();

            IEnumerable<string> codeSocietesComptable = GetCodeSocietesComptablesInFlux(flux);
            foreach (string codeSocieteComptable in codeSocietesComptable)
            {
                SocieteEnt societe = societeManager.GetSocieteByCodeSocieteComptable(codeSocieteComptable);

                string importationError = await ImportationBySocieteAsync(societe, codeSocieteComptable, bypassDate, flux.DateDerniereExecution);
                if (!importationError.IsNullOrEmpty())
                {
                    errorMessage.AppendLine(importationError);
                }

            }

            return errorMessage.ToString();
        }

        private IEnumerable<string> GetCodeSocietesComptablesInFlux(FluxEnt flux)
        {
            if (flux == null || string.IsNullOrEmpty(flux.SocieteCode))
            {
                return new List<string>();
            }

            return flux.SocieteCode.Split(',');
        }

        private async Task<string> ImportationBySocieteAsync(SocieteEnt societe, string codeSocieteCompatbleRequested, bool bypassDate, DateTime? dateDerniereExecution)
        {
            string error = string.Empty;
            LogManager.GetCurrentClassLogger().Info($"[IMPORT-EXPORT][IMPORT-CI-BY-SOCIETE] - Importation de la sociéte {societe?.Code} {societe?.Libelle} ({societe?.CodeSocieteComptable}).");
            try
            {
                var importCisBySocieteInputs = new ImportCisBySocieteInputs
                {
                    DateDerniereExecution = dateDerniereExecution,
                    IsFullImport = bypassDate,
                    Societe = societe,
                    CodeSocieteCompatble = codeSocieteCompatbleRequested
                };

                ImportResult importResult = await importCiAnaelSystemManager.ImportCiBySocieteAsync(importCisBySocieteInputs);

                importResult.ErrorMessages.ForEach(e => error += e);
            }
            catch (Exception ex)
            {
                error = $"[IMPORT-EXPORT][IMPORT-CI-BY-SOCIETE] {ex} {ex.InnerException}";
            }

            return error;
        }

        private void UpdateDateExecution(FluxEnt flux)
        {
            // oui c'est possible : lorsque l'on change va valeur semantique de codeflux 
            // (avant on mettait le codeSocieteComptable maintenant on met un codeflux)
            if (flux == null)
                return;

            flux.DateDerniereExecution = DateTime.UtcNow;
            fluxManager.Update(flux);
        }
    }
}
