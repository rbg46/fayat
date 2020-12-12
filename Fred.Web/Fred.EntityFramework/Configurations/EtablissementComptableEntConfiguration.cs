using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class EtablissementComptableEntConfiguration : IEntityTypeConfiguration<EtablissementComptableEnt>
    {
        public void Configure(EntityTypeBuilder<EtablissementComptableEnt> builder)
        {
            builder.ToTable("FRED_ETABLISSEMENT_COMPTABLE");
            builder.HasKey(x => x.EtablissementComptableId);
            builder.HasIndex(x => new { x.Code, x.SocieteId })
                .HasName("IX_UniqueCodeAndSociete")
                .IsUnique();
            builder.HasIndex(x => x.OrganisationId)
                .IsUnique(false);

            builder.Property(x => x.EtablissementComptableId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.SocieteId)
                .IsRequired();
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(x => x.Libelle)
                .HasMaxLength(500);
            builder.Property(x => x.Adresse)
                .HasMaxLength(500);
            builder.Property(x => x.Ville)
                .HasMaxLength(500);
            builder.Property(x => x.CodePostal)
                .HasMaxLength(20);
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
            builder.Ignore(x => x.EtablissementsPaie);

            builder.Ignore(x => x.CGAFournitureFileName);
            builder.Ignore(x => x.CGAFournitureFilePath);
            builder.Ignore(x => x.CGALocationFileName);
            builder.Ignore(x => x.CGALocationFilePath);
            builder.Ignore(x => x.CGAPrestationFileName);
            builder.Ignore(x => x.CGAPrestationFilePath);

            builder.Property(b => b.RessourcesRecommandeesEnabled)
                .HasDefaultValue(0);

            builder.HasOne(c => c.Organisation)
                .WithOne(o => o.Etablissement)
                .HasForeignKey<EtablissementComptableEnt>(ec => ec.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Societe)
                .WithMany(s => s.EtablissementComptables)
                .HasForeignKey(x => x.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Pays)
               .WithMany()
               .HasForeignKey(x => x.PaysId);
            builder.HasOne(x => x.AuteurCreation)
                .WithMany()
                .HasForeignKey(x => x.AuteurCreationId);
            builder.HasOne(x => x.AuteurModification)
                .WithMany()
                .HasForeignKey(x => x.AuteurModificationId);
            builder.HasOne(x => x.AuteurSuppression)
                .WithMany()
                .HasForeignKey(x => x.AuteurSuppressionId);
        }
    }
}