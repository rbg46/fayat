using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SeuilValidationEntConfiguration : IEntityTypeConfiguration<SeuilValidationEnt>
    {
        public void Configure(EntityTypeBuilder<SeuilValidationEnt> builder)
        {
            builder.ToTable("FRED_ROLE_DEVISE");
            builder.HasKey(sv => sv.SeuilValidationId);
            builder.Property(sv => sv.SeuilValidationId)
                .ValueGeneratedOnAdd();

            builder.HasOne(sv => sv.Devise)
                .WithMany(d => d.SeuilValidations)
                .HasForeignKey(sv => sv.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sv => sv.Role)
                .WithMany(r => r.SeuilsValidation)
                .HasForeignKey(sv => sv.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}