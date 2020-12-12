using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Commande;
using Fred.Web.Models.Commande;

namespace Fred.Web.API
{
    public class StatutCommandeController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly IStatutCommandeManager statutCommandeManager;

        public StatutCommandeController(
            IMapper mapper,
            IStatutCommandeManager statutCommandeManager)
        {
            this.mapper = mapper;
            this.statutCommandeManager = statutCommandeManager;
        }

        /// <summary>
        /// Récupération de la liste Statut 
        /// </summary>
        /// <returns>Liste de types de Statut Commande</returns>
        [HttpGet]
        [Route("api/StatutCommande")]
        public HttpResponseMessage GetAll()
        {
            return Get(() => mapper.Map<List<StatutCommandeModel>>(statutCommandeManager.GetAll()));
        }
    }
}
