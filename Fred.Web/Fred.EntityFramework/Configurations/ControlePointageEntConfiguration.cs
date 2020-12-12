using Fred.Entities.ValidationPointage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ControlePointageEntConfiguration : IEntityTypeConfiguration<ControlePointageEnt>
    {
        public void Configure(EntityTypeBuilder<ControlePointageEnt> builder)
        {
            builder.ToTable("FRED_CONTROLE_POINTAGE");
            builder.HasKey(cp => cp.ControlePointageId);
            builder.Property(cp => cp.ControlePointageId)
                .ValueGeneratedOnAdd();

            builder.Property(cp => cp.DateDebut)
                .HasColumnType("datetime");
            builder.Property(cp => cp.DateFin)
                .HasColumnType("datetime");

            builder.HasOne(cp => cp.LotPointage)
                .WithMany(lp => lp.ControlePointages)
                .HasForeignKey(cp => cp.LotPointageId);
            builder.HasOne(cp => cp.AuteurCreation)
                .WithMany()
                .HasForeignKey(cp => cp.AuteurCreationId);
        }
    }
}
