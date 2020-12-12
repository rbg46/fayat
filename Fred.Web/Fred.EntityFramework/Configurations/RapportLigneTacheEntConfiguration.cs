using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportLigneTacheEntConfiguration : IEntityTypeConfiguration<RapportLigneTacheEnt>
    {
        public void Configure(EntityTypeBuilder<RapportLigneTacheEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_LIGNE_TACHE");
            builder.HasKey(sbec => sbec.RapportLigneTacheId);
            builder.Property(sbec => sbec.RapportLigneTacheId)
                .ValueGeneratedOnAdd();

            builder.Property(sbec => sbec.HeureTache)
                .HasColumnType("float");

            builder.Ignore(sbec => sbec.IsCreated);
            builder.Ignore(sbec => sbec.IsDeleted);
            builder.Ignore(sbec => sbec.Commentaire);

            builder.HasOne(sbec => sbec.RapportLigne)
                .WithMany(c => c.ListRapportLigneTaches)
                .HasForeignKey(sbec => sbec.RapportLigneId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbec => sbec.Tache)
                .WithMany(c => c.RapportLigneTaches)
                .HasForeignKey(sbec => sbec.TacheId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}