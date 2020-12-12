using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class EtablissementPaieEntConfiguration : IEntityTypeConfiguration<EtablissementPaieEnt>
    {
        public void Configure(EntityTypeBuilder<EtablissementPaieEnt> builder)
        {
            builder.ToTable("FRED_ETABLISSEMENT_PAIE");
            builder.HasKey(x => x.EtablissementPaieId);
            builder.HasIndex(x => new { x.Code, x.SocieteId })
                .HasName("IX_UniqueCodeAndSociete")
                .IsUnique();

            builder.HasIndex(p => new { p.Code, p.Libelle });

            builder.Property(x => x.EtablissementPaieId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(x => x.Libelle)
                .HasMaxLength(500);
            builder.Property(x => x.Adresse)
                .HasMaxLength(500);
            builder.Property(x => x.SocieteId)
                .IsRequired();

            builder.HasOne(x => x.Pays)
               .WithMany()
               .HasForeignKey(x => x.PaysId);
            builder.HasOne(x => x.AgenceRattachement)
                .WithMany(ep => ep.EtablissementPaies)
                .HasForeignKey(x => x.AgenceRattachementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Societe)
                .WithMany(s => s.EtablissementPaies)
                .HasForeignKey(x => x.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.EtablissementComptable)
                .WithMany(ec => ec.EtablissementsPaie)
                .HasForeignKey(x => x.EtablissementComptableId);
        }
    }
}

