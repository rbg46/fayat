using Fred.Entities.Facturation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FacturationTypeEntConfiguration : IEntityTypeConfiguration<FacturationTypeEnt>
    {
        public void Configure(EntityTypeBuilder<FacturationTypeEnt> builder)
        {
            builder.ToTable("FRED_FACTURATION_TYPE");
            builder.HasKey(ft => ft.FacturationTypeId);
            builder.Property(ft => ft.FacturationTypeId)
                .ValueGeneratedOnAdd();
            builder.Property(ft => ft.Code)
                .HasDefaultValue(0);

            builder.HasIndex(ft => ft.Code)
                .HasName("IX_UniqueCode")
                .IsUnique();
        }
    }
}
