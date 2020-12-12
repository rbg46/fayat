using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Delegation;
using Fred.Business.Personnel;
using Fred.Entities.Delegation;
using Fred.Web.Models.Delegation;
using Fred.Web.Models.Personnel;

namespace Fred.Web.API
{
    public class DelegationController : ApiControllerBase
    {
        private readonly IPersonnelManager personnelManager;
        private readonly IDelegationManager delegationManager;
        private readonly IMapper mapper;


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="PersonnelController" />.
        /// </summary>
        /// <param name="personnelManager">Manager du personnel</param>     
        /// <param name="delegationManager">Manager des délégations</param>     
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public DelegationController(IDelegationManager delegationManager, IPersonnelManager personnelManager, IMapper mapper)
        {
            this.delegationManager = delegationManager;
            this.personnelManager = personnelManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération des délégation
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel de référence</param>
        /// <returns>Retourne la liste des délégations</returns>
        [HttpGet]
        [Route("api/Delegation/{personnelId}")]
        public HttpResponseMessage GetDelegation(int personnelId)
        {
            return Get(() => this.mapper.Map<IEnumerable<DelegationModel>>(this.delegationManager.GetDelegationByPersonnelId(personnelId)));
        }

        /// <summary>
        ///   Récupère la liste des délégués potentiels
        /// </summary>
        /// <param name="delegantId">Identifiant unique du délégant</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Nom recherché</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche2">Prénom recherché</param>
        /// <param name="recherche3">Autres infos recherchées</param>
        /// <returns>Http Response</returns>
        [HttpGet]
        [Route("api/Delegation/SplitDelegue/{delegantId}/{recherche?}/{page?}/{pageSize?}/{recherche2?}/{recherche3?}/{societeId?}")]
        public HttpResponseMessage GetSplitDelegue(int delegantId, int societeId, string recherche = "", int page = 1, int pageSize = 20, string recherche2 = "", string recherche3 = "")
        {
            return Get(() => this.mapper.Map<IEnumerable<PersonnelModel>>(this.personnelManager.GetDelegue(delegantId, societeId, recherche, page, pageSize, recherche2, recherche3)));
        }

        /// <summary>
        ///   Récupère la liste des délégués potentiels
        /// </summary>
        /// <param name="delegantId">Identifiant unique du délégant</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Http Response</returns>
        [HttpGet]
        [Route("api/Delegation/Delegue/{delegantId}/{recherche?}/{page?}/{pageSize?}/{societeId?}")]
        public HttpResponseMessage GetDelegue(int delegantId, int societeId, string recherche = "", int page = 1, int pageSize = 20)
        {
            return Get(() => this.mapper.Map<IEnumerable<PersonnelModel>>(this.personnelManager.GetDelegue(delegantId, societeId, recherche, page, pageSize)));
        }

        /// <summary>
        /// Ajout d'une délégation
        /// </summary>
        /// <param name="delegationEnt">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Delegation/Active")]
        public HttpResponseMessage GetDelegationAlreadyActive(DelegationEnt delegationEnt)
        {
            return Post(() => this.delegationManager.GetDelegationAlreadyActive(delegationEnt));
        }

        /// <summary>
        /// Ajout d'une délégation
        /// </summary>
        /// <param name="delegationModel">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Delegation/Add")]
        public HttpResponseMessage AddDelegation(DelegationModel delegationModel)
        {
            return Post(() => this.delegationManager.AddDelegation(this.mapper.Map<DelegationEnt>(delegationModel)));
        }

        /// <summary>
        /// Moification d'une délégation
        /// </summary>
        /// <param name="delegationModel">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Delegation/Update")]
        public HttpResponseMessage UpdateDelegation(DelegationModel delegationModel)
        {
            return Put(() => this.delegationManager.UpdateDelegation(this.mapper.Map<DelegationEnt>(delegationModel)));
        }

        /// <summary>
        /// Désactivation d'une délégation
        /// </summary>
        /// <param name="delegationModel">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Delegation/Desactivate")]
        public HttpResponseMessage DesactivateDelegation(DelegationModel delegationModel)
        {
            return Put(() => this.delegationManager.DesactivateDelegation(this.mapper.Map<DelegationEnt>(delegationModel)));
        }

        /// <summary>
        /// Suppression d'une délégation en fonction de son identifiant
        /// </summary>
        /// <param name="delegationModel">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Delegation/Delete")]
        public HttpResponseMessage DeleteDelegationById(DelegationModel delegationModel)
        {
            return Delete(() => this.delegationManager.DeleteDelegationById(this.mapper.Map<DelegationEnt>(delegationModel)));
        }
    }
}
