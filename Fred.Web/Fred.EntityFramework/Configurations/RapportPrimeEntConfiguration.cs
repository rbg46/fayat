using Fred.Entities.RapportPrime;
using Fred.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportPrimeEntConfiguration : DeletableConfiguration<RapportPrimeEnt>, IEntityTypeConfiguration<RapportPrimeEnt>
    {
        public new void Configure(EntityTypeBuilder<RapportPrimeEnt> builder)
        {
            base.Configure(builder);

            builder.ToTable("FRED_RAPPORTPRIME");
            builder.HasKey(rp => rp.RapportPrimeId);
            builder.Property(rp => rp.RapportPrimeId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateRapportPrime)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.Property(cod => cod.SocieteId)
                .HasDefaultValue(1);

            builder.HasOne(rp => rp.Societe)
                .WithMany()
                .HasForeignKey(rp => rp.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
