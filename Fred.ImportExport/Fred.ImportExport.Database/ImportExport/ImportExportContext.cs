using System.Data.Entity;
using Fred.ImportExport.Entities;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Entities.Transposition;

namespace Fred.ImportExport.Database.ImportExport
{
    public class ImportExportContext : DbContext
    {
        public ImportExportContext()
          : base("FredIEConnection") { }

        public DbSet<FluxEnt> Flux { get; set; }
        public DbSet<JobEnt> Jobs { get; set; }
        public DbSet<NLogFredIeEnt> NLogs { get; set; }
        public DbSet<LogicielTiersEnt> LogicielTiers { get; set; }
        public DbSet<WorkflowLogicielTiersEnt> WorkflowLogicielTiersEnt { get; set; }
        public DbSet<WorkflowPointageEnt> WorkflowPointageEnt { get; set; }
        public DbSet<TranspoCodeEmploiToRessourceEnt> TranspoCodeEmploiToRessources { get; set; }
        public DbSet<SocieteCodeImportMaterielEnt> SocieteCodeImportMateriel { get; set; }
        public DbSet<InterfaceTransfertDonneeEnt> InterfaceTransfertDonneeEnt { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("importExport");
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<ImportExportContext, Migrations.Configuration>());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FluxEnt>().ToTable("Flux");
            modelBuilder.Entity<FluxEnt>().HasKey(f => f.FluxId);
            modelBuilder.Entity<FluxEnt>().HasIndex(f => f.Code).HasName("UniqueCode").IsUnique();
            modelBuilder.Entity<FluxEnt>().Property(f => f.Code).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<FluxEnt>().Property(f => f.GroupCode).IsRequired();
        }
    }
}
