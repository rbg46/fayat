
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CodeMajorationEntConfiguration : IEntityTypeConfiguration<CodeMajorationEnt>
    {
        public void Configure(EntityTypeBuilder<CodeMajorationEnt> builder)
        {
            builder.ToTable("FRED_CODE_MAJORATION");
            builder.HasKey(cm => cm.CodeMajorationId);
            builder.HasIndex(cm => new { cm.Code, cm.GroupeId })
                .HasName("IX_UniqueCodeAndGroupe")
                .IsUnique();

            builder.Property(cm => cm.CodeMajorationId)
                .ValueGeneratedOnAdd();
            builder.Property(cm => cm.Code) 
                .HasMaxLength(20);
            builder.Property(cm => cm.Libelle)
                .HasMaxLength(500);
            builder.Property(cm => cm.IsHeureNuit)
                .HasDefaultValue(0);

            builder.Ignore(cm => cm.IsLinkedToCI);

            builder.HasOne(cm => cm.Groupe)
                .WithMany(g => g.CodeMajorations)
                .HasForeignKey(cm => cm.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cm => cm.AuteurCreation)
                .WithMany()
                .HasForeignKey(cm => cm.AuteurCreationId);
            builder.HasOne(cm => cm.AuteurModification)
               .WithMany()
               .HasForeignKey(cm => cm.AuteurModificationId);
            builder.HasOne(cm => cm.AuteurSuppression)
               .WithMany()
               .HasForeignKey(cm => cm.AuteurSuppressionId);
        }
    }
}

