
using System.Collections.Generic;
using Fred.Entities.Permission;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repo de Permission
    /// </summary>
    public interface IPermissionRepository : IFredRepository<PermissionEnt>
    {
        /// <summary>
        /// Retourne une persmission en fonction de sa 
        /// </summary>
        /// <param name="permissionKey">la clé</param>
        /// <returns>La permission</returns>
        PermissionEnt GetPermissionByKey(string permissionKey);

        /// <summary>
        /// Retourne la liste des permissions non utilisées
        /// </summary>
        /// <param name="text">text à comparer</param>
        /// <param name="page">numero de page</param>
        /// <param name="pageSize">taille de page</param>
        /// <param name="permissionAlreadyUsed">liste des permissions déjà utilisé</param>
        /// <returns>permissions non utilisé</returns>
        IEnumerable<PermissionEnt> GetUnusedPermissions(string text, int page, int pageSize, IEnumerable<int> permissionAlreadyUsed);
    }
}