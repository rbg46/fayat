using Fred.Entities.ObjectifFlash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ObjectifFlashTacheRapportRealiseEntConfiguration : IEntityTypeConfiguration<ObjectifFlashTacheRapportRealiseEnt>
    {
        public void Configure(EntityTypeBuilder<ObjectifFlashTacheRapportRealiseEnt> builder)
        {
            builder.ToTable("FRED_OBJECTIF_FLASH_TACHE_RAPPORT_REALISE");
            builder.HasKey(x => x.ObjectifFlashTacheRapportRealiseId);

            builder.Property(x => x.ObjectifFlashTacheRapportRealiseId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.QuantiteRealise)
                .HasColumnType("decimal(18, 3)");
            builder.Property(cod => cod.DateRealise)
                .HasColumnType("datetime");

            builder.HasOne(x => x.ObjectifFlashTache)
                .WithMany(oft => oft.TacheRealisations)
                .HasForeignKey(x => x.ObjectifFlashTacheId);
            builder.HasOne(x => x.Rapport)
                .WithMany()
                .HasForeignKey(x => x.RapportId);
        }
    }
}
