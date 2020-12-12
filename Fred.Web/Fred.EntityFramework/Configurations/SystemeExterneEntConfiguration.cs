using Fred.Entities.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SystemeExterneEntConfiguration : IEntityTypeConfiguration<SystemeExterneEnt>
    {
        public void Configure(EntityTypeBuilder<SystemeExterneEnt> builder)
        {
            builder.ToTable("FRED_SYSTEME_EXTERNE");
            builder.HasKey(se => se.SystemeExterneId);
            builder.Property(se => se.SystemeExterneId)
                .ValueGeneratedOnAdd();

            builder.Property(se => se.Code)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(se => se.LibelleAffiche)
                .HasColumnName("Libelle")
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(se => se.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(se => se.Societe)
                .WithMany()
                .HasForeignKey(se => se.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(se => se.SystemeExterneType)
                .WithMany()
                .HasForeignKey(se => se.SystemeExterneTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(se => se.SystemeImport)
                .WithMany()
                .HasForeignKey(se => se.SystemeImportId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
