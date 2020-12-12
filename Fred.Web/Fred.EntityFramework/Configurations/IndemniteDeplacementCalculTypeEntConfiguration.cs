using Fred.Entities.IndemniteDeplacement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class IndemniteDeplacementCalculTypeEntConfiguration : IEntityTypeConfiguration<IndemniteDeplacementCalculTypeEnt>
    {
        public void Configure(EntityTypeBuilder<IndemniteDeplacementCalculTypeEnt> builder)
        {
            builder.ToTable("FRED_INDEMNITE_DEPLACEMENT_CALCUL_TYPE");
            builder.HasKey(idct => idct.IndemniteDeplacementCalculTypeId);

            builder.Property(idct => idct.IndemniteDeplacementCalculTypeId)
                .ValueGeneratedOnAdd();
            builder.Property(idct => idct.Libelle)
                .HasMaxLength(50);
        }
    }
}
