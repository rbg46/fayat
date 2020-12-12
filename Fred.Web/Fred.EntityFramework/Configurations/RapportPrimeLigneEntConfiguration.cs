using Fred.Entities.RapportPrime;
using Fred.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportPrimeLigneEntConfiguration : DeletableConfiguration<RapportPrimeLigneEnt>, IEntityTypeConfiguration<RapportPrimeLigneEnt>
    {
        public new void Configure(EntityTypeBuilder<RapportPrimeLigneEnt> builder)
        {
            base.Configure(builder);

            builder.ToTable("FRED_RAPPORTPRIME_LIGNE");

            builder.HasKey(rpl => rpl.RapportPrimeLigneId);
            builder.Property(rpl => rpl.RapportPrimeLigneId)
                .ValueGeneratedOnAdd();

            builder.Ignore(rpl => rpl.IsCreated);
            builder.Ignore(rpl => rpl.IsDeleted);
            builder.Ignore(rpl => rpl.IsUpdated);

            builder.Property(cod => cod.DateVerrou)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateValidation)
                .HasColumnType("datetime");

            builder.Property(rpl => rpl.IsValidated)
                .HasDefaultValue(0);

            builder.HasOne(rpl => rpl.RapportPrime)
                .WithMany(rp => rp.ListLignes)
                .HasForeignKey(rpl => rpl.RapportPrimeId);
            builder.HasOne(rpl => rpl.Ci)
                .WithMany()
                .HasForeignKey(rpl => rpl.CiId);
            builder.HasOne(rpl => rpl.AuteurVerrou)
                .WithMany()
                .HasForeignKey(rpl => rpl.AuteurVerrouId);
            builder.HasOne(rpl => rpl.AuteurValidation)
                .WithMany()
                .HasForeignKey(rpl => rpl.AuteurValidationId);
            builder.HasOne(rpl => rpl.Personnel)
                .WithMany()
                .HasForeignKey(rpl => rpl.PersonnelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
