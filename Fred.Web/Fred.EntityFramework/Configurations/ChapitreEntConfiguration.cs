
using Fred.Entities.ReferentielFixe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ChapitreEntConfiguration : IEntityTypeConfiguration<ChapitreEnt>
    {
        public void Configure(EntityTypeBuilder<ChapitreEnt> builder)
        {
            builder.ToTable("FRED_CHAPITRE");
            builder.HasKey(c => c.ChapitreId);
            builder.HasIndex(c => new { c.Code, c.GroupeId })
                .IsUnique()
                .HasName("IX_UniqueCodeAndGroupe");

            builder.Property(c => c.ChapitreId)
                .ValueGeneratedOnAdd();
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(c => c.Libelle)
                .HasMaxLength(500);

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(a => a.Groupe)
                .WithMany(b => b.Chapitres)
                .HasForeignKey(c => c.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.AuteurCreation)
               .WithMany()
               .HasForeignKey(c => c.AuteurCreationId);
            builder.HasOne(c => c.AuteurModification)
               .WithMany()
               .HasForeignKey(c => c.AuteurModificationId);
            builder.HasOne(c => c.AuteurSuppression)
               .WithMany()
               .HasForeignKey(c => c.AuteurSuppressionId);
        }
    }
}

