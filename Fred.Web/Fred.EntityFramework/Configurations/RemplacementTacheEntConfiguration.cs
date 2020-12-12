using Fred.Entities.Depense;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RemplacementTacheEntConfiguration : IEntityTypeConfiguration<RemplacementTacheEnt>
    {
        public void Configure(EntityTypeBuilder<RemplacementTacheEnt> builder)
        {
            builder.ToTable("FRED_REMPLACEMENT_TACHE");
            builder.HasKey(rt => rt.RemplacementTacheId);
            builder.Property(rt => rt.RemplacementTacheId)
                .ValueGeneratedOnAdd();

            builder.Ignore(rt => rt.CiId);
            builder.Ignore(rt => rt.IsPeriodeCloturee);

            builder.Property(cod => cod.DateComptableRemplacement)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateRemplacement)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(rt => rt.AuteurCreation)
                .WithMany()
                .HasForeignKey(rt => rt.AuteurCreationId);
            builder.HasOne(rt => rt.AuteurSuppression)
                .WithMany()
                .HasForeignKey(rt => rt.AuteurSuppressionId);
            builder.HasOne(rt => rt.GroupeRemplacementTache)
                .WithMany()
                .HasForeignKey(rt => rt.GroupeRemplacementTacheId);
            builder.HasOne(rt => rt.Tache)
                .WithMany()
                .HasForeignKey(rt => rt.TacheId);
        }
    }
}
