using Fred.Entities.Budget.Avancement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AvancementEtatEntConfiguration : IEntityTypeConfiguration<AvancementEtatEnt>
    {
        public void Configure(EntityTypeBuilder<AvancementEtatEnt> builder)
        {
            builder.ToTable("FRED_AVANCEMENT_ETAT");
            builder.HasKey(ae => ae.AvancementEtatId);
            builder.Property(ae => ae.AvancementEtatId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(ae => ae.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(ae => ae.Code)
                .HasMaxLength(20);
        }
    }
}
