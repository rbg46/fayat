using Fred.Entities.Facture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FactureLigneEntConfiguration : IEntityTypeConfiguration<FactureLigneEnt>
    {
        public void Configure(EntityTypeBuilder<FactureLigneEnt> builder)
        {
            builder.ToTable("FRED_FACTURE_LIGNE");
            builder.HasKey(fl => fl.LigneFactureId);

            builder.Property(fl => fl.LigneFactureId)
                .ValueGeneratedOnAdd();
            builder.Property(fl => fl.Quantite)
                .HasColumnType("numeric(15, 3)");
            builder.Property(fl => fl.PrixUnitaire)
                .HasColumnType("numeric(15, 3)");
            builder.Property(fl => fl.MontantHT)
                .HasColumnType("numeric(15, 3)");
            builder.Property(fl => fl.NoBonLivraison)
                .HasMaxLength(100);

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(fl => fl.Nature)
                .WithMany(n => n.FactureLignes)
                .HasForeignKey(fl => fl.NatureId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(fl => fl.Facture)
                .WithMany(f => f.ListLigneFacture)
                .HasForeignKey(fl => fl.FactureId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(fl => fl.CI)
                .WithMany(c => c.FactureLignes)
                .HasForeignKey(fl => fl.AffaireId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(fl => fl.UtilisateurCreation)
                .WithMany()
                .HasForeignKey(fl => fl.UtilisateurCreationId);
            builder.HasOne(fl => fl.UtilisateurModification)
                .WithMany()
                .HasForeignKey(fl => fl.UtilisateurModificationId);
            builder.HasOne(fl => fl.UtilisateurSuppression)
                .WithMany()
                .HasForeignKey(fl => fl.UtilisateurSuppressionId);
        }
    }
}

