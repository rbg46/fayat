
using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetCopyHistoEntConfiguration : IEntityTypeConfiguration<BudgetCopyHistoEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetCopyHistoEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_COPY_HISTO");
            builder.HasKey(bch => bch.BudgetCopyHistoId);
            builder.Property(bch => bch.BudgetCopyHistoId)
                .ValueGeneratedOnAdd();

            builder.Property(bch => bch.BudgetSourceVersion)
                .IsRequired();
            builder.Property(bch => bch.DateCopy)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasOne(bch => bch.Budget)
                .WithMany(b => b.CopyHistos)
                .HasForeignKey(bch => bch.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bch => bch.BudgetSourceCI)
                .WithMany()
                .HasForeignKey(bch => bch.BudgetSourceCIId);
            builder.HasOne(bch => bch.BibliothequePrixSourceCI)
                .WithMany()
                .HasForeignKey(bch => bch.BibliothequePrixSourceCIId);
        }
    }
}

