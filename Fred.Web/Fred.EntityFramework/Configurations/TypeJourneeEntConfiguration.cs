using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class TypeJourneeEntConfiguration : IEntityTypeConfiguration<TypeJourneeEnt>
    {
        public void Configure(EntityTypeBuilder<TypeJourneeEnt> builder)
        {
            builder.ToTable("FRED_TYPE_JOURNEE");
            builder.HasKey(to => to.TypeJourneeId);
            builder.Property(to => to.TypeJourneeId)
                .ValueGeneratedOnAdd();

            builder.Property(to => to.Code)
                .IsRequired();
        }
    }
}
