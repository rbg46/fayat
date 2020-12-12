using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI.Services.Interfaces;
using Fred.DataAccess.Interfaces;
using Fred.Entities;

namespace Fred.Business.CI.Services
{
    /// <summary>
    /// Recherche les roles valides/actif
    /// </summary>
    public class RoleValidsForPermissionService : IRoleValidsForPermissionService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IFonctionnaliteRepository fonctionnaliteRepository;

        /// <summary>
        /// ctor
        /// </summary>       
        /// <param name="roleRepository">roleRepository</param>
        /// <param name="fonctionnaliteRepository">fonctionnaliteRepository</param>
        public RoleValidsForPermissionService(IRoleRepository roleRepository,
            IFonctionnaliteRepository fonctionnaliteRepository)
        {

            this.roleRepository = roleRepository;
            this.fonctionnaliteRepository = fonctionnaliteRepository;
        }

        /// <summary>
        /// Recherche les roles valides pour une permission particuliere et une liste de mode.
        /// Un role valide est un role qui est actif, donc la fonctionnalité est active, dont le module parent est  actif aussi.
        /// Un role est valide si la 'fonctionnalite'(terme metier) est dans un mode passé en parametre. 
        /// </summary>
        /// <param name="rolesIdsRequested">Ids des roles dans laquelle on cherche a savoir s'ils sont valides</param>
        /// <param name="permissionIdRequested">La permission</param>
        /// <param name="modesAutorized">Le mode que l'on considere comme valide</param>
        /// <returns>Liste d'ids des roles valides</returns>
        public List<int> GetRoleIdsValidsForRolesAndPermission(List<int> rolesIdsRequested, int permissionIdRequested, List<FonctionnaliteTypeMode> modesAutorized)
        {
            var societeIds = roleRepository.GetRoleByIds(rolesIdsRequested).Select(x => x.SocieteId).ToList();

            return GetRoleIdsValidsForPermission(permissionIdRequested, societeIds, modesAutorized);
        }

        /// <summary>
        /// Recherche les roles valides pour une permission particuliere et une liste de societes.
        /// Un role valide est un role qui est actif, donc la fonctionnalité est active, dont le module parent est  actif aussi.
        /// Un role est valide si la 'fonctionnalite'(terme metier) est dans un mode passé en parametre. 
        /// </summary>
        /// /// <param name="societeId">Id de la societes dans laquels on cherche</param>
        /// <param name="permissionIdRequested">La permission</param>
        /// <param name="modesAutorized">Le mode que l'on considere comme valide</param>
        /// <returns>Liste d'ids des roles valides</returns>
        public List<int> GetRoleIdsValidsForSocieteAndPermission(int societeId, int permissionIdRequested, FonctionnaliteTypeMode modesAutorized)
        {
            return GetRoleIdsValidsForPermission(permissionIdRequested, new List<int>() { societeId }, new List<FonctionnaliteTypeMode>() { modesAutorized });
        }

        private List<int> GetRoleIdsValidsForPermission(int permissionIdRequested, List<int> societeIds, List<FonctionnaliteTypeMode> modesAutorized)
        {
            var result = new List<int>();

            var fonctionnaliteInactives = fonctionnaliteRepository.GetFonctionnalitesInactives(societeIds);

            var fonctionnalitesForPermissions = fonctionnaliteRepository.GetFonctionnalitesForPermission(permissionIdRequested, societeIds, modesAutorized);

            var fonctionnalitesWithPermission = fonctionnalitesForPermissions.Select(x => x.FonctionnaliteId).Distinct().ToList();

            foreach (var fonctionnaliteWithPermission in fonctionnalitesWithPermission)
            {
                foreach (var societeId in societeIds)
                {
                    var fonctionnalitesDesactiveOfSociete = fonctionnaliteInactives.Where(x => x.SocieteId == societeId);

                    var isExcluded = fonctionnalitesDesactiveOfSociete.Any(x => x.FonctionnaliteId == fonctionnaliteWithPermission);

                    if (!isExcluded)
                    {
                        var fonctionnalitesOfSociete = fonctionnalitesForPermissions
                                                            .Where(x => x.FonctionnaliteId == fonctionnaliteWithPermission && x.SocieteId == societeId)
                                                            .ToList();

                        var roleWithFonctionnalite = fonctionnalitesOfSociete.Select(x => x.RoleId).ToList();

                        result.AddRange(roleWithFonctionnalite);
                    }
                }
            }

            return result;
        }
    }
}
