
using Fred.Entities.Organisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AffectationSeuilOrgaEntConfiguration : IEntityTypeConfiguration<AffectationSeuilOrgaEnt>
    {
        public void Configure(EntityTypeBuilder<AffectationSeuilOrgaEnt> builder)
        {
            builder.ToTable("FRED_ROLE_ORGANISATION_DEVISE");
            builder.HasKey(aso => aso.SeuilRoleOrgaId);

            builder.Property(aso => aso.SeuilRoleOrgaId)
                .ValueGeneratedOnAdd();
            builder.Property(aso => aso.Seuil).
                HasColumnType("decimal(11, 2)");

            builder.HasOne(aso => aso.Role).WithMany(r => r.AffectationSeuilOrgas).HasForeignKey(aso => aso.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(aso => aso.Devise).WithMany(d => d.AffectationSeuilOrgas).HasForeignKey(aso => aso.DeviseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(aso => aso.Organisation).WithMany(o => o.AffectationsSeuilRoleOrga).HasForeignKey(aso => aso.OrganisationId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

