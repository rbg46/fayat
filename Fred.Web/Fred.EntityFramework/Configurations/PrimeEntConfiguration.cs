using Fred.Entities;
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PrimeEntConfiguration : IEntityTypeConfiguration<PrimeEnt>
    {
        public void Configure(EntityTypeBuilder<PrimeEnt> builder)
        {
            builder.ToTable("FRED_PRIME");
            builder.HasKey(sbec => sbec.PrimeId);
            builder.Property(sbec => sbec.PrimeId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(s => new
                {
                    s.Code,
                    s.SocieteId
                })
            .HasName("IX_UniqueCodeAndSociete")
            .IsUnique();

            builder.Property(sbec => sbec.Libelle)
                .IsRequired();
            builder.Property(sbec => sbec.Code)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(sbec => sbec.Libelle)
                .HasMaxLength(500);
            builder.Property(sbec => sbec.NombreHeuresMax)
                .HasColumnType("float");

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.Property(cod => cod.PrimeType)
                .HasDefaultValue((ListePrimeType)0);
            builder.Property(cod => cod.TargetPersonnel)
                .HasDefaultValue((TargetPersonnel)0);

            builder.Ignore(sbec => sbec.CodeLibelle);
            builder.Ignore(sbec => sbec.IsLinkedToCI);

            builder.HasOne(sbec => sbec.Societe)
                .WithMany(c => c.Primes)
                .HasForeignKey(sbec => sbec.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Groupe)
                .WithMany()
                .HasForeignKey(sbec => sbec.GroupeId);
            builder.HasOne(sbec => sbec.AuteurCreation)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurCreationId);
            builder.HasOne(sbec => sbec.AuteurModification)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurModificationId);
            builder.HasOne(sbec => sbec.AuteurSuppression)
                .WithMany()
                .HasForeignKey(sbec => sbec.AuteurSuppressionId);
        }
    }
}