using Fred.Entities.Affectation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AstreinteEntConfiguration : IEntityTypeConfiguration<AstreinteEnt>
    {
        public void Configure(EntityTypeBuilder<AstreinteEnt> builder)
        {
            builder.ToTable("FRED_ASTREINTE");
            builder.HasKey(a => a.AstreintId);
            builder.Property(a => a.AstreintId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.DateAstreinte)
                .HasColumnType("datetime");

            builder.HasOne(a => a.Affectation)
                .WithMany(a => a.Astreintes)
                .HasForeignKey(a => a.AffectationId);
        }
    }
}
