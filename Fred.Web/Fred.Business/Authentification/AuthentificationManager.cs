using System;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.Entites;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Security;


namespace Fred.Business.Authentification
{
    /// <summary>
    ///  Gestionnaire des authentification.
    /// </summary>
    public class AuthentificationManager : Manager, IAuthentificationManager
    {

        private readonly IUtilisateurManager userManager;
        private readonly ISecurityManager securityManager;
        private readonly IPersonnelManager personnelManager;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="AuthentificationManager" />
        /// </summary>    
        /// <param name="userManager">Gestionnaire des utilisateurs</param>
        /// <param name="securityManager">Gestionnaire de la Sécurité</param>
        /// <param name="personnelManager">Gestionnaire des personnels</param>
        public AuthentificationManager(IUtilisateurManager userManager, ISecurityManager securityManager, IPersonnelManager personnelManager)
        {
            this.userManager = userManager;
            this.securityManager = securityManager;
            this.personnelManager = personnelManager;
        }

        /// <summary>
        ///   Retourne l'utilisateur avec une correspondance Login / Mot de passe
        ///   Si le domaine est renseigné, alors le système va checker sur l'Active Directory.
        ///   En revanche si le mot de passe n'existe pas, on checker en base de données référentiel > ce controle est réalisé
        ///   directement à partir de la couche Fred.Business.
        ///   Dans les deux cas, en cas de réussite, on va récupérer  le jeton d'identité de l'utlisateur
        /// </summary>
        /// <param name="login">Nom d'utilisateur</param>
        /// <param name="password">Mot de passe</param>
        /// <returns>
        ///   Objet AuthenticationStatus comprenant soit la fiche détaillée de l'utilisateur, soit les détails de l'erreur
        ///   rencontrée
        /// </returns>
        public AuthenticationStatus Authenticate(string login, string password)
        {
            try
            {
                if (AuthentificationManagerHelper.CheckIsNullOrEmpty(login))
                {
                    return AuthentificationResponseHelper.CreateEmptyLoginResponse();
                }
                if (AuthentificationManagerHelper.CheckIsNullOrEmpty(password))
                {
                    return AuthentificationResponseHelper.CreateEmptyPasswordResponse();
                }

                UtilisateurEnt utilisateur = userManager.GetByLogin(login);

                if (utilisateur == null)
                {
                    return AuthentificationResponseHelper.CreateLoginAndPasswordNotFoundResponse();
                }

                var isActif = AuthentificationManagerHelper.GetPersonnelIsActif(utilisateur, personnelManager);
                if (!isActif)
                {
                    return AuthentificationResponseHelper.CreateNoActiveResponse();
                }

                // Vérification que l'utilisateur n'est pas consideré comme supprimé.
                // TI_US_54_C_001
                if (AuthentificationManagerHelper.CheckAccountIsDeleted(utilisateur))
                {
                    return AuthentificationResponseHelper.CreateDeletedUtilisateurResponse();
                }

#if DEBUG
                if (password == "freddebug")
                {
                    return AuthentificationResponseHelper.CreateOkResponse(utilisateur);
                }
                else
                {
#endif
                    // vérification que l'utilisateur est externe
                    // en vérifiant qu'il soit référencé dans la table Externe
                    if (utilisateur.ExternalDirectory != null)
                    {
                        return CreateResponseForExternalUtilisateur(login, password, utilisateur, userManager);
                    }
                    else
                    {
                        // Si l'utilisateur n'est pas référencé dans la table Externe => Alors on cherche dans les Active Directory       
                        return CreateResponseForInternalUtilisateur(login, password, utilisateur, securityManager);
                    }
#if DEBUG
                }
#endif
            }
            catch (Exception ex)
            {
                return AuthentificationResponseHelper.CreateTechnicalErrorResponse(ex);
            }
        }


        /// <summary>
        /// Verification pour le personnel externe
        /// </summary>
        /// <param name="login">login</param>
        /// <param name="password">password</param>
        /// <param name="utilisateur">utilisateur</param>
        /// <param name="userManager">userManager</param>
        /// <returns>AuthenticationStatus</returns>
        private AuthenticationStatus CreateResponseForExternalUtilisateur(string login, string password, UtilisateurEnt utilisateur, IUtilisateurManager userManager)
        {
            if (!AuthentificationManagerHelper.AuthenticateFredUser(login, password, userManager))
            {
                return AuthentificationResponseHelper.CreateLoginAndPasswordNotFoundResponse();
            }

            // Controle sur la date d'expiration
            if (utilisateur.ExternalDirectory.DateExpiration.HasValue && utilisateur.ExternalDirectory.DateExpiration.Value < DateTime.UtcNow)
            {
                return AuthentificationResponseHelper.CreateAccountExpiredResponse();
            }

            if (!utilisateur.ExternalDirectory.IsActived)
            {
                return AuthentificationResponseHelper.CreateNoActiveResponse();
            }

            return AuthentificationResponseHelper.CreateOkResponse(utilisateur);
        }


        /// <summary>
        ///   Authentifie un utilisateur via son nom de domain
        /// </summary>
        /// <param name="login">Login de  l'utilisateur FRED à trouver.</param>
        /// <param name="password">Mot de passe de  l'utilisateur FRED à trouver.</param>
        /// <param name="utilisateur">Utilisateur</param>
        /// <param name="securityManager">securityManager</param>
        /// <returns>Renvoie le statut de l'authentification</returns>
        private AuthenticationStatus CreateResponseForInternalUtilisateur(string login, string password, UtilisateurEnt utilisateur, ISecurityManager securityManager)
        {
            // vérification de l'existance du domaine dans le login
            if (!login.Contains('\\'))
            {
                return AuthentificationResponseHelper.CreateInternalBadlyFormatedResponse();
            }

            // Si domaine renseigné et définit, alors recherche sur Active Directory
            var extractionDomainAndLogin = login.Split('\\');
            string domain = extractionDomainAndLogin[0];
            string internalLogin = extractionDomainAndLogin[1];
            var authenticateUserIsInActiveDirectory = false;
            try
            {
                authenticateUserIsInActiveDirectory = securityManager.AuthenticateUserInActiveDirectory(domain, internalLogin, password);
            }
            catch (FredTechnicalException)
            {
                return AuthentificationResponseHelper.CreateLoginAndPasswordNotFoundResponse();
            }

            if (!authenticateUserIsInActiveDirectory)
            {
                return AuthentificationResponseHelper.CreateLoginAndPasswordNotFoundResponse();
            }

            var accountExpiration = AuthentificationManagerHelper.GetInternalExpirationDate(domain, internalLogin, password, securityManager);

            if (!string.IsNullOrEmpty(accountExpiration) && Convert.ToDateTime(accountExpiration) < DateTime.UtcNow)
            {
                return AuthentificationResponseHelper.CreateAccountExpiredResponse();
            }

            return AuthentificationResponseHelper.CreateOkResponse(utilisateur);
        }

        /// <summary>
        ///   Retourne l'utilisateur avec une correspondance Login ou email
        ///   Teste si Login ou email existe, si personnel interne ou superAdmin
        ///   Retourne l'utilisateur avec le détail de la réponse ou de l'erreur
        /// </summary>
        /// <param name="login">Nom d'utilisateur</param>
        /// <param name="email">email de l'utilisateur</param>
        /// <returns>
        ///   Objet AuthenticationStatus comprenant soit la fiche détaillée de l'utilisateur, soit les détails de l'erreur
        ///   rencontrée
        /// </returns>
        public AuthenticationStatus AuthenticateForResetPassword(string login, string email)
        {
            try
            {
                if (AuthentificationManagerHelper.CheckIsNullOrEmpty(login) && AuthentificationManagerHelper.CheckIsNullOrEmpty(email))
                {
                    return AuthentificationResponseHelper.CreateEmptyEmailAndLoginResponse();
                }

                UtilisateurEnt utilisateur = null;
                PersonnelEnt personnel = null;

                if (AuthentificationManagerHelper.CheckIsNullOrEmpty(email))
                {
                    utilisateur = userManager.GetByLoginForResetPassword(login);

                    if (utilisateur == null)
                    {
                        return AuthentificationResponseHelper.CreateLoginNotFoundResponse();
                    }

                    personnel = utilisateur.Personnel;


                    if (AuthentificationManagerHelper.CheckIsNullOrEmpty(personnel.Email))
                    {
                        return AuthentificationResponseHelper.CreateEmailNotFoundResponse();
                    }
                }
                if (AuthentificationManagerHelper.CheckIsNullOrEmpty(login))
                {
                    personnel = personnelManager.GetPersonnelByEmail(email);

                    if (personnel == null)
                    {
                        return AuthentificationResponseHelper.CreateEmailNotFoundResponse();
                    }

                    utilisateur = personnel.Utilisateur;
                }

                if (utilisateur == null)
                {
                    return AuthentificationResponseHelper.CreateNotFoundUtilisateurResponse();
                }
                var result = VerifyUserForResetPassword(utilisateur, personnel);

                if (result != null)
                {
                    return result;
                }


                return CreateResponseForResetPassword(utilisateur);
            }
            catch (Exception ex)
            {
                return AuthentificationResponseHelper.CreateTechnicalErrorResponse(ex);
            }
        }

        /// <summary>
        /// Verification si l'utilisateur est un personnel externe et si l'utilisateur est toujour actif
        /// </summary>
        /// <param name="utilisateur">Utilisateur</param>
        /// <param name="personnel">Personnel</param>
        /// <returns>AuthenticationStatus</returns>
        private AuthenticationStatus VerifyUserForResetPassword(UtilisateurEnt utilisateur, PersonnelEnt personnel)
        {

            var isActif = AuthentificationManagerHelper.GetPersonnelIsActif(utilisateur, personnelManager);
            if (!isActif)
            {
                return AuthentificationResponseHelper.CreateNoActiveResponse();
            }

            if (AuthentificationManagerHelper.CheckAccountIsDeleted(utilisateur))
            {
                return AuthentificationResponseHelper.CreateDeletedUtilisateurResponse();
            }

            if (utilisateur.SuperAdmin)
            {
                return AuthentificationResponseHelper.CreateAccountIsSuperAdminResponse();
            }

            if (personnel.IsInterne)
            {
                return AuthentificationResponseHelper.CreateAccountIsInterneResponse();
            }

            return null;
        }

        /// <summary>
        /// Verification pour le personnel externe
        /// </summary>
        /// <param name="utilisateur">utilisateur</param>
        /// <returns>AuthenticationStatus</returns>
        private AuthenticationStatus CreateResponseForResetPassword(UtilisateurEnt utilisateur)
        {
            // Controle sur la date d'expiration
            if (utilisateur.ExternalDirectory.DateExpiration.HasValue && utilisateur.ExternalDirectory.DateExpiration.Value < DateTime.UtcNow)
            {
                return AuthentificationResponseHelper.CreateAccountExpiredResponse();
            }

            if (!utilisateur.ExternalDirectory.IsActived)
            {
                return AuthentificationResponseHelper.CreateNoActiveResponse();
            }

            return AuthentificationResponseHelper.CreateSuccessAuthenticateForResetPasswordResponse(utilisateur);
        }

        /// <summary>
        /// Verification des mots de passe
        /// </summary>
        /// <param name="password">Mot de passe</param>
        /// <param name="passwordVerify">Vérification mot de passe</param>
        /// <returns>AuthenticationStatus</returns>
        public AuthenticationStatus PasswordVerify(string password, string passwordVerify)
        {
            if (AuthentificationManagerHelper.CheckIsNullOrEmpty(password))
            {
                return AuthentificationResponseHelper.CreateEmptyPasswordResponse();
            }

            if (AuthentificationManagerHelper.CheckIsNullOrEmpty(passwordVerify))
            {
                return AuthentificationResponseHelper.CreateEmptyPasswordVerifyResponse();
            }

            if (password.Length <= 6)
            {
                return AuthentificationResponseHelper.CreatePasswordRequiredLentghResponse();
            }

            if (password != passwordVerify)
            {
                return AuthentificationResponseHelper.CreateNotEqualsPasswordsResponse();
            }

            return AuthentificationResponseHelper.CreateSuccessPasswordVerifyResponse();

        }
    }
}
