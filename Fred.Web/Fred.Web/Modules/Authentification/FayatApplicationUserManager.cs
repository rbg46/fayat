using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Authentification;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Utilisateur;
using Fred.Entites;
using Fred.Entities;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Fred.Web.Modules.Authentification
{

    /// <summary>
    /// Gère la connexion des utilisateurs
    /// </summary>
    public class FayatApplicationUserManager : UserManager<User, int>
    {
        private readonly IAuthenticationManager authenticationManager; // Manager Microsoft.OWIN.Security
        private readonly IAuthentificationManager authentificationManager; // Manager FRED
        private readonly IHabilitationManager habilitationManager;




        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="store">The IUserStore is responsible for commiting changes via the UpdateAsync/CreateAsync methods</param>
        /// <param name="utilisateurManager">utilisateurManager</param>
        /// <param name="authenticationManager">authenticationManager</param>
        /// <param name="authentificationManager">authentificationManager</param>
        /// <param name="habilitationManager">habilitationManager</param>
        public FayatApplicationUserManager(IUtilisateurManager utilisateurManager,
                                    IAuthenticationManager authenticationManager,
                                    IAuthentificationManager authentificationManager,
                                    IHabilitationManager habilitationManager)
            : base(new FayatUserStore<User>(utilisateurManager))
        {
            this.authenticationManager = authenticationManager;
            this.authentificationManager = authentificationManager;
            this.habilitationManager = habilitationManager;
        }

        /// <summary>
        ///     Returns true if the store is an IUserRoleStore
        /// </summary>
        public override bool SupportsUserRole
        {
            get
            {
                return true;
            }
        }


        /// <summary>
        /// Permet de recuperer l'utilisateur correspondant a l'userName et au password.
        /// Utiliser par Asp.net(page) et Asp.net Web Api.
        /// </summary>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <returns>Utilistateur de l'application</returns>
        public override Task<User> FindAsync(string userName, string password)
        {

            AuthenticationStatus status = this.authentificationManager.Authenticate(userName, password);
            if (status.Success)
            {
                var fayatUtilisateur = status.Utilisateur;
                var user = new User()
                {
                    FirstName = fayatUtilisateur.Prenom,
                    Id = fayatUtilisateur.UtilisateurId,
                    UserName = fayatUtilisateur.Login,
                    Status = status
                };
                return Task.FromResult(user);
            }
            else
            {
                var user = new User()
                {
                    Status = status
                };
                return Task.FromResult(user);
            }
        }

        public async Task<User> FindAsyncByUserNameOrEmail(string username, string email)
        {
            AuthenticationStatus status = this.authentificationManager.AuthenticateForResetPassword(username, email);
            var user = new User() { Status = status };

            return await Task.FromResult(user);
        }

        public async Task<AuthenticationStatus> PasswordVerify(string password, string passwordVerify)
        {
            AuthenticationStatus status = this.authentificationManager.PasswordVerify(password, passwordVerify);

            return await Task.FromResult(status);
        }

        public SendMailStatut SendMailForResetPassword(UtilisateurEnt utilisateur, Guid guid, string url)
        {
            StringBuilder body = new StringBuilder();

            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_Html);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_Header_Html);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_BlockBody_Start_Html + utilisateur.Login);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_BlockBody_Middle_Html);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_Url_Start_Html + url);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_Url_Middle_Html + guid);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_Url_End_Html);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_BlockBody_End_Html);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Body_Footer_Html);
            body.Append(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_End_Html);

            try
            {
                using (SendMail sender = new SendMail())
                {
                    sender.EMail.IsBodyHtml = true;
                    sender.From(Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Mail_Support, Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Name_Support);
                    sender.To(utilisateur.Email, utilisateur.PrenomNom);
                    sender.Subject = Shared.App_LocalResources.EmailResetPassword.FayatApplicationUserManager_Subject;
                    sender.Body = body.ToString();
                    sender.Send();
                }

                return new SendMailStatut() { Success = true, Message = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.ResetPasswordSuccess) };
            }
            catch (FredTechnicalException e)
            {
                return new SendMailStatut() { Success = false, Message = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.TechnicalError) };
                throw new ValidationException(new List<ValidationFailure> { new ValidationFailure("Email", e.Message) });
            }
        }

        /// <summary>
        /// Permet de connecté l'utilisateur. L'utilisateur est considéré comme connecté.
        /// Utiliser par Asp.net(page).
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="isPersistent">isPersistent</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>   
        public async Task SignInAsync(User user, bool isPersistent)
        {
            SignOut();

            // Crée l'identité dans Asp.Net
            var identity = await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var claims = habilitationManager.GetGlobalsClaims(user.Status.Utilisateur);
            identity.AddClaims(claims);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

        }

        public void SignOut()
        {
            authenticationManager.SignOut();
        }



    }
}
