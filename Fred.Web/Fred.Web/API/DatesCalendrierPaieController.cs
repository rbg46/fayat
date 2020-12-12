using AutoMapper;
using Fred.Business.DatesCalendrierPaie;
using Fred.Entities.DatesCalendrierPaie;
using Fred.Web.Models.DatesCalendrierPaie;
using Fred.Web.Shared.App_LocalResources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Fred.Web.API
{
  /// <summary>
  ///   Controller WEB API des DatesCalendrierPaies
  /// </summary>
  public class DatesCalendrierPaieController : ApiController
  {
    /// <summary>
    ///   Manager business des DatesCalendrierPaies
    /// </summary>
    protected readonly IDatesCalendrierPaieManager DatesCalendrierPaieMgr;

    /// <summary>
    ///   Mapper Model / ModelVue
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="DatesCalendrierPaieController" />.
    /// </summary>
    /// <param name="datesCalendrierPaieMgr">Manager de DatesCalendrierPaies</param>
    /// <param name="mapper">Mapper Model / ModelVue</param>
    public DatesCalendrierPaieController(IDatesCalendrierPaieManager datesCalendrierPaieMgr, IMapper mapper)
    {
      this.DatesCalendrierPaieMgr = datesCalendrierPaieMgr;
      this.Mapper = mapper;
    }

    /// <summary>
    ///   Méthode GET de récupération des DatesCalendrierPaies
    /// </summary>
    /// <param name="societeId">L'id de la société</param>
    /// <param name="year">L' année</param>
    /// <returns>Retourne la liste des DatesCalendrierPaies</returns>
    [HttpGet]
    [Route("api/DatesCalendrierPaie/{societeId}/{year}/")]
    public HttpResponseMessage Search(int societeId, int year)
    {
      try
      {
        var listDcp = this.Mapper.Map<IEnumerable<DatesCalendrierPaieModel>>(this.DatesCalendrierPaieMgr.GetSocieteListDatesCalendrierPaieByIdAndYear(societeId, year));
        return Request.CreateResponse(HttpStatusCode.OK, listDcp);
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }

    /// <summary>
    ///   Méthode POST de mise à jour des DatesCalendrierPaies
    /// </summary>
    /// <param name="listDcp">La liste des calendriers</param>
    /// <returns>Retourne un HttpResponseMessage</returns>
    [HttpPost]
    [Route("api/DatesCalendrierPaie/AddOrUpdate")]
    public HttpResponseMessage AdOrUpdate(IEnumerable<DatesCalendrierPaieModel> listDcp)
    {
      StringBuilder sbMsgErreur = new StringBuilder();
      try
      {
        foreach (DatesCalendrierPaieModel dcp in listDcp)
        {
          if (this.DatesCalendrierPaieMgr.IsDateFinPtgLowerThanDateTransfertPtg(this.Mapper.Map<DatesCalendrierPaieEnt>(dcp)))
          {
            this.DatesCalendrierPaieMgr.AddOrUpdateDatesCalendrierPaie(this.Mapper.Map<DatesCalendrierPaieEnt>(dcp));
          }
          else
          {
            sbMsgErreur.AppendLine(string.Format(FeatureDatesCalendrierPaie.DateFinPointage_error, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dcp.DateFinPointages.Value.Month).ToUpper()) + "<br />");
          }
        }

        if (sbMsgErreur.Length == 0)
        {
          return Request.CreateResponse(HttpStatusCode.Created);
        }

        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, sbMsgErreur.ToString());
      }
      catch (Exception ex)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
      }
    }
  }
}