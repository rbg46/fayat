using Fred.Entities.Action;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ActionTypeEntConfiguration : IEntityTypeConfiguration<ActionTypeEnt>
    {
        public void Configure(EntityTypeBuilder<ActionTypeEnt> builder)
        {
            builder.ToTable("FRED_ACTION_TYPE");
            builder.HasKey(ct => ct.ActionTypeId);

            builder.Property(ct => ct.ActionTypeId)
                .ValueGeneratedOnAdd();
            builder.Property(ct => ct.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(ct => ct.Libelle)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
