using Fred.Entities.OperationDiverse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FamilleOperationDiverseEntConfiguration : IEntityTypeConfiguration<FamilleOperationDiverseEnt>
    {
        public void Configure(EntityTypeBuilder<FamilleOperationDiverseEnt> builder)
        {
            builder.ToTable("FRED_FAMILLE_OPERATION_DIVERSE");
            builder.HasKey(fod => fod.FamilleOperationDiverseId);
            builder.Property(fod => fod.FamilleOperationDiverseId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(fod => new { fod.Code, fod.SocieteId })
                .HasName("IX_UniqueCodeAndSociete")
                .IsUnique()
                .HasFilter(null);

            builder.Property(fod => fod.IsAccrued)
                .HasDefaultValue(0);
            builder.Property(fod => fod.MustHaveOrder)
                .HasDefaultValue(0);
            builder.Property(fod => fod.IsValued)
                .HasDefaultValue(0);

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.HasOne(fod => fod.Societe)
                .WithMany()
                .HasForeignKey(fod => fod.SocieteId);
            builder.HasOne(fod => fod.AuteurCreation)
                .WithMany()
                .HasForeignKey(fod => fod.AuteurCreationId);
            builder.HasOne(fod => fod.AuteurModification)
                .WithMany()
                .HasForeignKey(fod => fod.AuteurModificationId);

            builder.Property(fod => fod.Code).HasMaxLength(6);
            builder.Property(fod => fod.Libelle).HasMaxLength(250);
        }
    }
}
