using Fred.Entities.Action;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ActionJobEntConfiguration : IEntityTypeConfiguration<ActionJobEnt>
    {
        public void Configure(EntityTypeBuilder<ActionJobEnt> builder)
        {
            builder.ToTable("FRED_ACTION_JOB");
            builder.HasKey(ct => ct.ActionJobId);

            builder.Property(ct => ct.ActionJobId)
                .ValueGeneratedOnAdd();
            builder.Property(ct => ct.ExternalJobId)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(ct => ct.ExternalJobName)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
