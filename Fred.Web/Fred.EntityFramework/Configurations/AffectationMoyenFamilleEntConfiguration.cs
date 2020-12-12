using Fred.Entities.Moyen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AffectationMoyenFamilleEntConfiguration : IEntityTypeConfiguration<AffectationMoyenFamilleEnt>
    {
        public void Configure(EntityTypeBuilder<AffectationMoyenFamilleEnt> builder)
        {
            builder.ToTable("FRED_AFFECTATION_MOYEN_FAMILLE");
            builder.HasKey(am => am.AffectationMoyenFamilleId);
            builder.Property(am => am.AffectationMoyenFamilleId)
                .ValueGeneratedOnAdd();
        }
    }
}
