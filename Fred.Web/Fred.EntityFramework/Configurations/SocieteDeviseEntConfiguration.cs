using Fred.Entities.Societe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SocieteDeviseEntConfiguration : IEntityTypeConfiguration<SocieteDeviseEnt>
    {
        public void Configure(EntityTypeBuilder<SocieteDeviseEnt> builder)
        {
            builder.ToTable("FRED_SOCIETE_DEVISE");
            builder.HasKey(sd => sd.SocieteDeviseId);
            builder.Property(sd => sd.SocieteDeviseId)
                .ValueGeneratedOnAdd();

            builder.Property(sd => sd.DeviseDeReference)
                .HasDefaultValue(0);

            builder.HasOne(sd => sd.Societe)
                .WithMany(s => s.SocieteDevises)
                .HasForeignKey(sd => sd.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sd => sd.Devise)
                .WithMany(d => d.SocieteDevises)
                .HasForeignKey(sd => sd.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}