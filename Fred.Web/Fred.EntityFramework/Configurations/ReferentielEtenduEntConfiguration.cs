using Fred.Entities.ReferentielEtendu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ReferentielEtenduEntConfiguration : IEntityTypeConfiguration<ReferentielEtenduEnt>
    {
        public void Configure(EntityTypeBuilder<ReferentielEtenduEnt> builder)
        {
            builder.ToTable("FRED_SOCIETE_RESSOURCE_NATURE");
            builder.HasKey(re => re.ReferentielEtenduId);
            builder.Property(re => re.ReferentielEtenduId)
                .ValueGeneratedOnAdd();

            builder.Ignore(re => re.ListeUnitesAbregees);

            builder.Property(re => re.Achats)
                .HasDefaultValue(0);

            builder.HasOne(re => re.Societe)
                .WithMany(c => c.ReferentielEtendus)
                .HasForeignKey(re => re.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(re => re.Ressource)
                .WithMany(c => c.ReferentielEtendus)
                .HasForeignKey(re => re.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(re => re.Nature)
                .WithMany(c => c.ReferentielEtendus)
                .HasForeignKey(re => re.NatureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}