using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TypeSocieteEntConfiguration : IEntityTypeConfiguration<TypeSocieteEnt>
    {
        public void Configure(EntityTypeBuilder<TypeSocieteEnt> builder)
        {
            builder.ToTable("FRED_TYPE_SOCIETE");
            builder.HasKey(ts => ts.TypeSocieteId);
            builder.Property(ts => ts.TypeSocieteId)
                .ValueGeneratedOnAdd();

            builder.Property(ts => ts.Code)
                .IsRequired();
            builder.Property(ts => ts.Libelle)
                .IsRequired();
        }
    }
}
