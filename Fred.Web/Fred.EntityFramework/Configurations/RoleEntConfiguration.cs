using Fred.Entities.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RoleEntConfiguration : IEntityTypeConfiguration<RoleEnt>
    {
        public void Configure(EntityTypeBuilder<RoleEnt> builder)
        {
            builder.ToTable("FRED_ROLE");
            builder.HasKey(r => r.RoleId);
            builder.Property(r => r.RoleId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(s => new
            {
                s.CodeNomFamilier,
                s.SocieteId
            })
                .HasName("IX_UniqueCodeAndGroupe")
                .IsUnique();

            builder.Property(r => r.CodeNomFamilier)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(r => r.Libelle)
                .HasColumnType("nvarchar(255)")
                .IsUnicode(false);
            builder.Property(r => r.RoleSpecification)
                .HasColumnName("Specification");
            builder.Property(r => r.Code)
                .HasColumnType("varchar(255)")
                .IsUnicode(false);
            builder.Property(r => r.CommandeSeuilDefaut)
                .HasColumnType("varchar(255)")
                .IsUnicode(false);

            builder.Ignore(r => r.AffectationsByOrganisation);

            builder.HasOne(r => r.Societe)
                .WithMany(s => s.Roles)
                .HasForeignKey(r => r.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
