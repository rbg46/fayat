using Fred.Entities.Moyen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class MaterielLocationEntConfiguration : IEntityTypeConfiguration<MaterielLocationEnt>
    {
        public void Configure(EntityTypeBuilder<MaterielLocationEnt> builder)
        {
            builder.ToTable("FRED_MATERIEL_LOCATION");
            builder.HasKey(ml => ml.MaterielLocationId);
            builder.Property(ml => ml.MaterielLocationId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(ml => ml.Materiel)
                .WithMany(a => a.MaterielLocations)
                .HasForeignKey(ml => ml.MaterielId);
            builder.HasOne(ml => ml.AuteurCreation)
                .WithMany()
                .HasForeignKey(ml => ml.AuteurCreationId);
            builder.HasOne(ml => ml.AuteurModification)
                .WithMany()
                .HasForeignKey(ml => ml.AuteurModificationId);
            builder.HasOne(ml => ml.AuteurSuppression)
                .WithMany()
                .HasForeignKey(ml => ml.AuteurSuppressionId);
        }
    }
}
