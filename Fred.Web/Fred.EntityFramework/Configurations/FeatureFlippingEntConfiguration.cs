using Fred.Entities.FeatureFlipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FeatureFlippingEntConfiguration : IEntityTypeConfiguration<FeatureFlippingEnt>
    {
        public void Configure(EntityTypeBuilder<FeatureFlippingEnt> builder)
        {
            builder.ToTable("FRED_FEATURE_FLIPPING");
            builder.HasKey(ff => ff.FeatureId);
            builder.Property(ff => ff.FeatureId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(ff => ff.Code)
                .HasName("IX_UniqueFeatureCode")
                .IsUnique();
            builder.HasIndex(ff => ff.Name)
                .HasName("IX_UniqueFeatureName")
                .IsUnique()
                .HasFilter(null);

            builder.Property(ff => ff.Name)
                .HasMaxLength(250);
            builder.Property(ff => ff.Code)
                .HasDefaultValue(0);
            builder.Property(cod => cod.DateActivation)
                .HasColumnType("datetime");
        }
    }
}
