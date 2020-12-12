using System.Collections.Generic;
using Fred.Entities;
namespace Fred.Business.CI.Services.Interfaces
{
    /// <summary>
    /// Recherche les roles valides/actif
    /// </summary>
    public interface IRoleValidsForPermissionService : IService
    {
        /// <summary>
        /// Recherche les roles valides pour une permission particuliere et une liste de mode.
        /// Un role valide est un role qui est actif, donc la fonctionnalité est active, dont le module parent est  actif aussi.
        /// Un role est valide si la 'fonctionnalite'(terme metier) est dans un mode passé en parametre. 
        /// </summary>
        /// <param name="rolesIdsRequested">Ids des roles dans laquelle on cherche a savoir s'ils sont valides</param>
        /// <param name="permissionIdRequested">La permission</param>
        /// <param name="modesAutorized">Le mode que l'on considere comme valide</param>
        /// <returns>Liste d'ids des roles valides</returns>
        List<int> GetRoleIdsValidsForRolesAndPermission(List<int> rolesIdsRequested, int permissionIdRequested, List<FonctionnaliteTypeMode> modesAutorized);

        /// <summary>
        /// Recherche les roles valides pour une permission particuliere et une liste de societes.
        /// Un role valide est un role qui est actif, donc la fonctionnalité est active, dont le module parent est  actif aussi.
        /// Un role est valide si la 'fonctionnalite'(terme metier) est dans un mode passé en parametre. 
        /// </summary>
        /// /// <param name="societeId">Id de la societes dans laquels on cherche</param>
        /// <param name="permissionIdRequested">La permission</param>
        /// <param name="modesAutorized">Le mode que l'on considere comme valide</param>
        /// <returns>Liste d'ids des roles valides</returns>
        List<int> GetRoleIdsValidsForSocieteAndPermission(int societeId, int permissionIdRequested, FonctionnaliteTypeMode modesAutorized);
    }
}
