using Fred.Entities.Action;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ActionStatusEntConfiguration : IEntityTypeConfiguration<ActionStatusEnt>
    {
        public void Configure(EntityTypeBuilder<ActionStatusEnt> builder)
        {
            builder.ToTable("FRED_ACTION_STATUS");
            builder.HasKey(ct => ct.ActionStatusId);

            builder.Property(ct => ct.ActionStatusId)
                .ValueGeneratedOnAdd();
            builder.Property(ct => ct.Name)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(ct => ct.Description)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
