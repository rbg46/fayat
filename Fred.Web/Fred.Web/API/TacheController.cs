using AutoMapper;
using Fred.Business.Referential.Tache;
using Fred.Web.Models.Referential;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des types de dépenses
    /// </summary>
    public class TacheController : ApiControllerBase
    {
        private readonly ITacheManager tacheManager;
        private readonly IMapper mapper;

        public TacheController(ITacheManager tacheManager, IMapper mapper)
        {
            this.tacheManager = tacheManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération des types de dépenses
        /// </summary>
        /// <returns>Retourne la liste des types de dépenses</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var taches = this.tacheManager.GetTacheList();

            return Ok(mapper.Map<IEnumerable<TacheModel>>(taches));
        }

        /// <summary>
        /// Rechercher les référentiels CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="activeOnly">Seulement les tâches actives ou pas</param>
        /// <param name="isTechnicalTask">Détermine si l'ont prend les tâches techniques ou pas</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Tache/SearchLight/{page?}/{pageSize?}/{recherche?}/{ciId?}/{activeOnly?}/{isTechnicalTask?}")]
        public IHttpActionResult SearchLight(int? ciId, int page = 1, int pageSize = 20, string recherche = "", bool activeOnly = true, bool isTechnicalTask = false)
        {
            if (ciId.HasValue)
            {
                return Ok(mapper.Map<IEnumerable<TacheModel>>(tacheManager.SearchLight(recherche, page, pageSize, ciId.Value, activeOnly, isTechnicalTask: isTechnicalTask)));
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("api/Tache/Search/{page?}/{pageSize?}/{ciId?}/{recherche?}")]
        public async Task<IHttpActionResult> SearchAsync(int? ciId, int page = 1, int pageSize = 20, string recherche = "")
        {
            if (ciId.HasValue)
            {
                return Ok(mapper.Map<IEnumerable<TacheModel>>(await tacheManager.SearchLightAsync(ciId.Value, page, pageSize, recherche, true)));
            }

            return BadRequest();
        }
    }
}