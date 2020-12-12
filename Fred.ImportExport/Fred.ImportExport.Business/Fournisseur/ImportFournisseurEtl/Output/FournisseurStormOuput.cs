using AutoMapper;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Models;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Output
{
  /// <summary>
  /// Processus etl : Execution de la sortie de l'import des Fournisseurs
  /// Envoie dans Storm les fournisseurs
  /// </summary>
  internal class FournisseurStormOuput : IEtlOutput<FournisseurModel>
  {

    private readonly IMapper mapper;
    private readonly ApplicationsSapManager applicationsSapManager;
    private readonly int societeId;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FournisseurStormOuput"/>.
    /// </summary>
    /// <param name="mapper">Le gestionnaire Automapper.</param>
    /// <param name="applicationsSapManager">applicationsSapManager</param>
    /// <param name="societeId">codeSocieteComptables</param>
    public FournisseurStormOuput(int societeId)
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
    public void Execute(IEtlResult<FournisseurModel> result)
    {
      var applicationSapParameter = applicationsSapManager.GetParametersForSociete(societeId);

      if (applicationSapParameter.IsFound)
      {
        var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);

        List<FournisseurStormModel> fournisseurs = mapper.Map<List<FournisseurStormModel>>(result.Items);

        // Suite à la demande d'SAP, on envoi une liste de 1 éléments, je ne suis pas responsable de ce choix !
        foreach (var item in fournisseurs)
        {
          LogManager.GetCurrentClassLogger()
            .Info($"[EXPORT][FLUX_FOURNISSEUR_VERS_SAP] Tentative d'export vers SAP : Code du fournisseur ({item.Code}).");
          restClient.Post($"{applicationSapParameter.Url}&ACTION=XK01", item);
        }

      }
      else
      {
        LogManager.GetCurrentClassLogger()
        .Error($"[EXPORT][FLUX_FOURNISSEUR_VERS_SAP] Export 'FOURNISSEUR' vers SAP : Il n'y a pas de configuration correspondant à cette société({societeId}).");
      }

    }
  }
}
