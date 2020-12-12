using Fred.Entities.Facture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FactureEntConfiguration : IEntityTypeConfiguration<FactureEnt>
    {
        public void Configure(EntityTypeBuilder<FactureEnt> builder)
        {
            builder.ToTable("FRED_FACTURE");
            builder.HasKey(f => f.FactureId);

            builder.Property(f => f.FactureId)
                .ValueGeneratedOnAdd();
            builder.Property(f => f.NoFacture)
                .HasMaxLength(50);
            builder.Property(f => f.Commentaire)
                .HasMaxLength(500);
            builder.Property(f => f.NoBonlivraison)
                .HasMaxLength(100);
            builder.Property(f => f.NoBonCommande)
                .HasMaxLength(100);
            builder.Property(f => f.Typefournisseur)
                .HasMaxLength(50);
            builder.Property(f => f.CompteFournisseur)
                .HasMaxLength(50);
            builder.Property(f => f.NoFactureFournisseur)
                .HasMaxLength(100);
            builder.Property(f => f.NoFMFI)
                .HasMaxLength(50);
            builder.Property(f => f.ModeReglement)
                .HasMaxLength(100);
            builder.Property(f => f.Folio)
                .HasMaxLength(10);
            builder.Property(f => f.MontantHT)
                .HasColumnType("numeric(15, 3)");
            builder.Property(f => f.MontantTVA)
                .HasColumnType("numeric(15, 3)");
            builder.Property(f => f.MontantTTC)
                .HasColumnType("numeric(15, 3)");
            builder.Property(f => f.CompteGeneral)
                .HasMaxLength(50);

            builder.Ignore(f => f.RapprochableParUserCourant);
            builder.Ignore(f => f.CICode);
            builder.Ignore(f => f.CiCodeLibelles);
            builder.Ignore(f => f.ScanUrl);

            builder.Property(cod => cod.DateComptable)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateGestion)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateFacture)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateEcheance)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateImport)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateRapprochement)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCloture)
                .HasColumnType("datetime");

            builder.HasOne(f => f.Societe)
                .WithMany(s => s.Factures)
                .HasForeignKey(f => f.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.Fournisseur)
                .WithMany(f => f.Factures)
                .HasForeignKey(f => f.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.Etablissement)
                .WithMany(ec => ec.Factures)
                .HasForeignKey(f => f.EtablissementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.Journal)
                .WithMany(j => j.Factures)
                .HasForeignKey(f => f.JournalId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.Devise)
                .WithMany(d => d.Factures)
                .HasForeignKey(f => f.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.UtilisateurCreation)
                .WithMany()
                .HasForeignKey(f => f.UtilisateurCreationId);
            builder.HasOne(f => f.UtilisateurModification)
                .WithMany()
                .HasForeignKey(f => f.UtilisateurModificationId);
            builder.HasOne(f => f.UtilisateurSupression)
                .WithMany()
                .HasForeignKey(f => f.UtilisateurSupressionId);
            builder.HasOne(f => f.AuteurRapprochement)
               .WithMany()
               .HasForeignKey(f => f.AuteurRapprochementId);
        }
    }
}

