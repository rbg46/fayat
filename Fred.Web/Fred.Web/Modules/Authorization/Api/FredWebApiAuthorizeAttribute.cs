using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Fred.Entities;
using Fred.Web.Modules.Authorization.Common;

namespace Fred.Web.Modules.Authorization.Api
{
    /// <summary>
    /// Attribut d'authorisation, permet d'authoriser l'acces a une web api.
    /// globalPermissionKey est la valeur de PermissionKey d'une PermissionEnt.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
    public class FredWebApiAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string globalPermissionKey;
        private readonly FonctionnaliteTypeMode? mode;

        public FredWebApiAuthorizeAttribute(string globalPermissionKey)
        {
            this.globalPermissionKey = globalPermissionKey;
        }

        public FredWebApiAuthorizeAttribute(string globalPermissionKey, FonctionnaliteTypeMode mode)
        {
            this.globalPermissionKey = globalPermissionKey;
            this.mode = mode;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool isSuperAdmin = AuthorizeAttibuteHelper.IsSuperAdmin();
            if (isSuperAdmin)
            {
                base.OnAuthorization(actionContext);
                return;
            }
            if (mode.HasValue)
            {
                bool hasClaimPermissionOnMode = AuthorizeAttibuteHelper.HasPermissionClaimOnMode(globalPermissionKey, mode.Value);
                if (hasClaimPermissionOnMode)
                {
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                bool hasClaimPermission = AuthorizeAttibuteHelper.HasPermissionClaim(globalPermissionKey);
                if (hasClaimPermission)
                {
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }

        }

        /// <summary>
        /// Override pour renvoyer un message        
        /// </summary>
        /// <param name="actionContext">actionContext</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new HttpResponseMessage
                {
                    // Si on renvoie Unauthorized, on est renvoyé à la page de login. Donc on renvoie une 403 pour rester sur la page actuel
                    StatusCode = HttpStatusCode.Forbidden,
                    Content = new StringContent("Vous n'avez pas les habilitations nécessaires. Veuillez contacter votre administrateur fonctionnel."),
                };
            }
        }
    }
}
