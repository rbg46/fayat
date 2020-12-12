using Fred.Entities.Avis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AvisEntConfiguration : AuditableEntityConfiguration<AvisEnt>, IEntityTypeConfiguration<AvisEnt>
    {
        public void Configure(EntityTypeBuilder<AvisEnt> builder)
        {
            builder.ToTable("FRED_AVIS");
            builder.HasKey(a => a.AvisId);
            builder.Property(a => a.AvisId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Commentaire)
                .HasMaxLength(500);

            builder.HasOne(a => a.Expediteur)
                .WithMany()
                .HasForeignKey(a => a.ExpediteurId);
            builder.HasOne(a => a.Destinataire)
                .WithMany()
                .HasForeignKey(a => a.DestinataireId);
        }
    }
}
