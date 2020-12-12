using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PaysEntConfiguration : IEntityTypeConfiguration<PaysEnt>
    {
        public void Configure(EntityTypeBuilder<PaysEnt> builder)
        {
            builder.ToTable("FRED_PAYS");
            builder.HasKey(x => x.PaysId);
            builder.HasIndex(x => x.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(x => x.PaysId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Code)
                .IsFixedLength()
                .HasColumnType("nvarchar(3)");
            builder.Property(x => x.Libelle)
                .IsFixedLength()
                .HasColumnType("nvarchar(50)");
        }
    }
}

