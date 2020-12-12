
using Fred.Entities.CI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CICodeMajorationEntConfiguration : IEntityTypeConfiguration<CICodeMajorationEnt>
    {
        public void Configure(EntityTypeBuilder<CICodeMajorationEnt> builder)
        {
            builder.ToTable("FRED_CI_CODE_MAJORATION");
            builder.HasKey(x => x.CiCodeMajorationId);

            builder.Property(x => x.CiCodeMajorationId)
                .ValueGeneratedOnAdd();

            builder.HasOne(ccm => ccm.CI)
                .WithMany(b => b.CICodeMajorations)
                .HasForeignKey(c => c.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ccm => ccm.CodeMajoration)
                .WithMany(b => b.CICodesMajoration)
                .HasForeignKey(c => c.CodeMajorationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

