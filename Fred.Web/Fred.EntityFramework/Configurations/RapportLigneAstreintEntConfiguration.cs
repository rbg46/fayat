using Fred.Entities.Rapport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class RapportLigneAstreintEntConfiguration : IEntityTypeConfiguration<RapportLigneAstreinteEnt>
    {
        public void Configure(EntityTypeBuilder<RapportLigneAstreinteEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_LIGNE_ASTREINTE");
            builder.HasKey(rla => rla.RapportLigneAstreinteId);

            builder.Property(rla => rla.RapportLigneAstreinteId)
                .ValueGeneratedOnAdd();
            builder.Property(cod => cod.DateDebutAstreinte)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateFinAstreinte)
                .HasColumnType("datetime");

            builder.HasOne(rla => rla.RapportLigne)
                .WithMany(rl => rl.ListRapportLigneAstreintes)
                .HasForeignKey(rla => rla.RapportLigneId);
            builder.HasOne(rla => rla.Astreinte)
                .WithMany()
                .HasForeignKey(rla => rla.AstreinteId);
        }
    }
}
