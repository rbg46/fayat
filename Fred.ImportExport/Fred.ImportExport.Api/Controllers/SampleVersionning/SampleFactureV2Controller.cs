#pragma warning disable SA1600 // Elements must be documented : SAMPLE CODE
#pragma warning disable CS1591 // Elements must be documented : SAMPLE CODE
#pragma warning disable S125 // Sections of code should not be "commented out" : SAMPLE CODE

using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Web.Http;

namespace Fred.ImportExport.Api.Controllers
{
    [ApiVersion("2.0")]
    [RoutePrefix("api/v{api-version:apiVersion}/SampleFacture")]
    [ControllerName("SampleFacture")] // Pour que Asp.Net si retrouve, car le nom de la classe contient V1
                                      // [Authorize(Users = "userserviceFred", Roles = "service")]
    public class SampleFactureV2Controller : ApiControllerBase
    {
        /// <summary>
        /// Gets a single person.
        /// </summary>
        /// <param name="id">The requested person identifier.</param>
        /// <returns>The requested person.</returns>
        /// <response code="200">The person was successfully retrieved.</response>
        /// <response code="404">The person does not exist.</response>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(SampleFactureV2Model))]
        public IHttpActionResult Get(int id) =>
            Ok(new SampleFactureV2Model
            {
                Id = id,
                FactureCode = "FactureV2Code",
                FactureData = "FactureV2Data",
                FactureOwner = "OwnerV2"
            }
            );
    }
}


#pragma warning restore SA1600 // Elements must be documented : SAMPLE CODE
#pragma warning restore CS1591 // Elements must be documented : SAMPLE CODE
#pragma warning restore S125 // Sections of code should not be "commented out" : SAMPLE CODE