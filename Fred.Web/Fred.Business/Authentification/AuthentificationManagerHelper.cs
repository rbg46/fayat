using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.Framework.Security;
using System;
using System.Linq;

namespace Fred.Business.Authentification
{
    /// <summary>
    /// Class helper du manager AuthentificationManagerHelper
    /// </summary>
    public static class AuthentificationManagerHelper
    {
        /// <summary>
        /// Verifie si le param est null or empty
        /// </summary>
        /// <param name="param">param</param>
        /// <returns>true ou false</returns>
        public static bool CheckIsNullOrEmpty(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Verifie si le personnel est Actif
        /// </summary>
        /// <param name="utilisateur">utilisateur associé a l'utilisateur</param>
        /// <param name="personnelManager">personnelManager</param>
        /// <returns>true ou false</returns>
        public static bool GetPersonnelIsActif(UtilisateurEnt utilisateur, IPersonnelManager personnelManager)
        {
            var personnelId = utilisateur.PersonnelId;
            var peronnel = personnelManager.GetPersonnel(personnelId.Value);
            return peronnel.GetPersonnelIsActive();
        }

        /// <summary>
        /// Verifie si l'utilisateur est supprimé
        /// </summary>
        /// <param name="utilisateur">utilisateur</param>
        /// <returns>true ou false</returns>
        public static bool CheckAccountIsDeleted(UtilisateurEnt utilisateur)
        {
            if (utilisateur.IsDeleted)
            {
                return true;
            }
            if (utilisateur.DateSupression != null && utilisateur.DateSupression < DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Authentifie un utilisateur de la base de données FRED
        /// </summary>
        /// <param name="login">Login de  l'utilisateur FRED à trouver.</param>
        /// <param name="password">Mot de passe de  l'utilisateur FRED à trouver.</param>
        /// <param name="userManager">userManager</param>
        /// <returns>Vrai si l'utilisateur est retrouvé, sinon faux</returns>
        public static bool AuthenticateFredUser(string login, string password, IUtilisateurManager userManager)
        {
            int found = userManager.GetList().Count(u => u.Login == login.Trim() && u.ExternalDirectory.MotDePasse == password);
            return found > 0;
        }

        /// <summary>
        /// Retourne la date d'expiration d'un personnel interne
        /// </summary>
        /// <param name="domain">domain</param>
        /// <param name="internalLogin">internalLogin</param>
        /// <param name="password">password</param>
        /// <param name="securityManager">securityManager</param>
        /// <returns>la date d'expiration</returns>
        public static string GetInternalExpirationDate(string domain, string internalLogin, string password, ISecurityManager securityManager)
        {
            // récupération des Attributs Actives Directory via les credentials de l'utilisateur
            var userModel = securityManager.GetUserIdentity(domain, internalLogin, password);

            // récupération de la date d'expiration
            return userModel.AccountExpires;
        }

        /// <summary>
        /// Converti un entier en message d'erreur pour ConnexionStatus
        /// </summary>
        /// <param name="connexionStatusInt">entier representant un ConnexionStatus</param>
        /// <returns>Message d'erreur</returns>
        public static string ConvertConnexionStatusIntToString(int connexionStatusInt)
        {
            var status = connexionStatusInt.ToEnum<ConnexionStatus>();
            return ConvertConnexionStatusToString(status);
        }

        /// <summary>
        /// Converti ConnexionStatus en message d'erreur pour ConnexionStatus
        /// </summary>
        /// <param name="status">Enum ConnexionStatus</param>
        /// <returns>Message d'erreur</returns>
        public static string ConvertConnexionStatusToString(ConnexionStatus status)
        {
            switch (status)
            {
                case ConnexionStatus.EmptyPassword:
                    return Web.Shared.App_LocalResources.Authentification.Authentification_Model_MotDePassRequis;
                case ConnexionStatus.EmptyLogin:
                    return Web.Shared.App_LocalResources.Authentification.Authentification_Model_UtilisateurRequis;
                case ConnexionStatus.LoginAndPasswordNotFound:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_LoginAndPasswordNotFound;
                case ConnexionStatus.AccountInactive:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_AccountInactif;
                case ConnexionStatus.AccountDeleted:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_AccountSupprime;
                case ConnexionStatus.AccountExpired:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_AccountExpire;
                case ConnexionStatus.BadlyFormated:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_AccountInternalBadlyFormated;
                case ConnexionStatus.TechnicalError:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_TechnicalError;
                case ConnexionStatus.EmptyEmailAndLogin:
                    return Web.Shared.App_LocalResources.Authentification.Authentification_Model_EmptyEmailAndLogin;
                case ConnexionStatus.AccountIsInterne:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_AccountIsInterne;
                case ConnexionStatus.AccountIsSuperAdmin:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_AccountIsSuperAdmin;
                case ConnexionStatus.UserNotFound:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_UserNotFound;
                case ConnexionStatus.LoginNotFound:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_LoginNotFound;
                case ConnexionStatus.EmailNotFound:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_EmailNotFound;
                case ConnexionStatus.ResetPasswordSuccess:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_ResetPasswordSuccess;
                case ConnexionStatus.EmptyPasswordVerify:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_EmptyPasswordVerify;
                case ConnexionStatus.PasswordRequiredLength:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_PasswordRequiredLength;
                case ConnexionStatus.NotEqualsPasswords:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_NotEqualsPasswords;
                case ConnexionStatus.UpdatePasswordSuccess:
                    return Web.Shared.App_LocalResources.Authentification.AuthentificationManager_UpdatePasswordSuccess;
                default:
                    return string.Empty;
            }
        }



        /// <summary>
        /// Converti ConnexionErrorOrigin en message d'erreur
        /// </summary>
        /// <param name="errorOrigin">ConnexionErrorOrigin int value</param>
        /// <returns>Message d'erreur</returns>
        public static string ConvertErrorOriginIntToText(int errorOrigin)
        {
            var result = string.Empty;
            var status = errorOrigin.ToEnum<ConnexionErrorOrigin>();
            if (status == ConnexionErrorOrigin.Api)
            {
                result = Web.Shared.App_LocalResources.AuthentificationLog.AuthentificationLog_Controller_Index_ErrorOrigin_Api;
            }
            if (status == ConnexionErrorOrigin.Forms)
            {
                result = Web.Shared.App_LocalResources.AuthentificationLog.AuthentificationLog_Controller_Index_ErrorOrigin_Forms;
            }
            return result;
        }
    }
}
