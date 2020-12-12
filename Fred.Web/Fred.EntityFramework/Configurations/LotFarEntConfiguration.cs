using Fred.Entities.Depense;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class LotFarEntConfiguration : IEntityTypeConfiguration<LotFarEnt>
    {
        public void Configure(EntityTypeBuilder<LotFarEnt> builder)
        {
            builder.ToTable("FRED_LOT_FAR");
            builder.HasKey(lr => lr.LotFarId);
            builder.Property(lr => lr.LotFarId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateComptable)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");

            builder.HasOne(lr => lr.AuteurCreation)
                .WithMany()
                .HasForeignKey(lr => lr.AuteurCreationId);
        }
    }
}
