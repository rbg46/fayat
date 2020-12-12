using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CommandeAvenantEntConfiguration : IEntityTypeConfiguration<CommandeAvenantEnt>
    {
        public void Configure(EntityTypeBuilder<CommandeAvenantEnt> builder)
        {
            builder.ToTable("FRED_COMMANDE_AVENANT");
            builder.HasKey(ca => ca.CommandeAvenantId);
            builder.Property(ca => ca.CommandeAvenantId)
                .ValueGeneratedOnAdd();

            builder.Property(ca => ca.DateValidation)
                .HasColumnType("datetime");

            builder.HasOne(ca => ca.Commande)
                .WithMany()
                .HasForeignKey(ca => ca.CommandeId);
            builder.HasOne(ca => ca.AuteurValidation)
                .WithMany()
                .HasForeignKey(ca => ca.AuteurValidationId);
            builder.HasOne(ca => ca.AuteurCreation)
                .WithMany()
                .HasForeignKey(ca => ca.AuteurCreationId);
        }
    }
}
