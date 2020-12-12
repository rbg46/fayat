using Fred.Entites;
using Fred.Entities;
using Fred.Entities.Utilisateur;
using System;

namespace Fred.Business.Authentification
{
    /// <summary>
    /// Service qui cré des reponse d'authentification.
    /// </summary>
    public static class AuthentificationResponseHelper
    {
        /// <summary>
        /// Créer une reponse d'erreur quand le mot de passe est vide.
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateEmptyPasswordResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.EmptyPassword;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.EmptyPassword);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand le login est vide
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateEmptyLoginResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.EmptyLogin;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.EmptyLogin);
            return result;
        }


        /// <summary>
        ///  Créer une reponse d'erreur quand l'utilisateur n'est pas trouvé
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateLoginAndPasswordNotFoundResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.LoginAndPasswordNotFound;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.LoginAndPasswordNotFound);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand l'utilisateur est inactif
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateNoActiveResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.AccountInactive;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.AccountInactive);
            return result;
        }


        /// <summary>
        ///  Créer une reponse d'erreur quand l'utilisateur est supprimé
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateDeletedUtilisateurResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.AccountDeleted;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.AccountDeleted);
            return result;
        }



        /// <summary>
        ///  Créer une reponse ok 
        /// </summary>
        /// <param name="utilisateur">UtilisateurEnt</param>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateOkResponse(UtilisateurEnt utilisateur)
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = true;
            result.ConnexionStatus = ConnexionStatus.Ok;
            result.Utilisateur = utilisateur;
            return result;
        }


        /// <summary>
        ///  Créer une reponse d'erreur quand le compte a expiré
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateAccountExpiredResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.ConnexionStatus = ConnexionStatus.AccountExpired;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.AccountExpired);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand le login est mal formaté pour un personnel interne.
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateInternalBadlyFormatedResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.BadlyFormated;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.BadlyFormated);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand une erreur technique survient.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateTechnicalErrorResponse(Exception ex)
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.TechnicalError;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.TechnicalError);
            result.TechnicalError = ex.Message + "\n\n" + ex.StackTrace;
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand l'email et le login sont vide
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateEmptyEmailAndLoginResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.EmptyEmailAndLogin;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.EmptyEmailAndLogin);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand le compte est un compte interne
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateAccountIsInterneResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.AccountIsInterne;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.AccountIsInterne);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand le compte est un compte superadmin
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateAccountIsSuperAdminResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.AccountIsSuperAdmin;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.AccountIsSuperAdmin);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand l'utilisateur n'est pas trouvé
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateNotFoundUtilisateurResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.UserNotFound;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.UserNotFound);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand l'utilisateur n'est pas trouvé
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateLoginNotFoundResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.LoginNotFound;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.LoginNotFound);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand l'utilisateur n'est pas trouvé
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateEmailNotFoundResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.EmailNotFound;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.EmailNotFound);
            return result;
        }

        /// <summary>
        ///  Créer une reponse pour le succès de l'envoie du mail pour la réinitialiser le mot de passe
        /// </summary>
        /// <param name="utilisateur">UtilisateurEnt</param>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateSuccessSendMailForResetPasswordResponse(UtilisateurEnt utilisateur)
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = true;
            result.ConnexionStatus = ConnexionStatus.ResetPasswordSuccess;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.ResetPasswordSuccess);
            result.Utilisateur = utilisateur;
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand le champ de vérification de mot de passe est vide
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateEmptyPasswordVerifyResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.EmptyPasswordVerify;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.EmptyPasswordVerify);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand le mot de passe n'a pas la taille requise
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreatePasswordRequiredLentghResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.PasswordRequiredLength;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.PasswordRequiredLength);
            return result;
        }

        /// <summary>
        ///  Créer une reponse d'erreur quand les champs mot de passe et vérification de mot de passe ne correspondent pas
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateNotEqualsPasswordsResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = false;
            result.ConnexionStatus = ConnexionStatus.NotEqualsPasswords;
            result.ErrorAuthReason = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.NotEqualsPasswords);
            return result;
        }

        /// <summary>
        ///  Créer une reponse de succès quand la vérfication des mots de passe a réussi
        /// </summary>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateSuccessPasswordVerifyResponse()
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = true;
            return result;
        }

        /// <summary>
        ///  Créer une reponse de succès quand l'authentification pour la réinitialisation du mot de passe a réussi
        /// </summary>
        /// <param name="utilisateur">UtilisateurEnt</param>
        /// <returns>AuthenticationStatus</returns>
        public static AuthenticationStatus CreateSuccessAuthenticateForResetPasswordResponse(UtilisateurEnt utilisateur)
        {
            AuthenticationStatus result = new AuthenticationStatus();
            result.Success = true;
            result.Utilisateur = utilisateur;
            return result;
        }
    }
}
