using Fred.Entities.ReferentielEtendu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class UniteReferentielEtenduEntConfiguration : IEntityTypeConfiguration<UniteReferentielEtenduEnt>
    {
        public void Configure(EntityTypeBuilder<UniteReferentielEtenduEnt> builder)
        {     builder.ToTable("FRED_UNITE_REFERENTIEL_ETENDU");
            builder.HasKey(ure => ure.UniteReferentielEtenduId);
            builder.Property(ure => ure.UniteReferentielEtenduId)
                .ValueGeneratedOnAdd();

            builder.Ignore(ure => ure.IsDeleted);

            builder.HasOne(ure => ure.ReferentielEtendu)
                .WithMany(re => re.UniteReferentielEtendus)
                .HasForeignKey(ure => ure.ReferentielEtenduId);
            builder.HasOne(ure => ure.Unite)
                .WithMany()
                .HasForeignKey(ure => ure.UniteId);
       
        }
    }
}
