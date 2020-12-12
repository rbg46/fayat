
using Fred.Entities.CI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CIEntConfiguration : IEntityTypeConfiguration<CIEnt>
    {
        public void Configure(EntityTypeBuilder<CIEnt> builder)
        {
            builder.ToTable("FRED_CI");
            builder.HasKey(c => c.CiId);

            builder.Property(c => c.CiId)
                .ValueGeneratedOnAdd();
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(c => c.CodeInterne)
                .ValueGeneratedOnAddOrUpdate()
                .HasComputedColumnSql("CONCAT(Code, '-', EtablissementComptableId, '-', SocieteId)");

            builder.HasIndex(c => c.CodeInterne)
                .HasName("IX_UniqueCodeInterne")
                .IsUnique()
                .HasFilter(null);

            builder.Property(c => c.Libelle)
                .HasMaxLength(500);
            builder.Property(c => c.Adresse)
                .HasMaxLength(500);
            builder.Property(c => c.Ville)
                .HasMaxLength(500);
            builder.Property(c => c.CodePostal)
                .HasMaxLength(20);
            builder.Property(c => c.EnteteLivraison)
                .HasMaxLength(100);
            builder.Property(c => c.AdresseLivraison)
                .HasMaxLength(500);
            builder.Property(c => c.CodePostalLivraison)
                .HasMaxLength(20);
            builder.Property(c => c.VilleLivraison)
                .HasMaxLength(500);
            builder.Property(c => c.AdresseFacturation)
                .HasMaxLength(500);
            builder.Property(c => c.CodePostalFacturation)
                .HasMaxLength(20);
            builder.Property(c => c.VilleFacturation)
                .HasMaxLength(500);
            builder.Property(c => c.ResponsableChantier)
                .HasMaxLength(100);
            builder.Property(c => c.FraisGeneraux)
                .HasColumnType("decimal(4, 2)");
            builder.Property(c => c.TauxHoraire)
                .HasColumnType("decimal(4, 2)");
            builder.Property(c => c.TypeCI)
                .HasMaxLength(100);
            builder.Property(c => c.MontantHT)
                .HasColumnType("decimal(18, 2)");
            builder.Property(c => c.Sep)
                .HasDefaultValue(0);
            builder.Property(c => c.ZoneModifiable)
                .HasDefaultValue(0);
            builder.Property(c => c.CarburantActif)
                .HasDefaultValue(0);
            builder.Property(c => c.ChantierFRED)
                .HasDefaultValue(0);
            builder.Property(c => c.IsAstreinteActive)
                .HasDefaultValue(0);

            builder.Property(cod => cod.DateOuverture)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateFermeture)
                .HasColumnType("datetime");
            builder.Property(cod => cod.HoraireDebutM)
                .HasColumnType("datetime");
            builder.Property(cod => cod.HoraireFinM)
                .HasColumnType("datetime");
            builder.Property(cod => cod.HoraireDebutS)
                .HasColumnType("datetime");
            builder.Property(cod => cod.HoraireFinS)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateImport)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateUpdate)
                .HasColumnType("datetime");

            builder.Ignore(c => c.IsClosed);
            builder.Ignore(c => c.IsCiHaveManyDevise);
            builder.Ignore(c => c.CodeLibelle);
            builder.Ignore(c => c.Parents);

            builder.HasOne(c => c.Organisation)
                .WithOne(o => o.CI)
                .HasForeignKey<CIEnt>(c => c.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.EtablissementComptable)
                .WithMany()
                .HasForeignKey(c => c.EtablissementComptableId);
            builder.HasOne(c => c.Societe)
                .WithMany()
                .HasForeignKey(c => c.SocieteId);
            builder.HasOne(c => c.Pays)
                .WithMany()
                .HasForeignKey(c => c.PaysId);
            builder.HasOne(c => c.PaysLivraison)
                .WithMany()
                .HasForeignKey(c => c.PaysLivraisonId);
            builder.HasOne(c => c.PaysFacturation)
                .WithMany()
                .HasForeignKey(c => c.PaysFacturationId);
            builder.HasOne(c => c.ResponsableAdministratif)
                .WithMany()
                .HasForeignKey(c => c.ResponsableAdministratifId);
            builder.HasOne(c => c.CIType)
                .WithMany()
                .HasForeignKey(c => c.CITypeId);
            builder.HasOne(c => c.MontantDevise)
                .WithMany().
                HasForeignKey(c => c.MontantDeviseId);
            builder.HasOne(c => c.CompteInterneSep)
                .WithMany()
                .HasForeignKey(c => c.CompteInterneSepId);
            builder.HasOne(c => c.PersonnelResponsableChantier)
                .WithMany()
                .HasForeignKey(c => c.ResponsableChantierId);
        }
    }
}

