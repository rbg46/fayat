using Fred.Entities.Avis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AvisCommandeAvenantEntConfiguration : IEntityTypeConfiguration<AvisCommandeAvenantEnt>
    {
        public void Configure(EntityTypeBuilder<AvisCommandeAvenantEnt> builder)
        {
            builder.ToTable("FRED_AVIS_COMMANDE_AVENANT");
            builder.HasKey(aca => aca.AvisCommandeAvenantId);
            builder.Property(aca => aca.AvisCommandeAvenantId)
                .ValueGeneratedOnAdd();

            builder.HasOne(aca => aca.Avis)
                .WithMany()
                .HasForeignKey(aca => aca.AvisId);
            builder.HasOne(aca => aca.CommandeAvenant)
                .WithMany(ca => ca.AvisCommandeAvenant)
                .HasForeignKey(aca => aca.CommandeAvenantId);
        }
    }
}
