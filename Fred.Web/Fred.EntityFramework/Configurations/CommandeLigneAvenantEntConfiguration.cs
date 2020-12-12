using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CommandeLigneAvenantEntConfiguration : IEntityTypeConfiguration<CommandeLigneAvenantEnt>
    {
        public void Configure(EntityTypeBuilder<CommandeLigneAvenantEnt> builder)
        {
            builder.ToTable("FRED_COMMANDE_LIGNE_AVENANT");
            builder.HasKey(cla => cla.CommandeLigneAvenantId);
            builder.Property(cla => cla.CommandeLigneAvenantId)
                .ValueGeneratedOnAdd();

            builder.HasOne(cla => cla.Avenant)
                .WithMany()
                .HasForeignKey(cla => cla.AvenantId);
        }
    }
}
