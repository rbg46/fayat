using Fred.Entities.Moyen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AffectationMoyenTypeEntConfiguration : IEntityTypeConfiguration<AffectationMoyenTypeEnt>
    {
        public void Configure(EntityTypeBuilder<AffectationMoyenTypeEnt> builder)
        {
            builder.ToTable("FRED_AFFECTATION_MOYEN_TYPE");
            builder.HasKey(am => am.AffectationMoyenTypeId);
            builder.Property(am => am.AffectationMoyenTypeId)
                .ValueGeneratedOnAdd();

            builder.HasOne(am => am.AffectationMoyenFamille)
                .WithMany()
                .HasForeignKey(am => am.AffectationMoyenFamilleId);
        }
    }
}
