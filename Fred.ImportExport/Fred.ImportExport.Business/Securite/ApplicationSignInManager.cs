using Fred.ImportExport.Entities.Securite;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fred.ImportExport.Business.Securite
{
  /// <summary>
  /// Gestionnaire de connexion de l'application
  /// </summary>
  public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
  {
    /// <summary>
    ///  Initialise une nouvelle instance
    /// </summary>
    /// <param name="userManager">Le manager des utilisateurs pour l'application</param>
    /// <param name="authenticationManager">Le manager des authentifications</param>
    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        : base(userManager, authenticationManager)
    {
    }

    /// <summary>
    /// Permet de générer une nouvelle "Identity".
    /// </summary>
    /// <param name="user">L'utilisateur de l'application.</param>
    /// <returns>Une "Identity".</returns>
    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    {
      return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    }

    /// <summary>
    /// Permet de créer un gestionnaire de connexion de l'application.
    /// </summary>
    /// <param name="options">Les options pour un IdentityFactoryMiddleware.</param>
    /// <param name="context">Le context Owin.</param>
    /// <returns>Un gestionnaire de connexion de l'application.</returns>
    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    {
      return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    }
  }
}
