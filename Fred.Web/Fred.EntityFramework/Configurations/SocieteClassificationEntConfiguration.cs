using Fred.Entities.Societe.Classification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class SocieteClassificationEntConfiguration : IEntityTypeConfiguration<SocieteClassificationEnt>
    {
        public void Configure(EntityTypeBuilder<SocieteClassificationEnt> builder)
        {
            builder.ToTable("FRED_SOCIETE_CLASSIFICATION");
            builder.HasKey(sc => sc.SocieteClassificationId);

            builder.Property(sc => sc.SocieteClassificationId)
                .ValueGeneratedOnAdd();
            builder.Property(sc => sc.Code)
                .HasMaxLength(4000)
                .IsRequired();
            builder.Property(sc => sc.Libelle)
                .HasMaxLength(4000)
                .IsRequired();

            builder.Ignore(sc => sc.CodeLibelle);
        }
    }
}
