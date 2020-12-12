using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Groupe;
using Fred.Web.Models.Groupe;

namespace Fred.Web.API
{
    public class GroupeController : ApiControllerBase
    {
        private readonly IGroupeManager groupeManager;
        private readonly IMapper mapper;

        public GroupeController(
            IGroupeManager groupeManager,
            IMapper mapper)
        {
            this.groupeManager = groupeManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Methode de Get pour obtenir l'Urltuto du groupe
        /// </summary>
        /// <returns>réponse http</returns>
        [HttpGet]
        [Route("api/Groupe/GetUrlTutoByGroupe")]
        public HttpResponseMessage GetUrlTutoByGroupe()
        {
            return Get(() => groupeManager.GetUrlTutoByGroupe());
        }

        /// <summary>
        /// Methode de Get pour obtenir toute la liste des groupes
        /// </summary>
        /// <returns>réponse http</returns>
        [HttpGet]
        [Route("api/Groupe/GetAll")]
        public HttpResponseMessage GetAll()
        {
            return Get(() => mapper.Map<IEnumerable<GroupeModel>>(groupeManager.GetAll()));
        }

        /// <summary>
        /// Methode de Get pour obtenir toute la liste des groupes qui appartient au perimetre de l'utilisateur
        /// </summary>
        /// <returns>réponse http</returns>
        [HttpGet]
        [Route("api/Groupe/GetAllGroupForConnectedUser")]
        public HttpResponseMessage GetAllGroupForUser()
        {
            return Get(() => mapper.Map<List<GroupeModel>>(groupeManager.GetAllGroupForUser()));
        }
    }
}
