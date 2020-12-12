using Fred.Entities.EcritureComptable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class EcritureComptableEntConfiguration : IEntityTypeConfiguration<EcritureComptableEnt>
    {
        public void Configure(EntityTypeBuilder<EcritureComptableEnt> builder)
        {
            builder.ToTable("FRED_ECRITURE_COMPTABLE");
            builder.HasKey(ec => ec.EcritureComptableId);
            builder.Property(ec => ec.EcritureComptableId)
                .ValueGeneratedOnAdd();

            builder.Property(ec => ec.DateCreation)
                .HasColumnType("datetime");
            builder.Property(ec => ec.DateComptable)
                .HasColumnType("datetime");
            builder.Property(ec => ec.Quantite)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(ec => ec.Journal)
                .WithMany()
                .HasForeignKey(ec => ec.JournalId);
            builder.HasOne(ec => ec.CI)
                .WithMany()
                .HasForeignKey(ec => ec.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ec => ec.Devise)
                .WithMany()
                .HasForeignKey(ec => ec.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ec => ec.Commande)
                .WithMany()
                .HasForeignKey(ec => ec.CommandeId);
            builder.HasOne(ec => ec.FamilleOperationDiverse)
                .WithMany()
                .HasForeignKey(ec => ec.FamilleOperationDiverseId);

            builder.Ignore(ec => ec.NombreOD);
            builder.Ignore(ec => ec.MontantTotalOD);
        }
    }
}
