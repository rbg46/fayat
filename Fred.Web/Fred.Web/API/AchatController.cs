using AutoMapper;
using Fred.Business.Commande.Models;
using Fred.Business.ReferentielFixe;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Models.ReferentielFixe;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    public class AchatController : ApiControllerBase
    {
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Referentiel fixes contructor
        /// </summary>
        /// <param name="refFixeManager">Gestionnaire des Chapitres</param>
        /// <param name="mapper">AutoMapper</param>
        public AchatController(IReferentielFixeManager refFixeManager, IMapper mapper)
        {
            this.referentielFixeManager = refFixeManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Recherche des ressources par CI
        /// </summary>
        /// <param name="searchFilter">Filtre de recherche</param>
        /// <returns>retoune une liste de ressources</returns>
        [HttpGet]
        [Route("api/Achat/SearchRessources")]
        public async Task<IHttpActionResult> SearchRessources([FromUri] SearchRessourcesAchatModel searchFilter)
        {
            List<RessourceEnt> ressourcesAchat = await this.referentielFixeManager.SearchRessourcesForAchatAsync(searchFilter);
            return Ok(mapper.Map<List<RessourceModel>>(ressourcesAchat));
        }

        /// <summary>
        /// Recherche des ressources par filtre
        /// </summary>
        /// <param name="searchFilter">Filtre de recherche</param>
        /// <returns>retoune une liste de ressources</returns>
        [HttpGet]
        [Route("api/Achat/Search")]
        public HttpResponseMessage Search([FromUri] SearchRessourcesAchatModel searchFilter)
        {
            return Get(() =>
            {
                return mapper.Map<List<RessourceModel>>(this.referentielFixeManager.SearchLight(searchFilter));
            });
        }
    }
}
