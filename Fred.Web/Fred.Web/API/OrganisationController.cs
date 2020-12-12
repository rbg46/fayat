using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Organisation;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Framework.Extensions;
using Fred.Web.Models.Organisation;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des organisations
    /// </summary>
    public class OrganisationController : ApiControllerBase
    {
        private readonly IOrganisationManager organisationManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="OrganisationController" />.
        /// </summary>
        /// <param name="organisationMgr">Manager des organisations</param>
        /// <param name="userManager">Manager des utilisateurs</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>    
        public OrganisationController(IOrganisationManager organisationMgr, IMapper mapper)
        {
            this.organisationManager = organisationMgr;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération des organisations
        /// </summary>
        /// <param name="id">id de l'organisation</param>
        /// <returns>Retourne le détail de organisation</returns>
        [Route("api/Organisation/{id}")]
        [HttpGet]
        public HttpResponseMessage GetOrganisationByIdGet(int id)
        {
            return this.Get(() => this.mapper.Map<OrganisationModel>(this.organisationManager.GetOrganisationById(id)));
        }

        /// <summary>
        /// Méthode GET de récupération des organisations parents !!! ATTENTION ON S'ARRETE AU GROUPE !!!
        /// </summary>
        /// <param name="organisationId">id de l'organisation fille</param>
        /// <returns>Retourne les organisations parents</returns>
        [Route("api/Organisation/Parents")]
        [HttpGet]
        public HttpResponseMessage GetOrganisationParentsByOrgaIdMaxGroupe(int organisationId)
        {
            return this.Get(() => this.mapper.Map<List<OrganisationModel>>(this.organisationManager.GetOrganisationParentByOrganisationId(organisationId, OrganisationType.Groupe.ToIntValue())));
        }

        [HttpGet]
        [Route("api/Organisation/GetParents/{id}")]
        public HttpResponseMessage GetParentOrganisations(int id)
        {
            return this.Get(() =>
            {
                var listParents = new List<OrganisationModel>();
                var organisation = this.mapper.Map<OrganisationModel>(this.organisationManager.GetOrganisationById(id));
                listParents.Add(organisation);
                while (organisation.PereId.HasValue)
                {
                    organisation = this.mapper.Map<OrganisationModel>(this.organisationManager.GetOrganisationById(organisation.PereId.Value));
                    listParents.Add(organisation);
                }
                return listParents;
            });
        }

        /// <summary>
        /// Méthode GET de récupération des organisations
        /// </summary>
        /// <returns>Retourne la liste des organisations</returns>
        [HttpGet]
        [Route("api/Organisation/GetList")]
        public HttpResponseMessage GetList()
        {
            return this.Get(() => this.mapper.Map<IEnumerable<OrganisationModel>>(this.organisationManager.GetList()));
        }

        /// <summary>
        /// Méthode GET de récupération des organisations
        /// </summary>
        /// <returns>Retourne la liste des organisations</returns>
        [HttpGet]
        [Route("api/Organisation/GetTypesOrganisation")]
        public HttpResponseMessage GetTypesOrganisation()
        {
            return this.Get(() => this.mapper.Map<IEnumerable<TypeOrganisationModel>>(this.organisationManager.GetOrganisationTypesList()));
        }

        /// <summary>
        /// Méthode GET de récupération des seuils de commande
        /// </summary>
        /// <param name="organisationId">id de l'organisation recherchée</param>
        /// <param name="roleId">id du rôle</param>
        /// <returns>Retourne la liste des seuils de commandes</returns>
        [HttpGet]
        [Route("api/Organisation/GetThresholdByOrganisationIdAndRoleId/{organisationId}/{roleId?}")]
        public HttpResponseMessage GetSeuilsByOrganisationId(int organisationId, int? roleId)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<AffectationSeuilOrgaModel>>(this.organisationManager.GetSeuilByOrganisationId(organisationId, roleId)));
        }

        [HttpPost]
        [Route("api/Organisation/ThresholdOrganisation")]
        public HttpResponseMessage AddThresholdOrganisation(AffectationSeuilOrgaModel threshold)
        {
            return Post(() => this.mapper.Map<AffectationSeuilOrgaModel>(this.organisationManager.AddOrganisationThreshold(this.mapper.Map<AffectationSeuilOrgaEnt>(threshold))));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="threshold">SeuilOrganisation</param>    
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Organisation/ThresholdOrganisation")]
        public HttpResponseMessage UpdateThresholdOrganisation(AffectationSeuilOrgaModel threshold)
        {
            return this.Put(() => this.mapper.Map<AffectationSeuilOrgaModel>(this.organisationManager.UpdateThresholdOrganisation(this.mapper.Map<AffectationSeuilOrgaEnt>(threshold))));
        }

        [HttpDelete]
        [Route("api/Organisation/ThresholdOrganisation/{thresholdId}")]
        public HttpResponseMessage DeleteThresholdOrganisation(int thresholdId)
        {
            return this.Delete(() => this.organisationManager.DeleteThresholdOrganisationById(thresholdId));
        }

        [HttpGet]
        [Route("api/Organisation/GetRoleOrganisationDevise/{organisationId}/{roleId?}")]
        public HttpResponseMessage GetRoleOrganisationDevise(int organisationId, int? roleId)
        {
            return this.Get(() =>
            {
                return new List<AffectationSeuilOrgaModel>();
            });
        }

        /// <summary>
        /// Rechercher les organisation de type etablissement à société
        /// </summary>    
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>    
        /// <param name="typeOrganisation">Liste des type d'organisation</param>
        /// <param name="bypassUser">Ignore l'utilisateur ou pas</param>
        /// <param name="onlyCiNoClose"> seulement avec des ci non cloturé </param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Organisation/SearchLightOrganisation/{page?}/{pageSize?}/{recherche?}/{typeOrganisation?}/{bypassUser?}/{onlyCiNoClose?}/{forAnalytique?}")]
        public HttpResponseMessage SearchLightOrganisation(int page = 1, int pageSize = 20, string recherche = "", string typeOrganisation = "", bool bypassUser = false, bool onlyCiNoClose = true)
        {
            return this.Get(() =>
            {
                List<string> typeOrgaList = typeOrganisation.Split(',').ToList();
                return this.mapper.Map<IEnumerable<OrganisationLightModel>>(this.organisationManager.SearchLightOrganisation(page, pageSize, recherche, typeOrgaList, bypassUser, onlyCiNoClose));
            });
        }

        /// <summary>
        /// Recherche les orgas a partir d'une societeId
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <returns>etouner une liste  d'organisation</returns>
        [HttpGet]
        [Route("api/Organisation/SearchLightOrganisationById/{organisationId}")]
        public HttpResponseMessage SearchLightForSocieteId(int organisationId)
        {
            return this.Get(() =>
            {
                return this.mapper.Map<OrganisationLightModel>(this.organisationManager.GetOrganisationById(organisationId));
            });
        }

        /// <summary>
        /// Rechercher les organisations
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Organisation/SearchLight/{recherche?}")]
        public HttpResponseMessage SearchLight(int page, int pageSize, string recherche = "")
        {
            return this.Get(() => this.mapper.Map<IEnumerable<OrganisationLightModel>>(this.organisationManager.SearchLight(page, pageSize, recherche)));
        }


        /// <summary>
        /// Recherche les orgas a partir d'une societeId
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="recherche">recherche</param>
        /// <param name="typeOrganisation">typeOrganisation</param>
        /// <param name="societeId">societeId</param>
        /// <returns>etouner une liste  d'organisation</returns>
        [HttpGet]
        [Route("api/Organisation/SearchLightForSocieteId/{page?}/{pageSize?}/{recherche?}/{typeOrganisation?}/{societeId?}")]
        public HttpResponseMessage SearchLightForSocieteId(int page = 1, int pageSize = 20, string recherche = "", string typeOrganisation = "", int? societeId = null)
        {
            return this.Get(() =>
            {
                List<string> typeOrgaList = typeOrganisation.Split(',').ToList();
                return this.mapper.Map<IEnumerable<OrganisationLightModel>>(this.organisationManager.SearchLightForSocieteId(page, pageSize, recherche, typeOrgaList, societeId));
            });
        }

        /// <summary>
        /// Récuperer l'ensemble des societes dont l'utilisateur est habilité ou habilité sur leurs etab comptables
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <returns>List des organisation (societe)</returns>
        [HttpGet]
        [Route("api/Organisation/SearchLightSocieteOrganisationForEtabComptable/{page?}/{pageSize?}/{recherche?}")]
        public async Task<IHttpActionResult> SearchLightSocieteOrganisationForEtabComptable(int page, int pageSize, string recherche)
        {
            IEnumerable<OrganisationLightEnt> etabs = await organisationManager.SearchLightSocieteOrganisationForEtabComptable(page, pageSize, recherche);
            return this.Ok(this.mapper.Map<IEnumerable<OrganisationLightModel>>(etabs));
        }
    }
}
