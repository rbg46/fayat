using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetBibliothequePrixEntConfiguration : IEntityTypeConfiguration<BudgetBibliothequePrixEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetBibliothequePrixEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_BIBLIOTHEQUE_PRIX");
            builder.HasKey(bbp => bbp.BudgetBibliothequePrixId);
            builder.Property(bbp => bbp.BudgetBibliothequePrixId)
                .ValueGeneratedOnAdd();

            builder.Property(bbp => bbp.DateCreation)
                .HasColumnType("datetime");

            builder.HasOne(bbp => bbp.Organisation)
                .WithMany()
                .HasForeignKey(bbp => bbp.OrganisationId);
            builder.HasOne(bbp => bbp.Devise)
                .WithMany()
                .HasForeignKey(bbp => bbp.DeviseId);
            builder.HasOne(bbp => bbp.AuteurCreation)
                .WithMany()
                .HasForeignKey(bbp => bbp.AuteurCreationId);
        }
    }
}
