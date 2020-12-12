using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AgenceEntConfiguration : AuditableEntityConfiguration<AgenceEnt>, IEntityTypeConfiguration<AgenceEnt>
    {
        public void Configure(EntityTypeBuilder<AgenceEnt> builder)
        {
            builder.ToTable("FRED_AGENCE");
            builder.HasKey(bt4 => bt4.AgenceId);
            builder.Property(bt4 => bt4.AgenceId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.Libelle)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.SIRET)
                .HasMaxLength(19);
            builder.Property(a => a.Telephone)
                .HasMaxLength(15);
            builder.Property(a => a.Fax)
                .HasMaxLength(15);
            builder.Property(a => a.Email)
                .HasMaxLength(250);

            builder.HasOne(a => a.Adresse)
                .WithMany()
                .HasForeignKey(a => a.AdresseId);
            builder.HasOne(a => a.Fournisseur)
                .WithMany(f => f.Agences)
                .HasForeignKey(a => a.FournisseurId);
        }
    }
}
