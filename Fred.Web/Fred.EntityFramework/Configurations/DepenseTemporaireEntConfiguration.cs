
using Fred.Entities.Depense;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class DepenseTemporaireEntConfiguration : IEntityTypeConfiguration<DepenseTemporaireEnt>
    {
        public void Configure(EntityTypeBuilder<DepenseTemporaireEnt> builder)
        {
            builder.ToTable("FRED_DEPENSE_TEMPORAIRE");
            builder.HasKey(dt => dt.DepenseId);

            builder.Property(dt => dt.DepenseId)
                .ValueGeneratedOnAdd();
            builder.Property(dt => dt.Libelle)
                .HasMaxLength(500);
            builder.Property(dt => dt.Quantite)
                .HasColumnType("numeric(11, 2)");
            builder.Property(dt => dt.PUHT)
                .HasColumnType("numeric(11, 2)");
            builder.Property(dt => dt.Commentaire)
                .HasMaxLength(500);
            builder.Property(dt => dt.NumeroBL)
                .HasMaxLength(50);
            builder.Property(dt => dt.TypeDepense)
                .HasMaxLength(20);
            builder.Property(dt => dt.FactureId)
                .IsRequired();
            builder.Ignore(dt => dt.RapprochableParUserCourant);
            builder.Ignore(dt => dt.MontantHT);
            builder.Ignore(dt => dt.ListErreurs);

            builder.Property(cod => cod.Date)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateReception)
                .HasColumnType("datetime");

            builder.HasOne(dt => dt.CommandeLigne)
                .WithMany(cl => cl.DepenseTemporaires)
                .HasForeignKey(dt => dt.CommandeLigneId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.CI)
                .WithMany(c => c.DepenseTemporaires)
                .HasForeignKey(dt => dt.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.Fournisseur)
                .WithMany(f => f.DepenseTemporaires)
                .HasForeignKey(dt => dt.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.Ressource)
                .WithMany(r => r.DepenseTemporaires)
                .HasForeignKey(dt => dt.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.Tache)
                .WithMany(t => t.DepenseTemporaires)
                .HasForeignKey(dt => dt.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.Unite)
                .WithMany()
                .HasForeignKey(dt => dt.UniteId);
            builder.HasOne(dt => dt.AuteurCreation)
               .WithMany()
               .HasForeignKey(dt => dt.AuteurCreationId);
            builder.HasOne(dt => dt.Devise)
                .WithMany(d => d.DepenseTemporaires)
                .HasForeignKey(dt => dt.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.DepenseOrigine)
                .WithMany(da => da.DepenseOrigine)
                .HasForeignKey(dt => dt.DepenseOrigineId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.DepenseParent)
                .WithMany(da => da.DepenseTemporaireEnts)
                .HasForeignKey(dt => dt.DepenseParentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.FactureLigne)
                .WithMany(fl => fl.DepenseTemporaires)
                .HasForeignKey(dt => dt.FactureLigneId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.Facture)
                .WithMany(f => f.DepenseTemporaires)
                .HasForeignKey(dt => dt.FactureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

