
using Fred.Entities.CI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CIPrimeEntConfiguration : IEntityTypeConfiguration<CIPrimeEnt>
    {
        public void Configure(EntityTypeBuilder<CIPrimeEnt> builder)
        {
            builder.ToTable("FRED_CI_PRIME");
            builder.HasKey(cp => cp.CiPrimeId);

            builder.Property(cp => cp.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cp => cp.DateModification)
                .HasColumnType("datetime");
            builder.Property(cp => cp.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(cp => cp.CiPrimeId)
                .ValueGeneratedOnAdd();

            builder.HasOne(cp => cp.CI)
                .WithMany(c => c.CIPrimes)
                .HasForeignKey(c => c.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cp => cp.Prime)
                .WithMany(p => p.CIPrimes)
                .HasForeignKey(c => c.PrimeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cp => cp.AuteurCreation)
                .WithMany()
                .HasForeignKey(cp => cp.AuteurCreationId);
            builder.HasOne(cp => cp.AuteurModification)
               .WithMany()
               .HasForeignKey(cp => cp.AuteurModificationId);
            builder.HasOne(cp => cp.AuteurSuppression)
               .WithMany()
               .HasForeignKey(cp => cp.AuteurSuppressionId);
        }
    }
}

