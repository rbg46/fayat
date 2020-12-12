using System.Collections.Generic;
using Fred.Entities.Permission;

namespace Fred.Business.PermissionFonctionnalite
{
    /// <summary>
    /// Interface pour PermissionManager
    /// </summary>
    public interface IPermissionManager : IManager
    {
        /// <summary>
        /// Recupere la liste des permissions non utilisées
        /// </summary>
        /// <param name="text">text recherché</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns>Liste de permission</returns>
        IEnumerable<PermissionEnt> GetUnusedPermissions(string text, int page, int pageSize);

        /// <summary>
        /// Retourne une persmission en fonction de sa 
        /// </summary>
        /// <param name="permissionKey">la clé</param>
        /// <returns>La permission</returns>
        PermissionEnt GetPermissionByKey(string permissionKey);
    }
}
