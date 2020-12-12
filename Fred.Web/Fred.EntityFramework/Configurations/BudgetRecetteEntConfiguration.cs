using Fred.Entities.Budget.Recette;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetRecetteEntConfiguration : IEntityTypeConfiguration<BudgetRecetteEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetRecetteEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_RECETTE");
            builder.HasKey(br => br.BudgetRecetteId);
            builder.Property(br => br.BudgetRecetteId)
                .ValueGeneratedNever()
                .HasDefaultValue(0);

            builder.HasOne(br => br.Budget)
                .WithOne(b => b.Recette)
                .HasForeignKey<BudgetRecetteEnt>(br => br.BudgetRecetteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
