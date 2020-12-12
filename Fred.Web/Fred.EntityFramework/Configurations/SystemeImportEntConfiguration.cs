using Fred.Entities.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SystemeImportEntConfiguration : IEntityTypeConfiguration<SystemeImportEnt>
    {
        public void Configure(EntityTypeBuilder<SystemeImportEnt> builder)
        {
            builder.ToTable("FRED_SYSTEME_IMPORT");
            builder.HasKey(si => si.SystemeImportId);
            builder.Property(si => si.SystemeImportId)
                .ValueGeneratedOnAdd();

            builder.Property(si => si.Code)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
