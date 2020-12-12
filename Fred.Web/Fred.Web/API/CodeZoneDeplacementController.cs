using AutoMapper;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.Referential;
using Fred.Web.Models.CodeZoneDeplacement;
using Fred.Web.Models.Societe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  /// <summary>
  /// Controller WEB API des codes zone deplacement
  /// </summary>
  public class CodeZoneDeplacementController : ApiControllerBase
  {
    /// <summary>
    /// Manager business codes zone deplacement
    /// </summary>
    protected readonly ICodeZoneDeplacementManager codeZoneDeplacementMgr;

    /// <summary>
    /// Manager business sociétés
    /// </summary>
    protected readonly ISocieteManager societeMgr;

    /// <summary>
    /// Mapper Model / ModelVue
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Id de l'utilisateur courant
    /// </summary>
    protected readonly int utilisateurID;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="CodeZoneDeplacementController" />.
    /// </summary>
    /// <param name="codeZoneDeplacementMgr">Manager de codeZoneDeplacement</param>
    /// <param name="societeMgr">Manager de societe</param>
    /// <param name="mapper">Mapper Model / ModelVue</param>
    /// <param name="utilisateurMgr">Manager des utilisateurs</param>
    public CodeZoneDeplacementController(ICodeZoneDeplacementManager codeZoneDeplacementMgr, ISocieteManager societeMgr, IMapper mapper, IUtilisateurManager utilisateurMgr)
    {
      this.codeZoneDeplacementMgr = codeZoneDeplacementMgr;
      this.societeMgr = societeMgr;
      this.Mapper = mapper;
      this.utilisateurID = utilisateurMgr.GetContextUtilisateurId();
    }

    /// <summary>
    /// Méthode GET de récupération de toutes les sociétés.
    /// </summary>
    /// <returns>Retourne la liste de toutes les sociétés</returns>
    [HttpGet]
    [Route("api/CodeZoneDeplacement/GetSocieteAll/")]
    public HttpResponseMessage GetSocieteAll()
    {
      try
      {
        var societes = this.Mapper.Map<IEnumerable<SocieteModel>>(this.societeMgr.GetSocieteListAll());
        return Request.CreateResponse(HttpStatusCode.OK, societes);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Méthode GET de récupération de tous les codes zone deplacement par societe id.
    /// </summary>
    /// <param name="societeId">societeId</param>
    /// <returns>Retourne la liste de tous les codes zone deplacements</returns>
    [HttpGet]
    [Route("api/CodeZoneDeplacement/All/{societeId}")]
    public HttpResponseMessage GetCodeZoneDeplacementBySocieteId(int societeId)
    {
      try
      {
        var codeZoneDeplacements = this.Mapper.Map<IEnumerable<CodeZoneDeplacementModel>>(this.codeZoneDeplacementMgr.GetCodeZoneDeplacementBySocieteId(societeId)).ToList();
        return Request.CreateResponse(HttpStatusCode.OK, codeZoneDeplacements);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Méthode GET de récupération de tous les codes zone deplacement.
    /// </summary>
    /// <returns>Retourne la liste de tous les codes zone deplacements</returns>
    [HttpGet]
    [Route("api/CodeZoneDeplacement/GetCodeZoneDeplacements")]
    public HttpResponseMessage GetCodeZoneDeplacements()
    {
      try
      {
        var codeZoneDeplacements = this.Mapper.Map<IEnumerable<CodeZoneDeplacementModel>>(this.codeZoneDeplacementMgr.GetCodeZoneDeplacementList());
        return Request.CreateResponse(HttpStatusCode.OK, codeZoneDeplacements);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    [HttpPost]
    [Route("api/CodeZoneDeplacement/")]
    public HttpResponseMessage Post(CodeZoneDeplacementModel codeZoneDeplacementModel)
    {
      try
      {
        if (this.ModelState.IsValid)
        {
          codeZoneDeplacementModel.AuteurCreation = utilisateurID;
          codeZoneDeplacementModel.DateCreation = DateTime.Now;

          this.codeZoneDeplacementMgr.AddCodeZoneDeplacement(this.Mapper.Map<CodeZoneDeplacementEnt>(codeZoneDeplacementModel));
          HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, codeZoneDeplacementModel);
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
    /// DELETE api/controller/5
    /// </summary>
    /// <param name="codeZoneDeplacement">code zone deplacement à supprimer</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPost]
    [Route("api/CodeZoneDeplacement/Delete")]
    public HttpResponseMessage Delete(CodeZoneDeplacementModel codeZoneDeplacement)
    {
      try
      {
        if (!this.ModelState.IsValid)
        {
          return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        if (codeZoneDeplacement != null)
        {
          if (this.codeZoneDeplacementMgr.DeleteCodeZoneDeplacementById(this.Mapper.Map<CodeZoneDeplacementEnt>(codeZoneDeplacement)))
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
    /// PUT api/controller/5
    /// </summary>
    /// <param name="codeZoneDeplacement">Code zone deplacement à traiter</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPut]
    [Route("api/CodeZoneDeplacement/")]
    public HttpResponseMessage Put(CodeZoneDeplacementModel codeZoneDeplacement)
    {
      try
      {
        if (!this.ModelState.IsValid)
        {
          return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        if (!this.CheckExistsCode(codeZoneDeplacement))
        {
          codeZoneDeplacement.AuteurModification = utilisateurID;
          codeZoneDeplacement.DateModification = DateTime.Now;
          this.codeZoneDeplacementMgr.UpdateCodeZoneDeplacement(this.Mapper.Map<CodeZoneDeplacementEnt>(codeZoneDeplacement));
          return Request.CreateResponse(HttpStatusCode.OK);
        }
        else
        {
          return Request.CreateResponse(HttpStatusCode.Conflict);
        }
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    private bool CheckExistsCode(CodeZoneDeplacementModel codeZoneDeplacement)
    {
      bool resu = false;

      bool exists = this.codeZoneDeplacementMgr.IsCodeZoneDeplacementExistsByCode(codeZoneDeplacement.CodeZoneDeplacementId, codeZoneDeplacement.Code, codeZoneDeplacement.SocieteId);
      if (exists)
      {
        resu = true;
      }

      return resu;
    }

    [HttpGet]
    [Route("api/CodeZoneDeplacement/CheckExistsCode/{codeZoneDeplacement}/{idCourant}/{societeId}/")]
    public HttpResponseMessage CheckExistsCode(string codeZoneDeplacement, int idCourant, int societeId)
    {
      try
      {
        var exists = this.codeZoneDeplacementMgr.IsCodeZoneDeplacementExistsByCode(idCourant, codeZoneDeplacement, societeId);
        return Request.CreateResponse(HttpStatusCode.OK, exists);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Méthode GET de récupération d'une nouvelle instance code zone deplacement intialisée.
    /// </summary>
    /// <param name="societeId">societeId</param>
    /// <returns>Retourne une nouvelle instance code zone deplacement intialisée</returns>
    [HttpGet]
    [Route("api/CodeZoneDeplacement/New/{societeId}")]
    public HttpResponseMessage New(int societeId)
    {
      try
      {
        var societe = this.Mapper.Map<CodeZoneDeplacementModel>(this.codeZoneDeplacementMgr.GetNewCodeZoneDeplacement(societeId));
        return Request.CreateResponse(HttpStatusCode.OK, societe);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Méthode GET de recherche des CodeZoneDeplacements
    /// </summary>
    /// <param name="filters">filters</param>
    /// <param name="societeId">societeId</param>
    /// <param name="recherche">recherche</param>
    /// <returns>Retourne la liste des CodeZoneDeplacements</returns>
    [HttpPost]
    [Route("api/CodeZoneDeplacement/SearchCodeZoneDeplacementAllBySocieteId/{societeId}/{recherche?}")]
    public HttpResponseMessage SearchCodeZoneDeplacementAllBySocieteId(SearchCodeZoneDeplacementModel filters, int societeId, string recherche = "")
    {
      try
      {
        var codeZoneDeplacements = this.codeZoneDeplacementMgr.SearchCodeZoneDeplacementAllBySocieteIdWithFilters(this.Mapper.Map<SearchCodeZoneDeplacementEnt>(filters), societeId, recherche);
        return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<CodeZoneDeplacementModel>>(codeZoneDeplacements));
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Méthode POST de récupération des filtres de recherche sur CodeZoneDeplacement
    /// </summary>
    /// <returns>Retourne la liste des filtres de recherche sur CodeZoneDeplacement</returns>
    [HttpGet]
    [Route("api/CodeZoneDeplacement/Filter/")]
    public HttpResponseMessage Filters()
    {
      try
      {
        var filters = this.Mapper.Map<SearchCodeZoneDeplacementModel>(this.codeZoneDeplacementMgr.GetDefaultFilter());
        return Request.CreateResponse(HttpStatusCode.OK, filters);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    /// Rechercher les référentiels CodeDeplacement
    /// </summary>
    /// <param name="ciId">Identifiant du CI</param>
    /// <param name="page">page index</param>
    /// <param name="pageSize">page size</param>
    /// <param name="recherche">text</param>
    /// <returns>retouner une liste de CI</returns>
    [HttpGet]
    [Route("api/CodeZoneDeplacement/SearchLight/{page?}/{pageSize?}/{recherche?}/{ciId?}/{isOuvrier?}/{isEtam?}/{isCadre?}")]

    public HttpResponseMessage SearchLight(int? ciId, int page = 1, int pageSize = 20, string recherche = "", bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
    {
      return this.Get(() => this.Mapper.Map<IEnumerable<CodeZoneDeplacementModel>>(this.codeZoneDeplacementMgr.SearchLight(recherche, page, pageSize, ciId,isOuvrier,isEtam,isCadre)));
    }

    /// <summary>
    ///  Verifie si l'entité est déja utilisée.
    /// </summary>
    /// <param name="codeZoneDeplacementId">codeZoneDeplacementId</param>  
    /// <returns>retouner une liste de CI</returns>
    [HttpGet]
    [Route("api/CodeZoneDeplacement/IsAlreadyUsed/{codeZoneDeplacementId}")]
    public HttpResponseMessage IsAlreadyUsed(int codeZoneDeplacementId)
    {
      return Get(() => new
      {
        id = codeZoneDeplacementId,
        isAlreadyUsed = this.codeZoneDeplacementMgr.IsAlreadyUsed(codeZoneDeplacementId)
      });
    }
  }
}
