using Fred.Entities.Personnel.Interimaire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class EtatContratInterimaireEntConfiguration : IEntityTypeConfiguration<EtatContratInterimaireEnt>
    {
        public void Configure(EntityTypeBuilder<EtatContratInterimaireEnt> builder)
        {
            builder.ToTable("FRED_ETAT_CONTRAT_INTERIMAIRE");
            builder.HasKey(ci => ci.EtatContratInterimaireId);

            builder.Property(ci => ci.EtatContratInterimaireId)
                .HasColumnName("EtatContratInterimaireId")
                .ValueGeneratedOnAdd();

            builder.Property(ci => ci.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(ci => ci.Libelle)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
