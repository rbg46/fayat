using Fred.Entities.LogImport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class LogImportEntConfiguration : IEntityTypeConfiguration<LogImportEnt>
    {
        public void Configure(EntityTypeBuilder<LogImportEnt> builder)
        {
            builder.ToTable("FRED_LOG_IMPORT");
            builder.HasKey(l => l.LogImportId);

            builder.Property(l => l.LogImportId)
                .ValueGeneratedOnAdd();
            builder.Property(l => l.TypeImport)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(l => l.MessageErreur)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(l => l.Data)
                .IsRequired();

            builder.Property(l => l.DateImport)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
        }
    }
}

