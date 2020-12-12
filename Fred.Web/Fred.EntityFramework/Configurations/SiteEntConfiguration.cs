using Fred.Entities.Moyen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SiteEntConfiguration : IEntityTypeConfiguration<SiteEnt>
    {
        public void Configure(EntityTypeBuilder<SiteEnt> builder)
        {
            builder.ToTable("FRED_SITE");
            builder.HasKey(s => s.SiteId);
            builder.Property(s => s.SiteId)
                .ValueGeneratedOnAdd();

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
        }
    }
}
