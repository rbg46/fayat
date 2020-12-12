using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportEntConfiguration : IEntityTypeConfiguration<RapportEnt>
    {
        public void Configure(EntityTypeBuilder<RapportEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT");
            builder.HasKey(r => r.RapportId);
            builder.Property(r => r.RapportId)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Meteo)
                .HasMaxLength(255);
            builder.Property(r => r.Evenements)
                .HasMaxLength(255);
            builder.Property(r => r.IsGenerated)
                .HasDefaultValue(0);

            builder.Property(r => r.DateChantier)
                .HasColumnType("datetime");
            builder.Property(r => r.HoraireDebutM)
                .HasColumnType("datetime");
            builder.Property(r => r.HoraireDebutS)
                .HasColumnType("datetime");
            builder.Property(r => r.HoraireFinM)
                .HasColumnType("datetime");
            builder.Property(r => r.HoraireFinS)
                .HasColumnType("datetime");
            builder.Property(r => r.DateCreation)
                .HasColumnType("datetime");
            builder.Property(r => r.DateModification)
                .HasColumnType("datetime");
            builder.Property(r => r.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(r => r.DateVerrou)
                .HasColumnType("datetime");
            builder.Property(r => r.DateValidationCDC)
                .HasColumnType("datetime");
            builder.Property(r => r.DateValidationCDT)
                .HasColumnType("datetime");
            builder.Property(r => r.DateValidationDRC)
                .HasColumnType("datetime");

            builder.Ignore(r => r.NbMaxPrimes);
            builder.Ignore(r => r.ListPrimes);
            builder.Ignore(r => r.NbMaxTaches);
            builder.Ignore(r => r.ListTaches);
            builder.Ignore(r => r.ListMajorations);
            builder.Ignore(r => r.IsStatutEnCours);
            builder.Ignore(r => r.IsStatutValideRedacteur);
            builder.Ignore(r => r.IsStatutValideConducteur);
            builder.Ignore(r => r.IsStatutValideDirection);
            builder.Ignore(r => r.IsStatutVerrouille);
            builder.Ignore(r => r.TypeRapportEnum);
            builder.Ignore(r => r.CanBeDeleted);
            builder.Ignore(r => r.CanBeLocked);
            builder.Ignore(r => r.CanBeValidated);
            builder.Ignore(r => r.ValidationSuperieur);
            builder.Ignore(r => r.Cloture);
            builder.Ignore(r => r.Verrouille);
            builder.Ignore(r => r.ListErreurs);
            builder.Ignore(r => r.IsStatutValide);
            builder.Ignore(r => r.TypeStatutRapportEnum);
            builder.HasOne(r => r.RapportStatut)
                .WithMany(c => c.Rapports)
                .HasForeignKey(r => r.RapportStatutId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(r => r.AuteurCreation)
                .WithMany()
                .HasForeignKey(r => r.AuteurCreationId);
            builder.HasOne(r => r.AuteurModification)
                .WithMany()
                .HasForeignKey(r => r.AuteurModificationId);
            builder.HasOne(r => r.AuteurSuppression)
                .WithMany()
                .HasForeignKey(r => r.AuteurSuppressionId);
            builder.HasOne(r => r.AuteurVerrou)
                .WithMany()
                .HasForeignKey(r => r.AuteurVerrouId);
            builder.HasOne(r => r.CI)
                .WithMany(r => r.Rapports)
                .HasForeignKey(r => r.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(r => r.ValideurCDC)
                .WithMany()
                .HasForeignKey(r => r.ValideurCDCId);
            builder.HasOne(r => r.ValideurCDT)
                .WithMany()
                .HasForeignKey(r => r.ValideurCDTId);
            builder.HasOne(r => r.ValideurDRC)
                .WithMany()
                .HasForeignKey(r => r.ValideurDRCId);
        }
    }
}
