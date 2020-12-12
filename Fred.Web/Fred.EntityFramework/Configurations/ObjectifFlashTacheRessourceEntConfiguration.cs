using Fred.Entities.ObjectifFlash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ObjectifFlashTacheRessourceEntConfiguration : IEntityTypeConfiguration<ObjectifFlashTacheRessourceEnt>
    {
        public void Configure(EntityTypeBuilder<ObjectifFlashTacheRessourceEnt> builder)
        {
            builder.ToTable("FRED_OBJECTIF_FLASH_TACHE_RESSOURCE");
            builder.HasKey(x => x.ObjectifFlashTacheRessourceId);

            builder.Property(x => x.ObjectifFlashTacheRessourceId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.QuantiteObjectif)
                .HasColumnType("decimal(18, 3)");
            builder.Property(x => x.PuHT)
                .HasColumnType("decimal(18, 3)");
            builder.Ignore(x => x.ChapitreCode);
            builder.Ignore(x => x.TotalQuantiteJournalise);
            builder.Ignore(x => x.ListErreurs);

            builder.HasOne(x => x.ObjectifFlashTache)
                .WithMany(oft => oft.Ressources)
                .HasForeignKey(x => x.ObjectifFlashTacheId);
            builder.HasOne(x => x.Ressource)
                .WithMany()
                .HasForeignKey(x => x.RessourceId);
            builder.HasOne(x => x.Unite)
                .WithMany()
                .HasForeignKey(x => x.UniteId);
        }
    }
}
