using Fred.Entities.Budget.Avancement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AvancementTacheEntConfiguration : IEntityTypeConfiguration<AvancementTacheEnt>
    {
        public void Configure(EntityTypeBuilder<AvancementTacheEnt> builder)
        {
            builder.ToTable("FRED_AVANCEMENT_TACHE");
            builder.HasKey(at => at.AvancementTacheId);
            builder.Property(at => at.AvancementTacheId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(at => new { at.BudgetId, at.Periode, at.TacheId })
                .HasName("IX_UniqueBudgetPeriodeTache")
                .IsUnique();

            builder.HasOne(at => at.Budget)
                .WithMany()
                .HasForeignKey(at => at.BudgetId);
            builder.HasOne(at => at.Tache)
                .WithMany()
                .HasForeignKey(at => at.TacheId);
            builder.Property(at => at.Commentaire)
                .HasMaxLength(150);
        }
    }
}
