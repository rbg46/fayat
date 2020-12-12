using Fred.Entities.Personnel.Interimaire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class MotifRemplacementEntConfiguration : IEntityTypeConfiguration<MotifRemplacementEnt>
    {
        public void Configure(EntityTypeBuilder<MotifRemplacementEnt> builder)
        {
            builder.ToTable("FRED_MOTIF_REMPLACEMENT");
            builder.HasKey(mr => mr.MotifRemplacementId);
            builder.Property(mr => mr.MotifRemplacementId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(mr => mr.Code)
                .HasName("IX_UniqueCode")
                .IsUnique();

            builder.Property(mr => mr.Code).IsRequired().HasMaxLength(5);
            builder.Property(mr => mr.Libelle).HasMaxLength(50);
        }
    }
}
