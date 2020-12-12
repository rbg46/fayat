using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CodeAstreinteEntConfiguration : IEntityTypeConfiguration<CodeAstreinteEnt>
    {
        public void Configure(EntityTypeBuilder<CodeAstreinteEnt> builder)
        {
            builder.ToTable("FRED_CODE_ASTREINTE");
            builder.HasKey(ca => ca.CodeAstreinteId);
            builder.Property(ca => ca.CodeAstreinteId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(ca => new { ca.Code, ca.GroupeId })
                .HasName("IX_UniqueCodeAndGroupe")
                .IsUnique();

            builder.Property(ca => ca.Code).IsRequired().HasMaxLength(20);
        }
    }
}
