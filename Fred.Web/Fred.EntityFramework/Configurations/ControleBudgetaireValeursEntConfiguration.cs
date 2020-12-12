using Fred.Entities.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ControleBudgetaireValeursEntConfiguration : IEntityTypeConfiguration<ControleBudgetaireValeursEnt>
    {
        public void Configure(EntityTypeBuilder<ControleBudgetaireValeursEnt> builder)
        {
            builder.ToTable("FRED_CONTROLE_BUDGETAIRE_VALEURS");
            builder.HasKey(cbv => new { cbv.ControleBudgetaireId, cbv.Periode, cbv.TacheId, cbv.RessourceId });

            builder.Property(cbv => cbv.Ajustement)
                .HasColumnType("decimal(20, 3)");
            builder.Property(cbv => cbv.Periode)
                .HasDefaultValue(0);

            builder.HasOne(cbv => cbv.ControleBudgetaire)
                .WithMany(cb => cb.Valeurs)
                .HasForeignKey(cbv => new { cbv.ControleBudgetaireId, cbv.Periode });
            builder.HasOne(cbv => cbv.Tache)
                .WithMany()
                .HasForeignKey(cbv => cbv.TacheId);
            builder.HasOne(cbv => cbv.Ressource)
                .WithMany()
                .HasForeignKey(cbv => cbv.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
