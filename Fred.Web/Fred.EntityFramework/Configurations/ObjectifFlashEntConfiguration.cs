using Fred.Entities.ObjectifFlash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ObjectifFlashEntConfiguration : IEntityTypeConfiguration<ObjectifFlashEnt>
    {
        public void Configure(EntityTypeBuilder<ObjectifFlashEnt> builder)
        {
            builder.ToTable("FRED_OBJECTIF_FLASH");
            builder.HasKey(of => of.ObjectifFlashId);
            builder.Property(of => of.ObjectifFlashId)
                .ValueGeneratedOnAdd();

            builder.Property(of => of.CiId)
                .IsRequired();
            builder.Property(of => of.TotalMontantObjectif)
                .HasColumnType("decimal(18, 3)");

            builder.Property(cod => cod.DateDebut)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateFin)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCloture)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.Property(cod => cod.IsActif)
                .HasDefaultValue(0);

            builder.Ignore(of => of.TotalMontantRealise);
            builder.Ignore(of => of.EcartRealiseObjectif);
            builder.Ignore(of => of.TotalMontantJournalise);
            builder.Ignore(of => of.IsDeleted);
            builder.Ignore(of => of.IsClosed);
            builder.Ignore(of => of.Journalisations);

            builder.HasOne(of => of.Ci)
                .WithMany()
                .HasForeignKey(of => of.CiId);
            builder.HasOne(of => of.AuteurCreation)
                .WithMany()
                .HasForeignKey(of => of.AuteurCreationId);
            builder.HasOne(of => of.AuteurModification)
                .WithMany()
                .HasForeignKey(of => of.AuteurModificationId);
            builder.HasOne(of => of.AuteurSuppression)
                .WithMany()
                .HasForeignKey(of => of.AuteurSuppressionId);
        }
    }
}
