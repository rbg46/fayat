using AutoMapper;
using Fred.Business.Facture;
using Fred.Business.Parametre;
using Fred.Entities.Facture;
using Fred.Web.Models.Facture;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  public class FactureController : ApiControllerBase
  {
    /// <summary>
    /// Manager de Facture
    /// </summary>
    protected readonly IFactureManager FactureManager;

    /// <summary>
    /// Manager de Parametre
    /// </summary>
    protected readonly IParametreManager ParametreManager;

    /// <summary>
    /// Manager du mapper
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FactureController" />.
    /// </summary>
    /// <param name="factureManager">Manager des Factures</param>
    /// <param name="mapper">Auto Mapper</param>
    ///// <param name="logMgr"></param>
    public FactureController(IFactureManager factureManager, IMapper mapper)
    {
      this.FactureManager = factureManager;
      this.Mapper = mapper;
    }

    /// <summary>
    ///   POST Récupération des résultats de la recherche en fonction du filtre
    /// </summary>
    /// <param name="filters">Objet de recherche et de tri des factures</param>
    /// <param name="page">Numéro de page</param>
    /// <param name="pageSize">Taille de la page</param>
    /// <returns>Liste de factures</returns>
    [HttpPost]
    [Route("api/Facture/SearchWithFilters/{page?}/{pageSize?}/")]
    public HttpResponseMessage SearchWithFilters(SearchFactureModel filters, int page = 1, int pageSize = 20)
    {
      return Post(() => Mapper.Map<IEnumerable<FactureModel>>(FactureManager.SearchFactureListWithFilterForRapprochement(Mapper.Map<SearchFactureEnt>(filters), page, pageSize)));
    }

    /// <summary>
    ///   GET Récupération d'un filtre de recherche sur Facture
    /// </summary>
    /// <returns>Retourne un Objet de recherche sur Facture</returns>
    [HttpGet]
    [Route("api/Facture/Filter")]
    public HttpResponseMessage Filter()
    {
      return Get(() => Mapper.Map<SearchFactureModel>(FactureManager.GetFilter()));
    }

    /// <summary>
    ///   GET Récupération de la liste complète des Factures
    /// </summary>
    /// <returns>Retourne la liste des Factures</returns>
    [HttpGet]
    [Route("api/Facture/All")]
    public HttpResponseMessage All()
    {
      return Get(() => Mapper.Map<IEnumerable<FactureModel>>(FactureManager.GetFactureARList()));
    }

    /// <summary>
    /// PUT api/controller
    /// </summary>
    /// <param name="model">Objet Model envoyé</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPut]
    [Route("api/Facture")]
    public HttpResponseMessage UpdateFacture(FactureModel model)
    {
      return this.Put(() => this.FactureManager.Update(this.Mapper.Map<FactureEnt>(model)));
    }

    /// <summary>
    ///   GET Récupération de l'URL du scan d'une Facture
    /// </summary>
    /// <param name="factureId">Identifiant de la facture</param>
    /// <returns>Retourne l'URL du scan d'une Facture</returns>
    [HttpGet]
    [Route("api/Facture/URLFacture/{factureId?}")]
    public HttpResponseMessage URLFacture(int factureId)
    {
      FactureEnt facture = this.FactureManager.GetFactureById(factureId);
      return Get(() => Mapper.Map<FactureModel>(facture));
    }
  }
}