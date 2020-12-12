using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AffectationAbsenceEntConfiguration : IEntityTypeConfiguration<AffectationAbsenceEnt>
    {
        public void Configure(EntityTypeBuilder<AffectationAbsenceEnt> builder)
        {
            builder.ToTable("FRED_AFFECTATION_ABSENCE");
            builder.HasKey(to => to.AffectationAbsenceId);
            builder.Property(to => to.AffectationAbsenceId)
                .ValueGeneratedOnAdd();

            builder.HasOne(a => a.Personnel)
                .WithMany()
                .HasForeignKey(a => a.PersonnelId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.CodeAbsence)
                .WithMany()
                .HasForeignKey(a => a.CodeAbsenceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.StatutAbsence)
                .WithMany()
                .HasForeignKey(a => a.StatutAbsenceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.TypeDebut)
                .WithMany()
                .HasForeignKey(a => a.TypeDebutId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.TypeFin)
                .WithMany()
                .HasForeignKey(a => a.TypeFinId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.AuteurCreation)
                .WithMany()
                .HasForeignKey(a => a.AuteurCreationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.AuteurModification)
                .WithMany()
                .HasForeignKey(a => a.AuteurModificationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.AuteurValidation)
                .WithMany()
                .HasForeignKey(a => a.AuteurValidationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
