using Fred.Entities.ObjectifFlash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ObjectifFlashTacheRessourceJournalisationEntConfiguration : IEntityTypeConfiguration<ObjectifFlashTacheRessourceJournalisationEnt>
    {
        public void Configure(EntityTypeBuilder<ObjectifFlashTacheRessourceJournalisationEnt> builder)
        {
            builder.ToTable("FRED_OBJECTIF_FLASH_TACHE_RESSOURCE_JOURNALISATION");
            builder.HasKey(x => x.ObjectifFlashTacheRessourceJournalisationId);

            builder.Property(x => x.ObjectifFlashTacheRessourceJournalisationId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.QuantiteObjectif)
                .HasColumnType("decimal(18, 3)");
            builder.Property(cod => cod.DateJournalisation)
                .HasColumnType("datetime");

            builder.HasOne(x => x.ObjectifFlashTacheRessource)
               .WithMany(oftr => oftr.TacheRessourceJournalisations)
               .HasForeignKey(x => x.ObjectifFlashTacheRessourceId);
        }
    }
}
