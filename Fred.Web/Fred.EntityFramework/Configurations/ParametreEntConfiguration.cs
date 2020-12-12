using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ParametreEntConfiguration : IEntityTypeConfiguration<ParametreEnt>
    {
        public void Configure(EntityTypeBuilder<ParametreEnt> builder)
        {
            builder.ToTable("FRED_PARAMETRE");
            builder.HasKey(p => p.ParametreId);

            builder.Property(p => p.ParametreId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(p => p.Libelle)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(p => p.Valeur)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(p => p.Groupe)
                .WithMany(g => g.Parametres)
                .HasForeignKey(p => p.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

