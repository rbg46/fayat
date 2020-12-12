using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetBibliothequePrixItemValuesHistoEntConfiguration : IEntityTypeConfiguration<BudgetBibliothequePrixItemValuesHistoEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetBibliothequePrixItemValuesHistoEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM_VALUES_HISTO");
            builder.HasKey(bbpivh => bbpivh.BudgetBibliothequePrixItemValuesHistoId);
            builder.Property(bbpivh => bbpivh.BudgetBibliothequePrixItemValuesHistoId)
                .ValueGeneratedOnAdd();

            builder.Property(bbpivh => bbpivh.DateInsertion)
                .HasColumnType("datetime");
            builder.Property(bbpivh => bbpivh.Prix)
                .HasColumnType("decimal(18, 3)");

            builder.HasOne(bbpivh => bbpivh.Item)
                .WithMany(bbpivh => bbpivh.ItemValuesHisto)
                .HasForeignKey(bbpivh => bbpivh.ItemId);
            builder.HasOne(bbpivh => bbpivh.Unite)
                .WithMany()
                .HasForeignKey(bbpivh => bbpivh.UniteId);
        }
    }
}
