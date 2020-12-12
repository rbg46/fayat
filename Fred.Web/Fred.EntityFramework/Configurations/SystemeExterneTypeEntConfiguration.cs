using Fred.Entities.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SystemeExterneTypeEntConfiguration : IEntityTypeConfiguration<SystemeExterneTypeEnt>
    {
        public void Configure(EntityTypeBuilder<SystemeExterneTypeEnt> builder)
        {
            builder.ToTable("FRED_SYSTEME_EXTERNE_TYPE");
            builder.HasKey(set => set.SystemeExterneTypeId);
            builder.Property(set => set.SystemeExterneTypeId)
                .ValueGeneratedOnAdd();

            builder.Property(set => set.Libelle)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
