using Fred.Entities.Valorisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ValorisationEntConfiguration : IEntityTypeConfiguration<ValorisationEnt>
    {
        public void Configure(EntityTypeBuilder<ValorisationEnt> builder)
        {
            builder.ToTable("FRED_VALORISATION");
            builder.HasKey(v => v.ValorisationId);

            builder.Property(x => x.PUHT)
                .HasColumnType("decimal(18, 3)");
            builder.Property(x => x.Quantite)
                .HasColumnType("decimal(18, 3)");
            builder.Property(x => x.Montant)
                .HasColumnType("decimal(18, 3)");

            builder.Property(cod => cod.Date)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.Ignore(v => v.RemplacementTaches);

            builder.Property(v => v.ValorisationId)
                .ValueGeneratedOnAdd();
            builder.HasOne(v => v.CI)
                .WithMany()
                .HasForeignKey(v => v.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.Rapport)
                .WithMany()
                .HasForeignKey(v => v.RapportId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.RapportLigne)
                .WithMany()
                .HasForeignKey(v => v.RapportLigneId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.Tache)
                .WithMany()
                .HasForeignKey(v => v.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.Chapitre)
                .WithMany()
                .HasForeignKey(v => v.ChapitreId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.SousChapitre)
                .WithMany()
                .HasForeignKey(v => v.SousChapitreId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.ReferentielEtendu)
                .WithMany()
                .HasForeignKey(v => v.ReferentielEtenduId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.Bareme)
                .WithMany()
                .HasForeignKey(v => v.BaremeId);
            builder.HasOne(v => v.BaremeStorm)
                .WithMany()
                .HasForeignKey(v => v.BaremeStormId);
            builder.HasOne(v => v.Unite)
                .WithMany()
                .HasForeignKey(v => v.UniteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.Devise)
                .WithMany()
                .HasForeignKey(v => v.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.Personnel)
                .WithMany(p => p.Valorisations)
                .HasForeignKey(v => v.PersonnelId);
            builder.HasOne(v => v.Materiel)
                .WithMany(m => m.Valorisations)
                .HasForeignKey(v => v.MaterielId);
            builder.HasOne(v => v.GroupeRemplacementTache)
                .WithMany()
                .HasForeignKey(v => v.GroupeRemplacementTacheId);
        }
    }
}
