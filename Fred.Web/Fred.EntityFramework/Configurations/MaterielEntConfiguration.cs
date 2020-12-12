using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class MaterielEntConfiguration : IEntityTypeConfiguration<MaterielEnt>
    {
        public void Configure(EntityTypeBuilder<MaterielEnt> builder)
        {
            builder.ToTable("FRED_MATERIEL");
            builder.HasKey(m => m.MaterielId);
            builder.HasIndex(m => new { m.Code, m.SocieteId, m.EtablissementComptableId })
                .HasName("IX_UniqueCodeAndSocieteAndEtablissementComptable")
                .IsUnique();

            builder.Property(m => m.MaterielId)
                .ValueGeneratedOnAdd();
            builder.Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(70);
            builder.Property(m => m.Libelle)
                .IsRequired()
                .HasMaxLength(500);
            builder.Ignore(m => m.LibelleLong);

            builder.Property(m => m.DateCreation)
                .HasColumnType("datetime");
            builder.Property(m => m.DateModification)
                .HasColumnType("datetime");
            builder.Property(m => m.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(m => m.DateDebutLocation)
                .HasColumnType("datetime");
            builder.Property(m => m.DateMiseEnService)
                .HasColumnType("datetime");
            builder.Property(m => m.DateFinLocation)
                .HasColumnType("datetime");

            builder.Property(m => m.MaterielLocation)
                .HasDefaultValue(0);
            builder.Property(m => m.IsStorm)
                .HasDefaultValue(0);
            builder.Property(m => m.DimensionH)
                .HasDefaultValue(0);
            builder.Property(m => m.DimensionL)
                .HasDefaultValue(0);
            builder.Property(m => m.Dimensiionl)
                .HasDefaultValue(0);
            builder.Property(m => m.Puissance)
                .HasDefaultValue(0);
            builder.Property(m => m.IsLocation)
                .HasDefaultValue(0);
            builder.Property(m => m.IsImported)
                .HasDefaultValue(0);

            builder.HasOne(m => m.Societe)
                .WithMany(s => s.Materiels)
                .HasForeignKey(m => m.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(m => m.Ressource)
                .WithMany(r => r.Materiels)
                .HasForeignKey(m => m.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(m => m.Fournisseur)
                .WithMany()
                .HasForeignKey(m => m.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(m => m.EtablissementComptable)
                .WithMany()
                .HasForeignKey(m => m.EtablissementComptableId);
            builder.HasOne(m => m.SiteAppartenance)
                .WithMany()
                .HasForeignKey(m => m.SiteAppartenanceId);
            builder.HasOne(m => m.AuteurCreation)
                .WithMany()
                .HasForeignKey(m => m.AuteurCreationId);
            builder.HasOne(m => m.AuteurModification)
               .WithMany()
               .HasForeignKey(m => m.AuteurModificationId);
            builder.HasOne(m => m.AuteurSuppression)
               .WithMany()
               .HasForeignKey(m => m.AuteurSuppressionId);
        }
    }
}

