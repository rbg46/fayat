using Fred.Entities;
using Fred.Entities.RoleFonctionnalite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RoleFonctionnaliteEntConfiguration : IEntityTypeConfiguration<RoleFonctionnaliteEnt>
    {
        public void Configure(EntityTypeBuilder<RoleFonctionnaliteEnt> builder)
        {
            builder.ToTable("FRED_ROLE_FONCTIONNALITE");
            builder.HasKey(rf => rf.RoleFonctionnaliteId);

            builder.Property(rf => rf.Mode)
                .HasDefaultValue((FonctionnaliteTypeMode)0);

            builder.HasOne(rf => rf.Role)
                .WithMany()
                .HasForeignKey(rf => rf.RoleId);
            builder.HasOne(rf => rf.Fonctionnalite)
                .WithMany()
                .HasForeignKey(rf => rf.FonctionnaliteId);
        }
    }
}
