
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CodeAbsenceEntConfiguration : IEntityTypeConfiguration<CodeAbsenceEnt>
    {
        public void Configure(EntityTypeBuilder<CodeAbsenceEnt> builder)
        {
            builder.ToTable("FRED_CODE_ABSENCE");
            builder.HasKey(ca => ca.CodeAbsenceId);

            builder.Property(ca => ca.CodeAbsenceId)
                .ValueGeneratedOnAdd();
            builder.Property(ca => ca.Code)
                .HasMaxLength(20);
            builder.Property(ca => ca.Libelle)
                .HasMaxLength(500);

            builder.Ignore(ca => ca.CodeLibelle);

            builder.HasOne(ca => ca.Holding)
                .WithMany(h => h.CodeAbsences)
                .HasForeignKey(ca => ca.HoldingId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ca => ca.Societe)
                .WithMany(s => s.CodeAbsences)
                .HasForeignKey(ca => ca.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ca => ca.Groupe)
                .WithMany()
                .HasForeignKey(ca => ca.GroupeId);
            builder.HasOne(ca => ca.CodeAbsneceParent)
                .WithMany()
                .HasForeignKey(ca => ca.CodeAbsenceParentId);
        }
    }
}