using System;
using System.Threading.Tasks;
using System.Web.Http;
using Fred.Business.RapportPrime; 
using Fred.Entities.RapportPrime;
using Fred.Web.Shared.Models.RapportPrime;
using Fred.Web.Shared.Models.RapportPrime.Get;
using Fred.Web.Shared.Models.RapportPrime.Update;

namespace Fred.Web.API
{
    [RoutePrefix("api/RapportPrime")]
    public class RapportPrimeController : ApiControllerBase
    {
        private readonly IRapportPrimeManager rapportPrimeManager;

        public RapportPrimeController(IRapportPrimeManager rapportPrimeManager)
        {
            this.rapportPrimeManager = rapportPrimeManager;
        }

        [HttpGet]
        [Route("date/{date}")]
        public async Task<IHttpActionResult> GetAsync(DateTime date)
        {
            RapportPrimeGetModel rapportPrime = await rapportPrimeManager.GetAsync(date);

            return Ok(rapportPrime);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddAsync()
        {
            RapportPrimeEnt rapportPrime = await rapportPrimeManager.AddAsync();

            RapportPrimeGetModel rapportPrimeModel = await rapportPrimeManager.GetAsync(rapportPrime.DateRapportPrime);

            return Created($"api/RapportPrime/date/{rapportPrime.DateRapportPrime}", rapportPrimeModel);
        }

        [HttpPut]
        [Route("{rapportPrimeId}")]
        public async Task<IHttpActionResult> UpdateRapportPrimeNewAsync(int rapportPrimeId, RapportPrimeUpdateModel rapportPrimeUpdateModel)
        {
            await rapportPrimeManager.UpdateAsync(rapportPrimeId, rapportPrimeUpdateModel);

            RapportPrimeGetModel rapportPrimeGetModel = await rapportPrimeManager.GetAsync(rapportPrimeUpdateModel.DateRapportPrime);

            return Ok(rapportPrimeGetModel);
        }
    }
}
