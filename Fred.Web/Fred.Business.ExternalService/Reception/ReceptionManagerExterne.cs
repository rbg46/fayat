using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Reception;
using Fred.DataAccess.ExternalService.FredImportExport.Reception;
using Fred.Entities.Depense;
using Fred.Entities.Models;
using Fred.Entities.Models.Flux.Depense;
using Fred.Framework.Exceptions;
using NLog;

namespace Fred.Business.ExternalService.Reception
{
    /// <summary>
    /// Gestionnaire externe des Receptions
    /// </summary>
    public class ReceptionManagerExterne : IReceptionManagerExterne
    {
        private readonly IReceptionRepositoryExterne receptionRepoExt;
        private readonly IReceptionManager receptionManager;

        public ReceptionManagerExterne(IReceptionRepositoryExterne receptionRepoExt, IReceptionManager receptionManager)
        {
            this.receptionRepoExt = receptionRepoExt;
            this.receptionManager = receptionManager;
        }

        /// <summary>
        ///   Envoi des réceptions à SAP
        /// </summary>
        /// <param name="receptionIds">Liste des identifiants des réceptions à envoyer à SAP</param>
        /// <returns>Liste de résultats</returns>
        public async Task<List<ResultModel<DepenseFluxResponseModel>>> ExportReceptionListToSapAsync(IEnumerable<int> receptionIds)
        {
            try
            {
                // Envoi des réceptions vers SAP et on sauvegarde l'idenfiant du job Hangfire.
                List<ResultModel<DepenseFluxResponseModel>> hangfireResults = await receptionRepoExt.ExportReceptionListToSapAsync(receptionIds);
                foreach (ResultModel<DepenseFluxResponseModel> hangfireResult in hangfireResults)
                {
                    if (hangfireResult.Success)
                    {
                        receptionManager.UpdateHangfireJobId(hangfireResult.Value.ReceptionsIds, hangfireResult.Value.JobId);
                    }
                    else
                    {
                        string ids = string.Join(",", hangfireResult.Value.ReceptionsIds);

                        LogManager.GetCurrentClassLogger().Warn($"[FLUX MIGO - ON FRED WEB] FredIe n'a pas generé de job pour les ids de reception suivants : {ids}");
                    }
                }
                return hangfireResults;
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <summary>
        ///   Envoi des réceptions à SAP
        /// </summary>
        /// <param name="receptions">Liste des réceptions à envoyer à SAP</param>
        public async Task ExportReceptionListToSapAsync(List<DepenseAchatEnt> receptions)
        {
            List<int> receptionIds = receptions.Select(x => x.DepenseId).ToList();
            // Envoi des réceptions vers SAP et on sauvegarde l'idenfiant du job Hangfire.
            List<ResultModel<DepenseFluxResponseModel>> hangfireResults = await receptionRepoExt.ExportReceptionListToSapAsync(receptionIds);

            foreach (ResultModel<DepenseFluxResponseModel> hangfireResult in hangfireResults)
            {
                if (hangfireResult.Success)
                {
                    foreach (int id in hangfireResult.Value.ReceptionsIds)
                    {
                        DepenseAchatEnt reception = receptions.FirstOrDefault(x => x.DepenseId == id);
                        if (reception != null)
                        {
                            reception.HangfireJobId = hangfireResult.Value.JobId;
                        }
                        else
                        {
                            LogManager.GetCurrentClassLogger().Warn("[FLUX MIGO - ON FRED WEB] FredIe retourne un id de reception qui ne correspond pas a un id de reception envoyé.");
                        }
                    }

                }
                else
                {

                    string ids = string.Join(",", hangfireResult.Value.ReceptionsIds);

                    LogManager.GetCurrentClassLogger().Warn($"[FLUX MIGO - ON FRED WEB] FredIe n'a pas generé de job pour les ids de reception suivants : {ids}");
                }
            }

        }
    }
}
