using Fred.Entities.Societe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class UniteSocieteEntConfiguration : IEntityTypeConfiguration<UniteSocieteEnt>
    {
        public void Configure(EntityTypeBuilder<UniteSocieteEnt> builder)
        {
            builder.ToTable("FRED_UNITE_SOCIETE");
            builder.HasKey(us => us.UniteSocieteId);
            builder.Property(us => us.UniteSocieteId)
                .ValueGeneratedOnAdd();

            builder.HasOne(us => us.Unite)
                .WithMany()
                .HasForeignKey(us => us.UniteId);
            builder.HasOne(us => us.Societe)
                .WithMany()
                .HasForeignKey(us => us.SocieteId);
        }
    }
}
