using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetSousDetailEntConfiguration : IEntityTypeConfiguration<BudgetSousDetailEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetSousDetailEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_T4_RESSOURCE");
            builder.HasKey(bsd => bsd.BudgetSousDetailId);
            builder.Property(bsd => bsd.BudgetSousDetailId)
                .ValueGeneratedOnAdd();

            builder.Property(bsd => bsd.Montant)
                .HasColumnType("decimal(20, 8)");
            builder.Property(bsd => bsd.Quantite)
                .HasColumnType("decimal(20, 3)");
            builder.Property(bsd => bsd.QuantiteSD)
                .HasColumnType("decimal(20, 3)");
            builder.Property(bsd => bsd.PU)
                .HasColumnType("decimal(20, 8)");

            builder.Property(bsd => bsd.Commentaire)
                .HasMaxLength(200);
            builder.Property(bsd => bsd.QuantiteFormule)
                .HasMaxLength(200);
            builder.Property(bsd => bsd.QuantiteSDFormule)
                .HasMaxLength(200);
            builder.Property(bsd => bsd.BudgetSousDetailId)
                .HasColumnName("BudgetT4SousDetailId");

            builder.Property(bsd => bsd.Quantite)
                .HasDefaultValue((decimal)0);

            builder.HasOne(bsd => bsd.BudgetT4)
                .WithMany(bt4 => bt4.BudgetSousDetails)
                .HasForeignKey(bsd => bsd.BudgetT4Id);
            builder.HasOne(bsd => bsd.Ressource)
                .WithMany()
                .HasForeignKey(bsd => bsd.RessourceId);
            builder.HasOne(bsd => bsd.Ressource)
                .WithMany()
                .HasForeignKey(bsd => bsd.RessourceId);
        }
    }
}
