using Fred.ImportExport.Entities.Securite;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Fred.ImportExport.Database.Securite
{
  public class IdentityContext : IdentityDbContext<ApplicationUser>
  {
    public IdentityContext()
        : base("FredIEConnection", throwIfV1Schema: false) { }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("security");
      System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<IdentityContext, Migrations.Configuration>());
      base.OnModelCreating(modelBuilder);
    }

    public static IdentityContext Create()
    {
      return new IdentityContext();
    }
  }
}
