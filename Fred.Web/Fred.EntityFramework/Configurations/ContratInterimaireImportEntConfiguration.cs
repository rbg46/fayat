using Fred.Entities.Personnel.Interimaire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ContratInterimaireImportEntConfiguration : IEntityTypeConfiguration<ContratInterimaireImportEnt>
    {
        public void Configure(EntityTypeBuilder<ContratInterimaireImportEnt> builder)
        {
            builder.ToTable("FRED_CONTRAT_INTERIMAIRE_IMPORT");
            builder.HasKey(ci => ci.ImportId);

            builder.Property(ci => ci.ImportId)
                .HasColumnName("ImportId")
                .ValueGeneratedOnAdd();

            builder.Property(ci => ci.ContratInterimaireId)
                .IsRequired();

            builder.Property(ci => ci.TimestampImport)
                .IsRequired()
                .HasColumnType("bigint");

            builder.Property(ci => ci.HistoriqueImport)
                .HasMaxLength(1000);

            builder.HasOne(x => x.ContratInterimaire).WithMany(x => x.ContratInterimaireImports).HasForeignKey(x => x.ContratInterimaireId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

