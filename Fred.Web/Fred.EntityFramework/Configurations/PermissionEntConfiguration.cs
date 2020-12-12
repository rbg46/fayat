using Fred.Entities.Permission;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PermissionEntConfiguration : IEntityTypeConfiguration<PermissionEnt>
    {
        public void Configure(EntityTypeBuilder<PermissionEnt> builder)
        {
            builder.ToTable("FRED_PERMISSION");
            builder.HasKey(x => x.PermissionId);
            builder.HasIndex(x => x.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(x => x.PermissionId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Code)
                .HasMaxLength(20);
            builder.Ignore(x => x.Mode);
        }
    }
}
