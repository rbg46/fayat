using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class StatutAbsenceEntConfiguration : IEntityTypeConfiguration<StatutAbsenceEnt>
    {
        public void Configure(EntityTypeBuilder<StatutAbsenceEnt> builder)
        {
            builder.ToTable("FRED_STATUT_ABSENCE");
            builder.HasKey(to => to.StatutAbsenceId);
            builder.Property(to => to.StatutAbsenceId)
                .ValueGeneratedOnAdd();

            builder.Property(to => to.Code)
                .IsRequired();
        }
    }
}
