using Fred.Entities.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class JournalEntConfiguration : IEntityTypeConfiguration<JournalEnt>
    {
        public void Configure(EntityTypeBuilder<JournalEnt> builder)
        {
            builder.ToTable("FRED_JOURNAL");
            builder.HasKey(j => j.JournalId);
            builder.HasIndex(j => new { j.Code, j.SocieteId })
                .HasName("IX_UniqueCodeAndSociete")
                .IsUnique();

            builder.Property(j => j.JournalId)
                .ValueGeneratedOnAdd();
            builder.Property(j => j.Code)
                .IsRequired()
                .HasMaxLength(3);
            builder.Property(j => j.Libelle)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(j => j.TypeJournal)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(j => j.ParentFamilyODWithOrder)
                .HasDefaultValue(0);
            builder.Property(j => j.ParentFamilyODWithoutOrder)
                .HasDefaultValue(0);
            builder.Property(j => j.ImportFacture)
                .HasDefaultValue(0);
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCloture)
                .HasColumnType("datetime");

            builder.HasOne(j => j.Societe)
                .WithMany(s => s.Journals)
                .HasForeignKey(j => j.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(j => j.AuteurCreation)
                .WithMany()
                .HasForeignKey(j => j.AuteurCreationId);
            builder.HasOne(j => j.AuteurModification)
               .WithMany()
               .HasForeignKey(j => j.AuteurModificationId);
            builder.HasOne(j => j.AuteurCloture)
               .WithMany()
               .HasForeignKey(j => j.AuteurClotureId);
        }
    }
}

