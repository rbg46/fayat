
using Fred.Entities.Carburant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CarburantEntConfiguration : IEntityTypeConfiguration<CarburantEnt>
    {
        public void Configure(EntityTypeBuilder<CarburantEnt> builder)
        {
            builder.ToTable("FRED_CARBURANT");
            builder.HasKey(c => c.CarburantId);
            builder.HasIndex(c => c.Code)
                .IsUnique();

            builder.Property(c => c.CarburantId)
                .ValueGeneratedOnAdd();
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(c => c.Libelle)
                .IsRequired()
                .HasMaxLength(500);
            builder.Ignore(c => c.CodeLibelle);

            builder.HasOne(c => c.Unite)
                .WithMany(u => u.Carburants)
                .HasForeignKey(c => c.UniteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

