using Fred.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class NotificationEntConfiguration : IEntityTypeConfiguration<NotificationEnt>
    {
        public void Configure(EntityTypeBuilder<NotificationEnt> builder)
        {
            builder.ToTable("FRED_NOTIFICATION_UTILISATEUR");
            builder.HasKey(n => n.NotificationId);
            builder.Property(n => n.NotificationId)
                .ValueGeneratedOnAdd();

            builder.Property(n => n.Message)
                .IsRequired();
            builder.Property(n => n.TypeNotification)
                .HasDefaultValue((TypeNotification)0);
            builder.Property(n => n.Message)
                .HasDefaultValue("");
            builder.Property(n => n.DateCreation)
                .HasColumnType("datetime");
        }
    }
}
