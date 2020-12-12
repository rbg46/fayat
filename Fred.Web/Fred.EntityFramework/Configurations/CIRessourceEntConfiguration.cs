
using Fred.Entities.CI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CIRessourceEntConfiguration : IEntityTypeConfiguration<CIRessourceEnt>
    {
        public void Configure(EntityTypeBuilder<CIRessourceEnt> builder)
        {
            builder.ToTable("FRED_CI_RESSOURCE");
            builder.HasKey(x => x.CiRessourceId);

            builder.Property(x => x.CiRessourceId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Consommation)
                .HasColumnType("decimal(7, 2)")
                .IsRequired();

            builder.HasOne(a => a.CI)
                .WithMany(b => b.CIRessources)
                .HasForeignKey(c => c.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Ressource)
                .WithMany(b => b.CIRessources)
                .HasForeignKey(c => c.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

