using Fred.Entities.Fonctionnalite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FonctionnaliteEntConfiguration : IEntityTypeConfiguration<FonctionnaliteEnt>
    {
        public void Configure(EntityTypeBuilder<FonctionnaliteEnt> builder)
        {
            builder.ToTable("FRED_FONCTIONNALITE");
            builder.HasKey(f => f.FonctionnaliteId);
            builder.HasIndex(f => f.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(f => f.FonctionnaliteId)
                .ValueGeneratedOnAdd();
            builder.Property(f => f.Code)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Property(f => f.Libelle)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Ignore(x => x.Mode);
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(f => f.Module)
                .WithMany(m => m.Fonctionnalites)
                .HasForeignKey(f => f.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

