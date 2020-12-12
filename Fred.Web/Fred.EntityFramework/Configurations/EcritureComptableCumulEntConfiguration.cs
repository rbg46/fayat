using Fred.Entities.EcritureComptable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class EcritureComptableCumulEntConfiguration : IEntityTypeConfiguration<EcritureComptableCumulEnt>
    {
        public void Configure(EntityTypeBuilder<EcritureComptableCumulEnt> builder)
        {
            builder.ToTable("FRED_ECRITURE_COMPTABLE_CUMUL");
            builder.HasKey(ecc => ecc.EcritureComptableCumulId);
            builder.Property(ecc => ecc.EcritureComptableCumulId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateComptable)
                .HasColumnType("datetime");

            builder.Property(ecc => ecc.NumeroPiece).IsRequired();
        }
    }
}
