using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class UniteEntConfiguration : IEntityTypeConfiguration<UniteEnt>
    {
        public void Configure(EntityTypeBuilder<UniteEnt> builder)
        {
             builder.ToTable("FRED_UNITE");
             builder.HasKey(u => u.UniteId);
             builder.Property(u => u.UniteId)
                 .ValueGeneratedOnAdd();
             builder.HasIndex(u => u.Code)
                 .HasName("IX_UniqueCode")
                 .IsUnique();

            builder.Property(u => u.Code)
                 .IsRequired()
                 .HasMaxLength(20);
            builder.Property(u => u.Libelle)
                .IsRequired()
                .HasMaxLength(500);

            builder.Ignore(u => u.CodeLibelle);
        }
    }
}
