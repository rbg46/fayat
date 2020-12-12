using System.Collections.Generic;
using System.Security.Claims;

namespace Fred.Framework.Security
{
    /// <summary>
    ///   Interface ISecurityManager
    /// </summary>
    public interface ISecurityManager
    {
        /// <summary>Authentifie l'utilisateur sur l'active directory </summary>
        /// <param name="base64">base 64 obtenue via la BasicAuth</param>
        /// <param name="outDomaine">Retourne le domaine renseigné par l'utilisateur</param>
        /// <param name="outLogin">Retourne le login de l'utilisateur</param>
        /// <param name="outPassword">Retourne le mot de passe décrypté</param>
        /// <remarks>Retourne Domaine / Login et Mot de passe décrypté</remarks>
        void DecryptBase64(string base64, out string outDomaine, out string outLogin, out string outPassword);

        /// <summary>Authentification de l'utilisateur sur un active directory </summary>
        /// <param name="domain">Domaine sur lequel l'utilisateur devra s'authentifier</param>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        /// <returns>Retourne TRUE si l'authentification est validée</returns>
        bool AuthenticateUserInActiveDirectory(string domain, string login, string password);

        /// <summary>
        ///   Récupère le détail des informations de l'utilisateur
        /// </summary>
        /// <param name="domaine">Domaine sur lequel l'utilisateur devra s'authentifier</param>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="pwd">Mot de passe de l'utilisateur</param>
        /// <returns>Retourne le model de sécurité</returns>
        UserModel GetUserIdentity(string domaine, string login, string pwd);

        /// <summary>Vérifie si  le login est présent dans l'active directory</summary>
        /// <param name="samAccountName">Critère de recherche sur la propriété samAccountName</param>
        /// <param name="email">Critère de recherche sur le login</param>
        /// <param name="objectSid">Critère de recherche sur la propriété objectSid</param>
        /// <returns>Retourne Liste des utilisateurs</returns>
        List<UserModel> GetIdentity(string samAccountName, string email, string objectSid);

        /// <summary>Vérifie si  le login est présent dans l'active directory</summary>
        /// <param name="domaine">Critère de recherche sur la propriété domaine</param>
        /// <param name="samAccountName">Critère de recherche sur la propriété samAccountName</param>
        /// <param name="email">Critère de recherche sur le login</param>
        /// <param name="objectSid">Critère de recherche sur la propriété objectSid</param>
        /// <returns>Retourne Liste des utilisateurs</returns>
        List<UserModel> GetIdentity(string domaine, string samAccountName, string email, string objectSid);

        /// <summary>Vérifie si  le login est présent dans l'active directory</summary>
        /// <param name="domaine">Domaine de recherche</param>
        /// <param name="samAccountName">Critère de recherche sur la propriété samAccountName</param>
        /// <returns>Retourne Liste des utilisateurs</returns>
        List<UserModel> GetIdentity(string domaine, string samAccountName);

        /// <summary>
        ///   Ajout de la fiche utilisateur dans le Claim
        /// </summary>
        /// <param name="o">Objet utilisateur</param>
        /// <returns>Retourne un claim complet avec les informations de base</returns>
        ClaimsIdentity SetUtilisateurClaim(object o);

        /// <summary>
        ///   Suppression du claim utilisateur
        /// </summary>
        void RemoveClaim();

        /// <summary>
        ///   Obtient l'ID de l'utilisateur définit dans les claims d'authentification
        /// </summary>
        /// <returns>Id utilisateur</returns>
        int GetUtilisateurId();

        string GetCurrentServiceAccount();

        /// <summary>
        ///   Supprimer le claims (contexte) de l'utilisateur
        /// </summary>
        /// <returns>Id utilisateur</returns>
        bool Logout();
    }
}