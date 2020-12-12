using Fred.Entities.RepartitionEcart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RepartitionEcartEntConfiguration : IEntityTypeConfiguration<RepartitionEcartEnt>
    {
        public void Configure(EntityTypeBuilder<RepartitionEcartEnt> builder)
        {
            builder.ToTable("FRED_REPARTITION_ECART");
            builder.HasKey(re => re.RepartitionEcartId);
            builder.Property(re => re.RepartitionEcartId)
                .ValueGeneratedOnAdd();

            builder.Ignore(re => re.OperationDiverses);
            builder.Ignore(re => re.ChapitreCodes);
            builder.Ignore(re => re.FamilleOperationDiverse);
            builder.Ignore(re => re.EcritureComptables);

            builder.Property(cod => cod.DateCloture)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateComptable)
                .HasColumnType("datetime");

            builder.HasOne(re => re.CI)
                .WithMany()
                .HasForeignKey(re => re.CiId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
