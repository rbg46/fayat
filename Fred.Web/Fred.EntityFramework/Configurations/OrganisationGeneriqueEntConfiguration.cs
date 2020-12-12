using Fred.Entities.OrganisationGenerique;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class OrganisationGeneriqueEntConfiguration : IEntityTypeConfiguration<OrganisationGeneriqueEnt>
    {
        public void Configure(EntityTypeBuilder<OrganisationGeneriqueEnt> builder)
        {
            builder.ToTable("FRED_ORGANISATION_GENERIQUE");
            builder.HasKey(og => og.OrganisationGeneriqueId);
            builder.HasIndex(og => og.Code)
                .HasName("IX_UniqueCode")
                .IsUnique();

            builder.Property(og => og.OrganisationGeneriqueId)
                .ValueGeneratedOnAdd();
            builder.Property(og => og.Code)
                .IsRequired()
                .HasColumnType("nvarchar(20)");
            builder.Property(og => og.Libelle)
                .IsRequired()
                .HasColumnType("nvarchar(500)");

            builder.HasOne(og => og.Organisation)
                .WithOne(o => o.OrganisationGenerique)
                .HasForeignKey<OrganisationGeneriqueEnt>(og => og.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

