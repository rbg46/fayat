using Fred.Entities.IndemniteDeplacement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class IndemniteDeplacementEntConfiguration : IEntityTypeConfiguration<IndemniteDeplacementEnt>
    {
        public void Configure(EntityTypeBuilder<IndemniteDeplacementEnt> builder)
        {
            builder.ToTable("FRED_INDEMNITE_DEPLACEMENT");
            builder.HasKey(id => id.IndemniteDeplacementId);

            builder.Property(id => id.IndemniteDeplacementId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateDernierCalcul)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(id => id.CI)
                .WithMany()
                .HasForeignKey(id => id.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(id => id.CodeDeplacement)
                .WithMany(cd => cd.IndemniteDeplacements)
                .HasForeignKey(id => id.CodeDeplacementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(id => id.CodeZoneDeplacement)
                .WithMany(czd => czd.IndemniteDeplacements)
                .HasForeignKey(id => id.CodeZoneDeplacementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(id => id.Personnel)
                .WithMany(p => p.IndemniteDeplacements)
                .HasForeignKey(id => id.PersonnelId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(id => id.UtilisateurAuteurCreation)
                .WithMany()
                .HasForeignKey(id => id.AuteurCreation);
            builder.HasOne(id => id.UtilisateurAuteurModification)
               .WithMany()
               .HasForeignKey(id => id.AuteurModification);
            builder.HasOne(id => id.UtilisateurAuteurSuppression)
               .WithMany()
               .HasForeignKey(id => id.AuteurSuppression);
        }
    }
}

