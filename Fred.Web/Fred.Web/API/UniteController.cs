using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Entities;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  public class UniteController : ApiControllerBase
    {
    private readonly IMapper mapper;
    private readonly IUniteManager uniteMgr;
    private readonly IUniteReferentielEtenduManager uniteReferentielEtenduMgr;
    private readonly ISocieteManager societeMgr;
    private readonly ICIManager ciMgr;


    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="UniteController"/>.
    /// </summary>
    /// <param name="uniteManager">Manageur des Unite</param>
    /// <param name="societeMgr">Manager des sociétés</param>
    /// <param name="ciMgr">Manager des CIs</param>
    /// <param name="uniteReferentielEtenduMgr">Manager des relations unités/référentiel étendu</param>
    /// <param name="mapper">Auto Mapper modèle / entité</param>    
    public UniteController(IUniteManager uniteManager, ISocieteManager societeMgr, ICIManager ciMgr, IUniteReferentielEtenduManager uniteReferentielEtenduMgr, IMapper mapper)
    {
      this.ciMgr = ciMgr;
      this.uniteMgr = uniteManager;
      this.uniteReferentielEtenduMgr = uniteReferentielEtenduMgr;
      this.societeMgr = societeMgr;
      this.mapper = mapper;
    }

    /// <summary>
    ///   GET Rechercher les référentiels Unités
    /// </summary>
    /// <param name="page">page index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="recherche">text</param>
    /// <returns>retouner une liste de CI</returns>
    [HttpGet]
    [Route("api/Unite/SearchLight/{page?}/{pageSize?}/{recherche?}")]
    public HttpResponseMessage SearchLight(int page = 1, int pageSize = 20, string recherche = "")
    {   
     return Get(() => this.mapper.Map<IEnumerable<UniteModel>>(this.uniteMgr.SearchLight(recherche, page,  pageSize)));    
    }

    /// <summary>
    /// Rechercher les référentiels ressources
    /// </summary>
    /// <param name="societeId">Identifiant de la société</param>
    /// <param name="page">page index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="recherche">text</param>
    /// <param name="typeUnite">Type d'unité</param>
    /// <returns>retouner une liste des unités</returns>
    [HttpGet]
    [Route("api/Unite/SearchLightBySocieteId/{page?}/{pageSize?}/{recherche?}/{societeId?}/{typeUnite?}")]
    public HttpResponseMessage SearchLightBySocieteId(int? societeId, int page = 1, int pageSize = 20, string recherche = "", string typeUnite = null)
    {
      if (societeId.HasValue)
      {
        return Get(() => this.mapper.Map<IEnumerable<UniteModel>>(this.societeMgr.SearchLightUniteBySocieteId(recherche, societeId.Value, page, pageSize, typeUnite)));
      }
      else
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Empty);
      }
    }

    /// <summary>
    /// Rechercher les référentiels ressources
    /// </summary>
    /// <param name="ciId">Identifiant du Ci</param>
    /// <param name="ressourceId">Identifiant de la ressource</param>
    /// <param name="page">page index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="recherche">text</param>
    /// <returns>retouner une liste des unités</returns>
    [HttpGet]
    [Route("api/Unite/SearchLightBySocieteAndRessourceId/{page?}/{pageSize?}/{recherche?}/{ciId?}/{ressourceId?}")]
    public HttpResponseMessage SearchLightBySocieteAndRessourceId(int ciId, int ressourceId, int page = 1, int pageSize = 20, string recherche = "")
    {
      try
      {
        var societe = ciMgr.GetSocieteByCIId(ciId);
        // Appel vers la méthode de récupération des paramètres sociétés quand ceux-ci seront en place, en attendant on teste si la société appartiernt au groupe Fayat TP
        var limitationUnitesRessource = societeMgr.IsSocieteInGroupe(societe.SocieteId, Constantes.CodeGroupeFTP);
        if (limitationUnitesRessource)
        {
          return Get(() => this.mapper.Map<IEnumerable<UniteModel>>(uniteReferentielEtenduMgr.SearchLight(recherche, page, pageSize, societe.SocieteId, ressourceId)));
        }
        else
        {
          return Get(() => this.mapper.Map<IEnumerable<UniteModel>>(this.uniteMgr.SearchLight(recherche, page, pageSize)));
        }
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Retourne la liste des unités associé à un référentiel étendu
    /// ATTENTION : A UTILISER UNIQUEMENT SI LE PARAMETRAGE LIMITATION_UNITE_RESSOURCE
    /// </summary>
    /// <param name="ciId">Identifiant du Ci</param>
    /// <param name="ressourceId">Identifiant de la ressource</param>
    /// <returns>retouner une liste des unités</returns>
    [HttpGet]
    [Route("api/Unite/GetListUniteByRessourceId/{ciId}/{ressourceId}")]
    public HttpResponseMessage GetListUniteByRessourceId(int ciId, int ressourceId)
    {
      try
      {
        var societe = ciMgr.GetSocieteByCIId(ciId);
        return Get(() => this.mapper.Map<IEnumerable<UniteModel>>(this.uniteReferentielEtenduMgr.GetListUniteByRessourceId(societe.SocieteId, ressourceId)));
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }
  }
}
