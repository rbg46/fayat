using Fred.Entities.Moyen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AffectationMoyenEntConfiguration : IEntityTypeConfiguration<AffectationMoyenEnt>
    {
        public void Configure(EntityTypeBuilder<AffectationMoyenEnt> builder)
        {
            builder.ToTable("FRED_AFFECTATION_MOYEN");
            builder.HasKey(am => am.AffectationMoyenId);
            builder.Property(am => am.AffectationMoyenId)
                .ValueGeneratedOnAdd();

            builder.Property(am => am.DateDebut)
                .HasColumnType("datetime");
            builder.Property(am => am.DateFin)
                .HasColumnType("datetime");

            builder.HasOne(am => am.Materiel)
                .WithMany()
                .HasForeignKey(am => am.MaterielId);
            builder.HasOne(am => am.Ci)
                .WithMany()
                .HasForeignKey(am => am.CiId);
            builder.HasOne(am => am.Personnel)
                .WithMany()
                .HasForeignKey(am => am.PersonnelId);
            builder.HasOne(am => am.Conducteur)
                .WithMany()
                .HasForeignKey(am => am.ConducteurId);
            builder.HasOne(am => am.TypeAffectation)
                .WithMany()
                .HasForeignKey(am => am.AffectationMoyenTypeId);
            builder.HasOne(am => am.Site)
                .WithMany()
                .HasForeignKey(am => am.SiteId);
            builder.HasOne(am => am.MaterielLocation)
                .WithMany()
                .HasForeignKey(am => am.MaterielLocationId);
        }
    }
}
