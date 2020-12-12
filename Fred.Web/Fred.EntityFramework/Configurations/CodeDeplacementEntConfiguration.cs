
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CodeDeplacementEntConfiguration : IEntityTypeConfiguration<CodeDeplacementEnt>
    {
        public void Configure(EntityTypeBuilder<CodeDeplacementEnt> builder)
        {
            builder.ToTable("FRED_CODE_DEPLACEMENT");
            builder.HasKey(cd => cd.CodeDeplacementId);
            builder.HasIndex(cd => new { cd.Code, cd.SocieteId })
                .HasName("IX_UniqueCodeAndSociete")
                .IsUnique();

            builder.Property(cd => cd.CodeDeplacementId)
                .ValueGeneratedOnAdd();
            builder.Property(cd => cd.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(cd => cd.Libelle)
                .IsRequired()
                .HasMaxLength(500);
            builder.Ignore(cd => cd.CodeLibelle);

            builder.HasOne(cd => cd.Societe)
                .WithMany(s => s.CodeDeplacements)
                .HasForeignKey(cd => cd.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

