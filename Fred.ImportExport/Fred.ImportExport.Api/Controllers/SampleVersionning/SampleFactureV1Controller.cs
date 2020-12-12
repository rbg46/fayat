#pragma warning disable SA1600 // Elements must be documented : SAMPLE CODE
#pragma warning disable CS1591 // Elements must be documented : SAMPLE CODE
#pragma warning disable S125 // Sections of code should not be "commented out" : SAMPLE CODE


using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Web.Http;

namespace Fred.ImportExport.Api.Controllers.SampleVersionning
{
    [ApiVersion("1.0")]
    [ApiVersion("0.9", Deprecated = true)] // exemple
    [RoutePrefix("api/v{api-version:apiVersion}/SampleFacture")]
    [ControllerName("SampleFacture")] // Pour que Asp.Net si retrouve, car le nom de la classe contient V1
                                      //  [Authorize(Users = "userserviceFred", Roles = "service")]
    public class SampleFactureV1Controller : ApiControllerBase
    {
        /// <summary>
        /// Gets a single Facture.
        /// </summary>
        /// <param name="id">The requested facture identifier.</param>
        /// <returns>The requested facture.</returns>
        /// <response code="200">The facture was successfully retrieved.</response>
        /// <response code="404">The facture does not exist.</response>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(SampleFactureV1Model))]
        [MapToApiVersion("1.0")]
        public IHttpActionResult Get(int id) =>
            Ok(new SampleFactureV1Model
            {
                Id = id,
                FactureCode = "FactureV1Code",
                FactureData = "FactureV1Data"
            }
            );
    }
}



#pragma warning restore SA1600 // Elements must be documented : SAMPLE CODE
#pragma warning restore CS1591 // Elements must be documented : SAMPLE CODE
#pragma warning restore S125 // Sections of code should not be "commented out" : SAMPLE CODE