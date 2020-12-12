using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class RapportTacheEntConfiguration : IEntityTypeConfiguration<RapportTacheEnt>
    {
        public void Configure(EntityTypeBuilder<RapportTacheEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_TACHE");
            builder.HasKey(rt => rt.RapportTacheId);
            builder.Property(rt => rt.RapportTacheId)
                .ValueGeneratedOnAdd();

            builder.Property(rt => rt.Commentaire)
                .HasColumnType("varchar(250)");

            builder.Ignore(rt => rt.IsCreated);
            builder.Ignore(rt => rt.IsDeleted);

            builder.HasOne(rt => rt.Rapport)
                .WithMany(r => r.ListCommentaires)
                .HasForeignKey(rt => rt.RapportId);
            builder.HasOne(rt => rt.Tache)
                .WithMany()
                .HasForeignKey(rt => rt.TacheId);
        }
    }
}
