using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Referential;
using Fred.Web.Models;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des pays
    /// </summary>
    public class PaysController : ApiControllerBase
    {
        private readonly IPaysManager paysManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="PaysController" />.
        /// </summary>
        /// <param name="manager">Manager des pays</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public PaysController(IPaysManager manager, IMapper mapper)
        {
            this.paysManager = manager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération des pays
        /// </summary>
        /// <returns>Retourne la liste des pays</returns>
        [HttpGet]
        [Route("api/Pays")]
        public HttpResponseMessage GetPaysList()
        {
            return Get(() => mapper.Map<IEnumerable<PaysModel>>(this.paysManager.GetList()));
        }

        /// <summary>
        /// Rechercher les référentiels Pays
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <returns>retouner une liste des pays</returns>
        [HttpGet]
        [Route("api/Pays/SearchLight/{page?}/{pageSize?}/{recherche?}")]
        public HttpResponseMessage SearchLight(int page = 1, int pageSize = 20, string recherche = "")
        {
            return Get(() => this.mapper.Map<IEnumerable<PaysModel>>(this.paysManager.SearchLight(recherche, page, pageSize)));
        }

        /// <summary>
        /// Méthode GET de récupération d'un pays par son libellé
        /// </summary>
        /// <param name="libelle">Libellé du pays</param>
        /// <returns>Retourne le pays retrouvé</returns>
        [HttpGet]
        [Route("api/Pays/{libelle?}")]
        public HttpResponseMessage GetByLibelle(string libelle = "")
        {
            return Get(() => mapper.Map<PaysModel>(this.paysManager.GetByLibelle(libelle)));
        }

        /// <summary>
        /// Méthode GET de récupération d'un pays par son code
        /// </summary>
        /// <param name="code">code du pays</param>
        /// <returns>Retourne le pays retrouvé</returns>
        [HttpGet]
        [Route("api/Pays/Code/{code?}")]
        public HttpResponseMessage GetByCode(string code = "")
        {
            return Get(() => mapper.Map<PaysModel>(this.paysManager.GetByCode(code)));
        }
    }
}
