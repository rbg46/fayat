using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CommandeEntConfiguration : IEntityTypeConfiguration<CommandeEnt>
    {
        public void Configure(EntityTypeBuilder<CommandeEnt> builder)
        {
            builder.ToTable("FRED_COMMANDE");
            builder.HasKey(c => c.CommandeId);
            builder.HasIndex(c => c.Numero);

            builder.Property(c => c.CommandeId)
                .ValueGeneratedOnAdd();
            builder.Ignore(c => c.PatternNumeroCommandeManuelle);
            builder.Property(c => c.Numero)
               .IsRequired()
               .HasMaxLength(10);
            builder.Property(c => c.CiId)
                .IsRequired();
            builder.Property(c => c.StatutCommandeId)
                .IsRequired();
            builder.Property(c => c.LivraisonEntete)
              .HasMaxLength(250);
            builder.Ignore(c => c.IsVisable);
            builder.Ignore(c => c.IsClotureComptable);
            builder.Ignore(c => c.MontantHT);
            builder.Ignore(c => c.MontantHTReceptionne);
            builder.Ignore(c => c.MontantHTSolde);
            builder.Ignore(c => c.PourcentageReceptionne);
            builder.Ignore(c => c.MontantHTFacture);
            builder.Ignore(c => c.MontantFacture);
            builder.Ignore(c => c.IsCreated);
            builder.Ignore(c => c.IsStatutBrouillon);
            builder.Ignore(c => c.IsStatutAValider);
            builder.Ignore(c => c.IsStatutValidee);
            builder.Ignore(c => c.IsStatutCloturee);
            builder.Ignore(c => c.IsStatutManuelleValidee);
            builder.Ignore(c => c.IsValidable);
            builder.Ignore(c => c.CommandeManuelleAllowed);
            builder.Ignore(c => c.SoldeFar);
            builder.Property(c => c.Libelle)
                .HasMaxLength(250);
            builder.Property(c => c.DelaiLivraison)
                .HasMaxLength(100);
            builder.Property(c => c.ContactTel)
                .IsUnicode(false)
                .HasMaxLength(20);
            builder.Property(c => c.LivraisonAdresse)
                .IsUnicode(false)
                .HasMaxLength(500);
            builder.Property(c => c.LivraisonVille)
                .IsUnicode(false)
                .HasMaxLength(250);
            builder.Property(c => c.LivraisonCPostale)
                .IsUnicode(false)
                .HasMaxLength(10);
            builder.Property(c => c.FacturationAdresse)
                .IsUnicode(false)
                .HasMaxLength(500);
            builder.Property(c => c.FacturationVille)
                .IsUnicode(false)
                .HasMaxLength(250);
            builder.Property(c => c.FacturationCPostale)
                .IsUnicode(false)
                .HasMaxLength(10);
            builder.Property(c => c.Justificatif)
                .IsUnicode(false)
                .HasMaxLength(250);
            builder.Property(c => c.CommentaireFournisseur)
                .IsUnicode(false)
                .HasMaxLength(500);
            builder.Property(c => c.CommentaireInterne)
                .IsUnicode(false)
                .HasMaxLength(500);

            builder.Property(cod => cod.Date)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateMiseADispo)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateValidation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DatePremiereImpressionBrouillon)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DatePremiereReception)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateProchaineReception)
                .HasColumnType("datetime");

            builder.Property(b => b.IsAbonnement)
                .HasDefaultValue(0);
            builder.Property(b => b.IsMaterielAPointer)
                .HasDefaultValue(0);
            builder.Property(b => b.IsEnergie)
                .HasDefaultValue(0);

            builder.HasOne(c => c.CI)
                .WithMany(c => c.Commandes)
                .HasForeignKey(c => c.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.StatutCommande)
                .WithMany(sc => sc.Commandes)
                .HasForeignKey(c => c.StatutCommandeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.Contact)
                .WithMany()
                .HasForeignKey(c => c.ContactId);
            builder.HasOne(c => c.Suivi)
               .WithMany()
               .HasForeignKey(c => c.SuiviId);
            builder.HasOne(c => c.Valideur)
              .WithMany()
              .HasForeignKey(c => c.ValideurId);
            builder.HasOne(c => c.AuteurCreation)
             .WithMany()
             .HasForeignKey(c => c.AuteurCreationId);
            builder.HasOne(c => c.AuteurModification)
               .WithMany()
               .HasForeignKey(c => c.AuteurModificationId);
            builder.HasOne(c => c.AuteurSuppression)
              .WithMany()
              .HasForeignKey(c => c.AuteurSuppressionId);
            builder.HasOne(c => c.LivraisonPays)
               .WithMany()
               .HasForeignKey(c => c.LivraisonPaysId);
            builder.HasOne(c => c.FacturationPays)
              .WithMany()
              .HasForeignKey(c => c.FacturationPaysId);
            builder.HasOne(c => c.FournisseurPays)
              .WithMany()
              .HasForeignKey(c => c.FournisseurPaysId);
            builder.HasOne(c => c.Type)
                .WithMany(t => t.Commandes)
                .HasForeignKey(c => c.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.Devise)
                .WithMany(d => d.Commandes)
                .HasForeignKey(c => c.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.SystemeExterne)
               .WithMany()
               .HasForeignKey(c => c.SystemeExterneId);
            builder.HasOne(c => c.Fournisseur)
                .WithMany(f => f.Commandes)
                .HasForeignKey(c => c.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.AuteurPremiereImpressionBrouillon)
                .WithMany()
                .HasForeignKey(c => c.AuteurPremiereImpressionBrouillonId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.OldFournisseur)
              .WithMany()
              .HasForeignKey(c => c.OldFournisseurId);
            builder.HasOne(c => c.TypeEnergie)
              .WithMany()
              .HasForeignKey(c => c.TypeEnergieId);
        }
    }
}