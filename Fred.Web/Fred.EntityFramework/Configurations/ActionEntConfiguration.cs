using Fred.Entities.Action;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ActionEntConfiguration : IEntityTypeConfiguration<ActionEnt>
    {
        public void Configure(EntityTypeBuilder<ActionEnt> builder)
        {
            builder.ToTable("FRED_ACTION");
            builder.HasKey(acl => acl.ActionId);
            builder.Property(acl => acl.ActionId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Message)
                .HasMaxLength(2000);
            builder.Property(a => a.DateAction)
                .HasColumnType("datetime");

            builder.HasOne(c => c.ActionType)
                .WithMany()
                .HasForeignKey(c => c.ActionTypeId);
            builder.HasOne(c => c.ActionJob)
                .WithMany()
                .HasForeignKey(c => c.ActionJobId);
            builder.HasOne(c => c.ActionStatus)
                .WithMany()
                .HasForeignKey(c => c.ActionStatusId);
        }
    }
}
