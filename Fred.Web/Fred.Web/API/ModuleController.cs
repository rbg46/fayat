using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Fonctionnalite;
using Fred.Business.FonctionnaliteDesactive;
using Fred.Business.Module;
using Fred.Business.ModuleDesactive;
using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Business.PermissionFonctionnalite;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.Module;
using Fred.Web.Models.Fonctionnalite;
using Fred.Web.Models.Module;
using Fred.Web.Models.Organisation;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Module;

namespace Fred.Web.API
{
    public class ModuleController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IPermissionFonctionnaliteManager permissionFonctionnaliteManager;
        private readonly IFonctionnaliteManager fonctionnaliteManager;
        private readonly IModuleDesactiveManager moduleDesactiveManager;
        private readonly IOrganisationManager organisationManager;
        private readonly IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager;
        private readonly IPermissionManager permissionManager;
        private readonly IModuleManager moduleManager;

        public ModuleController(
            IMapper mapper,
            IOrganisationTreeService organisationTreeService,
            IPermissionFonctionnaliteManager permissionFonctionnaliteManager,
            IFonctionnaliteManager fonctionnaliteManager,
            IModuleDesactiveManager moduleDesactiveManager,
            IOrganisationManager organisationManager,
            IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager,
            IPermissionManager permissionManager,
            IModuleManager moduleManager)
        {
            this.mapper = mapper;
            this.organisationTreeService = organisationTreeService;
            this.permissionFonctionnaliteManager = permissionFonctionnaliteManager;
            this.fonctionnaliteManager = fonctionnaliteManager;
            this.moduleDesactiveManager = moduleDesactiveManager;
            this.organisationManager = organisationManager;
            this.fonctionnaliteDesactiveManager = fonctionnaliteDesactiveManager;
            this.permissionManager = permissionManager;
            this.moduleManager = moduleManager;
        }

        /// <summary>
        ///   GET api/Module
        /// </summary>
        /// <returns>Liste de modules</returns>
        [HttpGet]
        [Route("api/Module")]
        public HttpResponseMessage GetModuleList()
        {
            return Get(() => this.mapper.Map<IEnumerable<ModuleModel>>(this.moduleManager.GetModuleList()));
        }

        /// <summary>
        ///   GET api/Module/{moduleId}
        /// </summary>
        /// <param name="moduleId">Identifiant du Module</param>
        /// <returns>Liste de modules</returns>
        [HttpGet]
        [Route("api/Module/{moduleId}")]
        public HttpResponseMessage GetModuleById(int moduleId)
        {
            return Get(() => this.mapper.Map<ModuleModel>(this.moduleManager.GetModuleById(moduleId)));
        }

        /// <summary>
        ///   POST api/Module
        /// </summary>
        /// <param name="module">Le module à créer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Module")]
        public HttpResponseMessage CreateModule(ModuleModel module)
        {
            return Post(() => this.mapper.Map<ModuleModel>(this.moduleManager.AddModule(this.mapper.Map<ModuleEnt>(module))));
        }

        /// <summary>
        ///   GET api/Module/GetFeatureListByModuleId/{moduleId}
        /// </summary>
        /// <param name="moduleId">Identifiant du Module</param>
        /// <returns>Liste de fonctionnalités</returns>
        [HttpGet]
        [Route("api/Module/GetFeatureListByModuleId/{moduleId}")]
        public HttpResponseMessage GetFeatureListByModuleId(int moduleId)
        {
            return Get(() => this.mapper.Map<IEnumerable<FonctionnaliteModel>>(this.fonctionnaliteManager.GetFeatureListByModuleId(moduleId)));
        }

        /// <summary>
        ///   POST api/Module/AddFeature
        /// </summary>
        /// <param name="feature">Fonctionnalité à créer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Module/AddFeature")]
        public HttpResponseMessage AddFeature(FonctionnaliteModel feature)
        {
            return Post(() => this.mapper.Map<FonctionnaliteModel>(this.fonctionnaliteManager.AddFeature(this.mapper.Map<FonctionnaliteEnt>(feature))));
        }

        /// <summary>
        ///   PUT api/Module
        /// </summary>
        /// <param name="module">Module à mettre à jour</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Module")]
        public HttpResponseMessage UpdateModule(ModuleModel module)
        {
            return Put(() => this.mapper.Map<ModuleModel>(this.moduleManager.UpdateModule(this.mapper.Map<ModuleEnt>(module))));
        }

        /// <summary>
        ///   PUT api/Module/UpdateFeature
        /// </summary>
        /// <param name="feature">Fonctionnalité à mettre à jour</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Module/UpdateFeature")]
        public HttpResponseMessage UpdateFeature(FonctionnaliteModel feature)
        {
            return Put(() => this.mapper.Map<FonctionnaliteModel>(this.fonctionnaliteManager.UpdateFeature(this.mapper.Map<FonctionnaliteEnt>(feature))));
        }

        /// <summary>
        ///   DELETE api/Module/{id}
        /// </summary>
        /// <param name="id">Identifiant du module</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Module/{id}")]
        public HttpResponseMessage DeleteModuleById(int id)
        {
            return Delete(() => this.moduleManager.DeleteModuleById(id));
        }

        /// <summary>
        ///   DELETE api/Module/DeleteFeatureById/{id}
        /// </summary>
        /// <param name="id">Identifiant de la fonctionnalité</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Module/DeleteFeatureById/{id}")]
        public HttpResponseMessage DeleteFeatureById(int id)
        {
            return Delete(() => this.fonctionnaliteManager.DeleteFeatureById(id));
        }


        /// <summary>
        /// retourne la liste des PermissionFonctionnaliteModel
        /// </summary>
        /// <param name="fonctionnaliteId">id de la fonctionnalité</param>
        /// <returns>List de PermissionModel</returns>
        [HttpGet]
        [Route("api/Module/GetPermissionFonctionnalites/{fonctionnaliteId}")]
        public HttpResponseMessage GetPermissionFonctionnalites(int fonctionnaliteId)
        {
            return Get(() => this.mapper.Map<IEnumerable<PermissionFonctionnaliteModel>>(this.permissionFonctionnaliteManager.GetPermissionFonctionnalites(fonctionnaliteId)));
        }

        /// <summary>
        /// Permet de savoir si on peux Rajouter une permission a une fonctionnalité.
        /// Une Permission est lié à une et une seule fonctionnalité. 
        /// </summary> 
        /// <param name="permissionId">permissionId</param>   
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Module/CanAddPermissionFonctionnalite/{permissionId}")]
        public HttpResponseMessage CanAddPermissionFonctionnalite(int permissionId)
        {
            return Get(() =>
            {
                bool canAdd = this.permissionFonctionnaliteManager.CanAdd(permissionId);
                if (canAdd)
                {
                    return new { canAdd = true };
                }
                return new { canAdd = false };
            });
        }

        /// <summary>
        /// Recupere la liste des permissions non utilisées
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="recherche">recherche</param>
        /// <returns>Une liste de permission</returns>
        [HttpGet]
        [Route("api/Module/GetUnusedPermissions/{page?}/{pageSize?}/{recherche?}")]
        public HttpResponseMessage GetUnusedPermissions(int page = 1, int pageSize = 20, string recherche = "")
        {
            return Get(() => this.mapper.Map<IEnumerable<PermissionModel>>(this.permissionManager.GetUnusedPermissions(recherche, page, pageSize)));

        }

        /// <summary>
        ///  Ajout d'une relation PermissionFonctionnaliteModel
        /// </summary> 
        /// <param name="permissionId">permissionId</param>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Module/AddPermissionFonctionnalite/{permissionId}/{fonctionnaliteId}")]
        public HttpResponseMessage AddPermissionFonctionnalite(int permissionId, int fonctionnaliteId)
        {
            return Post(() => this.mapper.Map<PermissionFonctionnaliteModel>(this.permissionFonctionnaliteManager.Add(permissionId, fonctionnaliteId)));
        }

        /// <summary>
        /// Suppression d'une PermissionFonctionnaliteModel
        /// </summary>
        /// <param name="permissionFonctionnaliteId">permissionFonctionnaliteId</param>
        /// <returns>Rien</returns>
        [HttpDelete]
        [Route("api/Module/DeletePermissionFonctionnalite/{permissionFonctionnaliteId}")]
        public HttpResponseMessage DeletePermissionFonctionnalite(int permissionFonctionnaliteId)
        {
            return Delete(() => this.permissionFonctionnaliteManager.Delete(permissionFonctionnaliteId));
        }

        /// <summary>
        /// Retourne la liste des modules desactivé pour une societe
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Liste de module desactive</returns>
        [HttpGet]
        [Route("api/Module/GetInactifModulesForSocieteId/{societeId}")]
        public HttpResponseMessage GetInactifModulesForSocieteId(int societeId)
        {
            return Get(() => this.moduleDesactiveManager.GetInactifModulesForSocieteId(societeId));
        }

        /// <summary>
        /// Retourne la liste des modules desactivé pour une societe
        /// </summary>    
        /// <returns>Liste de module desactive</returns>
        [HttpGet]
        [Route("api/Module/GetModulesAndFonctionnalitesPartiallyDisabled")]
        public HttpResponseMessage GetModulesAndFonctionnalitesPartiallyDisabled()
        {
            return Get(() =>
            {
                IEnumerable<int> idsOfModulesPartiallyDisabled = this.moduleDesactiveManager.GetIdsOfModulesPartiallyDisabled();
                IEnumerable<int> idsOfFonctionnalitesPartiallyDisabled = this.fonctionnaliteDesactiveManager.GetIdsOfFonctionnalitesPartiallyDisabled();
                return new
                {
                    idsOfModulesPartiallyDisabled,
                    idsOfFonctionnalitesPartiallyDisabled
                };
            });
        }

        /// <summary>
        /// Desactive un module pour une societe.
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="societeId">societeId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Module/DisableModuleForSocieteId/{moduleId}/{societeId}")]
        public HttpResponseMessage DisableModuleForSoceiteId(int moduleId, int societeId)
        {
            return Post(() => this.moduleDesactiveManager.DisableModuleForSocieteId(moduleId, societeId));
        }

        /// <summary>
        /// Active  un module pour une societe.
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="societeId">societeId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Module/EnableModuleForSoceiteId/{moduleId}/{societeId}")]
        public HttpResponseMessage EnableModuleForSocieteId(int moduleId, int societeId)
        {
            return Delete(() => this.moduleDesactiveManager.EnableModuleForSocieteId(moduleId, societeId));
        }

        #region  GESTION DE L ARBO

        /// <summary>
        /// Rechercher les organisation a partir de l'arganisation pere
        /// </summary>    
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste d'organisation</returns>   
        [HttpGet]
        [Route("api/Module/GetOrganisationTreeForSocieteId/{page}/{pageSize}/{societeId}")]
        public HttpResponseMessage GetOrganisationTreeForSocieteId(int page, int pageSize, int societeId)
        {
            return this.Get(() =>
            {
                var organisationTree = organisationTreeService.GetOrganisationTree();

                var pole = organisationTree.GetPoleParentOfSociete(societeId);

                var result = this.mapper.Map<IEnumerable<OrganisationLightModel>>(this.organisationManager.GetOrganisationsForPole(page, pageSize, pole.OrganisationId));
                result = result.ToList();
                return result;
            });
        }

        /// <summary>
        /// Recuepere la liste des societe inactive pour un module donne
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <returns>Liste d'id de societes desactivés</returns>
        [HttpGet]
        [Route("api/Module/GetInactivesSocietesForModuleId/{moduleId}")]
        public HttpResponseMessage GetInactivesSocietesForModuleId(int moduleId)
        {
            return Get(() => this.moduleDesactiveManager.GetInactivesSocietesForModuleId(moduleId));
        }

        /// <summary>
        /// Recuepere la liste des societe inactive pour une fonctionnalite donnée
        /// </summary>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <returns>Liste d'id de societes desactivés</returns>
        [HttpGet]
        [Route("api/Module/GetInactivesSocietesForFonctionnaliteId/{fonctionnaliteId}")]
        public HttpResponseMessage GetInactivesSocietesForFonctionnaliteId(int fonctionnaliteId)
        {
            return Get(() => this.fonctionnaliteDesactiveManager.GetInactivesSocietesForFonctionnaliteId(fonctionnaliteId));
        }

        /// <summary>
        ///  Active et desactive les societes pour un module donné.
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="enableDisableInfoModel">organisationIds des societes a désactiver et activer</param>
        /// <returns>Les ids des societes activés et désactivé lors de la requette</returns>
        [HttpPost]
        [Route("api/Module/EnableOrDisableModuleByOrganisationIdsOfSocietesAndModuleId/{moduleId}")]
        public HttpResponseMessage EnableOrDisableModuleByOrganisationIdsOfSocietesAndModuleId(int moduleId, EnableDisableInfoModel enableDisableInfoModel)
        {
            return Post(() =>
            {
                var disables = this.moduleDesactiveManager.DisableModuleByOrganisationIdsOfSocietesAndModuleId(moduleId, enableDisableInfoModel.OrganisationIdsOfSocietesToDisable);
                var enables = this.moduleDesactiveManager.EnableModuleByOrganisationIdsOfSocietesAndModuleId(moduleId, enableDisableInfoModel.OrganisationIdsOfSocietesToEnable);
                return new { societeIdsEnables = enables, societeIdsDisables = disables };
            });
        }

        /// <summary>
        ///  Active et desactive les societes pour un fonctionnalite donné.
        /// </summary>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <param name="enableDisableInfoModel">organisationIds des societes a désactiver et activer</param>
        /// <returns>Les ids des societes activés et désactivé lors de la requette</returns>
        [HttpPost]
        [Route("api/Module/EnableOrDisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId/{fonctionnaliteId}")]
        public HttpResponseMessage EnableOrDisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(int fonctionnaliteId, EnableDisableInfoModel enableDisableInfoModel)
        {
            return Post(() =>
            {
                var disables = this.fonctionnaliteDesactiveManager.DisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(fonctionnaliteId, enableDisableInfoModel.OrganisationIdsOfSocietesToDisable);
                var enables = this.fonctionnaliteDesactiveManager.EnableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(fonctionnaliteId, enableDisableInfoModel.OrganisationIdsOfSocietesToEnable);
                return new { societeIdsEnables = enables, societeIdsDisables = disables };
            });
        }
        #endregion
    }
}
