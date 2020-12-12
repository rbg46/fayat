
using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CommandeLigneEntConfiguration : IEntityTypeConfiguration<CommandeLigneEnt>
    {
        public void Configure(EntityTypeBuilder<CommandeLigneEnt> builder)
        {
            builder.ToTable("FRED_COMMANDE_LIGNE");
            builder.HasKey(cl => cl.CommandeLigneId);

            builder.Property(cl => cl.CommandeLigneId)
                .ValueGeneratedOnAdd();
            builder.Property(cl => cl.Libelle)
                .HasMaxLength(500);
            builder.Property(cl => cl.Quantite)
                .HasColumnType("numeric(11, 3)");
            builder.Property(cl => cl.PUHT)
                .HasColumnType("numeric(11, 2)");
            builder.Ignore(cl => cl.MontantHT);
            builder.Ignore(cl => cl.MontantHTReceptionne);
            builder.Ignore(cl => cl.QuantiteReceptionnee);
            builder.Ignore(cl => cl.MontantHTSolde);
            builder.Ignore(cl => cl.Devise);
            builder.Ignore(cl => cl.IsCreated);
            builder.Ignore(cl => cl.IsDeleted);
            builder.Ignore(cl => cl.IsUpdated);
            builder.Ignore(cl => cl.NumeroLibelle);
            builder.Ignore(cl => cl.SoldeFar);
            builder.Ignore(cl => cl.MontantHTFacture);
            builder.Ignore(cl => cl.MontantFacture);
            builder.Ignore(cl => cl.DepensesReception);
            builder.Ignore(cl => cl.DepensesFacture);
            builder.Ignore(cl => cl.DepensesFactureEcart);
            builder.Ignore(cl => cl.DepensesFar);

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.HasOne(cl => cl.Ressource)
                .WithMany(r => r.CommandeLignes)
                .HasForeignKey(cl => cl.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cl => cl.Tache)
                .WithMany(t => t.CommandeLignes)
                .HasForeignKey(cl => cl.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cl => cl.Commande)
                .WithMany(l => l.Lignes)
                .HasForeignKey(cl => cl.CommandeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cl => cl.Unite)
                .WithMany()
                .HasForeignKey(cl => cl.UniteId);
            builder.HasOne(cl => cl.AuteurCreation)
                .WithMany()
                .HasForeignKey(cl => cl.AuteurCreationId);
            builder.HasOne(cl => cl.AuteurModification)
                .WithMany()
                .HasForeignKey(cl => cl.AuteurModificationId);
            builder.HasOne(cl => cl.AvenantLigne)
                .WithMany()
                .HasForeignKey(cl => cl.AvenantLigneId);
            builder.HasOne(cl => cl.Materiel)
               .WithMany(m => m.CommandeLignes)
               .HasForeignKey(cl => cl.MaterielId);
            builder.HasOne(cl => cl.Personnel)
               .WithMany()
               .HasForeignKey(cl => cl.PersonnelId);
        }
    }
}

