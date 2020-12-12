using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RessourceTacheDeviseEntConfiguration : IEntityTypeConfiguration<RessourceTacheDeviseEnt>
    {
        public void Configure(EntityTypeBuilder<RessourceTacheDeviseEnt> builder)
        {
            builder.ToTable("FRED_RESSOURCE_TACHE_DEVISE");
            builder.HasKey(rtd => rtd.RessourceTacheDeviseId);
            builder.Property(rtd => rtd.RessourceTacheDeviseId)
                .ValueGeneratedOnAdd();

            builder.Property(rtd => rtd.PrixUnitaire)
                .HasColumnType("float");

            builder.HasOne(rtd => rtd.RessourceTache)
                .WithMany(c => c.RessourceTacheDevises)
                .HasForeignKey(rtd => rtd.RessourceTacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(rtd => rtd.Devise)
                .WithMany(c => c.RessourceTacheDevises)
                .HasForeignKey(rtd => rtd.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}