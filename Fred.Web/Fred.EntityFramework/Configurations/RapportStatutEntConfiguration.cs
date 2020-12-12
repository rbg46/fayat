using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportStatutEntConfiguration : IEntityTypeConfiguration<RapportStatutEnt>
    {
        public void Configure(EntityTypeBuilder<RapportStatutEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_STATUT");
            builder.HasKey(sbec => sbec.RapportStatutId);
            builder.Property(sbec => sbec.RapportStatutId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(sbec => sbec.Code)
                .IsUnique();

            builder.Property(sbec => sbec.Code)
                .IsRequired()
                .HasColumnType("nvarchar(20)");
            builder.Property(sbec => sbec.Libelle)
                .IsRequired()
                .HasColumnType("nvarchar(500)");
        }
    }
}