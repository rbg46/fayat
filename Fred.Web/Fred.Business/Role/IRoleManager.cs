using Fred.Entities.Referential;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;
using System.Collections.Generic;

namespace Fred.Business.Role
{
    /// <summary>
    ///   Interface du gestionnaire des rôles.
    /// </summary>
    public interface IRoleManager : IManager<RoleEnt>
    {
        /// <summary>
        /// Retourne la liste des rôles pour un code.
        /// </summary>
        /// <param name="code">Le code du rôle.</param>
        /// <returns>Liste des rôles pour une code.</returns>
        IEnumerable<RoleEnt> GetRoles(string code);

        /// <summary>
        ///   Retourne la liste des rôles par societe
        /// </summary>
        /// <param name="societeId">Identifiant du societe</param>
        /// <returns>Liste des rôles d'un societe</returns>
        IEnumerable<RoleEnt> GetRoleListBySocieteId(int societeId);

        /// <summary>
        ///   Ajoute un nouveau rôle
        /// </summary>
        /// <param name="roleEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du rôle ajouté</returns>
        RoleEnt AddRole(RoleEnt roleEnt);

        /// <summary>
        ///   Met à jour un rôle
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>Role mis à jour</returns>
        /// <exception cref="System.Exception">
        ///   Les caractères spéciaux ne sont pas autorisés.
        ///   or
        ///   Le code et le libellé sont obligatoires.
        /// </exception>
        RoleEnt UpdateRole(RoleEnt role);

        /// <summary>
        ///   Supprime un rôle
        /// </summary>
        /// <param name="roleId">Identifiant du rôle à supprimer</param>
        void DeleteRole(int roleId);

        /// <summary>
        ///   Duplique un rôle ainsi que toutes les associations connexes (modules et seuils)
        /// </summary>
        /// <param name="roleEnt">Rôle à dupliquer</param>
        /// <param name="copythreshold">copie des seuils</param>
        /// <param name="copyRoleFeature">copie des roleFonctionnalites</param>
        /// <returns> L'identifiant du rôle dupliqué</returns> 
        RoleEnt DuplicateRole(RoleEnt roleEnt, bool copythreshold, bool copyRoleFeature);

        /// <summary>
        ///   Recherche de role dans le référentiel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="societeId">Identifiant du groupe auquel appartient le rôle</param>
        /// <returns>Une liste de personnel</returns>
        IEnumerable<RoleEnt> SearchLight(string text, int page, int pageSize, int? societeId);

        /// <summary>
        /// Clone les roles
        /// </summary>
        /// <param name="societeSourceId">societeSourceId</param>
        /// <param name="societeTargetId">societeTargetId</param>
        /// <param name="copyfeatures">copyfeatures</param>
        /// <param name="copythreshold">copythreshold</param>
        /// <returns>Liste de role</returns>
        IEnumerable<RoleEnt> CloneRoles(int societeSourceId, int societeTargetId, bool copyfeatures, bool copythreshold);

        /// <summary>
        /// Get utilisateur role list
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="roleId">Role id</param>
        /// <returns>IEnumerable of AffectationSeuilUtilisateurEnt</returns>
        IEnumerable<AffectationSeuilUtilisateurEnt> GetUtilisateurRoleList(int utilisateurId, int roleId);

        /// <summary>
        /// Get delegue role by organisation id
        /// </summary>
        /// <param name="organisationId">Organisation id</param>
        /// <returns>Delegue role id by organisation</returns>
        RoleEnt GetDelegueRoleByOrganisationId(int organisationId);

        /// <summary>
        /// Vérifier si le role est un Délegue
        /// </summary>
        /// <param name="roleId">Role identifier</param>
        /// <returns>True si le role est delegue</returns>
        bool IsRoleDelegue(int roleId);

        #region Gestion des seuils

        /// <summary>
        ///   Retourne la liste des seuils de validation associés au rôle
        /// </summary>
        /// <param name="roleId">ID du rôle pour lequel on recherche les seuils</param>
        /// <returns>Liste de seuils de validation</returns>
        IEnumerable<SeuilValidationEnt> GetSeuilValidationListByRoleId(int roleId);

        /// <summary>
        ///   Retourne un seuil à partir de son identifiant
        /// </summary>
        /// <param name="seuilId">Identifiant du seuil recherché </param>
        /// <returns>Un seuil d'après son identifiant</returns>
        SeuilValidationEnt GetSeuilValidationById(int seuilId);

        /// <summary>
        ///   Ajoute un nouveau seuil de validation pour un rôle
        /// </summary>
        /// <param name="seuil">Seuil à ajouter</param>
        /// <returns> ID du seuil créé </returns>
        SeuilValidationEnt AddSeuilValidation(SeuilValidationEnt seuil);

        /// <summary>
        ///   Met à rout un seuil de validation
        /// </summary>
        /// <param name="seuil">Seuil à mettre à jour</param>
        /// <returns>Seuil de validation mis à jour</returns>
        /// <exception cref="System.Exception">
        ///   Le montant du seuil doit être un entier positif et strictement inférieur à 10 000 000.
        ///   or
        ///   Un seuil avec la même devise existe dèjà pour ce rôle.
        /// </exception>
        SeuilValidationEnt UpdateSeuilValidation(SeuilValidationEnt seuil);

        /// <summary>
        ///   Supprime un seuil
        /// </summary>
        /// <param name="seuilId">ID seuil à supprimer</param>
        void DeleteSeuilValidationById(int seuilId);

        #endregion Gestion des seuils
    }
}
