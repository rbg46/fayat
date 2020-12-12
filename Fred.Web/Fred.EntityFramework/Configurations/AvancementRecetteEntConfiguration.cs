using Fred.Entities.Budget.Recette;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AvancementRecetteEntConfiguration : IEntityTypeConfiguration<AvancementRecetteEnt>
    {
        public void Configure(EntityTypeBuilder<AvancementRecetteEnt> builder)
        {
            builder.ToTable("FRED_AVANCEMENT_RECETTE");
            builder.HasKey(ar => ar.AvancementRecetteId);
            builder.Property(ar => ar.AvancementRecetteId)
                .ValueGeneratedOnAdd();

            builder.Property(ar => ar.Correctif)
                .HasDefaultValue(0);
            builder.Property(ar => ar.TauxFraisGeneraux)
                .HasDefaultValue(0);
            builder.Property(ar => ar.AjustementFraisGeneraux)
                .HasDefaultValue(0);
            builder.Property(ar => ar.TauxFraisGenerauxPFA)
                .HasDefaultValue(0);
            builder.Property(ar => ar.AjustementFraisGenerauxPFA)
                .HasDefaultValue(0);
            builder.Property(ar => ar.AvancementTauxFraisGeneraux)
                .HasDefaultValue(0);
            builder.Property(ar => ar.AvancementAjustementFraisGeneraux)
                .HasDefaultValue(0);

            builder.HasOne(ar => ar.BudgetRecette)
                .WithMany()
                .HasForeignKey(ar => ar.BudgetRecetteId);
        }
    }
}
