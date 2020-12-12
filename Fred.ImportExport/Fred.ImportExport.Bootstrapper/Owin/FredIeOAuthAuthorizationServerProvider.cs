using Fred.ImportExport.Business.Securite;
using Fred.ImportExport.Entities.Securite;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fred.ImportExport.Bootstrapper.Owin
{
  /// <summary>
  /// Fournisseur d'authentification OAuth
  /// </summary>
  public class FredIeOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
  {
    /// <summary>
    /// Permet de valider les informations d'identification sont correctes.
    /// </summary>
    /// <param name="context">Le contexte de l'identification</param>
    /// <returns>La tâche permettant l'exécution asynchrone</returns>
    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      ////string clientId;
      ////string clientSecret;

      ////if (context.TryGetBasicCredentials(out clientId, out clientSecret))
      ////{
      ////  // validate the client Id and secret against database or from configuration file.  
      ////  context.Validated();
      ////}
      ////else
      ////{
      ////  context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
      ////  context.Rejected();
      ////}

      ////await base.ValidateClientAuthentication(context);
      bool validated = context.Validated();
      return Task.FromResult(validated);
    }

    /// <summary>
    /// Permet d'émettre un jeton d'accès si les informations d'identification sont correctes. 
    /// </summary>
    /// <param name="context">Le contexte de l'identification</param>
    /// <returns>La tâche permettant l'exécution asynchrone</returns>
    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
      var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
      ApplicationUser user;

      try
      {
        user = await userManager.FindAsync(context.UserName, context.Password);
      }
      catch
      {
        // Could not retrieve the user due to error.  
        context.SetError("server_error");
        context.Rejected();
        return;
      }
      if (user != null)
      {
        ClaimsIdentity identity = await user.GenerateUserIdentityAsync(userManager);
        context.Validated(identity);
      }
      else
      {
        context.SetError("invalid_grant", "Invalid User Id or password'");
        context.Rejected();
      }
    }
  }
}