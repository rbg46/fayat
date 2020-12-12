using Fred.Entities.Holding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class HoldingEntConfiguration : IEntityTypeConfiguration<HoldingEnt>
    {
        public void Configure(EntityTypeBuilder<HoldingEnt> builder)
        {
            builder.ToTable("FRED_HOLDING");
            builder.HasKey(h => h.HoldingId);
            builder.HasIndex(h => h.Code)
                .HasName("IX_UniqueCode")
                .IsUnique();

            builder.Property(h => h.HoldingId)
                .ValueGeneratedOnAdd();
            builder.Property(h => h.Code)
                .IsRequired()
                .IsFixedLength()
                .HasColumnType("varchar(20)");
            builder.Property(h => h.Libelle)
                .IsRequired()
                .IsFixedLength()
                .HasColumnType("varchar(500)");
            builder.Ignore(h => h.CodeLibelle);

            builder.HasOne(h => h.Organisation)
                .WithOne(o => o.Holding)
                .HasForeignKey<HoldingEnt>(h => h.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

