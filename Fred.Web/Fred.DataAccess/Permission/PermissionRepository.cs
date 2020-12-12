using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Permission;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Permission
{
    /// <summary>
    /// Repo de l'entite permission 
    /// </summary>
    public class PermissionRepository : FredRepository<PermissionEnt>, IPermissionRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logMgr">logMgr</param>
        /// <param name="context">context</param>
        public PermissionRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retourne une persmission en fonction de sa 
        /// </summary>
        /// <param name="permissionKey">la clé</param>
        /// <returns>La permission</returns>
        public PermissionEnt GetPermissionByKey(string permissionKey)
        {
            return this.context.Permissions.Where(x => x.PermissionKey == permissionKey).AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Retourne la liste des permissions non utilisées
        /// </summary>
        /// <param name="text">text à comparer</param>
        /// <param name="page">numero de page</param>
        /// <param name="pageSize">taille de page</param>
        /// <param name="permissionAlreadyUsed">liste des permissions déjà utilisé</param>
        /// <returns>permissions non utilisé</returns>
        public IEnumerable<PermissionEnt> GetUnusedPermissions(string text, int page, int pageSize, IEnumerable<int> permissionAlreadyUsed)
        {
            return Query().Filter(permission => string.IsNullOrEmpty(text) ||
                                  permission.PermissionKey.Equals(text, StringComparison.OrdinalIgnoreCase) ||
                                  permission.Code.Equals(text, StringComparison.OrdinalIgnoreCase) ||
                                  permission.Libelle.Equals(text, StringComparison.OrdinalIgnoreCase))
                .Filter(permission => !permissionAlreadyUsed.Contains(permission.PermissionId))
                .OrderBy(o => o.OrderBy(oo => oo.Code))
                .GetPage(page, pageSize);
        }
    }
}
