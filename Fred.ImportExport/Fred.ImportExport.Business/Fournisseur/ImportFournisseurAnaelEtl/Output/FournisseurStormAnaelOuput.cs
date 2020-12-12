using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CommonServiceLocator;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Models;
using NLog;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Output
{
    /// <summary>
    /// Processus etl : Execution de la sortie de l'import des Fournisseurs
    /// Envoie dans Storm les fournisseurs
    /// </summary>
    internal class FournisseurStormAnaelOuput : IEtlOutput<FournisseurFredModel>
    {
        private readonly IMapper mapper;
        private readonly ApplicationsSapManager applicationsSapManager;
        private readonly int societeId;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="FournisseurStormAnaelOuput"/>.
        /// </summary>
        /// <param name="mapper">Le gestionnaire Automapper.</param>
        /// <param name="applicationsSapManager">applicationsSapManager</param>
        /// <param name="societeId">codeSocieteComptables</param>
        public FournisseurStormAnaelOuput(int societeId)
        {
            this.mapper = ServiceLocator.Current.GetInstance<IMapper>();
            this.applicationsSapManager = ServiceLocator.Current.GetInstance<ApplicationsSapManager>();
            this.societeId = societeId;
        }

        /// <summary>
        /// Appelé par l'ETL
        /// Envoie les fournisseurs à Storm
        /// </summary>
        /// <param name="result">liste des fournisseurs à envoyer à Storm</param>
        public async Task ExecuteAsync(IEtlResult<FournisseurFredModel> result)
        {
            Logger logger = NLog.LogManager.GetCurrentClassLogger();
            ApplicationSapParameter applicationSapParameter = applicationsSapManager.GetParametersForSociete(societeId);

            if (applicationSapParameter.IsFound)
            {
                var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);

                foreach (FournisseurStormModel item in mapper.Map<List<FournisseurStormModel>>(result.Items))
                {
                    try
                    {
                        logger.Info($"[EXPORT][FLUX_FOURNISSEUR_VERS_SAP] Tentative d'export vers SAP : Code du fournisseur ({item.Code}).");
                        await restClient.PostAndEnsureSuccessAsync($"{applicationSapParameter.Url}&ACTION=XK01", item);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"[EXPORT][FLUX_FOURNISSEUR_VERS_SAP] Echec : Code du fournisseur ({item.Code}), Message SAP : {ex.Message}");
                    }
                }
            }
            else
            {
                logger.Error($"[EXPORT][FLUX_FOURNISSEUR_VERS_SAP] Export 'FOURNISSEUR' vers SAP : Il n'y a pas de configuration correspondant à cette société({societeId}).");
            }
        }
    }
}
