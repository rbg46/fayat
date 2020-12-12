
using Fred.Entities.DatesCalendrierPaie;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class DatesCalendrierPaieEntConfiguration : IEntityTypeConfiguration<DatesCalendrierPaieEnt>
    {
        public void Configure(EntityTypeBuilder<DatesCalendrierPaieEnt> builder)
        {
            builder.ToTable("FRED_DATES_CALENDRIER_PAIE");
            builder.HasKey(dcp => dcp.DatesCalendrierPaieId);

            builder.Property(dcp => dcp.DatesCalendrierPaieId)
                .ValueGeneratedOnAdd();
            builder.Property(dcp => dcp.DateFinPointages)
                .HasColumnType("datetime");
            builder.Property(dcp => dcp.DateTransfertPointages)
                .HasColumnType("datetime");

            builder.HasOne(dcp => dcp.Societe)
                .WithMany(s => s.DatesCalendrierPaies)
                .HasForeignKey(dcp => dcp.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

