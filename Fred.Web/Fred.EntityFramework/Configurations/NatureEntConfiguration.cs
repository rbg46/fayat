using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class NatureEntConfiguration : IEntityTypeConfiguration<NatureEnt>
    {
        public void Configure(EntityTypeBuilder<NatureEnt> builder)
        {
            builder.ToTable("FRED_NATURE");
            builder.HasKey(n => n.NatureId);
            builder.HasIndex(n => new { n.Code, n.SocieteId })
                .HasName("IX_UniqueCodeAndSociete")
                .IsUnique()
                .HasFilter(null);
            builder.HasIndex(n => n.RessourceId)
                .HasName("IX_UniqueCodeAndResource")
                .HasFilter(null);

            builder.Property(n => n.NatureId)
                .ValueGeneratedOnAdd();
            builder.Property(n => n.Code)
                .HasMaxLength(50);
            builder.Property(n => n.Libelle)
                .HasMaxLength(500);
            builder.Property(n => n.DateCreation)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.HasOne(n => n.Societe)
                .WithMany(s => s.Natures)
                .HasForeignKey(n => n.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(n => n.AuteurCreation)
                .WithMany()
                .HasForeignKey(n => n.AuteurCreationId);
            builder.HasOne(n => n.AuteurModification)
               .WithMany()
               .HasForeignKey(n => n.AuteurModificationId);
        }
    }
}

