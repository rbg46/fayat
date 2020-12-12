using Fred.Entities.ReferentielFixe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class SousChapitreEntConfiguration : IEntityTypeConfiguration<SousChapitreEnt>
    {
        public void Configure(EntityTypeBuilder<SousChapitreEnt> builder)
        {
            builder.ToTable("FRED_SOUS_CHAPITRE");
            builder.HasKey(sc => sc.SousChapitreId);
            builder.Property(sc => sc.SousChapitreId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(sc => new
                {
                    sc.Code, sc.ChapitreId
                })
                .HasName("IX_UniqueCodeAndChapitreId")
                .IsUnique();

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.Property(sc => sc.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(sc => sc.Libelle)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(sc => sc.Chapitre)
                .WithMany(c => c.SousChapitres)
                .HasForeignKey(sc => sc.ChapitreId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sc => sc.AuteurCreation)
                .WithMany()
                .HasForeignKey(sc => sc.AuteurCreationId);
            builder.HasOne(sc => sc.AuteurModification)
                .WithMany()
                .HasForeignKey(sc => sc.AuteurModificationId);
            builder.HasOne(sc => sc.AuteurSuppression)
                .WithMany()
                .HasForeignKey(sc => sc.AuteurSuppressionId);
        }
    }
}