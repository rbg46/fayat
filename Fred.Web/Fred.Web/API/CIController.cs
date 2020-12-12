using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Carburant;
using Fred.Business.CI;
using Fred.Business.CI.Services.Interfaces;
using Fred.Business.ExternalService.Ci;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.ReferentielEtendu;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.Permission;
using Fred.Entities.Utilisateur;
using Fred.Web.Models;
using Fred.Web.Models.Carburant;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Societe;

namespace Fred.Web.API
{
    public class CIController : ApiControllerBase
    {
        private readonly ICIManager ciManager;
        private readonly IHabilitationForCiManager habilitationForCiManager;
        private readonly ISearchCiService searchCiService;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IReferentielEtenduManager referentielEtenduManager;
        private readonly ICiManagerExterne ciManagerExterne;
        private readonly IMapper mapper;
        private readonly IParametrageCarburantManager paramCarburantManager;

        public CIController(
          ICIManager ciManager,
          IMapper mapper,
          IParametrageCarburantManager paramCarburantManager,
          IHabilitationForCiManager habilitationForCiManager,
          ISearchCiService searchCiService,
          IUtilisateurManager utilisateurManager,
          IReferentielEtenduManager referentielEtenduManager,
          ICiManagerExterne ciManagerExterne)
        {
            this.ciManager = ciManager;
            this.mapper = mapper;
            this.paramCarburantManager = paramCarburantManager;
            this.habilitationForCiManager = habilitationForCiManager;
            this.searchCiService = searchCiService;
            this.utilisateurManager = utilisateurManager;
            this.referentielEtenduManager = referentielEtenduManager;
            this.ciManagerExterne = ciManagerExterne;
        }

        #region Gestion des CI

        /// <summary>
        ///   GET Récupération de la liste complète des CI
        /// </summary>
        /// <returns>Retourne la liste des cis</returns>
        [HttpGet]
        [Route("api/CI")]
        public HttpResponseMessage GetCI()
        {
            return Get(() => mapper.Map<IEnumerable<CIModel>>(ciManager.GetCIList()));
        }

        /// <summary>
        ///   GET Récupération de la liste des CI d'une Organisation en fonction de la période
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <param name="period">Période</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <remarks>Attention: la date est Locale. On a besoin que du mois et de l'année</remarks>
        /// <returns>Retourne la liste des cis</returns>
        [HttpGet]
        [Route(@"api/CI/GetCIListByOrganisationIdAndPeriod/{organisationId}/{period:datetime}/{page?}/{pageSize?}")]
        public HttpResponseMessage GetCIListByOrganisationId(int organisationId, DateTime period, int page = 1, int pageSize = 10)
        {
            return Get(() => mapper.Map<IEnumerable<CIModel>>(ciManager.GetCIList(organisationId, period, page, pageSize)));
        }

        /// <summary>
        ///   GET Récupération d'un CI en fonction de son identifiant
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne un CI</returns>
        [HttpGet]
        [Route("api/CI/{ciId}")]
        public HttpResponseMessage GetCIById(int ciId)
        {
            return Get(() => mapper.Map<CIModel>(ciManager.GetCIById(ciId, overrideSocietyByOrganisationParent: true)));
        }

        /// <summary>
        ///   GET Récupération d'un CI en fonction de son identifiant
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation Id du CI</param>
        /// <returns>Retourne un CI</returns>
        [HttpGet]
        [Route("api/CI/Organisation/{organisationId}")]
        public HttpResponseMessage GetCIByOrganisationId(int organisationId)
        {
            return Get(() => mapper.Map<CILightModel>(ciManager.GetCiByOrganisationId(organisationId)));
        }

        /// <summary>
        ///   PUT Mise à jour d'un CI
        /// </summary>        
        /// <param name="ciModel">CI à mettre à jour</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/CI")]
        public async Task<IHttpActionResult> UpdateCI(CIModel ciModel)
        {
            var updatedCi = await ciManagerExterne.OnUpdateCiAsync(ciModel.CiId, () => ciManager.UpdateCI(mapper.Map<CIEnt>(ciModel)));

            return Ok(mapper.Map<CIModel>(updatedCi));
        }

        /// <summary>
        ///   GET Récupération d'un filtre de recherche sur CI
        /// </summary>
        /// <returns>Retourne un filtre de recherche sur CI</returns>
        [HttpGet]
        [Route("api/CI/Filter")]
        public HttpResponseMessage GetFilter()
        {
            return Get(() => mapper.Map<SearchCIModel>(ciManager.GetFilter()));
        }

        /// <summary>
        ///   POST Récupération des résultats de la recherche en fonction du filtre
        /// </summary>
        /// <param name="filters">Filtre du CI</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/CI/SearchWithFilters/{page?}/{pageSize?}")]
        public HttpResponseMessage SearchWithFilters(SearchCIModel filters, int page = 1, int pageSize = 20)
        {
            filters.PermissionKey = PermissionKeys.AffichageMenuCIIndex;
            return Post(() =>
            {
                var searchResult = searchCiService.SearchForCiListView(mapper.Map<SearchCIEnt>(filters), page, pageSize);
                return mapper.Map<IEnumerable<CIModel>>(searchResult);
            });
        }

        /// <summary>
        ///   GET Rechercher les référentiels CI
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="personnelSocieteId">Identifiant de la société du Personnel</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CI/SearchLight/{page?}/{pageSize?}/{recherche?}/{personnelSocieteId?}")]
        public async Task<IHttpActionResult> SearchLightAsync(int page = 1, int pageSize = 20, string recherche = "", int? personnelSocieteId = null)
        {
            UtilisateurEnt currentUser = await utilisateurManager.GetContextUtilisateurAsync().ConfigureAwait(false);
            IEnumerable<CIEnt> searchLightCi = await ciManager.SearchLightAsync(recherche, page, pageSize, personnelSocieteId).ConfigureAwait(false);

            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES)
            {
                return Ok(mapper.Map<IEnumerable<CIFullLibelleModel>>(searchLightCi));
            }

            return Ok(mapper.Map<IEnumerable<CIModel>>(searchLightCi));
        }

        /// <summary>
        /// GET Rechercher les référentiels CI
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CI/SearchLightForSpecificCi/{page?}/{pageSize?}/{recherche?}")]
        public async Task<IHttpActionResult> SearchLightForSpecificCiAsync(int page = 1, int pageSize = 20, string recherche = "")
        {
            UtilisateurEnt currentUser = await utilisateurManager.GetContextUtilisateurAsync();

            int? personnelSocieteId = currentUser.Personnel.SocieteId;

            return Ok(await ciManager.SearchLightModelAsync(recherche, page, pageSize, personnelSocieteId));
        }

        /// <summary>
        ///   GET Rechercher les référentiels CI
        /// </summary>
        /// <param name="personnelSocieteId">Identifiant de la société du Personnel</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="includeSep">inclusion des SEP</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CI/SearchLightBySocieteId/{page?}/{pageSize?}/{recherche?}/{personnelSocieteId?}/{includeSep?}")]
        public HttpResponseMessage SearchLightBySocieteId(int personnelSocieteId, int page = 1, int pageSize = 20, string recherche = "", bool includeSep = true)
        {
            var currentUser = utilisateurManager.GetContextUtilisateur();
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES)
            {
                return Get(() => mapper.Map<IEnumerable<CIFullLibelleModel>>(ciManager.GetCisOfUserFilteredBySocieteId(recherche, page, pageSize, personnelSocieteId, includeSep)));
            }

            return Get(() => mapper.Map<IEnumerable<CIModel>>(ciManager.GetCisOfUserFilteredBySocieteId(recherche, page, pageSize, personnelSocieteId, includeSep)));
        }

        /// <summary>
        ///   GET Rechercher les référentiels CI Compte Interne SEP
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CI/SearchLight/CompteInterneSep/{page?}/{pageSize?}/{recherche?}/{ciId?}")]
        public HttpResponseMessage SearchLightCompteInterneSep(int page = 1, int pageSize = 20, string recherche = "", int ciId = 0)
        {
            return Get(() => mapper.Map<IEnumerable<CIModel>>(ciManager.SearchLightCompteInterneSep(recherche, page, pageSize, ciId)));
        }

        /// <summary>
        ///   GET Rechercher les CI pour les affectations des moyens en fonction des rôles de l'utilisateur
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CiForAffectationMoyen/SearchLight/{page?}/{pageSize?}/{recherche?}")]
        public HttpResponseMessage CiSearchLightForAffectationMoyen(int page = 1, int pageSize = 20, string recherche = "")
        {
            return Get(() => mapper.Map<IEnumerable<CIFullLibelleModel>>(ciManager.CiSearchLightForAffectationMoyen(recherche, page, pageSize)));
        }

        /// <summary>
        ///   GET Rechercher les référentiels CI
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="personnelId">personnel Id</param>
        /// <param name="societeId">societe Id</param>
        /// <returns>retouner une liste de CI by PersonnelId</returns>
        [HttpGet]
        [Route("api/CI/SearchLightByPersonnel/{personnelId?}/{page?}/{pageSize?}/{recherche?}/{societeId?}")]
        public async Task<IHttpActionResult> SearchLightByPersonnelAsync(int page = 1, int pageSize = 20, string recherche = "", int personnelId = 0, int? societeId = null)
        {
            if (personnelId != 0)
            {
                return Ok(mapper.Map<IEnumerable<CIFullLibelleModel>>(await ciManager.SearchLightByPersonnelId(recherche, page, pageSize, personnelId, societeId).ConfigureAwait(false)));
            }
            return Ok(mapper.Map<IEnumerable<CIFullLibelleModel>>(await ciManager.SearchLightAsync(recherche, page, pageSize).ConfigureAwait(false)));
        }

        /// <summary>
        ///   GET Rechercher les référentiels CI
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="interimaireId">Identifiant intérimaire</param>
        /// <param name="date">Date du pointage</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CI/SearchLightForInterimaire/{page?}/{pageSize?}/{recherche?}/{interimaireId?}/{date?}")]
        public HttpResponseMessage SearchLightForInterimaire(int page = 1, int pageSize = 20, string recherche = "", int interimaireId = 0, DateTime? date = null)
        {
            return Get(() => mapper.Map<IEnumerable<CIModel>>(ciManager.SearchLightForInterimaire(recherche, page, pageSize, interimaireId, date.Value)));
        }

        /// <summary>
        ///   GET Rechercher les référentiels CI liés à un budget
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="periodeApplication">période de mise en application</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CI/SearchLightForBudget/{page?}/{pageSize?}/{periodeApplication?}")]
        public HttpResponseMessage SearchLightForBudget(int page = 1, int pageSize = 20, string recherche = "", int? periodeApplication = null)
        {
            return Get(() => mapper.Map<IEnumerable<CIModel>>(ciManager.SearchLightForBudget(recherche, page, pageSize, periodeApplication)));
        }

        #endregion

        /// <summary>
        ///   GET Récupère la société d'un CI en fonction de son identifiant.
        /// </summary>
        /// <param name="ciId">Identifiant du ci</param>
        /// <returns>La société</returns>
        [HttpGet]
        [Route("api/CI/GetSocieteByCIId/{ciId}")]
        public HttpResponseMessage GetSocieteByCIId(int ciId)
        {
            return Get(() => mapper.Map<SocieteModel>(ciManager.GetSocieteByCIId(ciId)));
        }

        #region Gestion des Devises

        /// <summary>
        ///   GET Récupération des relations CI/Devise d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant de la relation CI/Devise</param>
        /// <returns>Retourne la liste des CI/Devise</returns>
        [HttpGet]
        [Route("api/CI/Devise/{ciId}")]
        public HttpResponseMessage GetCIDeviseList(int ciId)
        {
            return Get(() => mapper.Map<List<CIDeviseModel>>(ciManager.GetCIDevise(ciId)));
        }

        /// <summary>
        ///   GET Récupération de la devise de référence d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la devise de référence</returns>
        [HttpGet]
        [Route("api/CI/DeviseRef/{ciId}")]
        public HttpResponseMessage GetDeviseRef(int ciId)
        {
            return Get(() => mapper.Map<DeviseModel>(ciManager.GetDeviseRef(ciId)));
        }

        /// <summary>
        ///   GET Récupération de la devise de référence d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la devise de référence</returns>
        [HttpGet]
        [Route("api/CI/IsCiHaveManyDevises/{ciId}")]
        public HttpResponseMessage IsCiHaveManyDevises(int ciId)
        {
            return Get(() => ciManager.IsCiHaveManyDevises(ciId));
        }

        /// <summary>
        ///   GET Récupère la liste des devises secondaires d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/CI/DeviseSec/{ciId}")]
        public HttpResponseMessage GetCIDeviseSec(int ciId)
        {
            return Get(() => mapper.Map<IEnumerable<DeviseModel>>(ciManager.GetCIDeviseSecList(ciId)));
        }

        /// <summary>
        ///   GET Mise à jour d'une relation CI/Devise d'un CI
        /// </summary>
        /// <param name="ciModel">Identifiant de la relation CI/Devise</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/CI/Devise/{ciId}")]
        public HttpResponseMessage UpdateCIDevise(CIDeviseModel ciModel)
        {
            return Get(() => mapper.Map<List<CIDeviseModel>>(ciManager.UpdateCIDevise(mapper.Map<CIDeviseEnt>(ciModel))));
        }

        /// <summary>
        ///   POST Ajout ou Mise à jour des devises d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ciDeviseList">Liste des relations CI Devises</param>
        /// <returns>Met à jour les relations CI Sociétés pour un Ci donné</returns>
        [HttpPost]
        [Route("api/CI/ManageCIDevise/{ciId}")]
        public HttpResponseMessage ManageCIDevise(int ciId, IEnumerable<CIDeviseModel> ciDeviseList)
        {
            return Post(() => mapper.Map<IEnumerable<CIDeviseModel>>(ciManager.ManageCIDevise(ciId, mapper.Map<IEnumerable<CIDeviseEnt>>(ciDeviseList))));
        }

        #endregion

        #region Gestion des Carburants (Prix du carburant + Consommation des Ressources)

        /// <summary>
        ///   Récupère la liste des Ressources matérielles d'un CI sous forme d'une liste de chapitre
        /// </summary>
        /// <param name="ciId">Identifiant d'un CI</param>
        /// <param name="societeId">Identifiant de la société du CI</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/CI/Ressource/{ciId}/{societeId}")]
        public HttpResponseMessage GetCIRessourceList(int ciId, int societeId)
        {
            return Get(() => mapper.Map<IEnumerable<ChapitreModel>>(referentielEtenduManager.GetAllCIRessourceListAsChapitreList(ciId, societeId)));
        }

        /// <summary>
        ///   POST Ajout ou mise à jour des ressources d'un CI
        /// </summary>    
        /// <param name="ciRessourceList">Liste des relations CI Ressources</param>
        /// <returns>Met à jour les relations CI Ressource pour un Ci donné</returns>
        [HttpPost]
        [Route("api/CI/ManageCIRessource")]
        public HttpResponseMessage ManageCIRessource(IEnumerable<CIRessourceModel> ciRessourceList)
        {
            return Post(() => mapper.Map<IEnumerable<CIRessourceModel>>(ciManager.ManageCIRessource(mapper.Map<IEnumerable<CIRessourceEnt>>(ciRessourceList))));
        }

        /// <summary>
        ///   Récupère la liste des paramétrages du carburants au niveau du CI en fonction de la devise et de l'organisation
        /// </summary>
        /// <param name="organisationId">Identifiant d'une organisation</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpGet]
        [Route("api/CI/ParametrageCarburant/{organisationId}/{deviseId}")]
        public HttpResponseMessage GetParametrageCarburantList(int organisationId, int deviseId)
        {
            return Get(() => mapper.Map<IEnumerable<CarburantModel>>(paramCarburantManager.GetParametrageCarburantListAsCarburantList(organisationId, deviseId)));
        }

        /// <summary>
        ///   POST Gestion des paramétrages carburants
        /// </summary>
        /// <param name="paramCarburantList">Liste de paramétrage carburant à mettre à jour</param>    
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/CI/ManageParametrageCarburant")]
        public HttpResponseMessage ManageParametrageCarburant(IEnumerable<CarburantOrganisationDeviseModel> paramCarburantList)
        {
            return Post(() => mapper.Map<IEnumerable<CarburantOrganisationDeviseModel>>(paramCarburantManager.ManageParametrageCarburant(mapper.Map<IEnumerable<CarburantOrganisationDeviseEnt>>(paramCarburantList))));
        }

        #endregion

        [HttpGet]
        [Route("api/CI/GetContextualAuthorization/{ciId}/{permissionKey}")]
        public HttpResponseMessage GetContextualAuthorization(int ciId, string permissionKey)
        {
            return Get(() =>
            {
                IEnumerable<PermissionEnt> permissions = habilitationForCiManager.GetContextualAuthorization(ciId, permissionKey);
                return permissions?.Any() == true ? permissions.First(perm => perm.Mode == permissions.Max(p => p.Mode)) : null;
            });
        }
    }
}
