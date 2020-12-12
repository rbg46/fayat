using Fred.Framework.Services.Google;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  public class GoogleServiceController : ApiControllerBase
  {
    /// <summary>
    /// Manageur Map
    /// <seealso cref="IGeocodeService"/>
    /// </summaryManager
    private readonly IGeocodeService geocodeService;

   

    /// <summary>
    /// Controller Google Service
    /// </summary>
    /// <param name="geocodeService">geocodeService</param>   
    public GoogleServiceController(IGeocodeService geocodeService)
    {
      this.geocodeService = geocodeService;    
    }

    [HttpPost]
    [Route("api/GoogleService/Geocode")]
    public HttpResponseMessage Geocode(Address address)
    {
      return this.Post(() =>
      {      
        GeocodeResult georesponse = geocodeService.Geocode(address);      
        return georesponse.Results.ToList();
      });
    }

    [HttpPost]
    [Route("api/GoogleService/InverseGeocode")]
    public HttpResponseMessage InverseGeocode(Location location)
    {
      return this.Post(() =>
      {
        GeocodeResult georesponse = geocodeService.InverseGeocode(location);
        return georesponse.Results.ToList();
      });
    }

  }
}