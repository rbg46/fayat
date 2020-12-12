using Fred.Entities.ReferentielFixe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RessourceEntConfiguration : IEntityTypeConfiguration<RessourceEnt>
    {
        public void Configure(EntityTypeBuilder<RessourceEnt> builder)
        {
            builder.ToTable("FRED_RESSOURCE");
            builder.HasKey(sbec => sbec.RessourceId);
            builder.Property(sbec => sbec.RessourceId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(s => new
                {
                    s.Code,
                    s.SousChapitreId
                })
                .HasName("IX_UniqueCodeAndSousChapitreId")
                .IsUnique();

            builder.Property(sbec => sbec.Code)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(sbec => sbec.Libelle)
                .HasMaxLength(500);
            builder.Ignore(t => t.CodeLibelle);
            builder.Property(r => r.Consommation)
                .HasColumnType("decimal(7, 2)");

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(r => r.IsRessourceSpecifiqueCi)
                .HasDefaultValue(0);

            builder.Ignore(sbec => sbec.IsRecommandee);

            builder.HasOne(sbec => sbec.SousChapitre)
                .WithMany(c => c.Ressources)
                .HasForeignKey(sbec => sbec.SousChapitreId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.TypeRessource)
                .WithMany(c => c.Ressources)
                .HasForeignKey(sbec => sbec.TypeRessourceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Carburant)
                .WithMany(c => c.Ressources)
                .HasForeignKey(sbec => sbec.CarburantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Parent)
                .WithMany(c => c.RessourcesEnfants)
                .HasForeignKey(sbec => sbec.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.RessourceRattachement)
                .WithMany(c => c.RessourcesRattachementsEnfants)
                .HasForeignKey(sbec => sbec.RessourceRattachementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.SpecifiqueCi)
                .WithMany()
                .HasForeignKey(sbec => sbec.SpecifiqueCiId);

            builder.HasOne(sbec => sbec.AuteurCreation)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurCreationId);
            builder.HasOne(sbec => sbec.AuteurModification)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurModificationId);
            builder.HasOne(sbec => sbec.AuteurSuppression)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurSuppressionId);
        }
    }
}