using Fred.Entities.Delegation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class DelegationEntConfiguration : IEntityTypeConfiguration<DelegationEnt>
    {
        public void Configure(EntityTypeBuilder<DelegationEnt> builder)
        {
            builder.ToTable("FRED_DELEGATION");
            builder.HasKey(d => d.DelegationId);
            builder.Property(d => d.DelegationId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateDeDebut)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateDeFin)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateDesactivation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.Property(cod => cod.Activated)
                .HasDefaultValue(false);

            builder.HasOne(d => d.PersonnelAuteur)
                .WithMany()
                .HasForeignKey(d => d.PersonnelAuteurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.PersonnelDelegant)
                .WithMany()
                .HasForeignKey(d => d.PersonnelDelegantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.PersonnelDelegue)
                .WithMany()
                .HasForeignKey(d => d.PersonnelDelegueId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
