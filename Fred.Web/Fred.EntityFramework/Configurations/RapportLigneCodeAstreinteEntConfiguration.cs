using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class RapportLigneCodeAstreinteEntConfiguration : IEntityTypeConfiguration<RapportLigneCodeAstreinteEnt>
    {
        public void Configure(EntityTypeBuilder<RapportLigneCodeAstreinteEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_LIGNE_CODE_ASTREINTE");
            builder.HasKey(rlca => rlca.RapportLigneCodeAstreinteId);

            builder.Property(rlca => rlca.RapportLigneCodeAstreinteId)
                .ValueGeneratedOnAdd();
            builder.Property(cod => cod.IsPrimeNuit)
                .HasDefaultValue(false);

            builder.HasOne(rlca => rlca.RapportLigne)
                .WithMany(rl => rl.ListCodePrimeAstreintes)
                .HasForeignKey(rlca => rlca.RapportLigneId);
            builder.HasOne(rlca => rlca.CodeAstreinte)
                .WithMany()
                .HasForeignKey(rlca => rlca.CodeAstreinteId);
            builder.HasOne(rlca => rlca.RapportLigneAstreinte)
                .WithMany(rla => rla.ListCodePrimeSortiesAstreintes)
                .HasForeignKey(rlca => rlca.RapportLigneAstreinteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
