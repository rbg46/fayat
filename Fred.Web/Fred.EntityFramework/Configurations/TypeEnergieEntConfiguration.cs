using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TypeEnergieEntConfiguration : IEntityTypeConfiguration<TypeEnergieEnt>
    {
        public void Configure(EntityTypeBuilder<TypeEnergieEnt> builder)
        {
            builder.ToTable("FRED_TYPE_ENERGIE");
            builder.HasKey(te => te.TypeEnergieId);
            builder.Property(te => te.TypeEnergieId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(te => te.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(te => te.Code)
                .HasMaxLength(20);
        }
    }
}
