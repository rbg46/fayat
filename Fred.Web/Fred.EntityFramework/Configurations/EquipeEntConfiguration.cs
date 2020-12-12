using Fred.Entities.Affectation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    /// <summary>
    /// Equipe entity configuration
    /// </summary>
    public class EquipeEntConfiguration : IEntityTypeConfiguration<EquipeEnt>
    {
        public void Configure(EntityTypeBuilder<EquipeEnt> builder)
        {
            builder.ToTable("FRED_EQUIPE");
            builder.HasKey(v => v.EquipeId);
            builder.Property(v => v.EquipeId)
                .ValueGeneratedOnAdd();

            builder.HasOne(e => e.Proprietaire)
                .WithMany()
                .HasForeignKey(e => e.ProprietaireId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
