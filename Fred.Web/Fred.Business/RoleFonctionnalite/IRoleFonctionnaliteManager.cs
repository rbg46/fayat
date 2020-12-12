using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Entities.Role;
using Fred.Entities.RoleFonctionnalite;

namespace Fred.Business.RoleFonctionnalite
{
    /// <summary>
    ///   Interface du gestionnaire des rôles.
    /// </summary>
    public interface IRoleFonctionnaliteManager : IManager<RoleFonctionnaliteEnt>
    {
        /// <summary>
        /// Recupere la liste des RoleFonctionnalite pour un role donné.
        /// </summary>
        /// <param name="roleId">Identifiant du rôle associé</param>
        /// <returns> Une liste de roleFonctionnalites</returns>
        IEnumerable<RoleFonctionnaliteEnt> GetRoleFonctionnalitesByRoleId(int roleId);

        /// <summary>
        /// Recupere la liste des RoleFonctionnalite pour une liste de role donné.
        /// </summary>
        /// <param name="roleIdList">Liste des identifiants de rôle</param>
        /// <returns> Une liste de roleFonctionnalites</returns>
        IEnumerable<RoleFonctionnaliteEnt> GetRoleFonctionnalitesByRoles(IEnumerable<int> roleIdList);

        /// <summary>
        /// Recupere la liste des RoleFonctionnalite pour un role et une fonctionnalité donnés.
        /// </summary>
        /// <param name="roleId">Identifiant du rôle associé</param>
        /// <param name="fonctionnaliteId">Identifiant d'une fonctionnalité associé</param>
        /// <returns> Un roleFonctionnalite</returns>
        RoleFonctionnaliteEnt GetByRoleIdAndFonctionnaliteId(int roleId, int fonctionnaliteId);

        /// <summary>
        ///   Ajoute un nouveau RoleFonctionnalite
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <param name="mode">type de mode affecté a une fonctionnalite</param>  
        /// <returns>RoleFonctionnalite crée</returns>
        RoleFonctionnaliteEnt AddRoleFonctionnalite(int roleId, int fonctionnaliteId, FonctionnaliteTypeMode mode);

        /// <summary>
        ///   Supprimer un RoleFonctionnalite
        /// </summary>
        /// <param name="roleFonctionnaliteId">Identifiant du RoleFonctionnalite a supprimer.</param>
        void DeleteRoleFonctionnaliteById(int roleFonctionnaliteId);

        /// <summary>
        /// Duplique les roleFonctionnalite d'un role a un autre.
        /// </summary>
        /// <param name="sourceRoleId">sourceRoleId</param>
        /// <param name="targetRoleId">targetRoleId</param>
        void DuplicateRoleFonctionnalites(int sourceRoleId, int targetRoleId);

        /// <summary>
        /// Change le mode sur le RoleFonctionnaliteEnt
        /// </summary>
        /// <param name="roleFonctionnaliteId">roleFonctionnaliteId</param>
        /// <param name="mode">mode</param>
        /// <returns>RoleFonctionnaliteEnt</returns>
        RoleFonctionnaliteEnt ChangeMode(int roleFonctionnaliteId, FonctionnaliteTypeMode mode);

        /// <summary>
        /// Retourne un role roleFonctionnalite
        /// </summary>
        /// <param name="roleFonctionnaliteId">roleFonctionnaliteId</param>
        /// <returns>RoleFonctionnaliteEnt</returns>
        RoleFonctionnaliteEnt GetRoleFonctionnaliteDetail(int roleFonctionnaliteId);

        /// <summary>
        /// Get role et fonctionnalite by utilisateur id and fonctionnalite libelle
        /// </summary>
        /// <param name="userId">Utilisateur Id</param>
        /// <param name="fonctionnaliteLibelle">Fonctionnalite libelle</param>
        /// <returns>List des roles fonctionnalites</returns>
        Task<List<RoleFonctionnaliteEnt>> GetRoleFonctionnaliteByUserIdAsync(int userId, string fonctionnaliteLibelle);
                
        Task<List<RoleEnt>> GetByUserIdAndListFonctionnaliteAsync(int userId, List<string> fonctionnaliteCodeList);
    }
}
