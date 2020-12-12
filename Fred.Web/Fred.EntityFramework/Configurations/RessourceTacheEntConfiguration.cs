using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RessourceTacheEntConfiguration : IEntityTypeConfiguration<RessourceTacheEnt>
    {
        public void Configure(EntityTypeBuilder<RessourceTacheEnt> builder)
        {
            builder.ToTable("FRED_RESSOURCE_TACHE");
            builder.HasKey(sbec => sbec.RessourceTacheId);
            builder.Property(sbec => sbec.RessourceTacheId)
                .ValueGeneratedOnAdd();

            builder.Property(sbec => sbec.Quantite)
                .HasColumnType("float");
            builder.Property(sbec => sbec.QuantiteBase)
                .HasColumnType("float");
            builder.Property(sbec => sbec.PrixUnitaire)
                .HasColumnType("float");
            builder.Property(sbec => sbec.Formule)
                .HasColumnType("nvarchar(50)");

            builder.HasOne(sbec => sbec.Tache)
                .WithMany(c => c.RessourceTaches)
                .HasForeignKey(sbec => sbec.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Ressource)
                .WithMany(c => c.RessourceTaches)
                .HasForeignKey(sbec => sbec.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}