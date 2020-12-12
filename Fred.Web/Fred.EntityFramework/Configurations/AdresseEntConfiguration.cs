using Fred.Entities.Adresse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AdresseEntConfiguration : AuditableEntityConfiguration<AdresseEnt>, IEntityTypeConfiguration<AdresseEnt>
    {
        public void Configure(EntityTypeBuilder<AdresseEnt> builder)
        {
            builder.ToTable("FRED_ADRESSE");
            builder.HasKey(bt4 => bt4.AdresseId);
            builder.Property(bt4 => bt4.AdresseId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Ligne)
                .HasMaxLength(250);
            builder.Property(a => a.CodePostal)
                .HasMaxLength(10);
            builder.Property(a => a.Ville)
                .HasMaxLength(50);

            builder.HasOne(a => a.Pays)
                .WithMany()
                .HasForeignKey(a => a.PaysId);
        }
    }
}
