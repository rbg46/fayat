using Fred.Entities.RapportPrime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportPrimeLignePrimeEntConfiguration : IEntityTypeConfiguration<RapportPrimeLignePrimeEnt>
    {
        public void Configure(EntityTypeBuilder<RapportPrimeLignePrimeEnt> builder)
        {
            builder.ToTable("FRED_RAPPORTPRIME_LIGNE_PRIME");
            builder.HasKey(rplp => rplp.RapportPrimeLignePrimeId);
            builder.Property(rplp => rplp.RapportPrimeLignePrimeId)
                .ValueGeneratedOnAdd();

            builder.Ignore(rplp => rplp.IsCreated);
            builder.Ignore(rplp => rplp.IsDeleted);

            builder.Property(rplp => rplp.IsSendToAnael)
                .HasDefaultValue(0);
            builder.Property(rplp => rplp.UpdateDate)
                .HasColumnType("datetime");

            builder.HasOne(rplp => rplp.Prime)
                .WithMany()
                .HasForeignKey(rplp => rplp.PrimeId);
            builder.HasOne(rplp => rplp.RapportPrimeLigne)
                .WithMany(rpl => rpl.ListPrimes)
                .HasForeignKey(rplp => rplp.RapportPrimeLigneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
