using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetTacheEntConfiguration : IEntityTypeConfiguration<BudgetTacheEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetTacheEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_TACHE");
            builder.HasKey(bt => bt.BudgetTacheId);
            builder.Property(bt => bt.BudgetTacheId)
                .ValueGeneratedOnAdd();

            builder.Property(bt => bt.Commentaire)
                .HasMaxLength(150);

            builder.HasOne(bt => bt.Budget)
                .WithMany()
                .HasForeignKey(bt => bt.BudgetId);
            builder.HasOne(bt => bt.Tache)
                .WithMany()
                .HasForeignKey(bt => bt.TacheId);
        }
    }
}
