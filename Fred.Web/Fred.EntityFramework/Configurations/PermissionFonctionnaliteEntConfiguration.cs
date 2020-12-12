using Fred.Entities.PermissionFonctionnalite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PermissionFonctionnaliteEntConfiguration : IEntityTypeConfiguration<PermissionFonctionnaliteEnt>
    {
        public void Configure(EntityTypeBuilder<PermissionFonctionnaliteEnt> builder)
        {
            builder.ToTable("FRED_PERMISSION_FONCTIONNALITE");
            builder.HasKey(x => x.PermissionFonctionnaliteId);

            builder.Property(x => x.PermissionFonctionnaliteId)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId);
            builder.HasOne(x => x.Fonctionnalite)
                .WithMany()
                .HasForeignKey(x => x.FonctionnaliteId);
        }
    }
}
