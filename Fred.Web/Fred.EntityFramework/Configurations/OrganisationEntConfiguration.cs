using Fred.Entities.Organisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class OrganisationEntConfiguration : IEntityTypeConfiguration<OrganisationEnt>
    {
        public void Configure(EntityTypeBuilder<OrganisationEnt> builder)
        {
            builder.ToTable("FRED_ORGANISATION");
            builder.HasKey(o => o.OrganisationId);

            builder.Property(o => o.OrganisationId)
                .ValueGeneratedOnAdd();
            builder.Ignore(o => o.CodeOrdering);

            builder.HasOne(o => o.Pere)
                .WithMany(o => o.OrganisationsEnfants)
                .HasForeignKey(o => o.PereId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.TypeOrganisation)
                .WithMany(to => to.Organisations)
                .HasForeignKey(o => o.TypeOrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

