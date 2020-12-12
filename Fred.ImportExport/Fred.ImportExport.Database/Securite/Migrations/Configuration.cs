namespace Fred.ImportExport.Database.Securite.Migrations
{
  using Entities.Securite;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using System.Data.Entity.Migrations;
  using System.Linq;

  /// <summary>
  /// Gestionnaire de la base de données, partie sécurité.
  /// </summary>
  public sealed class Configuration : DbMigrationsConfiguration<Fred.ImportExport.Database.Securite.IdentityContext>
  {
    /// <summary>
    /// Initialise une nouvelle instance.
    /// </summary>
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
      AutomaticMigrationDataLossAllowed = false;
      MigrationsDirectory = @"Securite\Migrations";
    }

    /// <summary>
    /// Permet de mettre à jour les données de la base, fonctionne après la mise à niveau vers la dernière migration.
    /// </summary>
    /// <param name="context">Le context de la base de donnée</param>
    protected override void Seed(Fred.ImportExport.Database.Securite.IdentityContext context)
    {
      SeedRoles(context);
      SeedUsers(context);
    }

    /// <summary>
    /// Permet d'ajouter les rôles de sécurité
    /// </summary>
    /// <param name="context">Le context de la base de donnée</param>
    private void SeedRoles(IdentityContext context)
    {
      var roleStore = new RoleStore<IdentityRole>(context);
      var roleManager = new RoleManager<IdentityRole>(roleStore);

      string[] rolesName = new string[] { "admin", "service" };

      foreach (string roleName in rolesName)
      {
        if (!context.Roles.Any(r => r.Name == roleName))
        {
          roleManager.Create(new IdentityRole(roleName));
        }
      }
    }

    /// <summary>
    /// Permet d'ajouter l'administrateur et les utilisateurs des services
    /// </summary>
    /// <param name="context">Le context de la base de donnée</param>
    private void SeedUsers(IdentityContext context)
    {
      var userStore = new UserStore<ApplicationUser>(context);
      var userManager = new UserManager<ApplicationUser>(userStore);

      // Ajout de l'administrateur 
      if (!(context.Users.Any(u => u.UserName == "admin@admin.com")))
      {
        var userToInsert = new ApplicationUser
        {
          Email = "admin@admin.com",
          UserName = "admin@admin.com",
          EmailConfirmed = true
        };

        userManager.Create(userToInsert, "U82Jv!$M*iMl");
        userManager.AddToRole(userToInsert.Id, "admin");
      }

      // Ajout des utilisateurs des services
      if (!(context.Users.Any(u => u.UserName == "userserviceFred")))
      {
        var userToInsert = new ApplicationUser { UserName = "userserviceFred" };
        userManager.Create(userToInsert, "s8!FkTv8k*SE");
        userManager.AddToRole(userToInsert.Id, "service");
      }

      if (!(context.Users.Any(u => u.UserName == "userserviceStorm")))
      {
        var userToInsert = new ApplicationUser { UserName = "userserviceStorm" };
        userManager.Create(userToInsert, "4vK#dPG1Ir2j");
        userManager.AddToRole(userToInsert.Id, "service");
      }

      if (!(context.Users.Any(u => u.UserName == "userserviceFes")))
      {
        var userToInsert = new ApplicationUser { UserName = "userserviceFes" };
        userManager.Create(userToInsert, "o4x6uMAbtDGn");
        userManager.AddToRole(userToInsert.Id, "service");
      }

      if (!(context.Users.Any(u => u.UserName == "userserviceFtp")))
      {
        var userToInsert = new ApplicationUser { UserName = "userserviceFtp" };
        userManager.Create(userToInsert, "i41aYch4^M@p");
        userManager.AddToRole(userToInsert.Id, "service");
      }
    }
  }
}

