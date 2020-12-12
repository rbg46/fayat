using Fred.Entities.Directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ExternalDirectoryEntConfiguration : IEntityTypeConfiguration<ExternalDirectoryEnt>
    {
        public void Configure(EntityTypeBuilder<ExternalDirectoryEnt> builder)
        {
            builder.ToTable("FRED_EXTERNALDIRECTORY");
            builder.HasKey(x => x.FayatAccessDirectoryId);

            builder.Property(x => x.FayatAccessDirectoryId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.MotDePasse)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(250);
            builder.Property(cod => cod.DateExpiration)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateExpirationGuid)
                .HasColumnType("datetime");
        }
    }
}

