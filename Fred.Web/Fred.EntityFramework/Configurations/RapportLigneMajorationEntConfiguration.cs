using Fred.Entities.Rapport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class RapportLigneMajorationEntConfiguration : IEntityTypeConfiguration<RapportLigneMajorationEnt>
    {
        public void Configure(EntityTypeBuilder<RapportLigneMajorationEnt> builder)
        {
            builder.ToTable("FRED_RAPPORT_LIGNE_MAJORATION");
            builder.HasKey(x => x.RapportLigneMajorationId);

            builder.Property(x => x.RapportLigneMajorationId)
                .ValueGeneratedOnAdd();
            builder.Ignore(x => x.PointageMajorationId);
            builder.Ignore(x => x.PointageId);
            builder.Ignore(x => x.IsDeleted);
            builder.Ignore(x => x.IsCreated);

            builder.HasOne(x => x.CodeMajoration)
                .WithMany(cm => cm.ListRapportLignesMajoration)
                .HasForeignKey(x => x.CodeMajorationId);
            builder.HasOne(x => x.RapportLigne)
                .WithMany(rl => rl.ListRapportLigneMajorations)
                .HasForeignKey(x => x.RapportLigneId);
        }
    }
}
