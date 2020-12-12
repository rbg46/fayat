using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TypeDepenseEntConfiguration : IEntityTypeConfiguration<TypeDepenseEnt>
    {
        public void Configure(EntityTypeBuilder<TypeDepenseEnt> builder)
        {
            builder.ToTable("FRED_TYPE_DEPENSE");
            builder.HasKey(td => td.TypeDepenseId);
            builder.Property(td => td.TypeDepenseId)
                .ValueGeneratedOnAdd();

            builder.Property(td => td.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(td => td.Libelle)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
