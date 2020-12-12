using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI.Services.Interfaces;
using Fred.Business.Organisation.Tree;
using Fred.DataAccess.Permission;
using Fred.DataAccess.Rapport;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
namespace Fred.Business.CI.Services
{
    /// <summary>
    /// Service qui permet de savoir quel sont les cis accessibleS par un utilisateur
    /// </summary>
    public class CisAccessiblesService : ICisAccessiblesService
    {
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IRoleValidsForPermissionService roleValidsForPermissionService;
        private readonly IPermissionRepository permissionRepository;
        private readonly IRapportRepository rapportRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="organisationTreeService">organisationTreeService</param>      
        /// <param name="roleValidsForPermissionService">roleValidsForPermissionService</param>
        /// <param name="permissionRepository">permissionRepository</param>
        /// <param name="rapportRepository">rapport manager</param>
        /// 
        public CisAccessiblesService(IOrganisationTreeService organisationTreeService,
            IRoleValidsForPermissionService roleValidsForPermissionService,
            IPermissionRepository permissionRepository,
            IRapportRepository rapportRepository)
        {
            this.organisationTreeService = organisationTreeService;
            this.roleValidsForPermissionService = roleValidsForPermissionService;
            this.permissionRepository = permissionRepository;
            this.rapportRepository = rapportRepository;
        }

        /// <summary>
        ///  Permet de savoir quel sont les cis accessibleS par un utilisateur et pour une permission
        /// </summary>
        /// <param name="userId">utilisateur</param>
        /// <param name="permissionRequested">permission</param>
        /// <returns>Liste de cis</returns>
        public List<OrganisationBase> GetCisAccessiblesForUserAndPermission(int userId, string permissionRequested)
        {
            var permission = permissionRepository.GetPermissionByKey(permissionRequested);

            OrganisationTree organisationTree = organisationTreeService.GetOrganisationTree();

            List<int> rolesOfUser = organisationTree.GetRolesIdsOfUser(userId);

            List<FonctionnaliteTypeMode> modesAutorized = new List<FonctionnaliteTypeMode>
            {
                    FonctionnaliteTypeMode.Write,
                    FonctionnaliteTypeMode.Read
            };

            var rolesValidsForPermission = roleValidsForPermissionService.GetRoleIdsValidsForRolesAndPermission(rolesOfUser, permission.PermissionId, modesAutorized);

            List<OrganisationBase> cis = organisationTree.GetCisByUserAndRoles(userId, rolesValidsForPermission);

            return cis;
        }

        /// <summary>
        /// Filtre une liste d'identifiant ci pour ne récupérer que les ci apte à l'export
        /// </summary>
        /// <param name="ciIds">liste des identifiants ci</param>
        /// <returns>liste des identifiants ci filtré</returns>
        public List<int> GetCiIdsAvailablesForReceptionInterimaire(IEnumerable<int> ciIds)
        {
            return rapportRepository.GetCiIdsAvailablesForReceptionInterimaire(ciIds).ToList();
        }

    }
}
