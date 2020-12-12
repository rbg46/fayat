using Fred.Entities.Depense;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class DepenseTypeEntConfiguration : IEntityTypeConfiguration<DepenseTypeEnt>
    {
        public void Configure(EntityTypeBuilder<DepenseTypeEnt> builder)
        {
            builder.ToTable("FRED_DEPENSE_TYPE");
            builder.HasKey(dt => dt.DepenseTypeId);
            builder.Property(dt => dt.DepenseTypeId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(dt => dt.Code)
                .HasName("IX_UniqueCode")
                .IsUnique();

            builder.Property(dt => dt.Code)
                .HasDefaultValue(0);
        }
    }
}
