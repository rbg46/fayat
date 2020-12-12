using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PointageAnticipePrimeEntConfiguration : IEntityTypeConfiguration<PointageAnticipePrimeEnt>
    {
        public void Configure(EntityTypeBuilder<PointageAnticipePrimeEnt> builder)
        {
            builder.ToTable("FRED_POINTAGE_ANTICIPE_PRIME");
            builder.HasKey(sbec => sbec.PointageAnticipePrimeId);
            builder.Property(sbec => sbec.PointageAnticipePrimeId)
                .ValueGeneratedOnAdd();

            builder.Ignore(sbec => sbec.PointagePrimeId);
            builder.Ignore(sbec => sbec.PointageId);
            builder.Ignore(sbec => sbec.IsCreated);
            builder.Ignore(sbec => sbec.IsDeleted);

            builder.Property(sbec => sbec.HeurePrime)
                .HasColumnType("float");

            builder.HasOne(sbec => sbec.PointageAnticipe)
                .WithMany(c => c.ListPrimes)
                .HasForeignKey(sbec => sbec.PointageAnticipeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Prime)
                .WithMany(c => c.PointageAnticipePrimes)
                .HasForeignKey(sbec => sbec.PrimeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}