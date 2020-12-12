using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.PermissionFonctionnalite;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.PermissionFonctionnalite
{
    public class PermissionFonctionnaliteManager : Manager<PermissionFonctionnaliteEnt, IPermissionFonctionnaliteRepository>, IPermissionFonctionnaliteManager
    {
        private const string PermissionFonctionnaliteCacheKey = "PermissionsFonctionnalites";

        private readonly ICacheManager cacheManager;

        public PermissionFonctionnaliteManager(
            IUnitOfWork uow,
            IPermissionFonctionnaliteRepository permissionFonctionnaliteRepository,
            ICacheManager cacheManager)
            : base(uow, permissionFonctionnaliteRepository)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Ajoute une jointure entre permission et fonctionnalite
        /// </summary>
        /// <param name="permissionId">permissionId</param>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <returns>Le nouvel element</returns>
        public PermissionFonctionnaliteEnt Add(int permissionId, int fonctionnaliteId)
        {
            PermissionFonctionnaliteEnt permissionFonctionnaliteEnt = new PermissionFonctionnaliteEnt
            {
                PermissionId = permissionId,
                FonctionnaliteId = fonctionnaliteId
            };
            this.Repository.Insert(permissionFonctionnaliteEnt);
            this.Save();
            cacheManager.Remove(PermissionFonctionnaliteCacheKey);
            return permissionFonctionnaliteEnt;
        }

        /// <summary>
        /// Permet de savoir si on peux Rajouter une permission a une fonctionnalité.
        /// Une Permission est lié à une et une seule fonctionnalité. 
        /// </summary>
        /// <param name="permissionId">permissionId</param>   
        /// <returns>boolean</returns>
        public bool CanAdd(int permissionId)
        {
            PermissionFonctionnaliteEnt element = this.Repository.Query().Filter(pf => pf.PermissionId == permissionId).Get().FirstOrDefault();
            return element == null;
        }

        /// <summary>
        /// Supprime une PermissionFonctionnaliteEnt
        /// </summary>
        /// <param name="permissionFonctionnaliteId">permissionFonctionnaliteId</param>
        public void Delete(int permissionFonctionnaliteId)
        {
            if (this.Repository.FindById(permissionFonctionnaliteId) != null)
            {
                this.Repository.DeleteById(permissionFonctionnaliteId);
                this.Save();
                cacheManager.Remove(PermissionFonctionnaliteCacheKey);
            }
            else
            {
                throw new FredBusinessNotFoundException("Cet element n'existe pas.");
            }
        }

        /// <summary>
        /// Suppression physique des PermissionFonctionnalites en fonction de l'id de la fonctionnalité. 
        /// </summary>
        /// <param name="fonctionnaliteId">Id de la fonctionnalite</param>
        public void DeletePermissionFonctionnaliteListByFonctionnaliteId(int fonctionnaliteId)
        {
            var permissionFonctionnalites = GetPermissionFonctionnalites(fonctionnaliteId).ToList();

            foreach (var permissionFonctionnalite in permissionFonctionnalites)
            {
                this.Repository.DeleteById(permissionFonctionnalite.PermissionFonctionnaliteId);
            }
            this.Save();
            cacheManager.Remove(PermissionFonctionnaliteCacheKey);
        }

        /// <summary>
        ///  Retourne la table de jointure entre permission et fonctionnalite
        /// </summary>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <returns>liste de PermissionFonctionnaliteEnt</returns>
        public IEnumerable<PermissionFonctionnaliteEnt> GetPermissionFonctionnalites(int fonctionnaliteId)
        {
            return GetAllPermissionFonctionnalites().Where(pf => pf.FonctionnaliteId == fonctionnaliteId);
        }

        /// <summary>
        /// Retourne l'ensemble de la table de jointure entre permission et fonctionnalite
        /// </summary>
        /// <returns>Liste de PermissionFonctionnaliteEnt</returns>
        public IEnumerable<PermissionFonctionnaliteEnt> GetAllPermissionFonctionnalites()
        {
            IEnumerable<PermissionFonctionnaliteEnt> permissionFonctionnalites = cacheManager.GetOrCreate(
                PermissionFonctionnaliteCacheKey,
                () => null as IEnumerable<PermissionFonctionnaliteEnt>,
                new TimeSpan(0, 6, 0, 0, 0));

            if (permissionFonctionnalites == null)
            {
                cacheManager.Remove(PermissionFonctionnaliteCacheKey);
                permissionFonctionnalites = cacheManager.GetOrCreate(
                    PermissionFonctionnaliteCacheKey,
                    () => Repository.Query()
                        .Include(pf => pf.Permission)
                        .Get()
                        .AsNoTracking()
                        .ToList(),
                    new TimeSpan(0, 6, 0, 0, 0));
            }

            return permissionFonctionnalites;
        }
    }
}
