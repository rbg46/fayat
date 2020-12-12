using Fred.ImportExport.Database.Securite;
using Fred.ImportExport.Entities.Securite;
using Fred.ImportExport.Framework.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Threading.Tasks;

namespace Fred.ImportExport.Business.Securite
{
  /// <summary>
  /// Gestionnaire des utilisateurs de l'application.
  /// UserManager est défini dans ASP.NET Identity.
  /// </summary>
  public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    /// <summary>
    /// Initialise une nouvelle instance.
    /// </summary>
    /// <param name="store">Les apis basiques de gestion des utilisateurs.</param>
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
        : base(store)
    {
    }

    /// <summary>
    /// Permet de créer un gestionnaire des utilisateurs de l'application.
    /// </summary>
    /// <param name="options">Les options pour un IdentityFactoryMiddleware.</param>
    /// <param name="context">Le context Owin</param>
    /// <returns>Un gestionnaire des utilisateurs de l'application.</returns>
    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {
      var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<IdentityContext>()));
      // Configure validation logic for usernames
      manager.UserValidator = new UserValidator<ApplicationUser>(manager)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };

      // Configure validation logic for passwords
      manager.PasswordValidator = new PasswordValidator
      {
        RequiredLength = 6,
        RequireNonLetterOrDigit = true,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true,
      };

      // Configure user lockout defaults
      manager.UserLockoutEnabledByDefault = true;
      manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
      manager.MaxFailedAccessAttemptsBeforeLockout = 5;

      // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
      // You can write your own provider and plug it in here.
      manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
      {
        MessageFormat = "Your security code is {0}"
      });
      manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
      {
        Subject = "Security Code",
        BodyFormat = "Your security code is {0}"
      });
      manager.EmailService = new EmailService();
      manager.SmsService = new SmsService();
      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider =
            new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
      }
      return manager;
    }

    public static Func<CookieValidateIdentityContext, Task> CreateValidateIdentity()
    {
      return SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                  validateInterval: TimeSpan.FromMinutes(30),
                  regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager));
    }
  }
}
