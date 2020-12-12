using Fred.Entities.ValidationPointage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class LotPointageEntConfiguration : AuditableEntityConfiguration<LotPointageEnt>, IEntityTypeConfiguration<LotPointageEnt>
    {
        public new void Configure(EntityTypeBuilder<LotPointageEnt> builder)
        {
            base.Configure(builder);

            builder.ToTable("FRED_LOT_POINTAGE");
            builder.HasKey(lp => lp.LotPointageId);
            builder.Property(lp => lp.LotPointageId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.Periode)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateVisa)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.HasOne(lp => lp.AuteurVisa)
                .WithMany()
                .HasForeignKey(lp => lp.AuteurVisaId);
            builder.HasOne(lp => lp.AuteurModification)
                .WithMany()
                .HasForeignKey(lp => lp.AuteurModificationId);
        }
    }
}
