using System;
using Fred.Entities.ValidationPointage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ControlePointageErreurEntConfiguration : IEntityTypeConfiguration<ControlePointageErreurEnt>
    {
        public void Configure(EntityTypeBuilder<ControlePointageErreurEnt> builder)
        {
            builder.ToTable("FRED_CONTROLE_POINTAGE_ERREUR");
            builder.HasKey(cpe => cpe.ControlePointageErreurId);
            builder.Property(cpe => cpe.ControlePointageErreurId)
                .ValueGeneratedOnAdd();

            builder.Property(cpe => cpe.DateRapport)
                .HasColumnType("datetime");

            builder.HasOne(cpe => cpe.ControlePointage)
                .WithMany(cp => cp.Erreurs)
                .HasForeignKey(cpe => cpe.ControlePointageId);
            builder.HasOne(cpe => cpe.Personnel)
                .WithMany()
                .HasForeignKey(cpe => cpe.PersonnelId);

            builder.Property(be => be.Message).IsRequired();
        }
    }
}
