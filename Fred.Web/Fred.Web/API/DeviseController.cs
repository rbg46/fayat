using AutoMapper;
using Fred.Business.Referential;
using Fred.Entities.Referential;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  /// <summary>
  /// Représente une instance de contrôleur Web API pour le module devise.
  /// <seealso cref="ApiController" />
  /// </summary>
  public class DeviseController : ApiControllerBase
  {
    /// <summary>
    /// Manageur des Devises
    /// <seealso cref="IDeviseManager"/>
    /// </summary>
    protected readonly IDeviseManager DeviseMgr;

    /// <summary>
    /// Auto Mapper modèle / entité.
    /// <seealso cref="IMapper"/>
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="DeviseController"/>.
    /// </summary>
    /// <param name="commandeMgr">Manageur des commandes</param>
    /// <param name="manager">manager</param>
    /// <param name="mapper">Auto Mapper modèle / entité</param>
    /// <param name="logMgr">Manageur des logs</param>
    public DeviseController(IDeviseManager manager, IMapper mapper)
    {
      this.DeviseMgr = manager;
      this.Mapper = mapper;
    }

    /// <summary>
    /// GET api/controller
    /// </summary>
    /// <returns>Liste de devises</returns>
    [HttpGet]
    [Route("api/Devise")]
    public HttpResponseMessage Get()
    {
      try
      {
        var items = this.Mapper.Map<IEnumerable<DeviseModel>>(this.DeviseMgr.GetList());
        return Request.CreateResponse(HttpStatusCode.OK, items);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// POST api/controller
    /// </summary>
    /// <param name="model">Devise à traiter</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPost]
    [Route("api/Devise")]
    public HttpResponseMessage Post(DeviseModel model)
    {
      try
      {
        if (this.ModelState.IsValid)
        {
          int deviseId = this.DeviseMgr.Add(this.Mapper.Map<DeviseEnt>(model));
          model.DeviseId = deviseId;
          HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
          return response;
        }
        else
        {
          return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// PUT api/controller/5
    /// </summary>
    /// <param name="model">Devise à traiter</param>
    /// <returns>Retourne une réponse HTTP</returns>
    /// <exception cref="ArgumentNullException">Le code Devise est introuvable.</exception>
    [HttpPut]
    [Route("api/Devise")]
    public HttpResponseMessage Put(DeviseModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          this.DeviseMgr.Update(this.Mapper.Map<DeviseEnt>(model));
        }
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }

      return Request.CreateResponse(HttpStatusCode.OK);
    }

    /// <summary>
    /// DELETE api/controller/5
    /// </summary>
    /// <param name="model">model</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPost]
    public HttpResponseMessage Delete(DeviseModel model)
    {
      try
      {
        if (!this.ModelState.IsValid)
        {
          return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        if (model != null)
        {
          if (this.DeviseMgr.DeleteById(this.Mapper.Map<DeviseEnt>(model)))
          {
            return Request.CreateResponse(HttpStatusCode.OK);
          }
          else
          {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
          }
        }

        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Vérification de l'unicité du code Iso renseigné par l'utilisateur
    /// </summary>   
    /// <param name="codeDevise">Code Iso rechercher</param>
    /// <param name="idCourant">Id de la device (pour exclusion)</param>
    /// <returns>Retourne Vrai si déjà utilisé, sinon False (alors référence disponible)</returns>
    [HttpGet]
    [Route("api/Devise/CheckExistsCode/{idCourant}/{codeDevise}")]
    public bool CheckUnicityCodeIso(string codeDevise, int idCourant)
    {
      return this.DeviseMgr.CheckUnicityCodeIso(codeDevise, idCourant);
    }

    /// <summary>
    /// Méthode GET de recherche des devises
    /// </summary>
    /// <param name="filters">filters</param>
    /// <param name="recherche">recherche</param>
    /// <returns>Retourne la liste des devises</returns>CheckExistsCode
    [HttpPost]
    [Route("api/Devise/SearchAll/{recherche?}")]
    public HttpResponseMessage Search(SearchDeviseModel filters, string recherche = "")
    {
      try
      {
        var devise = this.DeviseMgr.SearchDeviseWithFilters(recherche, this.Mapper.Map<SearchDeviseEnt>(filters));
        return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<DeviseModel>>(devise));
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Méthode POST de récupération du filtre de recherche des devises
    /// </summary>
    /// <returns>Retourne le filtre de recherche des devises</returns>
    [HttpGet]
    [Route("api/Devise/Filter/")]
    public HttpResponseMessage Filter()
    {
      try
      {
        var filter = this.Mapper.Map<SearchDeviseModel>(this.DeviseMgr.GetDefaultFilter());
        return Request.CreateResponse(HttpStatusCode.OK, filter);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Méthode GET de récupération d'une nouvelle instance devise intialisée.
    /// </summary>
    /// <returns>Retourne une nouvelle instance devise intialisée</returns>
    [HttpGet]
    [Route("api/Devise/New/")]
    public HttpResponseMessage New()
    {
      try
      {
        var devise = this.Mapper.Map<DeviseModel>(this.DeviseMgr.GetNewDevise());
        return Request.CreateResponse(HttpStatusCode.OK, devise);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Rechercher les référentiels CI
    /// </summary>
    /// <param name="ciId">ciId</param>
    /// <param name="societeId">societeId</param>
    /// <param name="organisationId">Identifiant de l'organisation</param>
    /// <param name="page">page index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="recherche">text</param>
    /// <returns>retouner une liste de CI</returns>s 
    [HttpGet]
    [Route("api/Devise/SearchLight/{page?}/{pageSize?}/{recherche?}/{ciId?}/{societeId?}/{organisationId?}")]
    public HttpResponseMessage SearchLight(int? ciId, int? societeId, int? organisationId, int page = 1, int pageSize = 20, string recherche = "")
    {
      return this.Get(() => this.Mapper.Map<IEnumerable<DeviseModel>>(this.DeviseMgr.SearchLight(ciId, societeId, organisationId, recherche, page, pageSize)));
    }

    /// <summary>
    ///  Verifie si l'entité est déja utilisée.
    /// </summary>
    /// <param name="deviseId">deviseId</param>  
    /// <returns>retouner une liste de CI</returns>
    [HttpGet]
    [Route("api/Devise/IsAlreadyUsed/{deviseId}")]
    public HttpResponseMessage IsAlreadyUsed(int deviseId)
    {
      return Get(() => new
      {
        id = deviseId,
        isAlreadyUsed = this.DeviseMgr.IsAlreadyUsed(deviseId)
      });
    }
  }
}