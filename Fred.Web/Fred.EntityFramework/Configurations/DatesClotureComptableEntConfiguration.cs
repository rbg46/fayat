
using Fred.Entities.DatesClotureComptable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class DatesClotureComptableEntConfiguration : AuditableEntityConfiguration<DatesClotureComptableEnt>, IEntityTypeConfiguration<DatesClotureComptableEnt>
    {
        public new void Configure(EntityTypeBuilder<DatesClotureComptableEnt> builder)
        {
            base.Configure(builder);

            builder.ToTable("FRED_DATES_CLOTURE_COMPTABLE");
            builder.HasKey(dcc => dcc.DatesClotureComptableId);

            builder.Property(dcc => dcc.DatesClotureComptableId)
                .ValueGeneratedOnAdd();
            builder.Property(dcc => dcc.DateArretSaisie)
                .HasColumnType("datetime");
            builder.Property(dcc => dcc.DateCloture)
                .HasColumnType("datetime");
            builder.Property(dcc => dcc.DateTransfertFAR)
                .HasColumnType("datetime");

            builder.Property(dcc => dcc.Annee)
                .HasDefaultValue(0);
            builder.Property(dcc => dcc.Mois)
                .HasDefaultValue(0);
            builder.Property(dcc => dcc.Historique)
                .HasDefaultValue(false);

            builder.HasOne(dcc => dcc.CI)
                .WithMany(c => c.DatesClotureComptables)
                .HasForeignKey(dcc => dcc.CiId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

