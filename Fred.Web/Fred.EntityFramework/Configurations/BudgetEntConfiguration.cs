
using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetEntConfiguration : IEntityTypeConfiguration<BudgetEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetEnt> builder)
        {
            builder.ToTable("FRED_BUDGET");
            builder.HasKey(b => b.BudgetId);

            builder.Property(b => b.BudgetId)
                .ValueGeneratedOnAdd();
            builder.Property(b => b.Version)
                .IsRequired();
            builder.Property(b => b.Libelle)
                .HasMaxLength(50);
            builder.Property(b => b.Version)
                .HasDefaultValue("");
            builder.Property(b => b.Partage)
                .HasDefaultValue(0);
            builder.Property(beo => beo.DateSuppressionBudget)
                .HasColumnType("datetime");
            builder.Property(beo => beo.DateDeleteNotificationNewTask)
                .HasColumnType("datetime");

            builder.HasOne(b => b.Ci)
                .WithMany(c => c.Budgets)
                .HasForeignKey(c => c.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.Devise)
                .WithMany()
                .HasForeignKey(b => b.DeviseId);
            builder.HasOne(b => b.BudgetEtat)
                .WithMany()
                .HasForeignKey(b => b.BudgetEtatId);
        }
    }
}

