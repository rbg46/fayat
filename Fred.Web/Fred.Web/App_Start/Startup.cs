using System;
using System.Web;
using Fred.DesignPatterns.DI;
using Fred.Framework.Web.SignalR;
using Fred.Web.Bootstrapper.DependencyInjection;
using Fred.Web.Middlewares;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Fred.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseRequestMiddleware();

            IDependencyInjectionService dependencyInjectionService = DependencyInjectionConfig.DependencyInjectionService;

            // --------------------------------------------------------------
            // Appel Angular
            // --------------------------------------------------------------
            var cookieAuthOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                SlidingExpiration = true,
                CookieSecure = CookieSecureOption.SameAsRequest,
                LoginPath = new PathString("/Authentication/Connect"),
                LogoutPath = new PathString("/Authentication/Logout"),
                //Ce provider permet de faire un redirect si la requette n'est pas de type ajax.
                //Cela permet aussi de retourner une 401 si la requette est de type ajax
                Provider = new CookieAuthenticationProvider()
                {
                    OnApplyRedirect = ctx =>
                    {
                        if (!IsApiRequest(ctx.Request))
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                    }
                }
            };
            app.UseCookieAuthentication(cookieAuthOptions);

            // --------------------------------------------------------------
            // Appel Web API +  Compatibilité 
            // --------------------------------------------------------------    
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                Provider = new Modules.FayatAuthorizationServerProvider(dependencyInjectionService)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            app.MapServerNotificationsEngine(dependencyInjectionService);
        }

        /// <summary>
        /// Permet de savoir si une requette est de type ajax/web service
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>true, si une requette est de type ajax/web service</returns>
        private bool IsApiRequest(IOwinRequest request)
        {
            string apiPath = VirtualPathUtility.ToAbsolute("~/api/");
            return request.Uri.LocalPath.StartsWith(apiPath);
        }
    }
}