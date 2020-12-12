using Fred.Entities.ObjectifFlash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ObjectifFlashTacheJournalisationEntConfiguration : IEntityTypeConfiguration<ObjectifFlashTacheJournalisationEnt>
    {
        public void Configure(EntityTypeBuilder<ObjectifFlashTacheJournalisationEnt> builder)
        {
            builder.ToTable("FRED_OBJECTIF_FLASH_TACHE_JOURNALISATION");
            builder.HasKey(x => x.ObjectifFlashTacheJournalisationId);

            builder.Property(x => x.ObjectifFlashTacheJournalisationId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.QuantiteObjectif)
                .HasColumnType("decimal(18, 3)");
            builder.Property(cod => cod.DateJournalisation)
                .HasColumnType("datetime");
            builder.Ignore(x => x.TotalMontantRessource);

            builder.HasOne(x => x.ObjectifFlashTache)
                .WithMany(oft => oft.TacheJournalisations)
                .HasForeignKey(x => x.ObjectifFlashTacheId);
        }
    }
}
