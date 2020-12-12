using Fred.Entities.EcritureComptable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class EcritureComptableRejetEntConfiguration : IEntityTypeConfiguration<EcritureComptableRejetEnt>
    {
        public void Configure(EntityTypeBuilder<EcritureComptableRejetEnt> builder)
        {
            builder.ToTable("FRED_ECRITURE_COMPTABLE_REJET");
            builder.HasKey(ecr => ecr.EcritureComptableRejet);
            builder.Property(ecr => ecr.EcritureComptableRejet)
                .HasColumnName("EcritureComptableRejetId")
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateRejet)
                .HasColumnType("datetime");
        }
    }
}
