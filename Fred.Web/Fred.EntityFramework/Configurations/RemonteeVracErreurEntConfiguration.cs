using Fred.Entities.ValidationPointage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RemonteeVracErreurEntConfiguration : IEntityTypeConfiguration<RemonteeVracErreurEnt>
    {
        public void Configure(EntityTypeBuilder<RemonteeVracErreurEnt> builder)
        {
            builder.ToTable("FRED_REMONTEE_VRAC_ERREUR");
            builder.HasKey(tve => tve.RemonteeVracErreurId);
            builder.Property(tve => tve.RemonteeVracErreurId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateDebut)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateFin)
                .HasColumnType("datetime");

            builder.HasOne(tve => tve.RemonteeVrac)
                .WithMany(rv => rv.Erreurs)
                .HasForeignKey(tve => tve.RemonteeVracId);
            builder.HasOne(tve => tve.Societe)
                .WithMany()
                .HasForeignKey(tve => tve.SocieteId);
            builder.HasOne(tve => tve.EtablissementPaie)
                .WithMany()
                .HasForeignKey(tve => tve.EtablissementPaieId);
            builder.HasOne(tve => tve.Personnel)
                .WithMany()
                .HasForeignKey(tve => tve.PersonnelId);
        }
    }
}
