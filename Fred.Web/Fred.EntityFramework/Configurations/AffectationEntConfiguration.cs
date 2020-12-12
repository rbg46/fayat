using Fred.Entities.Affectation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AffectationEntConfiguration : IEntityTypeConfiguration<AffectationEnt>
    {
        public void Configure(EntityTypeBuilder<AffectationEnt> builder) 
        {
            builder.ToTable("FRED_AFFECTATION");
            builder.HasKey(a => a.AffectationId);
            builder.Property(v => v.AffectationId)
                .ValueGeneratedOnAdd();

            builder.Ignore(a => a.AffectationIsNewOrModified);
            builder.Property(a => a.IsDefault)
                .HasDefaultValue(0);
            builder.Property(a => a.IsDelete)
                .HasDefaultValue(0);

            builder.HasOne(a => a.CI)
                .WithMany(c => c.Affectations)
                .HasForeignKey(a => a.CiId);
            builder.HasOne(a => a.Personnel)
                .WithMany(c => c.Affectations)
                .HasForeignKey(a => a.PersonnelId);
        }
    }
}
