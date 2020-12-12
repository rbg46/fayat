using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CommandeContratInterimaireEntConfiguration : IEntityTypeConfiguration<CommandeContratInterimaireEnt>
    {
        public void Configure(EntityTypeBuilder<CommandeContratInterimaireEnt> builder)
        {
            builder.ToTable("FRED_COMMANDE_CONTRAT_INTERIMAIRE");
            builder.HasKey(cci => new { cci.CommandeId, cci.ContratId, cci.CiId });

            builder.Property(cci => cci.CommandeId)
                .HasColumnName("CommandeId");

            builder.HasOne(cci => cci.Commande)
                .WithOne(c => c.CommandeContratInterimaire)
                .HasForeignKey<CommandeContratInterimaireEnt>(cci => cci.CommandeId);
            builder.HasOne(cci => cci.Contrat)
                .WithMany(c => c.CommandeContratInterimaires)
                .HasForeignKey(cci => cci.ContratId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cci => cci.Ci)
                .WithMany()
                .HasForeignKey(cci => cci.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cci => cci.Interimaire)
                .WithMany()
                .HasForeignKey(cci => cci.InterimaireId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cci => cci.RapportLigne)
                .WithMany()
                .HasForeignKey(cci => cci.RapportLigneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
