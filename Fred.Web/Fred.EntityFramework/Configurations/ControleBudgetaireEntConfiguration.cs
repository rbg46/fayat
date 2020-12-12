using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ControleBudgetaireEntConfiguration : IEntityTypeConfiguration<ControleBudgetaireEnt>
    {
        public void Configure(EntityTypeBuilder<ControleBudgetaireEnt> builder)
        {
            builder.ToTable("FRED_CONTROLE_BUDGETAIRE");
            builder.HasKey(cb => new { cb.ControleBudgetaireId, cb.Periode });

            builder.Property(cb => cb.Periode)
                .HasDefaultValue(0);

            builder.HasOne(cb => cb.Budget)
                .WithMany()
                .HasForeignKey(cb => cb.ControleBudgetaireId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cb => cb.ControleBudgetaireEtat)
                .WithMany()
                .HasForeignKey(cb => cb.ControleBudgetaireEtatId);
        }
    }
}
