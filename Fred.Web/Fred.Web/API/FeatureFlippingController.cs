using AutoMapper;
using Fred.Business.FeatureFlipping;
using Fred.Entities.FeatureFlipping;
using Fred.Web.Models.FeatureFlipping;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  /// <summary>
  /// Représente une instance de contrôleur Web API pour le module FeatureFlipping.
  /// <seealso cref="ApiController" />
  /// </summary>
  public class FeatureFlippingController : ApiControllerBase
  {
    /// <summary>
    /// Manageur des Features Flipping
    /// </summary>
    protected readonly IFeatureFlippingManager featureFlippingMgr;

    /// <summary>
    /// Auto Mapper modèle / entité
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FeatureFlippingController"/>
    /// </summary>
    /// <param name="manager">Manager</param>
    /// <param name="mapper">Auto Mapper modèle / entité </param>
    public FeatureFlippingController(IFeatureFlippingManager manager, IMapper mapper)
    {
      this.featureFlippingMgr = manager;
      this.Mapper = mapper;
    }

    [HttpPost]
    [Route("api/FeatureFlipping/List/")]
    public HttpResponseMessage ListAll()
    {
      try
      {
        var items = this.Mapper.Map<IEnumerable<FeatureFlippingModel>>(this.featureFlippingMgr.GetFeatureFlippings());
        return Request.CreateResponse(HttpStatusCode.OK, items);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    [HttpPut]
    [Route("api/FeatureFlipping")]
    public HttpResponseMessage Put(FeatureFlippingModel model)
    {
      return Put(() => this.featureFlippingMgr.Update(this.Mapper.Map<FeatureFlippingEnt>(model)));
    }
  }
}