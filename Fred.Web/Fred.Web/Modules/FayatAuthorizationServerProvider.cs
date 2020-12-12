using System.Threading.Tasks;
using System.Web;
using Fred.Business;
using Fred.Business.Authentification;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Utilisateur;
using Fred.DesignPatterns.DI;
using Fred.Entites;
using Fred.Framework.Extensions;
using Fred.Web.Modules.Authentification;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Fred.Web.Modules
{
    /// <summary>
    /// Fournisseur d'authentification OAuth FAYAT
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerProvider" />
    public class FayatAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IDependencyInjectionService dependencyInjectionService;

        public FayatAuthorizationServerProvider(IDependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are
        /// present on the request. If the web application accepts Basic authentication credentials,
        /// context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. If the web
        /// application accepts "client_id" and "client_secret" as form encoded POST parameters,
        /// context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body.
        /// If context.Validated is not called the request will not proceed further.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has provided name and password
        /// credentials directly into the client application's user interface, and the client application is using those to acquire an "access_token" and
        /// optional "refresh_token". If the web application supports the
        /// resource owner credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an
        /// access token the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
        /// with the access token. The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
        /// The default behavior is to reject this grant type.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var authentificationManager = dependencyInjectionService.Resolve<IAuthentificationManager>();
            var authentificationLogManager = dependencyInjectionService.Resolve<IAuthentificationLogManager>();

            AuthenticationStatus status = authentificationManager.Authenticate(context.UserName, context.Password);

            var utilisateurManager = dependencyInjectionService.Resolve<IUtilisateurManager>();
            IHabilitationManager habilitationManager = dependencyInjectionService.Resolve<IHabilitationManager>();

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (!status.Success)
            {
                authentificationLogManager.SaveApiAuthentificationError(context.UserName,
                  context.Request.Uri.ToString(),
                  status.ConnexionStatus.ToIntValue(),
                  status.TechnicalError,
                  context.Request.RemoteIpAddress);
                context.SetError("invalid_grant", status.ErrorAuthReason);
                return;
            }
            else
            {
                var manager = new FayatApplicationUserManager(utilisateurManager, authenticationManager, authentificationManager, habilitationManager);

                var applicationUser = await manager.FindAsync(context.UserName, context.Password);

                var claimsIdentityApplicationUser = await manager.CreateIdentityAsync(applicationUser, context.Options.AuthenticationType);
                var globalsClaims = habilitationManager.GetGlobalsClaims(applicationUser.Status.Utilisateur);
                claimsIdentityApplicationUser.AddClaims(globalsClaims);

                var ticket = new AuthenticationTicket(claimsIdentityApplicationUser, null);
                context.Validated(ticket);
            }

            await Task.FromResult(0);
        }
    }
}