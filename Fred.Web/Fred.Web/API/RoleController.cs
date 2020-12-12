using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Fonctionnalite;
using Fred.Business.Module;
using Fred.Business.Organisation.Tree;
using Fred.Business.Role;
using Fred.Business.RoleFonctionnalite;
using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Entities.Role;
using Fred.Web.Models.Fonctionnalite;
using Fred.Web.Models.Module;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Role;
using Fred.Web.Shared.Models.RoleFonctionnalite;

namespace Fred.Web.API
{
    /// <summary>
    ///   Représente une instance de contrôleur Web API pour le module "Role".
    ///   <seealso cref="ApiController" />
    /// </summary>
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleManager roleManager;

        private readonly IMapper mapper;
        private readonly IModuleManager moduleManager;
        private readonly IRoleFonctionnaliteManager roleFonctionnaliteManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IFonctionnaliteManager fonctionnaliteManager;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="RoleController" />.
        /// </summary>
        /// <param name="roleMgr">Manageur des rôles</param>
        /// <param name="mapper">Auto Mapper modèle / entité</param>
        /// <param name="roleFonctionnaliteManager">roleFonctionnaliteManager</param> 
        /// <param name="moduleManager">moduleManager</param>
        /// <param name="fonctionnaliteManager">fonctionnaliteManager</param>
        /// <param name="organisationTreeService">organisationTreeService</param>
        public RoleController(IRoleManager roleMgr,
          IModuleManager moduleManager,
          IFonctionnaliteManager fonctionnaliteManager,
          IMapper mapper,
          IRoleFonctionnaliteManager roleFonctionnaliteManager,
          IOrganisationTreeService organisationTreeService)
        {
            this.roleManager = roleMgr;
            this.mapper = mapper;
            this.moduleManager = moduleManager;
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
            this.organisationTreeService = organisationTreeService;
            this.fonctionnaliteManager = fonctionnaliteManager;
        }

        /// <summary>
        /// Recupere les roles pour une societe
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Liste de rôles</returns>
        [HttpGet]
        [Route("api/Role/GetRolesBySocieteId/{societeId}")]
        public HttpResponseMessage GetRolesBySocieteId(int societeId)
        {
            return Get(() => this.mapper.Map<IEnumerable<RoleModel>>(this.roleManager.GetRoleListBySocieteId(societeId)));
        }

        /// <summary>
        ///   GET api/Role/GetRoleListByOrganisationId
        /// </summary>
        /// <param name="organisationId">Identifiant Organisation</param>
        /// <returns>Liste de rôles par groupeId</returns>
        [HttpGet]
        [Route("api/Role/GetRoleListByOrganisationId/{organisationId}")]
        public HttpResponseMessage GetRoleListByOrganisationId(int organisationId)
        {
            return Get(() =>
            {
                var organisationTree = organisationTreeService.GetOrganisationTree();

                var userSocieteList = organisationTree.GetSocieteParent(organisationId);

                if (userSocieteList == null)
                {
                    return new List<RoleModel>();
                }

                return this.mapper.Map<IEnumerable<RoleModel>>(this.roleManager.GetRoleListBySocieteId(userSocieteList.Id).Where(r => r.Actif));

            });
        }

        /// <summary>
        ///   POST api/Role
        /// </summary>
        /// <param name="roleModel">Rôle a créer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Role")]
        public IHttpActionResult AddRole(RoleModel roleModel)
        {
            var role = this.roleManager.AddRole(this.mapper.Map<RoleEnt>(roleModel));
            return Created(string.Empty, role);

        }

        /// <summary>
        ///   PUT api/Role/{roleId}
        /// </summary>
        /// <param name="roleModel">Rôle à mettre à jour</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Role")]
        public HttpResponseMessage UpdateRole(RoleModel roleModel)
        {
            return Put(() => this.roleManager.UpdateRole(this.mapper.Map<RoleEnt>(roleModel)));
        }

        /// <summary>
        ///   DELETE api/Role/{roleId}
        /// </summary>
        /// <param name="roleId">Identifiant du rôle à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Role/{roleId}")]
        public HttpResponseMessage DeleteRole(int roleId)
        {
            return Delete(() => this.roleManager.DeleteRole(roleId));
        }

        /// <summary>
        ///   POST api/Role/DuplicateRole
        /// </summary>
        /// <param name="roleToDuplicate">Rôle à dupliquer</param>
        /// <param name="copythreshold">copie des seuils</param>
        /// <param name="copyRoleFeature">copie des roleFonctionnalite</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Role/DuplicateRole/{copythreshold}/{copyRoleFeature}")]
        public HttpResponseMessage DuplicateRole(RoleModel roleToDuplicate, bool copythreshold, bool copyRoleFeature)
        {
            return Post(() => this.roleManager.DuplicateRole(this.mapper.Map<RoleEnt>(roleToDuplicate), copythreshold, copyRoleFeature));
        }

        #region Gestion Seuil de validation

        /// <summary>
        ///   GET api/Role/SeuilValidationListByRoleId/{roleId}
        /// </summary>
        /// <param name="roleId">Identifiant du Rôle</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Role/SeuilValidationListByRoleId/{roleId}")]
        public HttpResponseMessage GetSeuilValidationsByRoleId(int roleId)
        {
            return Get(() => this.mapper.Map<IEnumerable<SeuilValidationModel>>(this.roleManager.GetSeuilValidationListByRoleId(roleId)));
        }

        /// <summary>
        ///   POST api/Role/AddSeuilValidation
        /// </summary>
        /// <param name="seuilValidationModel">Seuil à enregistrer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Role/AddSeuilValidation")]
        public HttpResponseMessage AddRoleSeuilValidation(SeuilValidationModel seuilValidationModel)
        {
            return Post(() => this.mapper.Map<SeuilValidationModel>(this.roleManager.AddSeuilValidation(this.mapper.Map<SeuilValidationEnt>(seuilValidationModel))));
        }

        /// <summary>
        ///   PUT api/Role/UpdateSeuilValidation
        /// </summary>
        /// <param name="seuil">Seuil de validation à mettre à jour</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Role/UpdateSeuilValidation")]
        public HttpResponseMessage UpdateSeuilValidation(SeuilValidationModel seuil)
        {
            return Put(() => this.mapper.Map<SeuilValidationModel>(this.roleManager.UpdateSeuilValidation(this.mapper.Map<SeuilValidationEnt>(seuil))));
        }

        /// <summary>
        ///   DELETE api/cRole/DeleteValidationThreshold/{seuilValidationId}
        /// </summary>
        /// <param name="seuilValidationId">Identifiant du seuil de validation à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Role/DeleteSeuilValidation/{seuilValidationId}")]
        public HttpResponseMessage DeleteSeuilValidationById(int seuilValidationId)
        {
            return Delete(() => this.roleManager.DeleteSeuilValidationById(seuilValidationId));
        }

        #endregion

        /// <summary>
        ///   Rechercher les rôles
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="societeId">id de la société</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/Role/SearchLight/{page?}/{pageSize?}/{recherche?}/{societeId?}")]
        public HttpResponseMessage SearchLight(int page = 1, int pageSize = 20, string recherche = "", int? societeId = null)
        {
            return Get(() => this.mapper.Map<IEnumerable<RoleModel>>(this.roleManager.SearchLight(recherche, page, pageSize, societeId)));
        }

        /// <summary>
        /// Clone les roles
        /// </summary>
        /// <param name="societeSourceId">societeSourceId</param>
        /// <param name="societeTargetId">societeTargetId</param>
        /// <param name="copyfeatures">copyfeatures</param>
        /// <param name="copythreshold">copythreshold</param>
        /// <returns>Liste de role</returns>
        [HttpGet]
        [Route("api/Role/cloneRoles/{societeSourceId}/{societeTargetId}/{copyfeatures}/{copythreshold}")]
        public HttpResponseMessage CloneRoles(int societeSourceId, int societeTargetId, bool copyfeatures, bool copythreshold)
        {
            return Get(() => this.mapper.Map<IEnumerable<RoleModel>>(this.roleManager.CloneRoles(societeSourceId, societeTargetId, copyfeatures, copythreshold)));
        }

        /// <summary>
        /// Recupere la liste des RoleFonctionnalites pour un role donné
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>Liste de modules</returns>
        [HttpGet]
        [Route("api/Role/GetRoleFonctionnalitesByRoleId/{roleId}")]
        public HttpResponseMessage GetRoleFonctionnalitesByRoleId(int roleId)
        {
            return Get(() => this.mapper.Map<IEnumerable<RoleFonctionnaliteModel>>(this.roleFonctionnaliteManager.GetRoleFonctionnalitesByRoleId(roleId)));
        }

        /// <summary>
        /// Retourne la liste des modules disponibles pour la societe.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>La liste des modules disponibles pour la societe.</returns>
        [HttpGet]
        [Route("api/Role/GetModulesAvailablesForSocieteId/{societeId}")]
        public HttpResponseMessage GetModulesAvailablesForSocieteId(int societeId)
        {
            return Get(() => this.mapper.Map<IEnumerable<ModuleModel>>(this.moduleManager.GetModulesAvailablesForSocieteId(societeId)));
        }

        /// <summary>
        /// Retourne la liste des modules disponibles pour la societe.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="moduleId">moduleId</param>
        /// <returns>La liste des fonctionnalite disponibles pour la societe.</returns>
        [HttpGet]
        [Route("api/Role/GetFonctionnaliteAvailablesForSocieteIdAndModuleId/{societeId}/{moduleId}")]
        public HttpResponseMessage GetFonctionnaliteAvailablesForSocieteId(int societeId, int moduleId)
        {
            return Get(() => this.mapper.Map<IEnumerable<FonctionnaliteModel>>(this.fonctionnaliteManager.GetFonctionnaliteAvailablesForSocieteIdAndModuleId(societeId, moduleId)));
        }

        /// <summary>
        /// Ajoute un lien entre un role et une fonctionnalité.
        /// </summary>
        /// <param name="roleFonctionnalite">roleFonctionnalite</param>
        /// <returns>Un RoleFonctionnaliteModel</returns>
        [HttpPost]
        [Route("api/Role/AddRoleFonctionnalite")]
        public HttpResponseMessage AddRoleFonctionnalite([FromBody]RoleFonctionnaliteModel roleFonctionnalite)
        {
            return Post(() => this.mapper.Map<RoleFonctionnaliteModel>(this.roleFonctionnaliteManager.AddRoleFonctionnalite(roleFonctionnalite.RoleId, roleFonctionnalite.FonctionnaliteId, roleFonctionnalite.Mode)));
        }

        /// <summary>
        /// Supprime le lien entre un role et une fonctionnalité.
        /// </summary>
        /// <param name="roleFonctionnaliteId">roleFonctionnaliteId</param>
        /// <returns>Rien</returns>
        [HttpDelete]
        [Route("api/Role/DeleteRoleFonctionnaliteById/{roleFonctionnaliteId}")]
        public HttpResponseMessage DeleteRoleFonctionnaliteById(int roleFonctionnaliteId)
        {
            return this.Delete(() => this.roleFonctionnaliteManager.DeleteRoleFonctionnaliteById(roleFonctionnaliteId));
        }

        /// <summary>
        /// Change le mode sur le RoleFonctionnaliteEnt
        /// </summary>
        /// <param name="roleFonctionnaliteId">roleFonctionnaliteId</param>
        /// <param name="mode">mode</param>
        /// <returns>RoleFonctionnaliteEnt</returns>
        [HttpPut]
        [Route("api/Role/ChangeMode/{roleFonctionnaliteId}/{mode}")]
        public HttpResponseMessage ChangeMode(int roleFonctionnaliteId, FonctionnaliteTypeMode mode)
        {
            return this.Put(() => this.roleFonctionnaliteManager.ChangeMode(roleFonctionnaliteId, mode));
        }

        /// <summary>
        /// Retourne un role roleFonctionnalite
        /// </summary>
        /// <param name="roleFonctionnaliteId">roleFonctionnaliteId</param>
        /// <returns>RoleFonctionnaliteEnt</returns>
        [HttpGet]
        [Route("api/Role/GetRoleFonctionnaliteDetail/{roleFonctionnaliteId}")]
        public HttpResponseMessage GetRoleFonctionnaliteDetail(int roleFonctionnaliteId)
        {
            return this.Get(() => this.mapper.Map<RoleFonctionnaliteModel>(this.roleFonctionnaliteManager.GetRoleFonctionnaliteDetail(roleFonctionnaliteId)));
        }
    }
}
