using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Personnel.AnaelSystem;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Personnel.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Log;

namespace Fred.ImportExport.Business.Personnel
{
    public class PersonnelFluxAnaelSystemService : IPersonnelFluxAnaelSystemService
    {
        private readonly IImportPersonnelAnaelSystemManager importPersonnelManager;
        private readonly IImportExportLoggingService loggingService;
        private readonly ISocieteManager societeManager;
        private readonly IFluxManager fluxManager;
        private readonly IPaysManager paysManager;

        public PersonnelFluxAnaelSystemService(
            IImportPersonnelAnaelSystemManager importPersonnelManager,
            IImportExportLoggingService loggingService,
            ISocieteManager societeManager,
            IFluxManager fluxManager,
            IPaysManager paysManager)
        {
            this.importPersonnelManager = importPersonnelManager;
            this.loggingService = loggingService;
            this.societeManager = societeManager;
            this.fluxManager = fluxManager;
            this.paysManager = paysManager;
        }

        public async Task ImportationByCodeFluxAsync(string codeFlux, bool bypassDate)
        {
            FluxEnt flux = this.fluxManager.GetByCode(codeFlux);

            if (flux != null && flux.IsActif)
            {
                await ImportationBySocietesAsync(flux, bypassDate);

                UpdateDateDerniereExecution(flux, DateTime.UtcNow);
            }
            else
            {
                ThrowFluxException(flux, codeFlux);
            }
        }

        public async Task<ImportResult> ImportationByPersonnelsIdsAsync(List<int> ids)
        {
            if (ids != null && ids.Any())
            {
                var input = new ImportByPersonnelListInputs()
                {
                    PersonnelIds = ids
                };

                return await importPersonnelManager.ImportPersonnelByPersonnelIdsAsync(input);
            }
            else
            {
                loggingService.LogError(IEBusiness.ErrorNoPersonnelId);
                throw new FredBusinessException(IEBusiness.ErrorNoPersonnelId);
            }
        }

        private async Task ImportationBySocietesAsync(FluxEnt flux, bool bypassDate)
        {
            var errorMessage = new StringBuilder();

            List<string> codeSocietesPaye = GetCodeSocietesPayeInFlux(flux);

            if (codeSocietesPaye != null && codeSocietesPaye.Any())
            {
                IEnumerable<PaysEnt> allPaysList = await paysManager.GetListAsync().ConfigureAwait(false);

                foreach (string codeSocietePaye in codeSocietesPaye)
                {
                    SocieteEnt societe = societeManager?.GetSocieteByCodeSocietePaye(codeSocietePaye);
                    if (societe != null)
                    {
                        string importationError = await ImportationBySocieteAsync(societe, bypassDate, flux.DateDerniereExecution, allPaysList);

                        if (!importationError.IsNullOrEmpty())
                        {
                            errorMessage.AppendLine(importationError);
                        }
                    }
                    else
                    {
                        loggingService.LogError(string.Format(IEBusiness.ErrorNoSocieteFound, "personnels"));
                        throw new FredBusinessException(string.Format(IEBusiness.ErrorNoSocieteFound, "personnels"));
                    }
                }
            }
            else
            {
                loggingService.LogError(string.Format(IEBusiness.ErrorNoSocieteInFlux, "personnels"));
                throw new FredBusinessException(string.Format(IEBusiness.ErrorNoSocieteInFlux, "personnels"));
            }


            if (!errorMessage.ToString().IsNullOrEmpty())
            {
                throw new FredBusinessException(errorMessage.ToString());
            }
        }

        private async Task<string> ImportationBySocieteAsync(SocieteEnt societe, bool bypassDate, DateTime? dateDerniereExecution, IEnumerable<PaysEnt> allPaysList)
        {
            string error = string.Empty;
            loggingService.LogInfo($"[IMPORT-EXPORT][IMPORT-PERSONNEL-BY-SOCIETE] - Importation de la sociéte {societe?.Code} {societe?.Libelle} ({societe?.CodeSocieteComptable}).");
            try
            {
                ImportPersonnelsBySocieteInput importPersonnelsBySocieteInput = new ImportPersonnelsBySocieteInput
                {
                    DateDerniereExecution = dateDerniereExecution,
                    IsFullImport = bypassDate,
                    Societe = societe
                };

                //New import process for rzb
                ImportResult importResult = await importPersonnelManager.ImportPersonnelsBySocieteAsync(importPersonnelsBySocieteInput, allPaysList);

                importResult.ErrorMessages.ForEach(e => error += e);
            }
            catch (Exception ex)
            {
                error = "[IMPORT-EXPORT][IMPORT-PERSONNEL-BY-SOCIETE] " + ex.ToString() + " " + ex.InnerException?.ToString();
            }

            return error;
        }

        /// <summary>
        /// Retourne la liste de codeSocietePaye 
        /// </summary>
        /// <param name="flux">flux</param>
        /// <returns>liste de codeSocietePaye</returns>
        private List<string> GetCodeSocietesPayeInFlux(FluxEnt flux)
        {
            var result = new List<string>();
            if (flux == null)
            {
                return result;
            }
            if (string.IsNullOrEmpty(flux.SocieteCode))
            {
                return result;
            }
            return flux.SocieteCode.Split(',').ToList();
        }

        private void ThrowFluxException(FluxEnt flux, string codeFlux)
        {
            if (flux == null)
            {
                string msg = string.Format(IEBusiness.FluxInconnu, codeFlux);
                loggingService.LogError(msg);
                throw new FredBusinessException(msg);
            }
            else if (!flux.IsActif)
            {
                string msg = string.Format(IEBusiness.FluxInactif, codeFlux);
                loggingService.LogError(msg);
                throw new FredBusinessException(msg);
            }
        }

        private void UpdateDateDerniereExecution(FluxEnt flux, DateTime dateToUpdate)
        {
            //Mise a jour de la date d'execution si l'import termine avec succes.
            flux.DateDerniereExecution = dateToUpdate;
            this.fluxManager.Update(flux);
        }
    }
}
