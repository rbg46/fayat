
using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class BudgetRevisionEntConfiguration : IEntityTypeConfiguration<BudgetRevisionEnt>
    {
        public void Configure(EntityTypeBuilder<BudgetRevisionEnt> builder)
        {
            builder.ToTable("FRED_BUDGET_REVISION");
            builder.HasKey(x => x.BudgetRevisionId);

            builder.Property(x => x.BudgetRevisionId)
                .ValueGeneratedOnAdd();
            builder.Property(beo => beo.DateValidation)
                .HasColumnType("datetime");
            builder.Property(beo => beo.DateaValider)
                .HasColumnType("datetime");
            builder.Property(beo => beo.DateCreation)
                .HasColumnType("datetime");
            builder.Property(beo => beo.DateModification)
                .HasColumnType("datetime");

            builder.Ignore(br => br.Recettes);
            builder.Ignore(br => br.Depenses);
            builder.Ignore(br => br.MargeBrute);
            builder.Ignore(br => br.MargeBrutePercent);
            builder.Ignore(br => br.MargeNette);
            builder.Ignore(br => br.MargeNettePercent);

            builder.HasOne(br => br.Budget)
                .WithMany()
                .HasForeignKey(br => br.BudgetId);
            builder.HasOne(br => br.AuteurCreation)
                .WithMany()
                .HasForeignKey(br => br.AuteurCreationId);
            builder.HasOne(br => br.AuteurModification)
               .WithMany()
               .HasForeignKey(br => br.AuteurModificationId);
            builder.HasOne(br => br.AuteurValidation)
               .WithMany()
               .HasForeignKey(br => br.AuteurValidationId);
        }
    }
}

