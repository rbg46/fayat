using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Permission;

namespace Fred.Business.PermissionFonctionnalite
{
    /// <summary>
    /// Manager des PermissionEnt
    /// </summary>
    public class PermissionManager : Manager<PermissionEnt, IPermissionRepository>, IPermissionManager
    {
        private readonly IPermissionFonctionnaliteRepository permissionFonctionnaliteRepo;

        public PermissionManager(IUnitOfWork uow, IPermissionRepository permissionRepository, IPermissionFonctionnaliteRepository permissionFonctionnaliteRepo)
          : base(uow, permissionRepository)
        {
            this.permissionFonctionnaliteRepo = permissionFonctionnaliteRepo;
        }

        /// <summary>
        /// Retourne une persmission en fonction de sa 
        /// </summary>
        /// <param name="permissionKey">la clé</param>
        /// <returns>La permission</returns>
        public PermissionEnt GetPermissionByKey(string permissionKey)
        {
            return this.Repository.GetPermissionByKey(permissionKey);
        }

        /// <summary>
        ///  Recupere la liste des permissions non utilisées
        /// </summary>
        /// <param name="text">text recherché</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns>Liste de permission</returns>
        public IEnumerable<PermissionEnt> GetUnusedPermissions(string text, int page, int pageSize)
        {
            IEnumerable<int> permissionAlreadyUsed = permissionFonctionnaliteRepo.Query().Get().Select(pf => pf.PermissionId).ToList();
            return Repository.GetUnusedPermissions(text, page, pageSize, permissionAlreadyUsed);
        }
    }
}
