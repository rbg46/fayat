using Fred.Entities.Societe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SocieteEntConfiguration : IEntityTypeConfiguration<SocieteEnt>
    {
        public void Configure(EntityTypeBuilder<SocieteEnt> builder)
        {
            builder.ToTable("FRED_SOCIETE");
            builder.HasKey(s => s.SocieteId);
            builder.Property(s => s.SocieteId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(s => new
                {
                    s.Code, s.GroupeId
                })
                .HasName("IX_UniqueCodeAndGroupe")
                .IsUnique();

            builder.HasIndex(p => new { p.Code, p.Libelle });

            builder.Property(s => s.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(s => s.Libelle)
                .HasMaxLength(500);
            builder.Property(s => s.CodeSocietePaye)
                .HasMaxLength(20);
            builder.Property(s => s.CodeSocieteComptable)
                .HasMaxLength(20);
            builder.Property(s => s.CodePostal)
                .HasMaxLength(10);
            builder.Property(s => s.Ville)
                .HasMaxLength(50);
            builder.Property(s => s.SIREN)
                .HasMaxLength(19);
            builder.Property(s => s.PiedDePage)
                .HasMaxLength(400);
            builder.Property(s => s.Adresse)
                .HasMaxLength(250);

            builder.Property(s => s.EtablissementParDefaut)
                .HasDefaultValue(0);
            builder.Property(s => s.IsGenerationSamediCPActive)
                .HasDefaultValue(0);
            builder.Property(s => s.ImportFacture)
                .HasDefaultValue(0);
            builder.Property(s => s.TransfertAS400)
                .HasDefaultValue(0);
            builder.Property(s => s.IsInterimaire)
                .HasDefaultValue(0);

            builder.Ignore(s => s.CodeLibelle);
            builder.Ignore(s => s.SocieteGeranteId);
            builder.Ignore(s => s.SocieteGerante);
            builder.Ignore(s => s.IsBudgetAvancementEcart);
            builder.Ignore(s => s.IsBudgetTypeAvancementDynamique);
            builder.Ignore(s => s.IsBudgetSaisieRecette);

            builder.HasOne(s => s.Organisation)
                .WithOne(o => o.Societe)
                .HasForeignKey<SocieteEnt>(o => o.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.IndemniteDeplacementCalculType)
                .WithMany()
                .HasForeignKey(s => s.IndemniteDeplacementCalculTypeId);
            builder.HasOne(s => s.TypeSociete)
                .WithMany()
                .HasForeignKey(s => s.TypeSocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Fournisseur)
                .WithMany()
                .HasForeignKey(s => s.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Classification)
                .WithMany(sc => sc.Societes)
                .HasForeignKey(s => s.SocieteClassificationId);
            builder.HasOne(s => s.ImageLogin)
                .WithMany(sc => sc.LoginSocietes)
                .HasForeignKey(s => s.ImageLoginId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.ImageLogo)
                .WithMany(sc => sc.LogoSocietes)
                .HasForeignKey(s => s.ImageLogoId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.CGAFourniture)
                .WithMany()
                .HasForeignKey(s => s.CGAFournitureId);
            builder.HasOne(s => s.CGALocation)
                .WithMany()
                .HasForeignKey(s => s.CGALocationId);
            builder.HasOne(s => s.CGAPrestation)
                .WithMany()
                .HasForeignKey(s => s.CGAPrestationId);
            builder.HasOne(s => s.Groupe)
                .WithMany(g => g.Societes)
                .HasForeignKey(s => s.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

