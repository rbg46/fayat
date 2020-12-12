using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetEtatEntConfiguration : IEntityTypeConfiguration<BudgetEtatEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetEtatEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_ETAT");
            builder.HasKey(be => be.BudgetEtatId);
            builder.Property(be => be.BudgetEtatId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(be => be.Code)
                .HasName("IX_UniqueCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(be => be.Code).HasMaxLength(20);
        }
    }
}
