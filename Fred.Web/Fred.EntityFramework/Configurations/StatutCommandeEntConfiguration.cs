using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class StatutCommandeEntConfiguration : IEntityTypeConfiguration<StatutCommandeEnt>
    {
        public void Configure(EntityTypeBuilder<StatutCommandeEnt> builder)
        {
            builder.ToTable("FRED_STATUT_COMMANDE");
            builder.HasKey(sc => sc.StatutCommandeId);
            builder.Property(sc => sc.StatutCommandeId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(sc => sc.Code)
                .IsUnique();

            builder.Property(sc => sc.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(sc => sc.Libelle)
                .HasMaxLength(500);
        }
    }
}