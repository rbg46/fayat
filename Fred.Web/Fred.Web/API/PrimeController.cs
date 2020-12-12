using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Referential;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des primes
    /// </summary>
    public class PrimeController : ApiControllerBase
    {
        /// <summary>
        /// Manager business des primes
        /// </summary>
        private readonly IPrimeManager primeMgr;

        /// <summary>
        /// Manager business des primes
        /// </summary>
        private readonly ICIManager ciMgr;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="PrimeController" />.
        /// </summary>
        /// <param name="primeMgr">Manager de primes</param>
        /// <param name="ciMgr">Manager de CI</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public PrimeController(IPrimeManager primeMgr, ICIManager ciMgr, IMapper mapper)
        {
            this.primeMgr = primeMgr;
            this.ciMgr = ciMgr;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération des primes
        /// </summary>
        /// <returns>Retourne la liste des primes</returns>
        [HttpGet]
        [Route("api/Prime/All")]
        public IEnumerable<PrimeModel> GetAll()
        {
            var primes = this.primeMgr.GetPrimesList();
            return this.mapper.Map<IEnumerable<PrimeModel>>(primes);
        }

        [HttpGet]
        [Route("api/Prime/{id}")]
        public async Task<IHttpActionResult> GetByIdAsync(int id)
        {
            var prime = await primeMgr.GetPrimeByIdAsync(id);
            return Ok(this.mapper.Map<PrimeModel>(prime));
        }

        /// <summary>
        /// Méthode GET de récupération des primes actives
        /// </summary>
        /// <returns>Retourne la liste des primes actives</returns>
        [HttpGet]
        [Route("api/Prime/AllActive")]
        public IEnumerable<PrimeModel> GetAllActive()
        {
            var primes = this.primeMgr.GetActivesPrimesList();
            return this.mapper.Map<IEnumerable<PrimeModel>>(primes);
        }

        /// <summary>
        /// Méthode GET de récupération des codes déplacement par ID et par code
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="idCourant">idCourant</param>
        /// <param name="societeId">societeId</param>
        /// <returns>Retourne la liste des codes déplacement</returns>
        [HttpGet]
        [Route("api/Prime/CheckExistsCode/{code}/{idCourant}/{societeId}")]
        public async Task<IHttpActionResult> IsPrimeExistsAsync(string code, int idCourant, int societeId)
        {
            return Ok(await primeMgr.IsPrimeExistsByCodeAsync(idCourant, code, societeId));
        }

        /// <summary>
        /// Get default value for Prime model
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>default value of PrimeModel</returns>
        [HttpGet]
        [Route("api/Prime/New/{societeId}")]
        public IHttpActionResult New(int societeId)
        {
            var prime = this.primeMgr.GetNewPrime(societeId);
            return Ok(this.mapper.Map<PrimeModel>(prime));
        }

        /// <summary>
        /// Méthode GET de recherche des primes
        /// </summary>
        /// <param name="filters">filters</param>
        /// <param name="societeId">societeId</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Retourne la liste des primes</returns>
        [HttpPost]
        [Route("api/Prime/SearchAll/{societeId}/{recherche?}")]
        public async Task<IHttpActionResult> SearchAllAsync(SearchActiveModel filters, int societeId, string recherche = "")
        {
            var listPrimes = await primeMgr.SearchPrimeAllWithSearchPrimeTextAsync(societeId, recherche, this.mapper.Map<SearchPrimeEnt>(filters));
            return Ok(this.mapper.Map<IEnumerable<PrimeModel>>(listPrimes));
        }

        /// <summary>
        /// Méthode POST de récupération des filtres de recherche sur code déplacement
        /// </summary>
        /// <returns>Retourne la liste des filtres de recherche sur code déplacement</returns>
        [HttpGet]
        [Route("api/Prime/Filter/")]
        public HttpResponseMessage Filters()
        {
            try
            {
                var filters = this.mapper.Map<SearchActiveModel>(this.primeMgr.GetDefaultFilter());
                return Request.CreateResponse(HttpStatusCode.OK, filters);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Méthode GET de récupération des primes
        /// </summary>
        /// <param name="prime">prime</param>
        /// <returns>Retourne la liste des primes</returns>
        [HttpPost]
        [Route("api/Prime")]
        public async Task<IHttpActionResult> AddPrime(PrimeModel prime)
        {
            var createdPrime = await primeMgr.AddPrimeAsync(mapper.Map<PrimeEnt>(prime));
            return Created($"api/prime/{createdPrime.PrimeId}", this.mapper.Map<PrimeModel>(createdPrime));
        }

        /// <summary>
        /// Méthode PUT de mise à jour des primes
        /// </summary>
        /// <param name="prime">prime</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Prime")]
        public async Task<IHttpActionResult> UpdatePrimeAsync(PrimeModel prime)
        {
            await primeMgr.UpdatePrimeAsync(prime);
            return Ok();
        }

        /// <summary>
        /// DELETE api/controller/5
        /// </summary>
        /// <param name="prime">code prime à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Prime/Delete")]
        public async Task<IHttpActionResult> DeletePrimeAsync(PrimeModel prime)
        {
            await primeMgr.DeletePrimeAsync(prime.PrimeId);
            return Ok();
        }

        #region Associations Primes/CI

        /// <summary>
        /// Méthode GET de récupération des primes actives utilisables par un CI
        /// </summary>
        /// <param name="id">Identifiant du CI pour lequel on recherche les primes.</param>
        /// <returns>Retourne la liste des primes actives</returns>
        [HttpGet]
        [Route("api/Prime/GetPrimesListForCI/{id}")]
        public IEnumerable<PrimeModel> GetPrimesListForCI(int id)
        {
            var primes = this.primeMgr.GetPrimesListForCI(id);
            return this.mapper.Map<IEnumerable<PrimeModel>>(primes);
        }

        /// <summary>
        /// POST Ajout ou Mise à jour des primes associées à un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ciPrimeList">Liste des CIPrime</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Prime/ManageCIPrime/{ciId}")]
        public HttpResponseMessage ManageCIPrime(int ciId, IEnumerable<CIPrimeModel> ciPrimeList)
        {
            try
            {
                primeMgr.ManageCIPrime(ciId, mapper.Map<IEnumerable<CIPrimeEnt>>(ciPrimeList));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion

        /// <summary>
        /// Rechercher les référentiels Primes selon le type de rapport (Rapport Prime/Mensuel ou Rapport Journalier)
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="groupeId">Groupe Identifier</param>
        /// <param name="isRapportPrime">Si on travaille sur un RapportPrime / Mensuel (TRUE) ou sur un Rapport Journalier (FALSE)</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Prime/SearchLight/{page?}/{pageSize?}/{recherche?}/{societeId?}/{ciId?}/{groupeId?}/{isRapportPrime?}/{isOuvrier?}/{isEtam?}/{isCadre?}")]
        public async Task<IHttpActionResult> SearchLightAsync(int page = 1,
                                                              int pageSize = 20,
                                                              string recherche = "",
                                                              int? societeId = null,
                                                              int? ciId = null,
                                                              int? groupeId = null,
                                                              bool? isRapportPrime = false,
                                                              bool? isOuvrier = null,
                                                              bool? isEtam = null,
                                                              bool? isCadre = null)
        {
            // Cas classique
            if (!societeId.HasValue && ciId.HasValue)
            {
                societeId = ciMgr.GetSocieteByCIId(ciId.Value).SocieteId;
            }

            // Cas FES
            IEnumerable<PrimeEnt> primes = groupeId.HasValue && societeId.HasValue
                ? this.primeMgr.SearchByGroupe(groupeId.Value, societeId.Value, isRapportPrime, recherche, page, pageSize, ciId, isOuvrier, isEtam, isCadre)
                : await this.primeMgr.SearchLightAsync(recherche, page, pageSize, societeId, ciId, isRapportPrime.Value);

            return Ok(this.mapper.Map<IEnumerable<PrimeModel>>(primes));
        }

        /// <summary>
        ///  Verifie si l'entité est déja utilisée.
        /// </summary>
        /// <param name="primeId">primeId</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Prime/IsAlreadyUsed/{primeId}")]
        public async Task<IHttpActionResult> IsPrimeUsedAsync(int primeId)
        {
            return Ok(new
            {
                id = primeId,
                isAlreadyUsed = await primeMgr.IsPrimeUsedAsync(primeId)
            });
        }
    }
}
