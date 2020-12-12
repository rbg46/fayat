using Fred.Entities.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TranscoImportEntConfiguration : IEntityTypeConfiguration<TranscoImportEnt>
    {
        public void Configure(EntityTypeBuilder<TranscoImportEnt> builder)
        {
            builder.ToTable("FRED_TRANSCO_IMPORT");
            builder.HasKey(ti => ti.TranscoImportId);
            builder.Property(ti => ti.TranscoImportId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(ti => new
                {
                    ti.CodeInterne,
                    ti.SocieteId,
                    ti.SystemeImportId
                })
                .HasName("IX_UniqueCodeInterneAndSocieteAndSystemImport")
                .IsUnique();
            builder.HasIndex(ti => new
                {
                    ti.CodeExterne,
                    ti.SocieteId,
                    ti.SystemeImportId
                })
                .HasName("IX_UniqueCodeExterneAndSocieteAndSystemImport")
                .IsUnique();

            builder.Property(ti => ti.CodeInterne)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(ti => ti.CodeExterne)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(ti => ti.Societe)
                .WithMany()
                .HasForeignKey(ti => ti.SocieteId);
            builder.HasOne(ti => ti.SystemeImport)
                .WithMany()
                .HasForeignKey(ti => ti.SystemeImportId);
        }
    }
}
