using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class BudgetWorkflowConfiguration : IEntityTypeConfiguration<BudgetWorkflowEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetWorkflowEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_WORKFLOW");
            builder.HasKey(x => x.BudgetWorkflowId);

            builder.Property(x => x.BudgetWorkflowId)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Budget)
              .WithMany(b => b.Workflows)
              .HasForeignKey(x => x.BudgetId);
            builder.HasOne(x => x.EtatInitial)
             .WithMany()
             .HasForeignKey(x => x.EtatInitialId);
            builder.HasOne(x => x.EtatCible)
             .WithMany()
             .HasForeignKey(x => x.EtatCibleId);
            builder.HasOne(x => x.Auteur)
             .WithMany()
             .HasForeignKey(x => x.AuteurId);
        }
    }
}
