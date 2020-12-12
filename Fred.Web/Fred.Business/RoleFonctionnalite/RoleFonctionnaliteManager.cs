using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Role;
using Fred.Entities.RoleFonctionnalite;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.RoleFonctionnalite
{
    /// <summary>
    ///   Gestionnaire des rôles
    /// </summary>
    public class RoleFonctionnaliteManager : Manager<RoleFonctionnaliteEnt, IRoleFonctionnaliteRepository>, IRoleFonctionnaliteManager
    {
        public RoleFonctionnaliteManager(IUnitOfWork uow, IRoleFonctionnaliteRepository roleFonctionnaliteRepository)
          : base(uow, roleFonctionnaliteRepository)
        {
        }

        /// <summary>
        /// Recupere la liste des RoleFonctionnalite pour un role donné.
        /// </summary>
        /// <param name="roleId">Identifiant du rôle associé</param>
        /// <returns> Une liste de roleFonctionnalites</returns>
        public IEnumerable<RoleFonctionnaliteEnt> GetRoleFonctionnalitesByRoleId(int roleId)
        {
            return this.Repository.Query()
                      .Include(rf => rf.Fonctionnalite)
                      .Include(rf => rf.Fonctionnalite.Module)
                      .Filter(rf => rf.RoleId == roleId).Get()
                      .AsNoTracking().ToList();
        }

        /// <summary>
        /// Recupere la liste des RoleFonctionnalite pour une liste de role donné.
        /// </summary>
        /// <param name="roleIdList">Liste des identifiants de rôle</param>
        /// <returns> Une liste de roleFonctionnalites</returns>
        public IEnumerable<RoleFonctionnaliteEnt> GetRoleFonctionnalitesByRoles(IEnumerable<int> roleIdList)
        {
            return this.Repository.Query()
                      .Include(rf => rf.Fonctionnalite)
                      .Include(rf => rf.Fonctionnalite.Module)
                      .Filter(rf => roleIdList.Contains(rf.RoleId)).Get()
                      .AsNoTracking().ToList();
        }

        /// <summary>
        ///   Ajoute un nouveau RoleFonctionnalite
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <param name="mode">mode</param>  
        /// <returns>RoleFonctionnalite crée</returns>
        public RoleFonctionnaliteEnt AddRoleFonctionnalite(int roleId, int fonctionnaliteId, FonctionnaliteTypeMode mode)
        {
            var roleFonctionnalite = this.GetByRoleIdAndFonctionnaliteId(roleId, fonctionnaliteId);
            if (roleFonctionnalite != null)
            {
                throw new FredBusinessConflictException("Le lien entre le role et la fonctionnalite existe déjà.");
            }

            var result = new RoleFonctionnaliteEnt()
            {
                RoleId = roleId,
                FonctionnaliteId = fonctionnaliteId,
                Mode = mode
            };
            this.Repository.Insert(result);
            this.Save();
            Repository.PerformEagerLoading(result, x => x.Fonctionnalite);
            return result;
        }

        /// <summary>
        /// Recupere la liste des RoleFonctionnalite pour un role et une fonctionnalité donnés.
        /// </summary>
        /// <param name="roleId">Identifiant du rôle associé</param>
        /// <param name="fonctionnaliteId">Identifiant d'une fonctionnalité associé</param>
        /// <returns> Un roleFonctionnalite</returns>
        public RoleFonctionnaliteEnt GetByRoleIdAndFonctionnaliteId(int roleId, int fonctionnaliteId)
        {
            return this.Repository
              .Query()
              .Filter(rf => rf.RoleId == roleId && rf.FonctionnaliteId == fonctionnaliteId)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        ///   Supprimer un RoleFonctionnalite
        /// </summary>
        /// <param name="roleFonctionnaliteId">Identifiant du RoleFonctionnalite a supprimer.</param>
        public void DeleteRoleFonctionnaliteById(int roleFonctionnaliteId)
        {
            var roleFonctionnalite = this.Repository.FindById(roleFonctionnaliteId);
            if (roleFonctionnalite == null)
            {
                throw new FredBusinessNotFoundException("Le lien entre le role et la fonctionnalite n'existe pas ou plus.");
            }
            this.Repository.DeleteById(roleFonctionnaliteId);
            this.Save();
        }

        /// <summary>
        /// Duplique les roleFonctionnalite d'un role a un autre.
        /// </summary>
        /// <param name="sourceRoleId">sourceRoleId</param>
        /// <param name="targetRoleId">targetRoleId</param>
        public void DuplicateRoleFonctionnalites(int sourceRoleId, int targetRoleId)
        {
            var roleFonctionnalites = GetRoleFonctionnalitesByRoleId(sourceRoleId).ToList();
            foreach (RoleFonctionnaliteEnt roleFonctionnalite in roleFonctionnalites)
            {
                RoleFonctionnaliteEnt newRoleFonctionnalite = new RoleFonctionnaliteEnt()
                {
                    RoleId = targetRoleId,
                    FonctionnaliteId = roleFonctionnalite.FonctionnaliteId,
                    Mode = roleFonctionnalite.Mode
                };

                this.Repository.Insert(newRoleFonctionnalite);
            }
        }

        /// <summary>
        /// Change le mode sur le RoleFonctionnaliteEnt
        /// </summary>
        /// <param name="roleFonctionnaliteId">roleFonctionnaliteId</param>
        /// <param name="mode">mode</param>
        /// <returns>RoleFonctionnaliteEnt</returns>
        public RoleFonctionnaliteEnt ChangeMode(int roleFonctionnaliteId, FonctionnaliteTypeMode mode)
        {
            var roleFonctionnalite = this.Repository.FindById(roleFonctionnaliteId);
            if (roleFonctionnalite == null)
            {
                throw new FredBusinessNotFoundException("Le lien entre le role et la fonctionnalite n'existe pas ou plus.");
            }
            roleFonctionnalite.Mode = mode;
            this.Save();
            return roleFonctionnalite;
        }

        /// <summary>
        /// Retourne un roleFonctionnalite
        /// </summary>
        /// <param name="roleFonctionnaliteId">roleFonctionnaliteId</param>
        /// <returns>RoleFonctionnaliteEnt</returns>
        public RoleFonctionnaliteEnt GetRoleFonctionnaliteDetail(int roleFonctionnaliteId)
        {
            return this.Repository.Query()
                                  .Include(rf => rf.Fonctionnalite)
                                  .Include(rf => rf.Fonctionnalite.Module)
                                  .Filter(rf => rf.RoleFonctionnaliteId == roleFonctionnaliteId)
                                  .Get()
                                  .AsNoTracking()
                                  .FirstOrDefault();
        }


        /// <summary>
        /// Get role et fonctionnalite by utilisateur id and fonctionnalite libelle
        /// </summary>
        /// <param name="userId">Utilisateur Id</param>
        /// <param name="fonctionnaliteLibelle">Fonctionnalite libelle</param>
        /// <returns>List des roles fonctionnalites</returns>
        public async Task<List<RoleFonctionnaliteEnt>> GetRoleFonctionnaliteByUserIdAsync(int userId, string fonctionnaliteLibelle)
        {
            return await this.Repository.GetRoleFonctionnaliteByUserIdAsync(userId, fonctionnaliteLibelle).ConfigureAwait(false);
        }
                
        public async Task<List<RoleEnt>> GetByUserIdAndListFonctionnaliteAsync(int userId, List<string> fonctionnaliteCodeList)
        {
            IEnumerable<RoleFonctionnaliteEnt> roleFonctionnaliteList = await Repository.GetByUserIdAndListFonctionnaliteAsync(userId, fonctionnaliteCodeList).ConfigureAwait(false);
            return roleFonctionnaliteList?.Select(x=>x.Role).ToList();
        }
    }
}
