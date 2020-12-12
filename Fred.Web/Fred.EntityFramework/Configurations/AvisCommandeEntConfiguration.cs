using Fred.Entities.Avis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AvisCommandeEntConfiguration : IEntityTypeConfiguration<AvisCommandeEnt>
    {
        public void Configure(EntityTypeBuilder<AvisCommandeEnt> builder)
        {
            builder.ToTable("FRED_AVIS_COMMANDE");
            builder.HasKey(ac => ac.AvisCommandeId);
            builder.Property(ac => ac.AvisCommandeId)
                .ValueGeneratedOnAdd();

            builder.HasOne(ac => ac.Avis)
                .WithMany()
                .HasForeignKey(ac => ac.AvisId);
            builder.HasOne(ac => ac.Commande)
                .WithMany(ac => ac.AvisCommande)
                .HasForeignKey(ac => ac.CommandeId);
        }
    }
}
