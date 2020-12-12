using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetBibliothequePrixItemEntConfiguration : IEntityTypeConfiguration<BudgetBibliothequePrixItemEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetBibliothequePrixItemEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_BIBLIOTHEQUE_PRIX_ITEM");
            builder.HasKey(bbpi => bbpi.BudgetBibliothequePrixItemId);
            builder.Property(bbpi => bbpi.BudgetBibliothequePrixItemId)
                .ValueGeneratedOnAdd();

            builder.Property(bbpi => bbpi.Prix)
                .HasColumnType("decimal(18, 3)");
            builder.Property(bbpi => bbpi.DateCreation)
                .HasColumnType("datetime");
            builder.Property(bbpi => bbpi.DateModification)
                .HasColumnType("datetime");
            builder.Property(bbpi => bbpi.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(bbpi => bbpi.BudgetBibliothequePrix)
                .WithMany(bbpi => bbpi.Items)
                .HasForeignKey(bbpi => bbpi.BudgetBibliothequePrixId);
            builder.HasOne(bbpi => bbpi.Unite)
                .WithMany()
                .HasForeignKey(bbpi => bbpi.UniteId);
            builder.HasOne(bbpi => bbpi.Ressource)
                .WithMany()
                .HasForeignKey(bbpi => bbpi.RessourceId);
            builder.HasOne(bbpi => bbpi.AuteurCreation)
                .WithMany()
                .HasForeignKey(bbpi => bbpi.AuteurCreationId);
            builder.HasOne(bbpi => bbpi.AuteurModification)
                .WithMany()
                .HasForeignKey(bbpi => bbpi.AuteurModificationId);
            builder.HasOne(bbpi => bbpi.AuteurSuppression)
                .WithMany()
                .HasForeignKey(bbpi => bbpi.AuteurSuppressionId);
        }
    }
}
