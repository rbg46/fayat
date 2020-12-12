
using Fred.Entities.Carburant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CarburantOrganisationDeviseEntConfiguration : IEntityTypeConfiguration<CarburantOrganisationDeviseEnt>
    {
        public void Configure(EntityTypeBuilder<CarburantOrganisationDeviseEnt> builder)
        {
            builder.ToTable("FRED_CARBURANT_ORGANISATION_DEVISE");
            builder.HasKey(cod => cod.CarburantOrganisationDeviseId);

            builder.Property(cod => cod.CarburantOrganisationDeviseId)
                .ValueGeneratedOnAdd();
            builder.Property(cod => cod.Prix)
                .HasColumnType("decimal(10, 5)");
            builder.Ignore(cod => cod.Cloture);

            builder.Property(cod => cod.Periode)
                .HasColumnType("date");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(cod => cod.Carburant)
                .WithMany(c => c.ParametrageCarburants)
                .HasForeignKey(cod => cod.CarburantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cod => cod.Organisation)
                .WithMany(o => o.CarburantOrganisationDevises)
                .HasForeignKey(cod => cod.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cod => cod.Devise)
                .WithMany(d => d.CarburantOrganisationDevises)
                .HasForeignKey(cod => cod.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cod => cod.AuteurCreation)
                .WithMany()
                .HasForeignKey(cod => cod.AuteurCreationId);
            builder.HasOne(cod => cod.AuteurModification)
               .WithMany()
               .HasForeignKey(cod => cod.AuteurModificationId);
            builder.HasOne(cod => cod.AuteurSuppression)
               .WithMany()
               .HasForeignKey(cod => cod.AuteurSuppressionId);
        }
    }
}

