using Fred.Entities.OperationDiverse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class OperationDiverseEntConfiguration : IEntityTypeConfiguration<OperationDiverseEnt>
    {
        public void Configure(EntityTypeBuilder<OperationDiverseEnt> builder)
        {
            builder.ToTable("FRED_OPERATION_DIVERSE");
            builder.HasKey(x => x.OperationDiverseId);

            builder.Property(x => x.OperationDiverseId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Libelle)
                .HasMaxLength(250);
            builder.Property(od => od.DateCreation)
                .HasColumnType("datetime");
            builder.Property(od => od.DateComptable)
                .HasColumnType("datetime");
            builder.Property(od => od.DateCloture)
                .HasColumnType("datetime");
            builder.Property(od => od.OdEcart)
                .HasDefaultValue(0);

            builder.Ignore(od => od.RemplacementTaches);

            builder.HasOne(x => x.CI)
                .WithMany()
                .HasForeignKey(x => x.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Tache)
                .WithMany()
                .HasForeignKey(x => x.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Unite)
                .WithMany()
                .HasForeignKey(x => x.UniteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Devise)
                .WithMany()
                .HasForeignKey(x => x.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.GroupeRemplacementTache)
                .WithMany()
                .HasForeignKey(x => x.GroupeRemplacementTacheId);
            builder.HasOne(x => x.AuteurCreation)
                .WithMany()
                .HasForeignKey(x => x.AuteurCreationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.FamilleOperationDiverse)
               .WithMany()
               .HasForeignKey(x => x.FamilleOperationDiverseId);
            builder.HasOne(x => x.Ressource)
               .WithMany()
               .HasForeignKey(x => x.RessourceId);
            builder.HasOne(x => x.EcritureComptable)
               .WithMany()
               .HasForeignKey(x => x.EcritureComptableId);
            builder.HasOne(x => x.OperationDiverseMereAbonnement)
               .WithMany()
               .HasForeignKey(x => x.OperationDiverseMereIdAbonnement);
        }
    }
}
