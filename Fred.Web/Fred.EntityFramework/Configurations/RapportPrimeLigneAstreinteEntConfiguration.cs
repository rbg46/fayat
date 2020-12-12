using Fred.Entities.RapportPrime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportPrimeLigneAstreinteEntConfiguration : IEntityTypeConfiguration<RapportPrimeLigneAstreinteEnt>
    {
        public void Configure(EntityTypeBuilder<RapportPrimeLigneAstreinteEnt> builder)
        {
            builder.ToTable("FRED_RAPPORTPRIME_LIGNE_ASTREINTE");
            builder.HasKey(rpla => rpla.RapportPrimeLigneAstreinteId);
            builder.Property(rpla => rpla.RapportPrimeLigneAstreinteId)
                .ValueGeneratedOnAdd();

            builder.Ignore(rpla => rpla.IsCreated);
            builder.Ignore(rpla => rpla.IsDeleted);

            builder.HasOne(rpla => rpla.RapportPrimeLigne)
                .WithMany(c => c.ListAstreintes)
                .HasForeignKey(rpla => rpla.RapportPrimeLigneId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(rpla => rpla.Astreinte)
                .WithMany()
                .HasForeignKey(rpla => rpla.AstreinteId);
        }
    }
}
