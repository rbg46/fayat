using System;
using System.Collections.Generic;
using System.Text;
using Fred.Entities.Affectation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class EquipePersonnelEntConfiguration : IEntityTypeConfiguration<EquipePersonnelEnt>
    {
        public void Configure(EntityTypeBuilder<EquipePersonnelEnt> builder)
        {
            builder.ToTable("FRED_EQUIPE_PERSONNEL");
            builder.HasKey(ep => ep.EquipePersonnelId);
            builder.Property(ep => ep.EquipePersonnelId)
                .ValueGeneratedOnAdd();

            builder.HasOne(ep => ep.Equipe)
                .WithMany(e => e.EquipePersonnels)
                .HasForeignKey(ep => ep.EquipePersoId);
            builder.HasOne(ep => ep.Personnel)
                .WithMany(p => p.EquipePersonnels)
                .HasForeignKey(ep => ep.PersonnelId);
        }
    }
}
