using Fred.Entities.RessourcesRecommandees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RessourceRecommandeeEntConfiguration : IEntityTypeConfiguration<RessourceRecommandeeEnt>
    {
        public void Configure(EntityTypeBuilder<RessourceRecommandeeEnt> builder)
        {
            builder.ToTable("FRED_RESSOURCE_RECOMMANDEE_ORGANISATION");
            builder.HasKey(rr => rr.RessourceRecommandeeId);
            builder.HasIndex(rr => new
                {
                    rr.OrganisationId,
                    rr.ReferentielEtenduId
                })
                .HasName("IX_UniqueOrganisationIdAndReferentielEtenduId")
                .IsUnique();

            builder.Ignore(rr => rr.IsRecommandee);

            builder.HasOne(rr => rr.Organisation)
                .WithMany(o => o.RessourcesRecommandees)
                .HasForeignKey(rr => rr.OrganisationId);
            builder.HasOne(rr => rr.ReferentielEtendu)
                .WithMany(re => re.RessourcesRecommandees)
                .HasForeignKey(rr => rr.ReferentielEtenduId);
        }
    }
}
