using Fred.Entities.Budget.Recette;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TacheRecetteEntConfiguration : IEntityTypeConfiguration<TacheRecetteEnt>
    {
        public void Configure(EntityTypeBuilder<TacheRecetteEnt> builder)
        {
            builder.ToTable("FRED_TACHE_RECETTE");
            builder.HasKey(tr => tr.TacheRecetteId);
            builder.Property(tr => tr.TacheRecetteId)
                .ValueGeneratedOnAdd();

            builder.Property(tr => tr.Recette)
                .HasColumnType("float");

            builder.HasOne(tr => tr.Tache)
                .WithMany(t => t.TacheRecettes)
                .HasForeignKey(tr => tr.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(tr => tr.Devise)
                .WithMany(d => d.TacheRecettes)
                .HasForeignKey(tr => tr.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}