using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class BudgetT4EntConfiguration : IEntityTypeConfiguration<BudgetT4Ent>
    {
        public void Configure(EntityTypeBuilder<BudgetT4Ent> builder)
        {
            builder.ToTable("FRED_BUDGET_T4");
            builder.HasKey(bt4 => bt4.BudgetT4Id);
            builder.Property(bt4 => bt4.BudgetT4Id)
                .ValueGeneratedOnAdd();

            builder.Property(bt4 => bt4.MontantT4)
                .HasColumnType("decimal(20, 8)");
            builder.Property(bt4 => bt4.QuantiteARealiser)
                .HasColumnType("decimal(20, 3)");
            builder.Property(bt4 => bt4.QuantiteDeBase)
                .HasColumnType("decimal(20, 3)");
            builder.Property(bt4 => bt4.PU)
                .HasColumnType("decimal(20, 8)");
            builder.Property(bt4 => bt4.Commentaire)
                .HasMaxLength(500);

            builder.Property(b => b.TypeAvancement)
                .HasDefaultValue(0);
            builder.Property(b => b.QuantiteDeBase)
                .HasDefaultValue((decimal)0);
            builder.Property(b => b.PU)
                .HasDefaultValue((decimal)0);
            builder.Property(b => b.VueSD)
                .HasDefaultValue(1).ValueGeneratedNever();
            builder.Property(b => b.IsReadOnly)
                .HasDefaultValue(0);

            builder.HasOne(bt4 => bt4.Budget)
                .WithMany(b => b.BudgetT4s)
                .HasForeignKey(bt4 => bt4.BudgetId);
            builder.HasOne(bt4 => bt4.T4)
                .WithMany()
                .HasForeignKey(bt4 => bt4.T4Id);
            builder.HasOne(bt4 => bt4.Unite)
                .WithMany()
                .HasForeignKey(bt4 => bt4.UniteId);
            builder.HasOne(bt4 => bt4.T3)
                .WithMany()
                .HasForeignKey(bt4 => bt4.T3Id);
        }
    }
}
