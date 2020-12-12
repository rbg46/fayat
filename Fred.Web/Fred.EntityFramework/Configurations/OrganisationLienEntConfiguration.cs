using Fred.Entities.Organisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class OrganisationLienEntConfiguration : IEntityTypeConfiguration<OrganisationLienEnt>
    {
        public void Configure(EntityTypeBuilder<OrganisationLienEnt> builder)
        {
            builder.ToTable("FRED_ORGA_LIENS");
            builder.HasKey(ol => ol.OrganisationLiensId);

            builder.Property(ol => ol.OrganisationLiensId)
                .HasColumnName(@"OrgaLiensId")
                .ValueGeneratedOnAdd();
            builder.Property(ol => ol.OrganisationId)
                .IsRequired();

            builder.HasOne(ol => ol.EtablissementComptable)
                .WithMany(ec => ec.OrganisationLiens)
                .HasForeignKey(ol => ol.EtablissementComptableId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ol => ol.Organisation)
                .WithMany(o => o.OrganisationLiens)
                .HasForeignKey(ol => ol.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ol => ol.Societe)
               .WithMany(s => s.OrganisationLiens)
               .HasForeignKey(ol => ol.SocieteId);
        }
    }
}

