using AutoMapper;
using Fred.Business.Valorisation;
using Fred.Web.Shared.Models.Valorisation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  public class ValorisationController : ApiControllerBase
  {
    private readonly IValorisationManager valorisationManager;
    private readonly IMapper mapper;

    /// <summary>
    /// Controleur des rapports
    /// </summary>    
    /// <param name="valorisationManager">Manager de la valorisation</param>
    /// <param name="mapper">Mapper</param>
    public ValorisationController(IValorisationManager valorisationManager, IMapper mapper)
    {
      this.valorisationManager = valorisationManager;
      this.mapper = mapper;
    }
    
    /// <summary>
    ///   GET La liste des pointages pour un ci et une période comptable
    /// </summary>
    /// <param name="periode">Période comptable</param>
    /// <param name="ciId">Identifiant du ci</param>
    /// <returns>Lot de pointage signé</returns>
    [HttpGet]
    [Route("api/Valorisation/GetList")]
    public HttpResponseMessage GetValorisationByCiIdAndPeriode(DateTime periode, int ciId)
    {
      try
      {
        return Get(() => this.mapper.Map<List<ValorisationModel>>(this.valorisationManager.GetByCiAndPeriod(ciId, periode)));
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }
  }
}