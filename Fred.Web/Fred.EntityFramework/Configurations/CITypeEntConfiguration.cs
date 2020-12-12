using Fred.Entities.CI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CITypeEntConfiguration : IEntityTypeConfiguration<CITypeEnt>
    {
        public void Configure(EntityTypeBuilder<CITypeEnt> builder)
        {
            builder.ToTable("FRED_CI_TYPE");
            builder.HasKey(cit => cit.CITypeId);
            builder.Property(cit => cit.CITypeId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(cit => cit.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(cit => cit.Code).HasMaxLength(5);
        }
    }
}
