using Fred.Entities.Personnel.Interimaire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ZoneDeTravailEntConfiguration : IEntityTypeConfiguration<ZoneDeTravailEnt>
    {
        public void Configure(EntityTypeBuilder<ZoneDeTravailEnt> builder)
        {
            builder.ToTable("FRED_ZONE_DE_TRAVAIL");
            builder.HasKey(zdt => new
            {
                zdt.ContratInterimaireId,
                zdt.EtablissementComptableId
            });

            builder.HasOne(zdt => zdt.Contrat)
                .WithMany(ci => ci.ZonesDeTravail)
                .HasForeignKey(zdt => zdt.ContratInterimaireId);
            builder.HasOne(zdt => zdt.EtablissementComptable)
                .WithMany()
                .HasForeignKey(zdt => zdt.EtablissementComptableId);
        }
    }
}
