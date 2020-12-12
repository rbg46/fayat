using Fred.Entities.ValidationPointage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RemonteeVracEntConfiguration : IEntityTypeConfiguration<RemonteeVracEnt>
    {
        public void Configure(EntityTypeBuilder<RemonteeVracEnt> builder)
        {
            builder.ToTable("FRED_REMONTEE_VRAC");
            builder.HasKey(rv => rv.RemonteeVracId);
            builder.Property(rv => rv.RemonteeVracId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateDebut)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateFin)
                .HasColumnType("datetime");
            builder.Property(cod => cod.Periode)
                .HasColumnType("datetime");

            builder.HasOne(rv => rv.AuteurCreation)
                .WithMany()
                .HasForeignKey(rv => rv.AuteurCreationId);
        }
    }
}
