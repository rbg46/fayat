
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Utilisateur;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour la gestion des utilisateurs et de la sécurité applicative.
    /// </summary>
    public interface IUtilisateurRepository : IRepository<UtilisateurEnt>
    {
        /// <summary>
        ///   Retourne la liste des utilisateurs.
        /// </summary>
        /// <returns>La liste des utilisateurs.</returns>
        IEnumerable<UtilisateurEnt> GetList();

        /// <summary>
        ///   Retourne la liste des utilisateurs pour la synchronisation vers le mobile.
        /// </summary>
        /// <returns>La liste des utilisateurs.</returns>
        IEnumerable<UtilisateurEnt> GetListSync();

        /// <summary>
        ///   Retourne l'utilisateur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à trouver.</param>
        /// <returns>La fiche de l'utilisateur, sinon nulle.</returns>
        UtilisateurEnt GetById(int id);

        /// <summary>
        ///   Retourne l'utilisateur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à trouver.</param>
        /// <returns>La fiche de l'utilisateur, sinon nulle.</returns>
        UtilisateurEnt GetByIdAsNoTracking(int id);

        /// <summary>
        ///   Retourne la liste des tache.
        /// </summary>
        /// <returns>Liste des tache.</returns>
        IEnumerable<UtilisateurEnt> GetUtilisateurList();

        /// <summary>
        ///   Supprime un Utilisateur
        /// </summary>
        /// <param name="utilisateur">Utilisateur à supprimer</param>    
        /// <remarks>Attention, il s'agit d'une suppression logique et non physique</remarks>
        void DeleteUtilisateur(UtilisateurEnt utilisateur);

        /// <summary>
        ///   Renvoi la liste des affectations de rôle pour un utilisateur
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur dont on veut les affectaions</param>
        /// <returns>Une liste des affectations</returns>
        IEnumerable<AffectationSeuilUtilisateurEnt> GetAffectationRoles(int utilisateurId);

        /// <summary>
        ///   Retourne l'utilisateur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à trouver.</param>
        /// <returns>La fiche de l'utilisateur, sinon nulle.</returns>
        UtilisateurEnt GetUtilisateurById(int id);

        /// <summary>
        ///   Retourne l'utilisateur portant l'identifiant unique indiqué de manière async
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à trouver.</param>
        /// <returns>La fiche de l'utilisateur, sinon nulle.</returns>
        Task<UtilisateurEnt> GetUtilisateurByIdAsync(int id);

        /// <summary>
        ///   Met à jour les rôle d'un Utilisateur
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur à mettre à jour</param>
        /// <param name="listAffectations">liste des affectations de l'utilisateur</param>
        void UpdateRole(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations);

        /// <summary>
        /// Update des roles pour une liste d'utilisateurs à la fois . Avec un seul appel au UnitOfWork.Save()
        /// </summary>
        /// <param name="affectationListByUser">Affectation list by user</param>
        void UpdateRoleForUtilisateurList(Dictionary<int, List<AffectationSeuilUtilisateurEnt>> affectationListByUser);

        /// <summary>
        /// Remove role affecation for user and Ci
        /// </summary>
        /// <param name="utilisateurIdList">List des utilisateur id</param>
        /// <param name="organisationId">Organisation Id</param>
        /// <param name="roleId">Role Id</param>
        void RemoveRoleAffectationForCiUserList(IEnumerable<int> utilisateurIdList, int organisationId, int roleId);

        /// <summary>
        ///   Vérifie que l'utiisateur est SuperAdmin
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur</param>
        /// <returns>Vrai si l'utilisateur possède un rôle SuperAdmin</returns>
        bool IsSuperAdmin(int utilisateurId);

        bool DoesFolioExist(int userId, string folio, int userCompanyId);

        /// <summary>
        /// Vérifier si un personnel est un utilisateur Fred
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>Boolean indique si le personnel est un utilisateur Fred</returns>
        bool IsFredUser(int personnelId);

        /// <summary>
        ///   Retourne l'Utilisateur portant le login de l'Utilisateur.
        /// </summary>
        /// <param name="login">Login de  l'Utilisateur à trouver.</param>
        /// <returns>La fiche de l'Utilisateur, sinon nulle.</returns>
        UtilisateurEnt GetByLogin(string login);

        /// <summary>
        ///   Retourne l'utilisateur en fonction de son login.
        /// </summary>
        /// <param name="login">Login de l'utilisateur</param>
        /// <returns>L'utilisateur retrouvé en fonction de son login pour la vue ResetPassword</returns>
        UtilisateurEnt GetByLoginForResetPassword(string login);

        /// <summary>
        /// Retourne la liste des Ci dont l'utilisateur est reponsable : Delegue ou responsable CI
        /// </summary>
        /// <param name="roles">Delegue role id</param>
        /// <param name="utilisateurId">Utilisateur Id</param>
        /// <returns>IEnumerable of Ci objects dont l'utilisateur est responsable</returns>
        IEnumerable<CIEnt> GetCiListForRoles(IEnumerable<int> roles, int utilisateurId);

        /// <summary>
        /// Get ci list for delegue role
        /// </summary>
        /// <param name="utilisateurId">Utilisateur Id</param>
        /// <returns>IEnumerable of CiEnt</returns>
        IEnumerable<CIEnt> GetCiListForDelegue(int utilisateurId);

        /// <summary>
        /// Get affectation list of user (Delegue ou reponsable CI)
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="roles">Identifiant des rôles liés aux affectations</param>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <returns>IEnumerable of personnel Ent</returns>
        IEnumerable<AffectationEnt> GetAffectationList(int userId, IEnumerable<int> roles, string personnelStatut);

        // <summary>
        /// Modifie une liste d'utlidateur
        /// </summary>
        /// <param name="utilisateurList">Lsie d'utilisateur à modifier</param>
        /// <returns></returns>
        void UpdateUtilisateurList(IEnumerable<UtilisateurEnt> utilisateurList);
    }
}
