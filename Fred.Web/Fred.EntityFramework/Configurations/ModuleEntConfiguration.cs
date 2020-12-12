using Fred.Entities.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ModuleEntConfiguration : IEntityTypeConfiguration<ModuleEnt>
    {
        public void Configure(EntityTypeBuilder<ModuleEnt> builder)
        {
            builder.ToTable("FRED_MODULE");
            builder.HasKey(m => m.ModuleId);
            builder.HasIndex(m => m.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(m => m.ModuleId)
                .ValueGeneratedOnAdd();
            builder.Property(m => m.Code)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Property(m => m.Libelle)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Property(m => m.DateSuppression)
                .HasColumnType("datetime");
        }
    }
}

