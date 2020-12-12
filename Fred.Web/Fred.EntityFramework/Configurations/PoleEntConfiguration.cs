using Fred.Entities.Pole;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PoleEntConfiguration : IEntityTypeConfiguration<PoleEnt>
    {
        public void Configure(EntityTypeBuilder<PoleEnt> builder)
        {
            builder.ToTable("FRED_POLE");
            builder.HasKey(sbec => sbec.PoleId);
            builder.Property(sbec => sbec.PoleId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(s => new
            {
                s.Code,
                s.HoldingId
            })
            .HasName("IX_UniqueCodeAndHolding")
            .IsUnique();

            builder.Property(sbec => sbec.Code)
                .HasColumnType("nvarchar(20)")
                .IsRequired()
                .IsFixedLength();
            builder.Property(sbec => sbec.Libelle)
                .HasColumnType("nvarchar(500)")
                .IsRequired()
                .IsFixedLength();

            builder.Ignore(sbec => sbec.CodeLibelle);

            builder.HasOne(og => og.Organisation)
                .WithOne(o => o.Pole)
                .HasForeignKey<PoleEnt>(p => p.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Holding)
                .WithMany(c => c.Poles)
                .HasForeignKey(sbec => sbec.HoldingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}