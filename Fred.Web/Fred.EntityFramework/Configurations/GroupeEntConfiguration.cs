using Fred.Entities.Groupe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class GroupeEntConfiguration : IEntityTypeConfiguration<GroupeEnt>
    {
        public void Configure(EntityTypeBuilder<GroupeEnt> builder)
        {
            builder.ToTable("FRED_GROUPE");
            builder.HasKey(g => g.GroupeId);
            builder.HasIndex(g => new { g.Code, g.PoleId })
                .HasName("IX_UniqueCodeAndPole")
                .IsUnique();

            builder.Property(g => g.GroupeId)
                .ValueGeneratedOnAdd();
            builder.Property(g => g.Code)
                .IsRequired()
                .IsFixedLength()
                .HasColumnType("nvarchar(20)");
            builder.Property(g => g.Libelle)
                .IsRequired()
                .IsFixedLength()
                .HasColumnType("nvarchar(500)");
                
            builder.HasOne(g => g.Organisation)
                .WithOne(o => o.Groupe)
                .HasForeignKey<GroupeEnt>(g => g.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(g => g.Pole)
                .WithMany(p => p.Groupes)
                .HasForeignKey(g => g.PoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

