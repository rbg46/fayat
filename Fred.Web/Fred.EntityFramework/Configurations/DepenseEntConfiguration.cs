
using Fred.Entities.Depense;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class DepenseAchatEntConfiguration : IEntityTypeConfiguration<DepenseAchatEnt>
    {
        public void Configure(EntityTypeBuilder<DepenseAchatEnt> builder)
        {
            builder.ToTable("FRED_DEPENSE_ACHAT");
            builder.HasKey(da => da.DepenseId);

            builder.Property(da => da.DepenseId)
                .ValueGeneratedOnAdd();

            builder.Property(da => da.Libelle)
                .HasMaxLength(500);
            builder.Ignore(da => da.ListErreurs);
            builder.Ignore(da => da.RapprochableParUserCourant);
            builder.Ignore(da => da.MontantHT);
            builder.Ignore(da => da.MontantFacturationReception);
            builder.Ignore(da => da.QuantiteFacturationReception);
            builder.Ignore(da => da.MontantFacturationFactureEcart);
            builder.Ignore(da => da.QuantiteFacturationFactureEcart);
            builder.Ignore(da => da.MontantFacturationFacture);
            builder.Ignore(da => da.QuantiteFacturationFacture);
            builder.Ignore(da => da.MontantFacturation);
            builder.Ignore(da => da.QuantiteFacturation);
            builder.Ignore(da => da.SoldeFar);
            builder.Ignore(da => da.MontantFacture);
            builder.Ignore(da => da.Nature);
            builder.Ignore(da => da.MontantFactureHorsEcart);
            builder.Ignore(da => da.HasAjustementFar);
            builder.Ignore(da => da.DateTransfertFar);
            builder.Ignore(da => da.RemplacementTaches);
            builder.Property(da => da.Quantite)
                .HasColumnType("numeric(12, 3)");
            builder.Property(da => da.PUHT)
                .HasColumnType("numeric(18, 8)");
            builder.Property(da => da.Commentaire)
                .HasMaxLength(500);
            builder.Property(da => da.NumeroBL)
                .HasMaxLength(50);

            builder.Property(cod => cod.Date)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateComptable)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateVisaReception)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateFacturation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateControleFar)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateOperation)
                .HasColumnType("datetime");

            builder.Property(cod => cod.QuantiteDepense)
                .HasDefaultValue(0);
            builder.Property(cod => cod.FarAnnulee)
                .HasDefaultValue(0);
            builder.Property(cod => cod.AfficherPuHt)
                .HasDefaultValue(0);
            builder.Property(cod => cod.AfficherQuantite)
                .HasDefaultValue(0);
            builder.Property(cod => cod.IsReceptionInterimaire)
                .HasDefaultValue(0);
            builder.Property(cod => cod.IsReceptionMaterielExterne)
                .HasDefaultValue(0);

            builder.HasOne(da => da.CommandeLigne)
                .WithMany(cl => cl.AllDepenses)
                .HasForeignKey(da => da.CommandeLigneId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(da => da.CI)
                .WithMany(c => c.Depenses)
                .HasForeignKey(da => da.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(da => da.Fournisseur)
                .WithMany(f => f.Depenses)
                .HasForeignKey(da => da.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(da => da.Tache)
                .WithMany(t => t.Depenses)
                .HasForeignKey(da => da.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(da => da.Ressource)
                .WithMany(r => r.Depenses)
                .HasForeignKey(da => da.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(da => da.Unite)
                .WithMany()
                .HasForeignKey(da => da.UniteId);
            builder.HasOne(da => da.AuteurCreation)
                .WithMany()
                .HasForeignKey(da => da.AuteurCreationId);
            builder.HasOne(da => da.AuteurModification)
                .WithMany()
                .HasForeignKey(da => da.AuteurModificationId);
            builder.HasOne(da => da.AuteurSuppression)
                .WithMany()
                .HasForeignKey(da => da.AuteurSuppressionId);
            builder.HasOne(da => da.Devise)
                .WithMany(d => d.Depenses)
                .HasForeignKey(da => da.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(da => da.DepenseParent)
                .WithMany(da => da.Depenses)
                .HasForeignKey(da => da.DepenseParentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(da => da.DepenseType)
              .WithMany(dt => dt.Depenses)
              .HasForeignKey(da => da.DepenseTypeId);
            builder.HasOne(da => da.AuteurVisaReception)
                .WithMany()
                .HasForeignKey(da => da.AuteurVisaReceptionId);
            builder.HasOne(da => da.GroupeRemplacementTache)
                .WithMany()
                .HasForeignKey(da => da.GroupeRemplacementTacheId);
        }
    }
}

