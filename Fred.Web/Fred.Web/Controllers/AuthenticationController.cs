using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Fred.Business;
using Fred.Business.Authentification;
using Fred.Business.Delegation;
using Fred.Business.Directory;
using Fred.Business.ExternalService.Notification;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Utilisateur;
using Fred.Entites;
using Fred.Framework.Extensions;
using Fred.Web.Models;
using Fred.Web.Models.Authentification;
using Fred.Web.Modules.Authentification;
using Microsoft.Owin.Security;

namespace Fred.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private const string MaintenanceViewName = "Maintenance";

        private readonly IUtilisateurManager utilMgr;
        private readonly IDelegationManager delegationManager;
        private readonly IAuthenticationManager authManager;
        private readonly IAuthentificationManager authentificationManager;
        private readonly IAuthentificationLogManager authentificationLogManager;
        private readonly IHabilitationManager habilitationManager;
        private readonly IExternalDirectoryManager externalDirectoryManager;
        private readonly IMaintenanceManager maintenanceManager;
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly INotificationManagerExterne notificationManagerExterne;

        public AuthenticationController(IUtilisateurManager utilMgr,
                                        IDelegationManager delegationManager,
                                        IAuthentificationManager authentificationManager,
                                        IAuthentificationLogManager authentificationLogManager,
                                        IHabilitationManager habilitationManager,
                                        IExternalDirectoryManager externalDirectoryManager,
                                        IMaintenanceManager maintenanceManager,
                                        IContratInterimaireManager contratInterimaireManager,
                                        INotificationManagerExterne notificationManagerExterne)
        {
            this.utilMgr = utilMgr;
            this.delegationManager = delegationManager;
            this.authentificationManager = authentificationManager;
            authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            this.authentificationLogManager = authentificationLogManager;
            this.habilitationManager = habilitationManager;
            this.externalDirectoryManager = externalDirectoryManager;
            this.maintenanceManager = maintenanceManager;
            this.contratInterimaireManager = contratInterimaireManager;
            this.notificationManagerExterne = notificationManagerExterne;
        }


        /// <summary>
        /// Index GET : interface d'erreur d'authentification pour détailler l'erreur +  conseiller l'utilisateur de contacter le support FCI + renouveller une demande d'authentification ( à partir de l'url demandée)
        /// </summary>
        /// <param name="id">Code erreur</param>
        /// <param name="message">Message d'exception system</param>
        /// <param name="returnUrl">Url de retour</param>
        /// <returns>Action result</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Error(int? id = 0, string message = null, string returnUrl = null)
        {
            var model = new AuthentificationModel
            {
                idErrorCode = (int)id,
                returnUrl = returnUrl,
                message = message
            };

            return View(model);
        }

        public ActionResult Connect(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult NewPassword(string guid)
        {
            var model = new NewPasswordViewModel { Guid = guid };
            if (model.Guid != null)
            {
                model = externalDirectoryManager.VerifyGuidValid(model.Guid);
            }
            model.GuidIsValid = true;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Connect(ConnectViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var manager = new FayatApplicationUserManager(utilMgr, authManager, authentificationManager, habilitationManager);
            User user = await manager.FindAsync(model.UserName, model.Password);
            AuthenticationStatus status = user.Status;

            if (status.Success)
            {
                if (!maintenanceManager.IsAuthorizedToAccessTheWebsite(user.UserName))
                {
                    return View(MaintenanceViewName);
                }
                // Signe l'utilisateur dans l'application
                await manager.SignInAsync(user, isPersistent: false);
                utilMgr.UpdateDateDerniereConnexion(status.Utilisateur.UtilisateurId, DateTime.UtcNow);

                // met en cache les CI
                await utilMgr.GetAllCIbyUserAsync(user.Id, true);

                delegationManager.ActivateAndDesactivateDelegation();

                // Vérification date d'expiration compte avec date fin contrat interimaire
                contratInterimaireManager.CheckContratInterimaireAndExpirationDate(user.Id);

                await notificationManagerExterne.SubscribeToUserNotificationsAsync(user.Id);

                return RedirectToLocal(returnUrl);
            }

            authentificationLogManager.SaveFormsAuthentificationError(model.UserName,
                returnUrl,
                status.ConnexionStatus.ToIntValue(),
                user.Status.TechnicalError,
                AdressIpHelper.GetIPAddress());

            model.Error = user.Status.ErrorAuthReason;
            model.TechnicalError = user.Status.TechnicalError;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var manager = new FayatApplicationUserManager(utilMgr, authManager, authentificationManager, habilitationManager);
            User user = await manager.FindAsyncByUserNameOrEmail(model.UserName, model.Email);
            AuthenticationStatus status = user.Status;

            model.Message = status.ErrorAuthReason;
            if (status.Success)
            {
                Guid guid = Guid.NewGuid();
                SendMailStatut sendMailStatut = manager.SendMailForResetPassword(status.Utilisateur, guid, model.Url);
                if (sendMailStatut.Success)
                {
                    externalDirectoryManager.UpdateGuid(status.Utilisateur.ExternalDirectory, guid);
                }
                model.Message = sendMailStatut.Message;
                model.Success = sendMailStatut.Success;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewPassword(NewPasswordViewModel model)
        {
            var manager = new FayatApplicationUserManager(utilMgr, authManager, authentificationManager, habilitationManager);
            AuthenticationStatus status = await manager.PasswordVerify(model.Password, model.PasswordVerify);

            if (status.Success)
            {
                model = externalDirectoryManager.UpdatePassword(model);
            }
            else
            {
                model.Message = status.ErrorAuthReason;
            }

            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Déconnexion de l'utilisateur
        /// </summary>
        /// <returns>Page d'accueuil</returns>
        public ActionResult Logout()
        {
            var manager = new FayatApplicationUserManager(utilMgr, authManager, authentificationManager, habilitationManager);
            manager.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}
