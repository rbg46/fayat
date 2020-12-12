using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PointageAnticipeEntConfiguration : IEntityTypeConfiguration<PointageAnticipeEnt>
    {
        public void Configure(EntityTypeBuilder<PointageAnticipeEnt> builder)
        {
            builder.ToTable("FRED_POINTAGE_ANTICIPE");
            builder.HasKey(pa => pa.PointageAnticipeId);
            builder.Property(pa => pa.PointageAnticipeId)
                .ValueGeneratedOnAdd();

            builder.Property(pa => pa.PrenomNomTemporaire)
                .HasMaxLength(100);
            builder.Property(pa => pa.HeureNormale)
                .HasColumnType("float");
            builder.Property(pa => pa.HeureMajoration)
                .HasColumnType("float");
            builder.Property(pa => pa.HeureAbsence)
                .HasColumnType("float");

            builder.Property(pa => pa.DatePointage)
                .HasColumnType("datetime");
            builder.Property(pa => pa.DateCreation)
                .HasColumnType("datetime");
            builder.Property(pa => pa.DateModification)
                .HasColumnType("datetime");
            builder.Property(pa => pa.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(pa => pa.IsGenerated)
                .HasDefaultValue(0);

            builder.Ignore(pa => pa.PointageId);
            builder.Ignore(pa => pa.MaterielMarche);
            builder.Ignore(pa => pa.NbMaxPrimes);
            builder.Ignore(pa => pa.IsCreated);
            builder.Ignore(pa => pa.IsDeleted);
            builder.Ignore(pa => pa.ListErreurs);
            builder.Ignore(pa => pa.IsAnticipe);
            builder.Ignore(pa => pa.HeureTotalTravail);

            builder.HasOne(pa => pa.Ci)
                .WithMany(c => c.PointageAnticipes)
                .HasForeignKey(pa => pa.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pa => pa.Personnel)
                .WithMany(c => c.PointageAnticipes)
                .HasForeignKey(pa => pa.PersonnelId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pa => pa.CodeMajoration)
                .WithMany(c => c.PointageAnticipes)
                .HasForeignKey(pa => pa.CodeMajorationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pa => pa.CodeAbsence)
                .WithMany(c => c.PointageAnticipes)
                .HasForeignKey(pa => pa.CodeAbsenceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pa => pa.CodeDeplacement)
                .WithMany(c => c.PointageAnticipes)
                .HasForeignKey(pa => pa.CodeDeplacementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pa => pa.CodeZoneDeplacement)
                .WithMany(c => c.PointageAnticipes)
                .HasForeignKey(pa => pa.CodeZoneDeplacementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pa => pa.AuteurCreation)
                .WithMany()
                .HasForeignKey(pa => pa.AuteurCreationId);
            builder.HasOne(pa => pa.AuteurModification)
                .WithMany()
                .HasForeignKey(pa => pa.AuteurModificationId);
            builder.HasOne(pa => pa.AuteurSuppression)
                .WithMany()
                .HasForeignKey(pa => pa.AuteurSuppressionId);
        }
    }
}