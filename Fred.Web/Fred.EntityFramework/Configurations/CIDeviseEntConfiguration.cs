
using Fred.Entities.CI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CIDeviseEntConfiguration : IEntityTypeConfiguration<CIDeviseEnt>
    {
        public void Configure(EntityTypeBuilder<CIDeviseEnt> builder)
        {
            builder.ToTable("FRED_CI_DEVISE");
            builder.HasKey(x => x.CiDeviseId);

            builder.Property(x => x.CiDeviseId)
                .ValueGeneratedOnAdd();

            builder.HasOne(cd => cd.CI)
                .WithMany(c => c.CIDevises)
                .HasForeignKey(cd => cd.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cd => cd.Devise)
                .WithMany(d => d.CIDevises)
                .HasForeignKey(cd => cd.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

