using Fred.Entities.ReferentielEtendu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ParametrageReferentielEtenduEntConfiguration : IEntityTypeConfiguration<ParametrageReferentielEtenduEnt>
    {
        public void Configure(EntityTypeBuilder<ParametrageReferentielEtenduEnt> builder)
        {
            builder.ToTable("FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU");
            builder.HasKey(pre => pre.ParametrageReferentielEtenduId);

            builder.Property(pre => pre.ParametrageReferentielEtenduId)
                .ValueGeneratedOnAdd();
            builder.Property(pre => pre.Montant)
                .HasColumnType("decimal(11, 2)");
            builder.Ignore(pre => pre.ParametragesParent);
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(pre => pre.Organisation)
                .WithMany(o => o.ParametrageReferentielEtendus)
                .HasForeignKey(pre => pre.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pre => pre.Devise)
                .WithMany(d => d.ParametrageReferentielEtendus)
                .HasForeignKey(pre => pre.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pre => pre.ReferentielEtendu)
                .WithMany(re => re.ParametrageReferentielEtendus)
                .HasForeignKey(pre => pre.ReferentielEtenduId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pre => pre.Unite)
                .WithMany(u => u.ParametrageReferentielEtendus)
                .HasForeignKey(pre => pre.UniteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pre => pre.AuteurCreation)
                .WithMany()
                .HasForeignKey(pre => pre.AuteurCreationId);
            builder.HasOne(pre => pre.AuteurModification)
               .WithMany()
               .HasForeignKey(pre => pre.AuteurModificationId);
            builder.HasOne(pre => pre.AuteurSuppression)
               .WithMany()
               .HasForeignKey(pre => pre.AuteurSuppressionId);
        }
    }
}