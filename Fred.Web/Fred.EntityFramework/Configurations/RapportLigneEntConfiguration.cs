using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportLigneEntConfiguration : IEntityTypeConfiguration<RapportLigneEnt>
    {
        public void Configure(EntityTypeBuilder<RapportLigneEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_LIGNE");
            builder.HasKey(sbec => sbec.RapportLigneId);
            builder.Property(sbec => sbec.RapportLigneId)
                .ValueGeneratedOnAdd();

            builder.Property(sbec => sbec.PrenomNomTemporaire)
                .HasMaxLength(100);
            builder.Property(sbec => sbec.HeureNormale)
                .HasColumnType("float");
            builder.Property(sbec => sbec.HeureMajoration)
                .HasColumnType("float");
            builder.Property(sbec => sbec.HeureAbsence)
                .HasColumnType("float");
            builder.Property(sbec => sbec.MaterielMarche)
                .HasColumnType("float");
            builder.Property(sbec => sbec.MaterielArret)
                .HasColumnType("float");
            builder.Property(sbec => sbec.MaterielPanne)
                .HasColumnType("float");
            builder.Property(sbec => sbec.MaterielIntemperie)
                .HasColumnType("float");
            builder.Property(sbec => sbec.MaterielNomTemporaire)
                .HasMaxLength(100);
            builder.Property(sbec => sbec.Commentaire)
                .HasMaxLength(250);

            builder.Property(cod => cod.DatePointage)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateValidation)
                .HasColumnType("datetime");

            builder.Property(cod => cod.IsGenerated)
                .HasDefaultValue(0);
            builder.Property(cod => cod.AvecChauffeur)
                .HasDefaultValue(0);
            builder.Property(cod => cod.ReceptionInterimaire)
                .HasDefaultValue(0);
            builder.Property(cod => cod.HeuresTotalAstreintes)
                .HasDefaultValue((double)0);
            builder.Property(cod => cod.CodeZoneDeplacementSaisiManuellement)
                .HasDefaultValue(0);
            builder.Property(cod => cod.HeuresMachine)
                .HasDefaultValue((double)0);
            builder.Property(cod => cod.ReceptionMaterielExterne)
                .HasDefaultValue(0);

            builder.Ignore(sbec => sbec.Cloture);
            builder.Ignore(sbec => sbec.MonPerimetre);
            builder.Ignore(sbec => sbec.NbMaxPrimes);
            builder.Ignore(sbec => sbec.HasAstreinte);
            builder.Ignore(sbec => sbec.AstreinteId);
            builder.Ignore(sbec => sbec.IsCreated);
            builder.Ignore(sbec => sbec.IsDeleted);
            builder.Ignore(sbec => sbec.IsLocked);
            builder.Ignore(sbec => sbec.RapportLigneType);
            builder.Ignore(sbec => sbec.ListErreurs);
            builder.Ignore(sbec => sbec.IsAnticipe);
            builder.Ignore(sbec => sbec.HeureTotalTravail);
            builder.Ignore(sbec => sbec.IsUpdated);
            builder.Ignore(sbec => sbec.IsLotPointageIdUpdated);
            builder.Ignore(sbec => sbec.CodeDeplacementPlusFavorable);
            builder.Ignore(sbec => sbec.PointageId);
            builder.Ignore(sbec => sbec.IsAllReadyAddedInRapportHebdo);

            builder.HasOne(sbec => sbec.Rapport)
                .WithMany(c => c.ListLignes)
                .HasForeignKey(sbec => sbec.RapportId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Ci)
                .WithMany(c => c.RapportLignes)
                .HasForeignKey(sbec => sbec.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.AffectationMoyen)
                .WithMany()
                .HasForeignKey(sbec => sbec.AffectationMoyenId);
            builder.HasOne(sbec => sbec.Personnel)
                .WithMany(sbec => sbec.RapportLignes)
                .HasForeignKey(sbec => sbec.PersonnelId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.CodeMajoration)
                .WithMany(sbec => sbec.RapportLignes)
                .HasForeignKey(sbec => sbec.CodeMajorationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.CodeAbsence)
                .WithMany(sbec => sbec.RapportLignes)
                .HasForeignKey(sbec => sbec.CodeAbsenceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.CodeDeplacement)
                .WithMany(sbec => sbec.RapportLignes)
                .HasForeignKey(sbec => sbec.CodeDeplacementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.CodeZoneDeplacement)
                .WithMany()
                .HasForeignKey(sbec => sbec.CodeZoneDeplacementId);
            builder.HasOne(sbec => sbec.AuteurCreation)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurCreationId);
            builder.HasOne(sbec => sbec.AuteurModification)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurModificationId);
            builder.HasOne(sbec => sbec.AuteurSuppression)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurSuppressionId);
            builder.HasOne(sbec => sbec.Materiel)
                .WithMany(sbec => sbec.RapportLignes)
                .HasForeignKey(sbec => sbec.MaterielId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.RapportLigneStatut)
                .WithMany(sbec => sbec.RapportLignes)
                .HasForeignKey(sbec => sbec.RapportLigneStatutId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Valideur)
                .WithMany()
                .HasForeignKey(sbec => sbec.ValideurId);
        }
    }
}
