using Fred.Entities.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class NLogEntConfiguration : IEntityTypeConfiguration<NLogEnt>
    {
        public void Configure(EntityTypeBuilder<NLogEnt> builder) 
        {
            builder.ToTable("NLogs", "nlog");
            builder.HasKey(nl => nl.Id);
            builder.Property(nl => nl.Id)
                .ValueGeneratedOnAdd();

            builder.Property(nl => nl.Application)
                .HasMaxLength(50);
            builder.Property(nl => nl.Level)
                .HasMaxLength(50);
            builder.Property(nl => nl.UserName)
                .HasMaxLength(250);
            builder.Property(nl => nl.ServerAddress)
                .HasMaxLength(100);
            builder.Property(nl => nl.RemoteAddress)
                .HasMaxLength(100);
            builder.Property(nl => nl.Logger)
                .HasMaxLength(250);

            builder.Property(cod => cod.Logged)
                .HasColumnType("datetime");
        }
    }
}
