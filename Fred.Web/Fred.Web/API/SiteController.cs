using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Site;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Web.API
{
    public class SiteController : ApiControllerBase
    {
        private readonly ISiteManager siteManager;
        private readonly IMapper mapper;

        public SiteController(
          ISiteManager siteManager,
          IMapper mapper)
        {
            this.siteManager = siteManager;
            this.mapper = mapper;
        }

        /// <summary>
        ///   POST Récupération des résultats de la recherche d'un site
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <returns>Une réponse HTTP contenant la liste des sites</returns>
        [HttpGet]
        [Route("api/Site/SearchLight/{page?}/{pageSize?}/{recherche?}")]
        [AllowAnonymous]
        public HttpResponseMessage SearchLightForSite(int page = 1, int pageSize = 20, string recherche = null)
        {
            return Post(() => mapper.Map<IEnumerable<SiteModel>>(siteManager.SearchLightForSites(page, pageSize, recherche)));
        }
    }
}
