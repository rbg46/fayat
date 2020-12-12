using Fred.Entities.ObjectifFlash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ObjectifFlashTacheEntConfiguration : IEntityTypeConfiguration<ObjectifFlashTacheEnt>
    {
        public void Configure(EntityTypeBuilder<ObjectifFlashTacheEnt> builder)
        {
            builder.ToTable("FRED_OBJECTIF_FLASH_TACHE");
            builder.HasKey(oft => oft.ObjectifFlashTacheId);
            builder.Property(oft => oft.ObjectifFlashTacheId)
                .ValueGeneratedOnAdd();

            builder.HasOne(oft => oft.ObjectifFlash)
                .WithMany(of => of.Taches)
                .HasForeignKey(oft => oft.ObjectifFlashId);
            builder.HasOne(oft => oft.Tache)
                .WithMany()
                .HasForeignKey(oft => oft.TacheId);
            builder.HasOne(oft => oft.Unite)
                .WithMany()
                .HasForeignKey(oft => oft.UniteId);

            builder.Property(oft => oft.QuantiteObjectif).HasColumnType("decimal(18, 3)");

            builder.Ignore(oft => oft.TotalQuantiteJournalise);
            builder.Ignore(oft => oft.TotalMontantJournalise);
            builder.Ignore(oft => oft.TotalMontantRessource);
            builder.Ignore(oft => oft.ListErreurs);
        }
    }
}
