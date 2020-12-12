using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportLignePrimeEntConfiguration : IEntityTypeConfiguration<RapportLignePrimeEnt>
    {
        public void Configure(EntityTypeBuilder<RapportLignePrimeEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_LIGNE_PRIME");
            builder.HasKey(sbec => sbec.RapportLignePrimeId);
            builder.Property(sbec => sbec.RapportLignePrimeId)
                .ValueGeneratedOnAdd();

            builder.Property(sbec => sbec.HeurePrime)
                .HasColumnType("float");

            builder.Ignore(sbec => sbec.PointagePrimeId);
            builder.Ignore(sbec => sbec.PointageId);
            builder.Ignore(sbec => sbec.IsCreated);
            builder.Ignore(sbec => sbec.IsDeleted);

            builder.HasOne(sbec => sbec.RapportLigne)
                .WithMany(c => c.ListRapportLignePrimes)
                .HasForeignKey(sbec => sbec.RapportLigneId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Prime)
                .WithMany(c => c.RapportLignePrimes)
                .HasForeignKey(sbec => sbec.PrimeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}