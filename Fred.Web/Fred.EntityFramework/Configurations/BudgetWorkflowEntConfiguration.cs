using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetWorkflowEntConfiguration : IEntityTypeConfiguration<BudgetWorkflowEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetWorkflowEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_WORKFLOW");
            builder.HasKey(bw => bw.BudgetWorkflowId);
            builder.Property(bw => bw.BudgetWorkflowId)
                .ValueGeneratedOnAdd();

            builder.Property(bw => bw.Date)
                .HasColumnType("datetime");

            builder.HasOne(bw => bw.Budget)
                .WithMany(b => b.Workflows)
                .HasForeignKey(bw => bw.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bw => bw.EtatInitial)
                .WithMany()
                .HasForeignKey(bw => bw.EtatInitialId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bw => bw.EtatCible)
                .WithMany()
                .HasForeignKey(bw => bw.EtatCibleId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bw => bw.Auteur)
                .WithMany()
                .HasForeignKey(bw => bw.AuteurId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
